using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    private int maxCards = 48;
    private int firstHand = 6;
    public bool isMyTurn;
    public static CardManager instance;

    public GameObject cardPrefab;
    public Transform parentDeck;

    public List<GameObject> cardDeck; // 필드에 놓일 카드 덱 리스트 생성
    public List<GameObject> myHand; // 플레이어 자신의 패를 담당
    public List<GameObject> opponentHand; // 상대의 패를 담당

    public GameObject[] cardPostion;
    public GameObject[] myPosition;
    public GameObject[] opponentPostion;

    private void Awake()
    {
        instance = this;
        cardDeck = new List<GameObject>(); // 덱 리스트 할당
        myHand = new List<GameObject>(); // 패 리스트 할당
        opponentHand = new List<GameObject>(); // 패 리스트 할당
    }

    private void Start()
    {
        //각 카드의 위치 설정
        CreateDeck(); /// 플레이어가 준비한 카드 12장, 적이 준비한 카드 12장 을 더해 총 48장의 카드를 덱에 넣는다.
        ShuffleDeck(); //덱을 섞는다
        DrawCard(myHand, 10); // 내손에 10장씩 뽑는다.
        DrawCard(opponentHand, 10);  // 상대손에 10장 씩 뽑는다.
        //DrawCard(cardDeck, 8); // 필드에 8장씩 놓는다.
    }

    private void Update()
    {
        //if(GameManager.instance.myTurn == true)
        //{
        //    DrawCard(myHand, 1);
        //    //카드를 하나 낸다.
        //    //만약 맞으면 자기의 패에 놓는다.

        //}

        //else if(GameManager.instance.myTurn == false)
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
            cardDeck.Add(Instantiate(cardPrefab, parentDeck));
            cardDeck[i].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(("Images/" + i));
        }
    }

    public void DrawFirstHand()
    {
        if (cardDeck.Count > firstHand * 2)
        {
            for (int i = 0; i < firstHand; i++)
            {
                myHand.Add(cardDeck[i]);
            }
            for (int i = 0; i < firstHand; i++)
            {
                opponentHand.Add(cardDeck[i]);
            }
        }
    }

    public void DrawCard(List<GameObject> hand, int num)
    {
        if (num > cardDeck.Count)
        {
            num = cardDeck.Count;
        }

        for (int i = 0; i < num; i++)
        {
            SetCardPosition(hand, i);
            hand.Add(cardDeck[i]);
            cardDeck.RemoveAt(i);
        }
    }

    public void SetCardPosition(List<GameObject> hand, int num)
    {
        if(hand == myHand)
        {
            cardDeck[num].transform.position = myPosition[num].transform.position;
        }
        else if(hand == opponentHand)
        {
            cardDeck[num].transform.position = opponentPostion[num].transform.position;
        }
    }
}
