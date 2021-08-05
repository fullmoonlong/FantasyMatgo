using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class CardManager : MonoBehaviour
{
    #region INSTANCE
    public static CardManager instance;
    #endregion

    private int maxCards = 50;

    public Transform parentDeck; // Instantiate 할 대상(보기 좋게 묶기 용)

    private List<GameObject> cardPrefabs; // 카드의 프리팹 화
    public List<GameObject> cardDeck; // 필드에 놓일 카드 덱 리스트 생성

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
    public Vector3[] scoreRedFlagPosition;
    public Vector3[] scoreBlueFlagPosition;
    public Vector3[] scoreNormalFlagPosition;
    public Vector3[] scoreEnemyRedFlagPosition;
    public Vector3[] scoreEnemyBlueFlagPosition;
    public Vector3[] scoreEnemyNormalFlagPosition;
    public Vector3[] scoreSoldierPosition;
    public Vector3[] scoreEnemySoldierPosition;

    public GameObject ChociePanel;//두개 중 하나 고를 때 판넬

    public int kingEmptyIndex;
    public int enemyKingEmptyIndex;
    public int animalEmptyIndex;
    public int enemyAnimalEmptyIndex;
    public int redFlagEmptyIndex;
    public int blueFlagEmptyIndex;
    public int normalFlagEmptyIndex;
    public int enemyRedFlagEmptyIndex;
    public int enemyBlueFlagEmptyIndex;
    public int enemyNormalFlagEmptyIndex;
    public int soldierEmptyIndex;
    public int enemySoldierEmptyIndex;

    public bool isFlip;

    public List<int> emptyIndex;
    public List<int> sameTagCount;
    public List<GameObject> ChoiceObj;
    public List<GameObject> BombObj;
    private List<GameObject> storage;

    public int myCardCount;
    public GameObject curObj; // 클릭한 게임 오브젝트

    public GameObject BombCard;

    #region SCORE
    // my
    [HideInInspector] public int gwangCount;
    [HideInInspector] public int redFlagCount;
    [HideInInspector] public int blueFlagCount;
    [HideInInspector] public int normalFlagCount;
    [HideInInspector] public int animalCount;
    [HideInInspector] public int peeCount;
    // op
    [HideInInspector] public int enemyGwangCount;
    [HideInInspector] public int enemyRedFlagCount;
    [HideInInspector] public int enemyBlueFlagCount;
    [HideInInspector] public int enemyNormalFlagCount;
    [HideInInspector] public int enemyAnimalCount;
    [HideInInspector] public int enemyPeeCount;
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
        enemyAnimalEmptyIndex = 0;
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

        ChoiceObj = new List<GameObject>();
        BombObj = new List<GameObject>();

        isFlip = false;

        myCardCount = 0;
    }

    private void Start()
    {
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
            new Vector3(-5.9f, -3f, 0f),
            new Vector3(-5.6f, -3f, -0.1f),
            new Vector3(-5.3f, -3f, -0.2f),
            new Vector3(-5.0f, -3f, -0.3f),
            new Vector3(-4.7f, -3f, -0.4f),
            new Vector3(-4.4f, -3f, -0.5f),
            new Vector3(-4.1f, -3f, -0.6f),
            new Vector3(-3.8f, -3f, -0.7f),
            new Vector3(-3.5f, -3f, -0.8f)
        };
        scoreEnemyAnimalPosition = new[]
        {
            new Vector3(-5.9f, 3f, 0f),
            new Vector3(-5.6f, 3f, -0.1f),
            new Vector3(-5.3f, 3f, -0.2f),
            new Vector3(-5.0f, 3f, -0.3f),
            new Vector3(-4.7f, 3f, -0.4f),
            new Vector3(-4.4f, 3f, -0.5f),
            new Vector3(-4.1f, 3f, -0.6f),
            new Vector3(-3.8f, 3f, -0.7f),
            new Vector3(-3.5f, 3f, -0.8f)
        };
        //
        scoreRedFlagPosition = new[]
        {
            new Vector3(-2.6f, -3f, 0f),
            new Vector3(-2.3f, -3f, -0.1f),
            new Vector3(-2.0f, -3f, -0.2f),
        };
        scoreBlueFlagPosition = new[]
        {
            new Vector3(-1.1f, -3f, -0.3f),
            new Vector3(-0.8f, -3f, -0.4f),
            new Vector3(-0.5f, -3f, -0.5f),
        };

        scoreNormalFlagPosition = new[]
        {
            new Vector3(0.4f, -3f, -0.6f),
            new Vector3(0.7f, -3f, -0.7f),
            new Vector3(1.0f, -3f, -0.8f),
            new Vector3(1.3f, -3f, -0.9f),
        };

        scoreEnemyRedFlagPosition = new[]
        {
            new Vector3(-2.6f, 3f, 0f),
            new Vector3(-2.3f, 3f, -0.1f),
            new Vector3(-2.0f, 3f, -0.2f),
        };

        scoreEnemyBlueFlagPosition = new[]
        {
            new Vector3(-1.1f, 3f, -0.3f),
            new Vector3(-0.8f, 3f, -0.4f),
            new Vector3(-0.5f, 3f, -0.5f),
        };

        scoreEnemyNormalFlagPosition = new[]
        {
            new Vector3(0.4f, 3f, -0.6f),
            new Vector3(0.7f, 3f, -0.7f),
            new Vector3(1.0f, 3f, -0.8f),
            new Vector3(1.3f, 3f, -0.9f),
        };

        //
        for (int i = 0; i < 24; i++)
        {
            scoreSoldierPosition[i] = new Vector3(2.2f + (0.3f * i), -3f, 0f + (0.1f * i));
        }
        for (int i = 0; i < 24; i++)
        {
            scoreEnemySoldierPosition[i] = new Vector3(2.2f + (0.3f * i), 3f, 0f + (0.1f * i));
        }

        PrefabToCard(); // 프리팹 폴더에 존재하는 카드를 리스트에 담아 생성준비를 한다.
        CreateDeck(); // 플레이어가 준비한 카드 12장, 적이 준비한 카드 12장 을 더해 총 48장의 카드를 덱에 넣는다.
        ShuffleDeck(); //덱을 섞는다
        DrawCard(myHand, 6); // 내손에 6장 씩 뽑는다.
        DrawCard(opponentHand, 6);  // 상대손에 6장 씩 뽑는다.
        DrawCard(field, 8);

        //field.Add(GameObject.Find("48(Clone)"));
        //CardInitialPosition(field, field.Count - 1);
        //field.Add(GameObject.Find("49(Clone)"));
        //CardInitialPosition(field, field.Count - 1);
        FieldSameCard();
        Invoke("FieldBonusCard", 0.5f);
       
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Pee : " + isPee);
            Debug.Log("Gwang : " + isGwang3);
            Debug.Log("Chodan : " + isNormalFlag);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log("OpPee" + isOpPee);
            Debug.Log("OpGwang : " + isOpGwang3);
            Debug.Log("OpChodan : " + isOpNormalFlag);
        }
    }

    public int GetCardTagNum(GameObject obj)
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

    void FieldSameCard()
    {
        //print("fieldCount : " + field.Count);
        for (int j = 0; j < 12; j++) // 태그 12개 다 돌리기
        {
            for (int i = 0; i < field.Count; i++) // 필드 돌리기
            {
                if (j == GetCardTagNum(field[i])) // 필드 i의 태그가 j와 같다면
                {
                    sameTagCount[j]++; // 태그 값 더하기
                    if (sameTagCount[j] == 1) // 필드 태그가 하나밖에 없다면
                    {
                        storage.Add(field[i]);
                    }
                }

            }
            //print("Tag : " + j + " " + sameTagCount[j]);
        }

        int count = 0;

        for (int i = 0; i < storage.Count; i++)
        {
            for (int j = 0; j < field.Count; j++)
            {
                if (storage[i].CompareTag(field[j].tag) && storage[i].transform.position != field[j].transform.position)// 태그는 같고 포지션은 다를 때
                {
                    count++;

                    //CardClick.instance.EmptyFieldPosition(field[j]);
                    //같은 카드 다음 포지션은 같은 태그의 갯수 * 0.5 만큼 x축을 더해준다.
                    field[j].transform.position = new Vector3(storage[i].transform.position.x + 0.5f * count, storage[i].transform.position.y, storage[i].transform.position.z - 0.1f * count);

                    //만약 카드 카운트가 0 이상이면 -> 같은 카드의 자리가 비기 때문에
                    if (count > 0)
                    {
                        emptyIndex.Add(j);
                    }
                }
                //엠티 카운트가 없으면 어저지,,
            }
            count = 0;
        }
        EmptyIndexSort();
        for(int i=0;i<emptyIndex.Count;i++)
        {
            print("index i : " + emptyIndex[i]);
        }
    }

    void FieldBonusCard()
    {
        Sequence mysequence = DOTween.Sequence();
        int i = 0;
        List<int> index = new List<int>();

        while (i < field.Count)
        {
            if (field[i].tag == "Bonus")
            {
                index.Add(i);
            }

            i++;
        }

        int eIndex = emptyIndex[0];

        switch (index.Count)
        {
            
            case 1:
                {
                    //print(index[0]);
                    CardClick.instance.EmptyFieldPosition(field[index[0]]);
                    mysequence.Append(field[index[0]].transform.DOMove(scoreSoldierPosition[soldierEmptyIndex], 0.5f).SetEase(Ease.OutQuint));
                    soldierEmptyIndex++;
                    myHandScore.Add(field[index[0]]); // 점수에 더해주기 
                    field.Remove(field[index[0]]); // 필드에서 지우기

                    FlipCard();

                    //못맞췄을 때
                    emptyIndex.RemoveAt(0);

                    EmptyIndexSort();//빈곳 인덱스 오름차순 정렬

                    for (int j = 0; j < field.Count - 1; j++)
                    {
                        if (field[j].tag == field[field.Count - 1].tag)
                        {
                            field[field.Count - 1].transform.position = new Vector3(field[j].transform.position.x + 0.5f, field[j].transform.position.y, field[j].transform.position.z - 0.1f);
                            emptyIndex.Add(eIndex);
                            EmptyIndexSort();
                        }
                        
                    }

                    for (int j = 0; j < emptyIndex.Count; j++)
                    {
                        print("최종 카드 인덱스 " + j + " : " + emptyIndex[j]);
                    }

                    break;
                }
         
            case 2:
                {
                    CardClick.instance.EmptyFieldPosition(field[index[0]]);
                    mysequence.Append(field[index[0]].transform.DOMove(scoreSoldierPosition[soldierEmptyIndex], 0.5f).SetEase(Ease.OutQuint));// 둘다 옮기기
                    soldierEmptyIndex++;
                    CardClick.instance.EmptyFieldPosition(field[index[1]]);
                    mysequence.Append(field[index[1]].transform.DOMove(scoreSoldierPosition[soldierEmptyIndex], 0.5f).SetEase(Ease.OutQuint));
                    soldierEmptyIndex++;
                    myHandScore.Add(field[index[0]]); // 점수에 더해주기 W
                    myHandScore.Add(field[index[1]]);
                   
                   
                    field.Remove(field[index[0]]); // 필드에서 지우기
                    field.Remove(field[index[1] - 1]);

                    FlipCard();
                    //못맞췄을 때
                    emptyIndex.RemoveAt(0);

                    EmptyIndexSort();//빈곳 인덱스 오름차순 정렬

                    //맞췄을 때

                    for (int j = 0; j < field.Count - 1; j++)
                    {
                        if (field[j].tag == field[field.Count - 1].tag)
                        {
                            print("같은게있음");
                            field[field.Count - 1].transform.position = new Vector3(field[j].transform.position.x + 0.5f, field[j].transform.position.y, field[j].transform.position.z - 0.1f);
                            emptyIndex.Add(eIndex);
                            EmptyIndexSort();
                        }
                    }

                    FlipCard();

                    eIndex = emptyIndex[0];

                    emptyIndex.RemoveAt(0);

                    EmptyIndexSort();//빈곳 인덱스 오름차순 정렬

                    for (int j = 0; j < field.Count - 1; j++)
                    {
                        if (field[j].tag == field[field.Count - 1].tag)
                        {
                            print("같은게있음");
                            field[field.Count - 1].transform.position = new Vector3(field[j].transform.position.x + 0.5f, field[j].transform.position.y, field[j].transform.position.z - 0.1f);
                            emptyIndex.Add(eIndex);
                            EmptyIndexSort();
                        }
                    }
                    break;
                }
            default:
                break;

        }

        GameManager.instance.oneTime = false;

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

    public void DrawBombCard(List<GameObject> cardList)
    {
        for (int i = 0; i < 2; i++)
        {
            cardList.Add(Instantiate(BombCard, parentDeck));
            CardInitialPosition(cardList, cardList.Count - 1);
        }

    }
    public void FlipCard()
    {
        if (cardDeck.Count == 0)
        {
            isFlip = false;
            return;
        }
        isFlip = true;
        field.Add(cardDeck[0]);// 뒤집기
        field[field.Count - 1].transform.position = fieldPosition[emptyIndex[0]]; //마지막 포지션은 비어있는 필드 포지션
        if (GetCardTagNum(field[field.Count - 1]) < 13)
        {
            sameTagCount[GetCardTagNum(field[field.Count - 1])]++;
        }
        cardDeck.RemoveAt(0);//카드 덱 삭제
    }

    public void CardInitialPosition(List<GameObject> cardList, int index)
    {
        if (cardList == myHand)
        {
            //cardDeck[index].transform.position = myHandPosition[index];
            //cardList[index].transform.position = Vector3.Lerp(cardList[index].transform.position, myHandPosition[index], 3f);
            cardList[index].transform.DOMove(myHandPosition[index], 1f).SetEase(Ease.OutQuint);
            //cardList[index].transform.SetParent(GameObject.Find("MyHand").transform);
        }
        else if (cardList == opponentHand)
        {
            //cardDeck[index].transform.position = opponentHandPosition[index];
            cardList[index].transform.DOMove(opponentHandPosition[index], 1f).SetEase(Ease.OutQuint);
            //cardList[index].transform.SetParent(GameObject.Find("OpponentHand").transform);
        }
        else if (cardList == field)
        {
            cardList[index].transform.position = fieldPosition[index];
            //cardDeck[index].transform.DOMove(fieldPosition[index], 0.7f).SetEase(Ease.OutQuint);
            //cardList[index].transform.SetParent(GameObject.Find("Field").transform);
        }

    }

    public void ResetPosition(List<GameObject> cardList)
    {
        if (cardList == myHand)
        {
            for (int i = 0; i < myHand.Count; i++)
            {
                myHand[i].transform.position = myHandPosition[i];
            }
        }
        else if (cardList == opponentHand)
        {
            for (int i = 0; i < opponentHand.Count; i++)
            {
                opponentHand[i].transform.position = opponentHandPosition[i];
            }
        }
    }
    public void SetPosition(List<GameObject> list, GameObject obj)
    {
        obj.transform.position =
            new Vector3(list[list.Count].transform.position.x + 0.5f, list[list.Count].transform.position.y, list[list.Count].transform.position.z);
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
        Vector3 destination = new Vector3(0, 0, 0);
        if (list == myHandScore)
        {
            switch (clickedObject.GetComponent<CardClick>().type)
            {
                case "광":
                    destination = scoreKingPosition[kingEmptyIndex];
                    kingEmptyIndex++;
                    return destination;
                case "새":
                    destination = scoreAnimalPosition[animalEmptyIndex];
                    animalEmptyIndex++;
                    return destination;
                case "홍단":
                    destination = scoreRedFlagPosition[redFlagEmptyIndex];
                    redFlagEmptyIndex++;
                    return destination;
                case "청단":
                    destination = scoreBlueFlagPosition[blueFlagEmptyIndex];
                    blueFlagEmptyIndex++;
                    return destination;
                case "초단":
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
                case "광":
                    destination = scoreEnemyKingPosition[enemyKingEmptyIndex];
                    enemyKingEmptyIndex++;
                    return destination;
                case "새":
                    destination = scoreEnemyAnimalPosition[enemyAnimalEmptyIndex];
                    enemyAnimalEmptyIndex++;
                    return destination;
                case "홍단":
                    destination = scoreEnemyRedFlagPosition[enemyRedFlagEmptyIndex];
                    enemyRedFlagEmptyIndex++;
                    return destination;
                case "청단":
                    destination = scoreEnemyBlueFlagPosition[enemyBlueFlagEmptyIndex];
                    enemyBlueFlagEmptyIndex++;
                    return destination;
                case "초단":
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

    public void ArrangeHand(List<GameObject> hand)
    {
        for (int i = 0; i < hand.Count; i++)
        {
            if (hand == myHand)
            {
                hand[i].transform.position = myHandPosition[i];
            }

            if (hand == opponentHand)
            {
                hand[i].transform.position = opponentHandPosition[i];
            }
        }
    }
}
