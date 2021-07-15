using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    private int maxCards = 48;
    private int firstHand = 6;

    public static CardManager instance;

    public GameObject cardPrefab;
    public Transform parentDeck;

    public List<GameObject> cardDeck; // �ʵ忡 ���� ī�� �� ����Ʈ ����
    public List<GameObject> myHand; // �÷��̾� �ڽ��� �и� ���
    public List<GameObject> opponentHand; // ����� �и� ���

    private void Awake()
    {
        instance = this;
        cardDeck = new List<GameObject>(); // �� ����Ʈ �Ҵ�
        myHand = new List<GameObject>(); // �� ����Ʈ �Ҵ�
        opponentHand = new List<GameObject>(); // �� ����Ʈ �Ҵ�
    }

    private void Start()
    {
        CreateDeck(); /// �÷��̾ �غ��� ī�� 12��, ���� �غ��� ī�� 12�� �� ���� �� 48���� ī�带 ���� �ִ´�.
        ShuffleDeck();
    }

    private void Update()
    {
        if(GameManager.instance.myTurn == true)
        {
            DrawCard();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("Count : " + cardDeck.Count);
            Debug.Log("element : " + cardDeck[0].GetComponent<SpriteRenderer>().sprite.name);
        }
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

    public void DrawCard()
    {
        myHand.Add(cardDeck[0]);
    }
}
