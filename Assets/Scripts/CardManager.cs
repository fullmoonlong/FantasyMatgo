using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    private int maxCards = 48;
    private int firstHand = 6;

    public static CardManager instance;

    public List<GameObject> cardDeck; // 필드에 놓일 카드 덱 리스트 생성
    public List<GameObject> myHand; // 플레이어 자신의 패를 담당
    public List<GameObject> opponentHand; // 상대의 패를 담당

    private void Awake()
    {
        instance = this;
        cardDeck = new List<GameObject>(); // 덱 리스트 할당
        myHand = new List<GameObject>(); // 패 리스트 할당
        opponentHand = new List<GameObject>(); // 패 리스트 할당
    }

    public void ShuffleDeck()
    {
        int a, b;
        GameObject temp;
        
        for (int i = 0; i < (maxCards / 2); i++)
        {
            a = Random.Range(1, (maxCards / 2) + 1);
            b = Random.Range((maxCards / 2) + 1, (maxCards + 1));
            temp = cardDeck[a];
            cardDeck[a] = cardDeck[b];
            cardDeck[b] = temp;
        }
    }

    public void CreateDeck(GameObject card)
    {
        for (int i = 0; i < maxCards; i++) // 화투덱 숫자
        {
            cardDeck.Add(card);
        }
    }

    public void DrawCards()
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
}
