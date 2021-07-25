﻿using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CardClick : MonoBehaviour
{
    public static int myScore = 0;
    public static int opponentScore = 0;
    public string type = "";

    public GameObject ChoicePanel;

    GameObject hittedCard;
    GameObject[] BombObj;
    int myCardCount;

    private int gwangCount;
    private int enemyGwangCount;
    private int redFlagCount;
    private int enemyRedFlagCount;
    private int blueFlagCount;
    private int enemyBlueFlagCount;
    private int normalFlagCount;
    private int enemyNormalFlagCount;
    private int animalCount;
    private int enemyAnimalCount;
    private int peeCount;
    private int enemyPeeCount;
    //private bool initialTurn = true;

    private void Start()
    {
        switch (name)
        {
            //광
            case "0(Clone)":
            case "8(Clone)":
            case "28(Clone)":
            case "40(Clone)":
            case "44(Clone)":
                type = "광";
                break;

            case "1(Clone)":
            case "5(Clone)":
            case "9(Clone)":
                type = "홍단";
                break;

            case "21(Clone)":
            case "33(Clone)":
            case "37(Clone)":
                type = "청단";
                break;

            case "13(Clone)":
            case "17(Clone)":
            case "25(Clone)":
            case "46(Clone)":
                type = "초단";
                break;

            case "4(Clone)":
            case "12(Clone)":
            case "29(Clone)":
                type = "새";
                break;

            default:
                type = "피";
                break;
        }

        myCardCount = 0;
        BombObj = new GameObject[3];
    }

    private void Update()
    {
        if (gwangCount >= 3)
        {

        }
    }

    public void CalculateScore(List<GameObject> scoreList)
    {
        foreach (var item in scoreList)
        {
            switch (item.GetComponent<CardClick>().type)
            {
                case "광":
                    if (scoreList == CardManager.instance.myHandScore)
                    {
                        gwangCount++;
                    }
                    else
                    {
                        enemyGwangCount++;
                    }
                    break;
                case "홍단":
                    if (scoreList == CardManager.instance.myHandScore)
                    {
                        redFlagCount++;
                    }
                    else
                    {
                        enemyRedFlagCount++;
                    }
                    break;
                case "청단":
                    if (scoreList == CardManager.instance.myHandScore)
                    {
                        blueFlagCount++;
                    }
                    else
                    {
                        enemyBlueFlagCount++;
                    }
                    break;
                case "초단":
                    if (scoreList == CardManager.instance.myHandScore)
                    {
                        normalFlagCount++;
                    }
                    else
                    {
                        enemyNormalFlagCount++;
                    }
                    break;
                case "새":
                    if (scoreList == CardManager.instance.myHandScore)
                    {
                        animalCount++;
                    }
                    else
                    {
                        enemyAnimalCount++;
                    }
                    break;
                case "피":
                    if (scoreList == CardManager.instance.myHandScore)
                    {
                        peeCount++;
                    }
                    else
                    {
                        enemyPeeCount++;
                    }
                    break;
                default:
                    break;
            }
        }
    }

    public void CardCountToScore()
    {
        if (gwangCount == 3)
        {
            myScore = 3;
        }
        else if (gwangCount == 4)
        {
            myScore = 4;
        }
        else if (gwangCount == 5)
        {
            myScore = 5;
        }
    }

    public void OnMouseUp()
    {
        //int count = 0;
        print(CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(gameObject)]);

        print(CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(gameObject)]);


        if (GameManager.instance.isMyTurn == true)//만약 플레이어 턴이면
        {
            WhoTurn(CardManager.instance.myHand, CardManager.instance.myHandScore, false);
            //if (initialTurn == true)
            //{
            //    count++;
            //    print(count);

            //    CardManager.instance.DrawCard(CardManager.instance.opponentHand, 1);
            //    //CardManager.instance.ArrangeHand(CardManager.instance.opponentHand);
            //    initialTurn = false;
            //}
            CalculateScore(CardManager.instance.myHandScore);
        }

        else//만약 상대 턴이면
        {
            WhoTurn(CardManager.instance.opponentHand, CardManager.instance.opponentHandScore, true);
            CalculateScore(CardManager.instance.opponentHandScore);
        }
    }



    void WhoTurn(List<GameObject> hand, List<GameObject> handscore, bool isPlayer)
    {

        //카드 뽑는 애니메이션
        //print(CardManager.instance.sameTagCount[GetCardTagNum(gameObject)]);
        if (hand.Contains(gameObject)) // 내손에 이 게임오브젝트가 있을 때
        {
            print("내 손에 있음");

            CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(gameObject)]++; // 내가 낸 카드 ++ 해줌
            myCardCount = CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(gameObject)];
            CardManager.instance.field.Add(gameObject); //내손에 있는 카드 필드에 넣기
            hand.Remove(gameObject);//내손에서 지우기

            //print(myCardCount);
            switch (myCardCount) // 2, 3, 4개 맞췄을 시 다름
            {
                //카드가 안맞았을 때
                case 1:
                    {
                        print("카드 안맞음");
                        /*카드의 위치는 빈공간으로 간다.*/

                        //필드 빈공간에 게임오브젝트 넣어줌
                        NoMatchField(gameObject);

                        //카드 플립함
                        CardManager.instance.FlipCard();
                        CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(CardManager.instance.field[CardManager.instance.field.Count - 1])]++;

                        if (CardManager.instance.field[CardManager.instance.field.Count - 1].CompareTag(gameObject.tag)) // 뒤집은 카드랑 내가 냈던 카드랑 같으면
                        {
                            /*쪽*/
                            print("쪽");
                            Chu(handscore);

                            GameManager.instance.isMyTurn = isPlayer;
                        }

                        else
                        {
                            switch (CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(CardManager.instance.field[CardManager.instance.field.Count - 1])]) // 뒤집은 카드의 같은 태그 개수 별
                            {
                                case 1:
                                    {
                                        print("아무것도 못맞춤");

                                        NoMatchField(CardManager.instance.field[CardManager.instance.field.Count - 1]); //빈공간에 내가 둘 카드 둠

                                        GameManager.instance.isMyTurn = isPlayer;//턴 넘기기
                                        break;
                                    }

                                case 2:
                                    {
                                        print("두개 맞춤");
                                        /*다른거 두개 맞았을 때 -> 무조건 같은 태그 오브젝트 1개 존재*/
                                        hittedCard = GetHitCard(CardManager.instance.field[CardManager.instance.field.Count - 1]); // 뒤집은 카드와 맞은 카드 찾음

                                        CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(hittedCard)]--;
                                        CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(CardManager.instance.field[CardManager.instance.field.Count - 1])]--;

                                        NoMatchField(CardManager.instance.field[CardManager.instance.field.Count - 1]);

                                        EmptyFieldPosition(hittedCard); // 친 카드 필드포지션 비운거 체크
                                        print("피 이동");
                                        //hittedCard.transform.position = CardManager.instance.ScoreField(hittedCard, CardManager.instance.myHandScore); // 점수판 위치 이동
                                        //CardManager.instance.field[CardManager.instance.field.Count - 1].transform.position = CardManager.instance.ScoreField(CardManager.instance.field[CardManager.instance.field.Count - 1], CardManager.instance.myHandScore); // 점수판 위치 이동

                                        //CardManager.instance.myHandScore.Add(hittedCard); // 점수에 더해주기 
                                        //CardManager.instance.field.Remove(hittedCard); // 필드에서 지우기
                                        //CardManager.instance.myHandScore.Add(CardManager.instance.field[CardManager.instance.field.Count - 1]); // 점수에 더해주기
                                        //CardManager.instance.field.Remove(CardManager.instance.field[CardManager.instance.field.Count - 1]); // 필드에서 지우기

                                        //점수판으로 위치 옮기기
                                        MoveFieldScoreField(hittedCard, handscore);
                                        MoveFieldScoreField(CardManager.instance.field[CardManager.instance.field.Count - 1], handscore);
                                        GameManager.instance.isMyTurn = isPlayer;
                                        break;
                                    }

                                /*아직덜만듬*/
                                case 3:
                                    {
                                        /*다른거 세개 맞았을 때 -> 무조건 2개 존재*/
                                        print("둘중하나 고름");
                                        //FlipChoiceCard();
                                        GameManager.instance.isMyTurn = isPlayer;

                                        break;
                                    }

                                case 4:
                                    {
                                        //뒤집은 칻으가 폭탄
                                        print("폭탄");
                                        BombObj = FlipBombCard(CardManager.instance.field[CardManager.instance.field.Count - 1], handscore);

                                        NoMatchField(CardManager.instance.field[CardManager.instance.field.Count - 1]); //빈공간에 내가 둘 카드 둠

                                        EmptyFieldPosition(OrignFieldPosition(BombObj));

                                        MoveBombFieldScoreField(BombObj, CardManager.instance.field[CardManager.instance.field.Count - 1], handscore);
                                        GameManager.instance.isMyTurn = isPlayer;
                                        break;
                                    }
                            }
                        }
                        break;
                    }

                case 2:
                    {
                        print("일단 맞았음");
                        hittedCard = GetHitCard(gameObject); // 자리만 옮기기

                        CardManager.instance.FlipCard();
                        print("flip num : " + CardManager.instance.GetCardTagNum(CardManager.instance.field[CardManager.instance.field.Count - 1]));
                        CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(CardManager.instance.field[CardManager.instance.field.Count - 1])]++;
                        print("flip : " + CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(CardManager.instance.field[CardManager.instance.field.Count - 1])]);
                        if (CardManager.instance.field[CardManager.instance.field.Count - 1].CompareTag(gameObject.tag)) // 같은거 맞음
                        {
                            //뻑
                            //CardManager.instance.field[CardManager.instance.field.Count - 1].transform.position = new Vector3(gameObject.transform.position.x + 0.5f, gameObject.transform.position.y, gameObject.transform.position.z - 0.1f);//뒤집은 카드는 내가 냈던 카드 옆으로 위치 이동
                            CardManager.instance.field[CardManager.instance.field.Count - 1].transform.DOMove
                                (new Vector3(gameObject.transform.position.x + 0.5f,
                                gameObject.transform.position.y,
                                gameObject.transform.position.z - 0.1f), 0.5f).SetEase(Ease.OutQuint).onComplete();//뒤집은 카드는 내가 냈던 카드 옆으로 위치 이동
                            print("뻑");

                            GameManager.instance.isMyTurn = isPlayer;
                        }

                        else
                        {
                            print(CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(gameObject)]);
                            switch (CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(CardManager.instance.field[CardManager.instance.field.Count - 1])]) // 뒤집은 카드의 같은 태그 개수 별
                            {
                                case 1:
                                    {
                                        print("내가 낸 카드만 맞음");
                                        //게임 오브젝트가 가지고 있는 모든 같은 태그 카드 들고가기

                                        CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(hittedCard)]--;
                                        CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(gameObject)]--;
                                        NoMatchField(CardManager.instance.field[CardManager.instance.field.Count - 1]); //빈공간에 내가 둘 카드 둠

                                        EmptyFieldPosition(hittedCard);

                                        print("피 이동");
                                        //hittedCard.transform.position = CardManager.instance.ScoreField(hittedCard, CardManager.instance.myHandScore); // 점수판 위치 이동
                                        //gameObject.transform.position = CardManager.instance.ScoreField(gameObject, CardManager.instance.myHandScore); // 점수판 위치 이동

                                        //CardManager.instance.myHandScore.Add(hittedCard); // 점수에 더해주기 
                                        //CardManager.instance.field.Remove(hittedCard); // 필드에서 지우기
                                        //CardManager.instance.myHandScore.Add(gameObject); // 점수에 더해주기
                                        //CardManager.instance.field.Remove(gameObject); // 필드에서 지우기

                                        MoveFieldScoreField(hittedCard, handscore);
                                        MoveFieldScoreField(gameObject, handscore);
                                        GameManager.instance.isMyTurn = isPlayer;

                                        break;
                                    }

                                case 2:
                                    {
                                        print("내가 낸 카드, 뒤집은 카드도 맞음");
                                        //게임 오브젝트가 가지고 있는 모든 같은 태그 카드 들고가기
                                        //카드 뒤집는게 끝났다면
                                        CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(hittedCard)]--;
                                        CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(gameObject)]--;



                                        print("피 이동");
                                        //hittedCard.transform.position = CardManager.instance.ScoreField(hittedCard, CardManager.instance.myHandScore); // 점수판 위치 이동
                                        //gameObject.transform.position = CardManager.instance.ScoreField(gameObject, CardManager.instance.myHandScore); // 점수판 위치 이동

                                        //CardManager.instance.myHandScore.Add(hittedCard); // 점수에 더해주기 
                                        //CardManager.instance.field.Remove(hittedCard); // 필드에서 지우기
                                        //CardManager.instance.myHandScore.Add(gameObject); // 점수에 더해주기
                                        //CardManager.instance.field.Remove(gameObject); // 필드에서 지우기
                                        EmptyFieldPosition(hittedCard);

                                        MoveFieldScoreField(hittedCard, handscore);
                                        MoveFieldScoreField(gameObject, handscore);

                                        //뒤집은 카드랑 같은 태그 카드 들고가기
                                        hittedCard = GetHitCard(CardManager.instance.field[CardManager.instance.field.Count - 1]);

                                        EmptyFieldPosition(hittedCard);

                                        CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(hittedCard)]--;
                                        CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(CardManager.instance.field[CardManager.instance.field.Count - 1])]--;


                                        print("피 이동");
                                        //hittedCard.transform.position = CardManager.instance.ScoreField(hittedCard, CardManager.instance.myHandScore); // 점수판 위치 이동
                                        //CardManager.instance.field[CardManager.instance.field.Count - 1].transform.position = CardManager.instance.ScoreField(CardManager.instance.field[CardManager.instance.field.Count - 1], CardManager.instance.myHandScore); // 점수판 위치 이동

                                        //CardManager.instance.myHandScore.Add(hittedCard); // 점수에 더해주기 
                                        //CardManager.instance.field.Remove(hittedCard); // 필드에서 지우기
                                        //CardManager.instance.myHandScore.Add(CardManager.instance.field[CardManager.instance.field.Count - 1]); // 점수에 더해주기
                                        //CardManager.instance.field.Remove(CardManager.instance.field[CardManager.instance.field.Count - 1]); // 필드에서 지우기

                                        MoveFieldScoreField(hittedCard, handscore);
                                        MoveFieldScoreField(CardManager.instance.field[CardManager.instance.field.Count - 1], handscore);

                                        GameManager.instance.isMyTurn = isPlayer;
                                        break;
                                    }

                                case 3:
                                    {
                                        print("내가 낸 카드, 뒤집은 카드도 맞음 2개중 하나 골라야함");
                                        //게임오브젝트가 가지고 있는 모든 같은 태그 카드 들고가기
                                        //GetHitCard(gameObject);

                                        //2개중 한개 고르기
                                        //FlipChoiceCard();

                                        GameManager.instance.isMyTurn = isPlayer;
                                        break;
                                    }

                                case 4:
                                    {
                                        print("내가 낸 카드, 뒤집은 카드 폭탄");
                                        CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(hittedCard)]--;

                                        EmptyFieldPosition(hittedCard);

                                        MoveFieldScoreField(hittedCard, handscore);


                                        //폭탄
                                        BombObj = FlipBombCard(CardManager.instance.field[CardManager.instance.field.Count - 1], handscore);

                                        EmptyFieldPosition(OrignFieldPosition(BombObj));

                                        MoveBombFieldScoreField(BombObj, CardManager.instance.field[CardManager.instance.field.Count - 1], handscore);

                                        GameManager.instance.isMyTurn = isPlayer;
                                        break;
                                    }
                            }
                        }
                        break;
                    }


                case 3:
                    {
                        CardManager.instance.FlipCard();
                        CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(CardManager.instance.field[CardManager.instance.field.Count - 1])]++;

                        if (CardManager.instance.field[CardManager.instance.field.Count - 1].CompareTag(gameObject.tag)) // 같은거 맞음
                        {
                            //폭탄
                            print("폭탄입니다.");

                            BombObj = FlipBombCard(CardManager.instance.field[CardManager.instance.field.Count - 1], handscore);

                            EmptyFieldPosition(OrignFieldPosition(BombObj));

                            MoveBombFieldScoreField(BombObj, CardManager.instance.field[CardManager.instance.field.Count - 1], handscore);

                            GameManager.instance.isMyTurn = isPlayer;
                        }

                        else
                        {
                            //2개중 한개 고르기 // 뒤집은 카드의 갯수가 1개이면 안맞은거
                            switch (CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(CardManager.instance.field[CardManager.instance.field.Count - 1])]) // 뒤집은 카드의 같은 태그 개수 별
                            {
                                case 1:
                                    {
                                        //내가 낸 카드에서 내카드 빼고 두개중 한개 고르기
                                        print("내가 낸 카드에서 내카드 빼고 두개중 한개 고르기");
                                        //FlipChoiceCard();

                                        //그냥 빈 자리에 두기
                                        //CardManager.instance.EmptyIndexSort();//빈곳 인덱스 오름차순 정렬
                                        //gameObject.transform.position = CardManager.instance.fieldPosition[CardManager.instance.emptyIndex[0]]; // 마지막 필드포지션은 빈곳에 넣음
                                        //CardManager.instance.emptyIndex.RemoveAt(0);
                                        NoMatchField(gameObject);

                                        GameManager.instance.isMyTurn = isPlayer;
                                        break;
                                    }

                                case 2:
                                    {
                                        //뒤집은 카드랑 같은 태그 카드 들고가기
                                        //FlipChoiceCard();

                                        //GetHitCard(CardManager.instance.field[CardManager.instance.field.Count - 1]);

                                        GameManager.instance.isMyTurn = isPlayer;
                                        break;
                                    }
                                case 3:
                                    {
                                        //2개중 한개 고르기
                                        GameManager.instance.isMyTurn = isPlayer;
                                        break;
                                    }
                                case 4:
                                    {
                                        //폭탄
                                        //FlipChoiceCard(); // 낸카드 중하나 뽑기
                                        //FlipBombCard(); // 뒤집은 폭탄
                                        GameManager.instance.isMyTurn = isPlayer;
                                        break;

                                    }
                            }
                        }
                        break;
                    }
                case 4:
                    {
                        //폭탄
                        print("폭탄");
                        CardManager.instance.FlipCard();
                        CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(CardManager.instance.field[CardManager.instance.field.Count - 1])]++;

                        if (!(CardManager.instance.field[CardManager.instance.field.Count - 1].CompareTag(gameObject.tag))) // 다른거 맞음
                        {
                            switch (CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(CardManager.instance.field[CardManager.instance.field.Count - 1])]) // 뒤집은 카드의 같은 태그 개수 별
                            {
                                case 1:
                                    {
                                        NoMatchField(CardManager.instance.field[CardManager.instance.field.Count - 1]);

                                        BombObj = FlipBombCard(gameObject, handscore);

                                        EmptyFieldPosition(OrignFieldPosition(BombObj));

                                        MoveBombFieldScoreField(BombObj, gameObject, handscore);

                                        GameManager.instance.isMyTurn = isPlayer;
                                        break;
                                    }

                                case 2:
                                    {
                                        //뒤집은 카드랑 같은 태그 카드 들고가기
                                        BombObj = FlipBombCard(gameObject, handscore);

                                        EmptyFieldPosition(OrignFieldPosition(BombObj));

                                        hittedCard = GetHitCard(CardManager.instance.field[CardManager.instance.field.Count - 1]);

                                        EmptyFieldPosition(hittedCard);

                                        MoveBombFieldScoreField(BombObj, gameObject, handscore);

                                        MoveFieldScoreField(CardManager.instance.field[CardManager.instance.field.Count - 1], handscore);

                                        GameManager.instance.isMyTurn = isPlayer;
                                        break;
                                    }
                                case 3:
                                    {
                                        BombObj = FlipBombCard(gameObject, handscore);

                                        EmptyFieldPosition(OrignFieldPosition(BombObj));

                                        //뒤집은 카드 세개 중 하나 고르기
                                        MoveBombFieldScoreField(BombObj, gameObject, handscore);

                                        GameManager.instance.isMyTurn = isPlayer;

                                        break;
                                    }
                                case 4:
                                    {
                                        //폭탄
                                        BombObj = FlipBombCard(gameObject, handscore);

                                        EmptyFieldPosition(OrignFieldPosition(BombObj));

                                        GameObject[] BombObj2 = FlipBombCard(CardManager.instance.field[CardManager.instance.field.Count - 1], handscore);

                                        EmptyFieldPosition(OrignFieldPosition(BombObj2));

                                        MoveBombFieldScoreField(BombObj, gameObject, handscore);

                                        MoveBombFieldScoreField(BombObj2, CardManager.instance.field[CardManager.instance.field.Count - 1], handscore);

                                        GameManager.instance.isMyTurn = isPlayer;
                                        break;
                                    }

                            }
                        }

                        break;
                    }
            }
        }

        else
        {
            print("실행안됨");
        }
    }

    GameObject GetHitCard(GameObject obj)
    {
        print("GetHitCard 실행");
        int index = 0;
        print(CardManager.instance.field.Count - 1);
        for (int i = 0; i < CardManager.instance.field.Count - 1; i++) // 필드에서 돌림
        {
            if (CardManager.instance.field[i].CompareTag(obj.tag))
            {
                index = i;
                print("index : " + index);
                //obj.transform.position = new Vector3(CardManager.instance.field[i].transform.position.x + 0.5f, CardManager.instance.field[i].transform.position.y, CardManager.instance.field[i].transform.position.z - 0.1f);
                obj.transform.DOMove(new Vector3(CardManager.instance.field[i].transform.position.x + 0.5f, CardManager.instance.field[i].transform.position.y, CardManager.instance.field[i].transform.position.z - 0.1f), 0.5f).SetEase(Ease.OutQuint);
            }
        }
        return CardManager.instance.field[index];
    }

    void FlipChoiceCard()
    {
        int count = 0;
        GameObject[] sameTagObj = new GameObject[2];

        for (int i = 0; i < CardManager.instance.field.Count - 1; i++) // 카운트에서 1빼는 이유 -> 비교할 태그가 있음
        {
            if (CardManager.instance.field[i].CompareTag(CardManager.instance.field[CardManager.instance.field.Count - 1].tag)) // 태그가 같을 때 
            {
                sameTagObj[count] = CardManager.instance.field[i];

                //ChoicePanel.transform.GetChild(count).GetComponent<Image>().sprite = sameTagObj[count].GetComponent<SpriteRenderer>().sprite;

                count++;
            }

        }

        //같은 카드 다음 포지션은 같은 태그의 갯수 * 0.5 만큼 x축을 더해준다.
        //CardManager.instance.field[CardManager.instance.field.Count - 1].transform.position = new Vector3(Math.Max(sameTagObj[0].transform.position.x, sameTagObj[1].transform.position.x) + 0.5f, sameTagObj[0].transform.position.y, sameTagObj[0].transform.position.z - 0.1f * 3);
        CardManager.instance.field[CardManager.instance.field.Count - 1].transform.DOMove(new Vector3(Math.Max(sameTagObj[0].transform.position.x, sameTagObj[1].transform.position.x) + 0.5f, sameTagObj[0].transform.position.y, sameTagObj[0].transform.position.z - 0.1f * 3), 0.5f).SetEase(Ease.OutQuint);

        //뒤집은 카드랑 같은 태그 2개중 한개 고르기
        print("둘중 하나 고르기");
        //ChoicePanel.SetActive(true);

        //고른카드 필드에서 제거
        //필드위치 내점수 필드 위치로 바꾸기
        //내 점수리스트 add 
        //필드에서 삭제
    }

    GameObject[] FlipBombCard(GameObject obj, List<GameObject> score)//뒤집은 카든
    {
        int count = 0;
        //무조건 3개
        GameObject[] BombObj = new GameObject[3];

        for (int i = 0; i < CardManager.instance.field.Count - 1; i++) // 카운트에서 1빼는 이유 -> 비교할 태그가 있음
        {
            if (CardManager.instance.field[i].CompareTag(obj.tag)) // 태그가 같을 때 
            {
                BombObj[count] = CardManager.instance.field[i];
                print(count);
                count++;
            }

        }

        //obj.transform.position = new Vector3(Math.Max(Math.Max(BombObj[0].transform.position.x, BombObj[1].transform.position.x), BombObj[2].transform.position.x) + 0.5f,
        //                                                                            BombObj[0].transform.position.y, BombObj[0].transform.position.z - 0.1f * 3);// 필드에 먼저 놔둠
        obj.transform.DOMove(new Vector3(Math.Max(Math.Max(BombObj[0].transform.position.x, BombObj[1].transform.position.x), BombObj[2].transform.position.x) + 0.5f,
                                                                                    BombObj[0].transform.position.y, BombObj[0].transform.position.z - 0.1f * 3), 0.5f).SetEase(Ease.OutQuint);// 필드에 먼저 놔둠

        return BombObj;
    }

    void Chu(List<GameObject> score)
    {
        CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(gameObject)]--; // 내가 낸 카드
        CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(CardManager.instance.field[CardManager.instance.field.Count - 1])]--; //뒤집은 카드랑

        EmptyFieldPosition(gameObject);
        //CardManager.instance.field[CardManager.instance.field.Count - 1].transform.position = new Vector3(gameObject.transform.position.x + 0.5f, gameObject.transform.position.y, gameObject.transform.position.z - 0.1f); //맞춘 오브젝트 옆으로 이동
        CardManager.instance.field[CardManager.instance.field.Count - 1].transform.DOMove(new Vector3(gameObject.transform.position.x + 0.5f, gameObject.transform.position.y, gameObject.transform.position.z - 0.1f), 0.5f).SetEase(Ease.OutQuint); //맞춘 오브젝트 옆으로 이동


        //점수판으로 이동
        //CardManager.instance.field[CardManager.instance.field.Count - 1].transform.position = CardManager.instance.ScoreField(CardManager.instance.field[CardManager.instance.field.Count - 1], score); // 점수판위치로 이동
        CardManager.instance.field[CardManager.instance.field.Count - 1].transform.DOMove(CardManager.instance.ScoreField(CardManager.instance.field[CardManager.instance.field.Count - 1], score), 0.5f).SetEase(Ease.OutQuint); // 점수판위치로 이동
        //gameObject.transform.position = CardManager.instance.ScoreField(gameObject, score); // 점수판위치로 이동
        gameObject.transform.DOMove(CardManager.instance.ScoreField(gameObject, score), 0.5f).SetEase(Ease.OutQuint); // 점수판위치로 이동

        //얻은 카드
        score.Add(CardManager.instance.field[CardManager.instance.field.Count - 1]);
        CardManager.instance.field.Remove(CardManager.instance.field[CardManager.instance.field.Count - 1]);

        score.Add(gameObject);
        CardManager.instance.field.Remove(gameObject);
    }

    void NoMatchField(GameObject obj)
    {
        CardManager.instance.EmptyIndexSort();//빈곳 인덱스 오름차순 정렬
        //obj.transform.position = CardManager.instance.fieldPosition[CardManager.instance.emptyIndex[0]]; // 마지막 필드포지션은 빈곳에 넣음
        obj.transform.DOMove(CardManager.instance.fieldPosition[CardManager.instance.emptyIndex[0]], 0.5f).SetEase(Ease.OutQuint); // 마지막 필드포지션은 빈곳에 넣음
        CardManager.instance.emptyIndex.RemoveAt(0);
    }

    void MoveFieldScoreField(GameObject moveObj, List<GameObject> score)
    {
        //moveObj.transform.position = CardManager.instance.ScoreField(moveObj, score); // 점수판 위치 이동
        moveObj.transform.DOMove(CardManager.instance.ScoreField(moveObj, score), 0.5f).SetEase(Ease.OutQuint); // 점수판 위치 이동

        score.Add(moveObj); // 점수에 더해주기 
        CardManager.instance.field.Remove(moveObj); // 필드에서 지우기
    }

    void MoveBombFieldScoreField(GameObject[] bombObj, GameObject card, List<GameObject> score)
    {
        for (int i = 0; i < bombObj.Length; i++)
        {
            //bombObj[i].transform.position = CardManager.instance.ScoreField(card, score); // 점수 필드로 위치 옮김
            bombObj[i].transform.DOMove(CardManager.instance.ScoreField(card, score), 0.5f).SetEase(Ease.OutQuint); // 점수 필드로 위치 옮김

            //내 점수리스트 add 
            score.Add(bombObj[i]);//내 점수필드 리스트에 추가

            //필드에서 삭제
            CardManager.instance.field.Remove(bombObj[i]);//필드에서 제거
            CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(bombObj[i])]--;//필드에서 제거
        }



        //card.transform.position = CardManager.instance.ScoreField(card, score);
        card.transform.DOMove(CardManager.instance.ScoreField(card, score), 0.5f).SetEase(Ease.OutQuint);

        //내 점수리스트 add 
        score.Add(card);//내 점수필드 리스트에 추가

        //필드에서 삭제
        CardManager.instance.field.Remove(card);//필드에서 제거
        CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(card)]--;//필드에서 제거

    }

    void EmptyFieldPosition(GameObject obj)
    {

        int index = Array.IndexOf(CardManager.instance.fieldPosition, obj.transform.position);

        if (!CardManager.instance.emptyIndex.Contains(index)) // 원래 가지고 있는 값이 아니면
        {
            //print("Empty index : " + index);
            CardManager.instance.emptyIndex.Add(index);
        }

        for (int i = 0; i < CardManager.instance.emptyIndex.Count; i++)
        {
            print("empty list : " + CardManager.instance.emptyIndex[i]);
        }
    }

    GameObject OrignFieldPosition(GameObject[] obj)
    {
        int index = -1;
        for (int i = 0; i < obj.Length; i++)
        {
            for (int j = 0; j < CardManager.instance.fieldPosition.Length; i++)
            {
                if (obj[i].transform.position == CardManager.instance.fieldPosition[j])
                {
                    index = i;
                }
            }
        }
        return obj[index];
    }
}
