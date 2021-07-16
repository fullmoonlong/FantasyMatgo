using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    #region INSTANCE
    public static CardManager instance;
    #endregion

    private int maxCards = 48;

    public Transform parentDeck; // Instantiate �� ���(���� ���� ���� ��)

    private List<GameObject> cardPrefabs; // ī���� ������ ȭ
    private List<GameObject> cardDeck; // �ʵ忡 ���� ī�� �� ����Ʈ ����
    public List<GameObject> myHand; // �÷��̾� �ڽ��� �� ����Ʈ
    public List<GameObject> opponentHand; // ����� �� ����Ʈ
    public List<GameObject> field; // �ʵ� ����Ʈ

    public GameObject[] fieldPostion; // ī�带 �������� �ʵ� ��ġ
    public GameObject[] myHandPosition; // �ڽ� ���� ��ġ
    public GameObject[] opponentHandPosition; // ��� ���� ��ġ

    private void Awake()
    {
        instance = this;
        cardPrefabs = new List<GameObject>(); // ������ ����Ʈ �Ҵ�
        cardDeck = new List<GameObject>(); // �� ����Ʈ �Ҵ�
        myHand = new List<GameObject>(); // �� ����Ʈ �Ҵ�
        opponentHand = new List<GameObject>(); // �� ����Ʈ �Ҵ�
        field = new List<GameObject>(); // �� ����Ʈ �Ҵ�
    }

    private void Start()
    {
        //�� ī���� ��ġ ����
        PrefabToCard(); // ������ ������ �����ϴ� ī�带 ����Ʈ�� ��� �����غ� �Ѵ�.
        CreateDeck(); /// �÷��̾ �غ��� ī�� 12��, ���� �غ��� ī�� 12�� �� ���� �� 48���� ī�带 ���� �ִ´�.
        ShuffleDeck(); //���� ���´�
        DrawCard(myHand, 10); // ���տ� 10�徿 �̴´�.
        DrawCard(opponentHand, 10);  // ���տ� 10�� �� �̴´�.
        DrawCard(field, 8);
    }

    private void Update()
    {
        //if (GameManager.instance.myTurn == true)
        //{
        //    DrawCard(myHand, 1);
        //    GameManager.instance.myTurn = false;
        //    //ī�带 �ϳ� ����.
        //    //���� ������ �ڱ��� �п� ���´�.
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
        for (int i = 0; i < maxCards; i++) // ȭ���� ����
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
