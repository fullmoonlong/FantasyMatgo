﻿//내꺼
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

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

    public List<GameObject> myHandScore; // 내 점수 리스트
    public List<GameObject> opponentHandScore; // 상대 점수 리스트

    public List<Vector3> myHandPosition; // 자신 패의 위치
    public List<Vector3> opponentHandPosition; // 상대 패의 위치
    public Vector3[] fieldPosition; // 카드를 내려놓을 필드 위치

    public Vector3[] scoreKingPosition; // 5개
    public Vector3[] scoreEnemyKingPosition; // 5개
    public Vector3[] scoreAnimalPosition; // 9개 멍따
    public Vector3[] scoreEnemyAnimalPosition; // 9개 멍따
    public Vector3[] scoreFlagPosition;
    public Vector3[] scoreEnemyFlagPosition;
    public Vector3[] scoreSoldierPosition;
    public Vector3[] scoreEnemySoldierPosition;

    public GameObject ChociePanel;//두개 중 하나 고를 때 판넬

    int choiceNum; // 고른 카드

    public List<int> emptyIndex;

    public int kingEmptyIndex;
    public int enemyKingEmptyIndex;
    public int animalEmptyIndex;
    public int enemyAnimalEmptyIndex;
    public int flagEmptyIndex;
    public int enemyFlagEmptyIndex;
    public int soldierEmptyIndex;
    public int enemySoldierEmptyIndex;

    private void Awake()
    {
        instance = this;
        cardPrefabs = new List<GameObject>(); // 프리팹 리스트 할당
        cardDeck = new List<GameObject>(); // 덱 리스트 할당

        myHand = new List<GameObject>(); // 패 리스트 할당
        opponentHand = new List<GameObject>(); // 패 리스트 할당
        field = new List<GameObject>(); // 필드 리스트 할당

        myHandScore = new List<GameObject>(); // 점수 리스트 할당
        opponentHandScore = new List<GameObject>(); // 점수 리스트 할당
        
        emptyIndex = new List<int>(3) { 6, 7, 8 };
        
        kingEmptyIndex = 0;
        enemyKingEmptyIndex = 0;
        animalEmptyIndex = 0;
        enemyAnimalEmptyIndex = 0;
        flagEmptyIndex = 0;
        enemyFlagEmptyIndex = 0;
        soldierEmptyIndex = 0;
        enemySoldierEmptyIndex = 0;
    }

    private void Start()
    {
        scoreSoldierPosition = new Vector3[24];
        scoreEnemySoldierPosition = new Vector3[24];

        //각 카드의 위치 설정
        for (int i = 0; i < 8; i++)
        {
            myHandPosition.Add(new Vector3(i - 4, -4.3f, 0));
            opponentHandPosition.Add(new Vector3(i - 4, 4.3f, 0));
        }

        fieldPosition = new[] { new Vector3(-2,2,0),
                                new Vector3(2,2,0),
                                new Vector3(-3,0,0),
                                new Vector3(3,0,0),
                                new Vector3(-2,-2,0),
                                new Vector3(2,-2,0),
                                new Vector3(5, 2, 0),
                                new Vector3(-5, 2, 0),
                                new Vector3(5, -2, 0)};
        //
        scoreKingPosition = new[]
        {
            new Vector3(-8f, -3f, 0f),
            new Vector3(-7.7f, -3f, -0.1f),
            new Vector3(-7.4f, -3f, -0.2f),
            new Vector3(-7.1f, -3f, -0.3f),
            new Vector3(-6.8f, -3f, -0.4f),
        };
        scoreEnemyKingPosition = new[]
        {
            new Vector3(-8f, 3f, 0f),
            new Vector3(-7.7f, 3f, -0.1f),
            new Vector3(-7.4f, 3f, -0.2f),
            new Vector3(-7.1f, 3f, -0.3f),
            new Vector3(-6.8f, 3f, -0.4f),
        };
        //
        scoreAnimalPosition = new[]
        {
            new Vector3(-6.5f, -3f, 0f),
            new Vector3(-6.2f, -3f, -0.1f),
            new Vector3(-5.9f, -3f, -0.2f),
            new Vector3(-5.6f, -3f, -0.3f),
            new Vector3(-5.3f, -3f, -0.4f),
            new Vector3(-5.0f, -3f, -0.5f),
            new Vector3(-4.7f, -3f, -0.6f),
            new Vector3(-4.4f, -3f, -0.7f),
            new Vector3(-4.1f, -3f, -0.8f)
        };
        scoreEnemyAnimalPosition = new[]
        {
            new Vector3(-6.5f, 3f, 0f),
            new Vector3(-6.2f, 3f, -0.1f),
            new Vector3(-5.9f, 3f, -0.2f),
            new Vector3(-5.6f, 3f, -0.3f),
            new Vector3(-5.3f, 3f, -0.4f),
            new Vector3(-5.0f, 3f, -0.5f),
            new Vector3(-4.7f, 3f, -0.6f),
            new Vector3(-4.4f, 3f, -0.7f),
            new Vector3(-4.1f, 3f, -0.8f)
        };
        //
        scoreFlagPosition = new[]
        {
            new Vector3(-3.8f, -3f, 0f),
            new Vector3(-3.5f, -3f, -0.1f),
            new Vector3(-3.2f, -3f, -0.2f),
            new Vector3(-2.9f, -3f, -0.3f),
            new Vector3(-2.6f, -3f, -0.4f),
            new Vector3(-2.3f, -3f, -0.5f),
            new Vector3(-2.0f, -3f, -0.6f),
            new Vector3(-1.7f, -3f, -0.7f),
            new Vector3(-1.4f, -3f, -0.8f),
            new Vector3(-1.1f, -3f, -0.9f),
        };
        scoreEnemyFlagPosition = new[]
        {
            new Vector3(-3.8f, 3f, 0f),
            new Vector3(-3.5f, 3f, -0.1f),
            new Vector3(-3.2f, 3f, -0.2f),
            new Vector3(-2.9f, 3f, -0.3f),
            new Vector3(-2.6f, 3f, -0.4f),
            new Vector3(-2.3f, 3f, -0.5f),
            new Vector3(-2.0f, 3f, -0.6f),
            new Vector3(-1.7f, 3f, -0.7f),
            new Vector3(-1.4f, 3f, -0.8f),
            new Vector3(-1.1f, 3f, -0.9f),
        };
        //
        for (int i = 0; i < 24; i++)
        {
            scoreSoldierPosition[i] = new Vector3(-0.8f + (0.3f * i), -3f, 0f + (0.1f * i));
        }
        for (int i = 0; i < 24; i++)
        {
            scoreEnemySoldierPosition[i] = new Vector3(-0.8f + (0.3f * i), 3f, 0f + (0.1f * i));
        }

        PrefabToCard(); // 프리팹 폴더에 존재하는 카드를 리스트에 담아 생성준비를 한다.
        CreateDeck(); // 플레이어가 준비한 카드 12장, 적이 준비한 카드 12장 을 더해 총 48장의 카드를 덱에 넣는다.
        ShuffleDeck(); //덱을 섞는다
        DrawCard(myHand, 6); // 내손에 6장 씩 뽑는다.
        DrawCard(opponentHand, 6);  // 상대손에 6장 씩 뽑는다.
        DrawCard(field, 6);
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
            CardInitialPosition(cardList, cardList.Count - 1);
            cardDeck.RemoveAt(i);
        }
    }
    public void FlipCard()
    {
        Debug.Log(cardDeck.Count);
        if (cardDeck.Count == 0)
        {
            return;
        }
        field.Add(cardDeck[0]);// 뒤집기
        field[field.Count - 1].transform.position = fieldPosition[emptyIndex[0]]; //마지막 포지션은 비어있는 필드 포지션
        cardDeck.RemoveAt(0);//카드 덱 삭제
    }

    public void CardInitialPosition(List<GameObject> cardList, int index)
    {
        if (cardList == myHand)
        {
            //cardDeck[index].transform.position = myHandPosition[index];
            cardDeck[index].transform.DOMove(myHandPosition[index], 1f).SetEase(Ease.OutQuint);
            cardDeck[index].transform.SetParent(GameObject.Find("MyHand").transform);
        }
        else if (cardList == opponentHand)
        {
            //cardDeck[index].transform.position = opponentHandPosition[index];
            cardDeck[index].transform.DOMove(opponentHandPosition[index], 1f).SetEase(Ease.OutQuint);
            cardDeck[index].transform.SetParent(GameObject.Find("OpponentHand").transform);

        }
        else if (cardList == field)
        {
            cardDeck[index].transform.position = fieldPosition[index];
            //cardDeck[index].transform.DOMove(fieldPosition[index], 0.7f).SetEase(Ease.OutQuint);
            cardDeck[index].transform.SetParent(GameObject.Find("Field").transform);
        }
    }

    public void SetPosition(List<GameObject> list, GameObject obj)
    {
        obj.transform.position =
            new Vector3(list[list.Count].transform.position.x + 0.5f, list[list.Count].transform.position.y, list[list.Count].transform.position.z);
    }

    public void ChoicePanel()
    {
        ChociePanel.SetActive(true);
    }

    public void SelectTwoCard(int num)
    {
        choiceNum = num;
        ChociePanel.SetActive(false);
    }

    public void SetNextPosition(Vector3 position)
    {
        position = new Vector3(position.x + 0.5f, position.y, position.z);
    }

    public void EmptyIndexSort()
    {
        emptyIndex = emptyIndex.Distinct().ToList();
        emptyIndex.Sort();
    }
    public Vector3 ScoreField(GameObject clickedObject, List<GameObject> list)
    {
        Vector3 destination = new Vector3 ( 0, 0, 0 );
        if(list == CardManager.instance.myHandScore)
        {
            switch (clickedObject.GetComponent<CardClick>().type)
            {
                case "광":
                    destination = CardManager.instance.scoreKingPosition[CardManager.instance.kingEmptyIndex];
                    CardManager.instance.kingEmptyIndex++;
                    return destination;
                case "새":
                    destination = CardManager.instance.scoreAnimalPosition[CardManager.instance.animalEmptyIndex];
                    CardManager.instance.animalEmptyIndex++;
                    return destination;
                case "홍단":
                    destination = CardManager.instance.scoreFlagPosition[CardManager.instance.flagEmptyIndex];
                    CardManager.instance.flagEmptyIndex++;
                    return destination;
                case "청단":
                    destination = CardManager.instance.scoreFlagPosition[CardManager.instance.flagEmptyIndex];
                    CardManager.instance.flagEmptyIndex++;
                    return destination;
                case "초단":
                    destination = CardManager.instance.scoreFlagPosition[CardManager.instance.flagEmptyIndex];
                    CardManager.instance.flagEmptyIndex++;
                    return destination;
                default:
                    destination = CardManager.instance.scoreSoldierPosition[CardManager.instance.soldierEmptyIndex];
                    CardManager.instance.soldierEmptyIndex++;
                    return destination;
            }
        }

        if (list == CardManager.instance.opponentHandScore)
        {
            switch (clickedObject.GetComponent<CardClick>().type)
            {
                case "광":
                    destination = CardManager.instance.scoreKingPosition[CardManager.instance.kingEmptyIndex];
                    CardManager.instance.kingEmptyIndex++;
                    return destination;
                case "새":
                    destination = CardManager.instance.scoreAnimalPosition[CardManager.instance.animalEmptyIndex];
                    CardManager.instance.animalEmptyIndex++;
                    return destination;
                case "홍단":
                    destination = CardManager.instance.scoreFlagPosition[CardManager.instance.flagEmptyIndex];
                    CardManager.instance.flagEmptyIndex++;
                    return destination;
                case "청단":
                    destination = CardManager.instance.scoreFlagPosition[CardManager.instance.flagEmptyIndex];
                    CardManager.instance.flagEmptyIndex++;
                    return destination;
                case "초단":
                    destination = CardManager.instance.scoreFlagPosition[CardManager.instance.flagEmptyIndex];
                    CardManager.instance.flagEmptyIndex++;
                    return destination;
                default:
                    destination = CardManager.instance.scoreSoldierPosition[CardManager.instance.soldierEmptyIndex];
                    CardManager.instance.soldierEmptyIndex++;
                    return destination;
            }
        }

        else
            return destination;
    }

    public void ChoiceTwoCard(int num)
    {

    }

    
}
