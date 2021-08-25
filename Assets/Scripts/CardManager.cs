using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class CardManager : MonoBehaviour
{
    #region INSTANCE
    public static CardManager instance;
    #endregion

    private int maxCards = 50; // 전체 카드 개수 기본카드 48  + 보너스카드 2

    public Transform parentDeck; // Instantiate 할 대상(보기 좋게 묶기 용)

    private List<GameObject> cardPrefabs; // 카드의 프리팹 화
    [HideInInspector] public List<GameObject> cardDeck; // 필드에 놓일 카드 덱 리스트 생성
 
    [HideInInspector] public List<GameObject> myHand; // 플레이어 자신의 패 리스트
    [HideInInspector] public List<GameObject> opponentHand; // 상대의 패 리스트
    [HideInInspector] public List<GameObject> field; // 필드 리스트
   
    [HideInInspector] public List<GameObject> myHandScore; // 내 점수 리스트
    [HideInInspector] public List<GameObject> opponentHandScore; // 상대 점수 리스트
 
    [HideInInspector] public List<Vector3> myHandPosition; // 자신 패의 위치
    [HideInInspector] public List<Vector3> opponentHandPosition; // 상대 패의 위치
    [HideInInspector] public Vector3[] fieldPosition; // 카드를 내려놓을 필드 위치

    #region 점수포지션
    [HideInInspector] public Vector3[] scoreKingPosition; // 5개
    [HideInInspector] public Vector3[] scoreEnemyKingPosition; // 5개
    [HideInInspector] public Vector3[] scoreAnimalPosition; // 3개 고도리
    [HideInInspector] public Vector3[] scoreThingPosition; // 6개 뮬건
    [HideInInspector] public Vector3[] scoreEnemyAnimalPosition; // 3개 암흑오브
    [HideInInspector] public Vector3[] scoreEnemyThingPosition; // 6개 파란오브
    [HideInInspector] public Vector3[] scoreRedFlagPosition;
    [HideInInspector] public Vector3[] scoreBlueFlagPosition;
    [HideInInspector] public Vector3[] scoreNormalFlagPosition;
    [HideInInspector] public Vector3[] scoreEnemyRedFlagPosition;
    [HideInInspector] public Vector3[] scoreEnemyBlueFlagPosition;
    [HideInInspector] public Vector3[] scoreEnemyNormalFlagPosition;
    [HideInInspector] public Vector3[] scoreSoldierPosition;
    [HideInInspector] public Vector3[] scoreEnemySoldierPosition;
    #endregion

    #region 점수 필드 위치 지정용 인덱스 (점수로도 사용)
    [HideInInspector] public int kingEmptyIndex;
    [HideInInspector] public int enemyKingEmptyIndex;
    [HideInInspector] public int animalEmptyIndex;
    [HideInInspector] public int enemyAnimalEmptyIndex;
    [HideInInspector] public int redFlagEmptyIndex;
    [HideInInspector] public int blueFlagEmptyIndex;
    [HideInInspector] public int normalFlagEmptyIndex;
    [HideInInspector] public int enemyRedFlagEmptyIndex;
    [HideInInspector] public int enemyBlueFlagEmptyIndex;
    [HideInInspector] public int enemyNormalFlagEmptyIndex;
    [HideInInspector] public int soldierEmptyIndex;
    [HideInInspector] public int enemySoldierEmptyIndex;
    [HideInInspector] public int thingEmptyIndex;
    [HideInInspector] public int enemyThingEmptyIndex;
    #endregion

    [HideInInspector] public bool isFlip; // 플립할게 있는지 없는지 체크
  
    [HideInInspector] public List<int> emptyIndex; //필드에 남은 자리 위치 번호
 
    [HideInInspector] public List<int> sameTagCount; //필드안에 존재하는 십이지신 별 개수
   
    [HideInInspector] public List<GameObject> choiceObj; // 선택 할 카드
  
    [HideInInspector] public List<GameObject> bombObj; // 폭탄으로 들고올 카드 3장

    private List<GameObject> storage; 
  
    [HideInInspector] public int hitCardCount;
 
    [HideInInspector] public GameObject curObj; // 클릭한 게임 오브젝트
    
    [HideInInspector] public GameObject BombCard; // 폭탄을 하고나면 주는 폭탄 카드

    bool isSame; //손에서 필드에 같은 (맞출) 카드가 있는지 체크
    #region SCORE
    // my
    [HideInInspector] public bool isGwang3 = true;
    [HideInInspector] public bool isGwang4 = true;
    [HideInInspector] public bool isGwang5 = true;
    [HideInInspector] public bool isRedFlag = true;
    [HideInInspector] public bool isBlueFlag = true;
    [HideInInspector] public bool isNormalFlag = true;
    [HideInInspector] public bool isAnimal = true;
    [HideInInspector] public bool isPee = true;
    // Oppo
    [HideInInspector] public bool isOpGwang3 = true;
    [HideInInspector] public bool isOpGwang4 = true;
    [HideInInspector] public bool isOpGwang5 = true;
    [HideInInspector] public bool isOpRedFlag = true;
    [HideInInspector] public bool isOpBlueFlag = true;
    [HideInInspector] public bool isOpNormalFlag = true;
    [HideInInspector] public bool isOpAnimal = true;
    [HideInInspector] public bool isOpPee = true;
    #endregion


    private void Awake()
    {
        instance = this;

        //변수 초기화
        cardPrefabs = new List<GameObject>(); // 프리팹 리스트 할당
        cardDeck = new List<GameObject>(); // 덱 리스트 할당

        myHand = new List<GameObject>(); // 패 리스트 할당
        opponentHand = new List<GameObject>(); // 패 리스트 할당
        field = new List<GameObject>(); // 필드 리스트 할당

        myHandScore = new List<GameObject>(); // 점수 리스트 할당
        opponentHandScore = new List<GameObject>(); // 점수 리스트 할당

        emptyIndex = new List<int>(6) { 8, 9, 10, 11, 12 };

        kingEmptyIndex = 0;
        enemyKingEmptyIndex = 0;
        animalEmptyIndex = 0;
        thingEmptyIndex = 0;
        enemyAnimalEmptyIndex = 0;
        enemyThingEmptyIndex = 0;
        redFlagEmptyIndex = 0;
        blueFlagEmptyIndex = 0;
        normalFlagEmptyIndex = 0;
        enemyRedFlagEmptyIndex = 0;
        enemyBlueFlagEmptyIndex = 0;
        enemyNormalFlagEmptyIndex = 0;
        soldierEmptyIndex = 0;
        enemySoldierEmptyIndex = 0;

        sameTagCount = new List<int>(12) { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        storage = new List<GameObject>();

        choiceObj = new List<GameObject>();
        bombObj = new List<GameObject>();

        isFlip = false;

        hitCardCount = 0;

        isSame = false;
        //PlayerPrefs.DeleteAll();
        scoreSoldierPosition = new Vector3[26];
        scoreEnemySoldierPosition = new Vector3[26];

        //각 카드의 위치 설정
        for (int i = 0; i < 10; i++)
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
                                new Vector3(5, -2, 0),
                                new Vector3(7, 2, 0),
                                new Vector3(7, -2, 0),
                                new Vector3(-7, 2, 0),
                                new Vector3(-7, -2, 0)
        };
     
        scoreKingPosition = new[]
        {
            new Vector3(-8f, -3f, 0f),
            new Vector3(-7.7f, -3f, 0.1f),
            new Vector3(-7.4f, -3f, 0.2f),
            new Vector3(-7.1f, -3f, 0.3f),
            new Vector3(-6.8f, -3f, 0.4f),
        };
        scoreEnemyKingPosition = new[]
        {
            new Vector3(-8f, 3f, 0f),
            new Vector3(-7.7f, 3f, 0.1f),
            new Vector3(-7.4f, 3f, 0.2f),
            new Vector3(-7.1f, 3f, 0.3f),
            new Vector3(-6.8f, 3f, 0.4f),
        };

        scoreAnimalPosition = new[]
        {
            new Vector3(-5.9f, -3f, 0f),
            new Vector3(-5.6f, -3f, 0.1f),
            new Vector3(-5.3f, -3f, 0.2f),
        };
        scoreThingPosition = new[]
        {
            new Vector3(-5.0f, -3f, 0.3f),
            new Vector3(-4.7f, -3f, 0.4f),
            new Vector3(-4.4f, -3f, 0.5f),
            new Vector3(-4.1f, -3f, 0.6f),
            new Vector3(-3.8f, -3f, 0.7f),
            new Vector3(-3.5f, -3f, 0.8f)
        };
        scoreEnemyAnimalPosition = new[]
        {
            new Vector3(-5.9f, 3f, 0f),
            new Vector3(-5.6f, 3f, 0.1f),
            new Vector3(-5.3f, 3f, 0.2f),
        };
        scoreEnemyThingPosition = new[]
        {
            new Vector3(-5.0f, 3f, 0.3f),
            new Vector3(-4.7f, 3f, 0.4f),
            new Vector3(-4.4f, 3f, 0.5f),
            new Vector3(-4.1f, 3f, 0.6f),
            new Vector3(-3.8f, 3f, 0.7f),
            new Vector3(-3.5f, 3f, 0.8f)
        };

        scoreRedFlagPosition = new[]
        {
            new Vector3(-2.6f, -3f, 0f),
            new Vector3(-2.3f, -3f, 0.1f),
            new Vector3(-2.0f, -3f, 0.2f),
        };
        scoreBlueFlagPosition = new[]
        {
            new Vector3(-1.1f, -3f, 0.3f),
            new Vector3(-0.8f, -3f, 0.4f),
            new Vector3(-0.5f, -3f, 0.5f),
        };

        scoreNormalFlagPosition = new[]
        {
            new Vector3(0.4f, -3f, 0.6f),
            new Vector3(0.7f, -3f, 0.7f),
            new Vector3(1.0f, -3f, 0.8f),
            new Vector3(1.3f, -3f, 0.9f),
        };

        scoreEnemyRedFlagPosition = new[]
        {
            new Vector3(-2.6f, 3f, 0f),
            new Vector3(-2.3f, 3f, 0.1f),
            new Vector3(-2.0f, 3f, 0.2f),
        };

        scoreEnemyBlueFlagPosition = new[]
        {
            new Vector3(-1.1f, 3f, 0.3f),
            new Vector3(-0.8f, 3f, 0.4f),
            new Vector3(-0.5f, 3f, 0.5f),
        };

        scoreEnemyNormalFlagPosition = new[]
        {
            new Vector3(0.4f, 3f, 0.6f),
            new Vector3(0.7f, 3f, 0.7f),
            new Vector3(1.0f, 3f, 0.8f),
            new Vector3(1.3f, 3f, 0.9f),
        };

        for (int i = 0; i < 24; i++)
        {
            scoreSoldierPosition[i] = new Vector3(2.2f + (0.3f * i), -3f, 0f + (0.1f * i));
        }
        for (int i = 0; i < 24; i++)
        {
            scoreEnemySoldierPosition[i] = new Vector3(2.2f + (0.3f * i), 3f, 0f + (0.1f * i));
        }

    }

    private void Start()
    {
        PrefabToCard(); // 프리팹 폴더에 존재하는 카드를 리스트에 담아 생성준비를 한다.
        CreateDeck(); // 플레이어가 준비한 카드 12장, 적이 준비한 카드 12장 을 더해 총 48장의 카드를 덱에 넣는다.
        ShuffleDeck(); //덱을 섞는다
        //DrawBonusCard();
        DrawCard(myHand, 6); // 내손에 6장 씩 뽑는다.

        DrawCard(opponentHand, 6);  // 상대손에 6장 씩 뽑는다.

        DrawCard(field, 8);

        FieldSameCard(); // 필드에 같은 십이지신이 있는지 체크 있다면 겹치기
        CheckSameCard(myHand); // 내손 카드 sort
        CheckSameCard(opponentHand); // 상대 카드 sort

        Invoke("FieldBonusCard", 0.5f); // 0.5초 후에 필드에 보너스 카드가 있는지 체크

    }

    public int GetCardTagNum(GameObject obj) //가지고 있는 태그(십이지신)를 0~11로 표시
    {
        switch (obj.tag)
        {
            case "Rat":
                return 0;

            case "Cow":
                return 1;

            case "Tiger":
                return 2;

            case "Rabbit":
                return 3;

            case "Dragon":
                return 4;

            case "Snake":
                return 5;

            case "Horse":
                return 6;

            case "Sheep":
                return 7;

            case "Monkey":
                return 8;

            case "Cock":
                return 9;

            case "Dog":
                return 10;

            case "Pig":
                return 11;

            default:
                return 13;

        }
    }

    void FieldSameCard() // 필드에 같은 태그를 가진 카드들이 존재하면 겹쳐줌
    {
        //<필드에서 가지고 있는 모든 태그(십이지신) 종류 storage에 넣기>
        for (int j = 0; j < 12; j++) // 태그 12개 다 돌리기
        {
            for (int i = 0; i < field.Count; i++) // 필드 돌리기
            {
                if (j == GetCardTagNum(field[i])) // 필드 i의 태그가 j와 같다면
                {
                    sameTagCount[j]++; // 태그 값 더하기
                    if (sameTagCount[j] == 1) // 필드 태그가 하나일 때만
                    {
                        storage.Add(field[i]);
                    }
                }
            }
        }

        int count = 0; // 겹칠 카드 개수

        //<같은 태그가 2개 이상인 카드 찾기>
        for (int i = 0; i < storage.Count; i++)
        {
            for (int j = 0; j < field.Count; j++)
            {
                if (storage[i].CompareTag(field[j].tag) && storage[i].transform.position != field[j].transform.position)// 태그는 같고 포지션은 다를 때 -> 같은 태그가 2개 이상이라는 뜻
                {
                    count++;
                    //카드를 겹쳐줌
                    field[j].transform.position = new Vector3(storage[i].transform.position.x + 0.5f * count, storage[i].transform.position.y, storage[i].transform.position.z - 0.1f * count);

                    //만약 카드 카운트가 0 이상이면 -> 같은 카드의 자리가 비기 때문에
                    if (count > 0)
                    {
                        emptyIndex.Add(j);
                    }
                }
            }
            count = 0;
        }

        EmptyIndexSort(); // 빈 자리 정렬
    }
    public void DrawBonusCard()
    {

        for (int i = 0; i < cardDeck.Count; i++)
        {
            if (cardDeck[i].tag == "Bonus")
            {
                print("Bonus");
                field.Add(cardDeck[i]);
                cardDeck.RemoveAt(i);
            }
        }

    }
    void FieldBonusCard() //필드에 보너스 카드가 있을 때
    {
        Sequence mysequence = DOTween.Sequence();

        int i = 0;

        List<int> index = new List<int>();

        while (i < field.Count)
        {
            if (field[i].tag == "Bonus") // 필드에 보너스 카드가 있으면
            {
                index.Add(i); //인덱스에 몇번째 위치인지 넣어줌
                break;
            }

            i++;
        }

        if(index.Count > 0)
        {
            CardClick.instance.EmptyFieldPosition(field[index[0]]); //보너스 카드의 필드 포지션을 지워줌
            mysequence.Append(field[index[0]].transform.DOMove(scoreSoldierPosition[soldierEmptyIndex], 0.5f).SetEase(Ease.OutQuint)); //첫번째 사람의 피 점수 포지션으로 보너스 카드를 옮김
            soldierEmptyIndex++; // 피위치 ++
            myHandScore.Add(field[index[0]]); // 점수에 더해주기 
            field.Remove(field[index[0]]); // 필드에서 지우기

            FlipCard(); // 카드를 뒤집음

            FlipSameCard();

            FieldBonusCard(); // 플립한 카드가 보너스 카드일 수도 있어서 한번 더 체크
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
        for (int i = 0; i < maxCards; i++)
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

    public bool ScoreSameTag(GameObject obj) //점수판에 오브젝트와 같은 태그의 카드가 있는지 확인
    {
        for (int i = 0; i < myHandScore.Count; i++)
        {
            if (myHandScore[i].tag == obj.tag)
            {
                return true;
            }
        }

        for (int i = 0; i < opponentHandScore.Count; i++)
        {
            if (opponentHandScore[i].tag == obj.tag)
            {
                return true;
            }
        }

        return false;
    }
    public void CheckSameCard(List<GameObject> cardList) // cardlist와 필드에 같은 카드가 있는지 체크
    {
        for (int i = 0; i < cardList.Count; i++)
        {
            for (int j = 0; j < field.Count; j++)
            {
                if (cardList[i].tag == field[j].tag)
                {
                    isSame = true;
                }
            }

            if (GetCardTagNum(cardList[i]) != 13) // 보너스 카드나, 폭탄 카드가 아니라면
            {
                cardList[i].transform.GetChild(0).gameObject.SetActive(isSame);
                if (isSame)
                {
                    if (cardList == myHand)
                    {
                        if (ScoreSameTag(cardList[i]))
                        {
                            cardList[i].transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.black;
                        }
                        else
                        {
                            cardList[i].transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
                        }
                    }
                    if (cardList == opponentHand)
                    {
                        if (ScoreSameTag(cardList[i]))
                        {
                            cardList[i].transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.black;
                        }
                        else
                        {
                            cardList[i].transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
                        }
                    }
                }
                isSame = false;
            }

        }
    }

    public void DeleteOutline(GameObject obj) // 게임오브젝트의 테두리 안보이게 만듬
    {
        obj.transform.GetChild(0).gameObject.SetActive(false);
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
            cardDeck.RemoveAt(i);
        }

        ArrangeHandPosition(cardList);
    }
    public void DrawBombCard(List<GameObject> cardList)
    {
        for (int i = 0; i < 2; i++)
        {
            cardList.Add(Instantiate(BombCard, parentDeck));
        }

        ArrangeHandPosition(cardList);
    }
    public void FlipSameCard()
    {
        Sequence mysequence = DOTween.Sequence();

        bool SameTag = false;

        for (int j = 0; j < field.Count - 1; j++)
        {
            if (field[j].tag == field[field.Count - 1].tag) //만약 뒤집은 카드랑 필드에 같은 카드가 있으면 옆에 겹쳐줌
            {
                field[field.Count - 1].transform.position = new Vector3(field[j].transform.position.x + 0.5f, field[j].transform.position.y, field[j].transform.position.z - 0.1f);
                emptyIndex.Add(emptyIndex[0]); //자리 비워줌
                EmptyIndexSort(); // 정렬
                SameTag = true;
            }
        }

        if(!SameTag)
        {
            mysequence.Append(field[field.Count - 1].transform.DOMove(fieldPosition[emptyIndex[0]], 0.5f)); //마지막 포지션은 비어있는 필드 포지션
        }
    }
    public void FlipCard()
    {
        if (cardDeck.Count == 0) // 카드덱에 카드가 없다면
        {
            isFlip = false;
            return;
        }

        isFlip = true;
        field.Add(cardDeck[0]);// 뒤집기

        if (GetCardTagNum(field[field.Count - 1]) < 13)
        {
            sameTagCount[GetCardTagNum(field[field.Count - 1])]++;
        }

        cardDeck.RemoveAt(0);//카드 덱 삭제
    }
    public void ArrangeHandPosition(List<GameObject> cardList)
    {
        if (cardList == myHand)
        {
            cardList = cardList.OrderBy(x => GetCardTagNum(x)).ToList();
            for (int i = 0; i < cardList.Count; i++)
            {
                cardList[i].transform.DOMove(myHandPosition[i], 1f).SetEase(Ease.OutQuint).OnComplete(()=> CardClick.instance.OnOffPanel(false));
            }
            CheckSameCard(cardList);
            CheckSameCard(opponentHand);
        }
        else if (cardList == opponentHand)
        {
            cardList = cardList.OrderBy(x => GetCardTagNum(x)).ToList();
            for (int i = 0; i < cardList.Count; i++)
            {
                cardList[i].transform.DOMove(opponentHandPosition[i], 1f).SetEase(Ease.OutQuint).OnComplete(() => CardClick.instance.OnOffPanel(false));
            }
            CheckSameCard(cardList);
            CheckSameCard(myHand);
        }
        else if (cardList == field)
        {
            for (int i = 0; i < cardList.Count; i++)
            {
                cardList[i].transform.position = fieldPosition[i];
            }
        }
    }

    public void EmptyIndexSort()
    {
        emptyIndex = emptyIndex.Distinct().ToList();
        emptyIndex.Sort();
    }
    public Vector3 ScoreField(GameObject clickedObject, List<GameObject> list)
    {
        Vector3 destination = new Vector3(0, 0, 0);
        if (list == myHandScore)
        {
            switch (clickedObject.GetComponent<CardClick>().type)
            {
                case "큐브":
                    destination = scoreKingPosition[kingEmptyIndex];
                    kingEmptyIndex++;
                    return destination;

                case "암흑 오브":
                    destination = scoreAnimalPosition[animalEmptyIndex];
                    animalEmptyIndex++;
                    return destination;

                case "파란 오브":
                    destination = scoreThingPosition[thingEmptyIndex];
                    thingEmptyIndex++;
                    return destination;

                case "붉은 크리스탈":
                    destination = scoreRedFlagPosition[redFlagEmptyIndex];
                    redFlagEmptyIndex++;
                    return destination;

                case "파란 크리스탈":
                    destination = scoreBlueFlagPosition[blueFlagEmptyIndex];
                    blueFlagEmptyIndex++;
                    return destination;

                case "초록 크리스탈":
                    destination = scoreNormalFlagPosition[normalFlagEmptyIndex];
                    normalFlagEmptyIndex++;
                    return destination;

                default:
                    destination = scoreSoldierPosition[soldierEmptyIndex];
                    soldierEmptyIndex++;
                    return destination;
            }
        }

        if (list == opponentHandScore)
        {
            switch (clickedObject.GetComponent<CardClick>().type)
            {
                case "큐브":
                    destination = scoreEnemyKingPosition[enemyKingEmptyIndex];
                    enemyKingEmptyIndex++;
                    return destination;

                case "암흑 오브":
                    destination = scoreEnemyAnimalPosition[enemyAnimalEmptyIndex];
                    enemyAnimalEmptyIndex++;
                    return destination;

                case "파란 오브":
                    destination = scoreEnemyThingPosition[enemyThingEmptyIndex];
                    enemyThingEmptyIndex++;
                    return destination;

                case "붉은 크리스탈":
                    destination = scoreEnemyRedFlagPosition[enemyRedFlagEmptyIndex];
                    enemyRedFlagEmptyIndex++;
                    return destination;

                case "파란 크리스탈":
                    destination = scoreEnemyBlueFlagPosition[enemyBlueFlagEmptyIndex];
                    enemyBlueFlagEmptyIndex++;
                    return destination;

                case "초록 크리스탈":
                    destination = scoreEnemyNormalFlagPosition[enemyNormalFlagEmptyIndex];
                    enemyNormalFlagEmptyIndex++;
                    return destination;

                default:
                    destination = scoreEnemySoldierPosition[enemySoldierEmptyIndex];
                    enemySoldierEmptyIndex++;
                    return destination;
            }
        }

        else
            return destination;
    }

}
