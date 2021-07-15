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

    public List<GameObject> cardDeck; // �ʵ忡 ���� ī�� �� ����Ʈ ����
    public List<GameObject> myHand; // �÷��̾� �ڽ��� �и� ���
    public List<GameObject> opponentHand; // ����� �и� ���

    public GameObject[] cardPostion;
    public GameObject[] myPosition;
    public GameObject[] opponentPostion;

    private void Awake()
    {
        instance = this;
        cardDeck = new List<GameObject>(); // �� ����Ʈ �Ҵ�
        myHand = new List<GameObject>(); // �� ����Ʈ �Ҵ�
        opponentHand = new List<GameObject>(); // �� ����Ʈ �Ҵ�
    }

    private void Start()
    {
        //�� ī���� ��ġ ����
        CreateDeck(); /// �÷��̾ �غ��� ī�� 12��, ���� �غ��� ī�� 12�� �� ���� �� 48���� ī�带 ���� �ִ´�.
        ShuffleDeck(); //���� ���´�
        DrawCard(myHand, 10); // ���տ� 10�徿 �̴´�.
        DrawCard(opponentHand, 10);  // ���տ� 10�� �� �̴´�.
        //DrawCard(cardDeck, 8); // �ʵ忡 8�徿 ���´�.
    }

    private void Update()
    {
        //if(GameManager.instance.myTurn == true)
        //{
        //    DrawCard(myHand, 1);
        //    //ī�带 �ϳ� ����.
        //    //���� ������ �ڱ��� �п� ���´�.

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
        for (int i = 0; i < maxCards; i++) // ȭ���� ����
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
