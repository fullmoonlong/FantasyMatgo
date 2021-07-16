using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    #region INSTANCE
    public static CardManager instance;
    #endregion

    private int maxCards = 48;

    public Transform parentDeck; // Instantiate 할 대상(보기 좋게 묶기 용)

    private List<GameObject> cardPrefabs; // 카드의 프리팹 화
    private List<GameObject> cardDeck; // 필드에 놓일 카드 덱 리스트 생성
    public List<GameObject> myHand; // 플레이어 자신의 패 리스트
    public List<GameObject> opponentHand; // 상대의 패 리스트
    public List<GameObject> field; // 필드 리스트

    public GameObject[] fieldPostion; // 카드를 내려놓을 필드 위치
    public GameObject[] myHandPosition; // 자신 패의 위치
    public GameObject[] opponentHandPosition; // 상대 패의 위치

    private void Awake()
    {
        instance = this;
        cardPrefabs = new List<GameObject>(); // 프리팹 리스트 할당
        cardDeck = new List<GameObject>(); // 덱 리스트 할당
        myHand = new List<GameObject>(); // 패 리스트 할당
        opponentHand = new List<GameObject>(); // 패 리스트 할당
        field = new List<GameObject>(); // 패 리스트 할당
    }

    private void Start()
    {
        //각 카드의 위치 설정
        PrefabToCard(); // 프리팹 폴더에 존재하는 카드를 리스트에 담아 생성준비를 한다.
        CreateDeck(); /// 플레이어가 준비한 카드 12장, 적이 준비한 카드 12장 을 더해 총 48장의 카드를 덱에 넣는다.
        ShuffleDeck(); //덱을 섞는다
        DrawCard(myHand, 10); // 내손에 10장씩 뽑는다.
        DrawCard(opponentHand, 10);  // 상대손에 10장 씩 뽑는다.
        DrawCard(field, 8);
    }

    private void Update()
    {
        //if (GameManager.instance.myTurn == true)
        //{
        //    DrawCard(myHand, 1);
        //    GameManager.instance.myTurn = false;
        //    //카드를 하나 낸다.
        //    //만약 맞으면 자기의 패에 놓는다.
        //}

        //else if (GameManager.instance.myTurn == false)
        //{
        //    DrawCard(opponentHand, 1);
        //}

        //if (Input.GetKeyDown(KeyCode.K))
        //{
        //    Debug.Log("Count : " + cardDeck.Count);
        //    Debug.Log("element : " + cardDeck[0].GetComponent<SpriteRenderer>().sprite.name);
        //}
    }

    public void ShuffleDeck()
    {
        int a, b;
        GameObject temp;
        
        for (int i = 0; i < (maxCards / 2); i++)
        {
            a = Random.Range(0, maxCards / 2);
            b = Random.Range(maxCards / 2, maxCards);
            temp = cardDeck[a];
            cardDeck[a] = cardDeck[b];
            cardDeck[b] = temp;
        }
    }

    public void CreateDeck()
    {
        for (int i = 0; i < maxCards; i++) // 화투덱 숫자
        {
            cardDeck.Add(Instantiate(cardPrefabs[i], parentDeck));
        }
    }

    public void PrefabToCard()
    {
        for (int i = 0; i < maxCards; i++)
        {
            cardPrefabs.Add(Resources.Load<GameObject>("Prefabs/" + i));
        }
    }

    public void DrawCard(List<GameObject> cardList, int drawAmount)
    {
        if (drawAmount > cardDeck.Count)
        {
            drawAmount = cardDeck.Count;
        }

        for (int i = 0; i < drawAmount; i++)
        {
            cardList.Add(cardDeck[i]);
            SetCardPosition(cardList, cardList.Count - 1);
            cardDeck.RemoveAt(i);
        }
    }

    public void SetCardPosition(List<GameObject> cardList, int index)
    {
        if(cardList == myHand)
        {
            cardDeck[index].transform.position = myHandPosition[index].transform.position;
            cardDeck[index].transform.SetParent(myHandPosition[index].transform.parent);
        }
        else if(cardList == opponentHand)
        {
            cardDeck[index].transform.position = opponentHandPosition[index].transform.position;
            cardDeck[index].transform.SetParent(opponentHandPosition[index].transform.parent);

        }
        else if(cardList == field)
        {
            cardDeck[index].transform.position = fieldPostion[index].transform.position;
            cardDeck[index].transform.SetParent(fieldPostion[index].transform.parent);
        }
    }

    public void SelectCardInHand()
    {

    }
}
