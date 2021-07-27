using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class CardClick : MonoBehaviour
{
    #region SINGLETON
    public static CardClick instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion

    public int myScore = 0;
    public int opponentScore = 0;
    public string type = "";

    public GameObject ChoicePanel;

    GameObject hittedCard;
    int myCardCount;

    private GameObject OpenChoicePanel;

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
        myScore = 0;
        opponentScore = 0;

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
                        CardManager.instance.gwangCount++;
                    }
                    else
                    {
                        CardManager.instance.enemyGwangCount++;
                    }
                    break;
                case "홍단":
                    if (scoreList == CardManager.instance.myHandScore)
                    {
                        CardManager.instance.redFlagCount++;
                    }
                    else
                    {
                        CardManager.instance.enemyRedFlagCount++;
                    }
                    break;
                case "청단":
                    if (scoreList == CardManager.instance.myHandScore)
                    {
                        CardManager.instance.blueFlagCount++;
                    }
                    else
                    {
                        CardManager.instance.enemyBlueFlagCount++;
                    }
                    break;
                case "초단":
                    if (scoreList == CardManager.instance.myHandScore)
                    {
                        CardManager.instance.normalFlagCount++;
                    }
                    else
                    {
                        CardManager.instance.enemyNormalFlagCount++;
                    }
                    break;
                case "새":
                    if (scoreList == CardManager.instance.myHandScore)
                    {
                        CardManager.instance.animalCount++;
                    }
                    else
                    {
                        CardManager.instance.enemyAnimalCount++;
                    }
                    break;
                case "피":
                    if (scoreList == CardManager.instance.myHandScore)
                    {
                        CardManager.instance.peeCount++;
                        CardManager.instance.isPee = true;
                    }
                    else
                    {
                        CardManager.instance.enemyPeeCount++;
                        CardManager.instance.isOpPee = true;
                    }
                    break;
                default:
                    break;
            }
        }
    }

    public void OnMouseUp()
    {
        if (GameManager.instance.isMyTurn == true)//만약 플레이어 턴이면
        {
            WhoTurn(CardManager.instance.myHand, CardManager.instance.myHandScore, false);
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
        if (hand.Contains(gameObject)) // 내손에 이 게임오브젝트가 있을 때
        {
            CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(gameObject)]++; // 내가 낸 카드 ++ 해줌
            myCardCount = CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(gameObject)];
            CardManager.instance.field.Add(gameObject); //내손에 있는 카드 필드에 넣기
            hand.Remove(gameObject);//내손에서 지우기

            switch (myCardCount) // 2, 3, 4개 맞췄을 시 다름
            {
                //카드가 안맞았을 때
                case 1:
                    {
                        /*카드의 위치는 빈공간으로 간다.*/

                        //필드 빈공간에 게임오브젝트 넣어줌
                        NoMatchField(gameObject);

                        CardManager.instance.FlipCard();
                        if (!CardManager.instance.isFlip)
                        {
                            break;
                        }
                        //카드 플립함

                        else
                        {
                            if (CardManager.instance.field[CardManager.instance.field.Count - 1].CompareTag(gameObject.tag)) // 뒤집은 카드랑 내가 냈던 카드랑 같으면
                            {
                                /*쪽*/
                                print("쪽");
                                CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(gameObject)]--; // 내가 낸 카드
                                CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(CardManager.instance.field[CardManager.instance.field.Count - 1])]--; //뒤집은 카드랑

                                Chu(handscore);
                                break;
                            }

                            else
                            {
                                switch (CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(CardManager.instance.field[CardManager.instance.field.Count - 1])]) // 뒤집은 카드의 같은 태그 개수 별
                                {
                                    case 1:
                                        {
                                            NoMatchField(CardManager.instance.field[CardManager.instance.field.Count - 1]); //빈공간에 내가 둘 카드 둠
                                            break;//턴 넘기기
                                        }

                                    case 2:
                                        {
                                            /*다른거 두개 맞았을 때 -> 무조건 같은 태그 오브젝트 1개 존재*/
                                            hittedCard = GetHitCard(CardManager.instance.field[CardManager.instance.field.Count - 1]); // 뒤집은 카드와 맞은 카드 찾음

                                            CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(hittedCard)]--;
                                            CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(CardManager.instance.field[CardManager.instance.field.Count - 1])]--;

                                            EmptyFieldPosition(hittedCard); // 친 카드 필드포지션 비운거 체크

                                            //점수판으로 위치 옮기기
                                            MoveFieldScoreField(hittedCard, handscore);
                                            MoveFieldScoreField(CardManager.instance.field[CardManager.instance.field.Count - 1], handscore);
                                            break;
                                        }

                                    /*아직덜만듬*/
                                    case 3:
                                        {
                                            /*다른거 세개 맞았을 때 -> 무조건 2개 존재*/
                                            print("둘중하나 고름");
                                            FlipChoiceCard(CardManager.instance.field[CardManager.instance.field.Count - 1]);
                                            break;
                                        }

                                    case 4:
                                        {
                                            //뒤집은 칻으가 폭탄
                                            print("폭탄");
                                            GameObject[] temp = new GameObject[3];
                                            temp = FlipBombCard(CardManager.instance.field[CardManager.instance.field.Count - 1], handscore);

                                            EmptyFieldPosition(OrignFieldPosition(temp));

                                            MoveBombFieldScoreField(temp, CardManager.instance.field[CardManager.instance.field.Count - 1], handscore);
                                            break;
                                        }
                                }
                            }
                        }

                        break;
                    }

                case 2:
                    {
                        hittedCard = GetHitCard(gameObject); // 자리만 옮기기

                        CardManager.instance.FlipCard();

                        if (!CardManager.instance.isFlip)
                        {
                            CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(hittedCard)]--;
                            CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(gameObject)]--;

                            EmptyFieldPosition(hittedCard);

                            MoveFieldScoreField(hittedCard, handscore);
                            MoveFieldScoreField(gameObject, handscore);
                            break;
                        }

                        else
                        {
                            if (CardManager.instance.field[CardManager.instance.field.Count - 1].CompareTag(gameObject.tag)) // 같은거 맞음
                            {
                                //뻑
                                print(gameObject.name);
                                CardManager.instance.field[CardManager.instance.field.Count - 1].transform.position = new Vector3(gameObject.transform.position.x + 0.5f, gameObject.transform.position.y, gameObject.transform.position.z - 0.1f);//뒤집은 카드는 내가 냈던 카드 옆으로 위치 이동
                                //오류남                                                                                                                                                                                         //    gameObject.transform.position.z - 0.1f), 0.5f).SetEase(Ease.OutQuint);//뒤집은 카드는 내가 냈던 카드 옆으로 위치 이동
                                print("뻑");
                                break;
                            }

                            else
                            {
                                switch (CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(CardManager.instance.field[CardManager.instance.field.Count - 1])]) // 뒤집은 카드의 같은 태그 개수 별
                                {
                                    case 1:
                                        {
                                            //게임 오브젝트가 가지고 있는 모든 같은 태그 카드 들고가기

                                            CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(hittedCard)]--;
                                            CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(gameObject)]--;

                                            EmptyFieldPosition(hittedCard);

                                            MoveFieldScoreField(hittedCard, handscore);
                                            MoveFieldScoreField(gameObject, handscore);

                                            NoMatchField(CardManager.instance.field[CardManager.instance.field.Count - 1]); //빈공간에 내가 둘 카드 둠
                                            break;

                                        }

                                    case 2:
                                        {
                                            //게임 오브젝트가 가지고 있는 모든 같은 태그 카드 들고가기
                                            //카드 뒤집는게 끝났다면
                                            CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(hittedCard)]--;
                                            CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(gameObject)]--;

                                            EmptyFieldPosition(hittedCard);

                                            MoveFieldScoreField(hittedCard, handscore);
                                            MoveFieldScoreField(gameObject, handscore);

                                            //뒤집은 카드랑 같은 태그 카드 들고가기
                                            hittedCard = GetHitCard(CardManager.instance.field[CardManager.instance.field.Count - 1]);

                                            EmptyFieldPosition(hittedCard);

                                            CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(hittedCard)]--;
                                            CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(CardManager.instance.field[CardManager.instance.field.Count - 1])]--;

                                            MoveFieldScoreField(hittedCard, handscore);
                                            MoveFieldScoreField(CardManager.instance.field[CardManager.instance.field.Count - 1], handscore);

                                            break;
                                        }

                                    case 3:
                                        {
                                            print("내가 낸 카드, 뒤집은 카드도 맞음 2개중 하나 골라야함");
                                            //게임오브젝트가 가지고 있는 모든 같은 태그 카드 들고가기
                                            CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(hittedCard)]--;
                                            CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(gameObject)]--;
                                            EmptyFieldPosition(hittedCard);

                                            MoveFieldScoreField(hittedCard, handscore);
                                            MoveFieldScoreField(gameObject, handscore);

                                            //2개중 한개 고르기
                                            FlipChoiceCard(CardManager.instance.field[CardManager.instance.field.Count - 1]);

                                            break;
                                        }

                                    case 4:
                                        {
                                            print("내가 낸 카드, 뒤집은 카드 폭탄");
                                            CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(hittedCard)]--;
                                            CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(gameObject)]--;

                                            EmptyFieldPosition(hittedCard);

                                            MoveFieldScoreField(hittedCard, handscore);
                                            MoveFieldScoreField(gameObject, handscore);

                                            GameObject[] temp = new GameObject[3];

                                            //폭탄
                                            temp = FlipBombCard(CardManager.instance.field[CardManager.instance.field.Count - 1], handscore);

                                            EmptyFieldPosition(OrignFieldPosition(temp));

                                            MoveBombFieldScoreField(temp, CardManager.instance.field[CardManager.instance.field.Count - 1], handscore);

                                            break;
                                        }
                                }
                            }
                        }

                        break;
                    }


                case 3:
                    {
                        CardManager.instance.FlipCard();
                        if (!CardManager.instance.isFlip)
                        {
                            FlipChoiceCard(gameObject);
                            break;
                        }
                        else
                        {
                            if (CardManager.instance.field[CardManager.instance.field.Count - 1].CompareTag(gameObject.tag)) // 같은거 맞음
                            {
                                //폭탄
                                print("폭탄입니다.");

                                GameObject[] temp = new GameObject[3];
                                temp = FlipBombCard(CardManager.instance.field[CardManager.instance.field.Count - 1], handscore);

                                EmptyFieldPosition(OrignFieldPosition(temp));

                                MoveBombFieldScoreField(temp, CardManager.instance.field[CardManager.instance.field.Count - 1], handscore);

                                break;
                            }

                            else
                            {
                                //2개중 한개 고르기 // 뒤집은 카드의 갯수가 1개이면 안맞은거
                                switch (CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(CardManager.instance.field[CardManager.instance.field.Count - 1])]) // 뒤집은 카드의 같은 태그 개수 별
                                {
                                    case 1:
                                        {
                                            //내가 낸 카드에서 내카드 빼고 두개중 한개 고르기
                                            FlipChoiceCard(gameObject);

                                            NoMatchField(CardManager.instance.field[CardManager.instance.field.Count - 1]);

                                            break;
                                        }

                                    case 2:
                                        {
                                            //뒤집은 카드랑 같은 태그 카드 들고가기
                                            FlipChoiceCard(gameObject);

                                            hittedCard = GetHitCard(CardManager.instance.field[CardManager.instance.field.Count - 1]);

                                            CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(hittedCard)]--;
                                            CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(CardManager.instance.field[CardManager.instance.field.Count - 1])]--;

                                            EmptyFieldPosition(hittedCard);

                                            MoveFieldScoreField(hittedCard, handscore);
                                            MoveFieldScoreField(CardManager.instance.field[CardManager.instance.field.Count - 1], handscore);

                                            break;
                                        }
                                    case 3:
                                        {
                                            FlipChoiceCard(gameObject);

                                            FlipChoiceCard(CardManager.instance.field[CardManager.instance.field.Count - 1]);
                                            //2개중 한개 고르기
                                            break;
                                        }
                                    case 4:
                                        {
                                            FlipChoiceCard(gameObject);
                                            //폭탄
                                            //FlipChoiceCard(); // 낸카드 중하나 뽑기
                                            GameObject[] temp = new GameObject[3];
                                            temp = FlipBombCard(CardManager.instance.field[CardManager.instance.field.Count - 1], handscore); // 뒤집은 폭탄
                                            EmptyFieldPosition(OrignFieldPosition(temp));

                                            MoveBombFieldScoreField(temp, CardManager.instance.field[CardManager.instance.field.Count - 1], handscore);

                                            break;

                                        }
                                }
                            }
                            break;
                        }

                    }
                case 4:
                    {
                        //폭탄
                        CardManager.instance.FlipCard();

                        if (!CardManager.instance.isFlip)
                        {
                            //이건 됨
                            GameObject[] temp = new GameObject[3];

                            temp = FlipBombCard(gameObject, handscore);

                            EmptyFieldPosition(OrignFieldPosition(temp));

                            MoveBombFieldScoreField(temp, gameObject, handscore);

                            break;
                        }

                        else
                        {
                            if (!(CardManager.instance.field[CardManager.instance.field.Count - 1].CompareTag(gameObject.tag))) // 다른거 맞음
                            {
                                switch (CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(CardManager.instance.field[CardManager.instance.field.Count - 1])]) // 뒤집은 카드의 같은 태그 개수 별
                                {
                                    case 1:
                                        {

                                            GameObject[] temp = new GameObject[3];

                                            temp = FlipBombCard(gameObject, handscore);

                                            EmptyFieldPosition(OrignFieldPosition(temp));

                                            MoveBombFieldScoreField(temp, gameObject, handscore);

                                            NoMatchField(CardManager.instance.field[CardManager.instance.field.Count - 1]);
                                            break;
                                        }

                                    case 2:
                                        {
                                            GameObject[] temp = new GameObject[3];

                                            //뒤집은 카드랑 같은 태그 카드 들고가기
                                            temp = FlipBombCard(gameObject, handscore);

                                            EmptyFieldPosition(OrignFieldPosition(temp));

                                            MoveBombFieldScoreField(temp, gameObject, handscore);

                                            hittedCard = GetHitCard(CardManager.instance.field[CardManager.instance.field.Count - 1]);

                                            CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(hittedCard)]--;
                                            CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(CardManager.instance.field[CardManager.instance.field.Count - 1])]--;

                                            EmptyFieldPosition(hittedCard);

                                            MoveFieldScoreField(hittedCard, handscore);
                                            MoveFieldScoreField(CardManager.instance.field[CardManager.instance.field.Count - 1], handscore);

                                            break;
                                        }
                                    case 3:
                                        {
                                            GameObject[] temp = new GameObject[3];

                                            temp = FlipBombCard(gameObject, handscore);

                                            EmptyFieldPosition(OrignFieldPosition(temp));

                                            MoveBombFieldScoreField(temp, gameObject, handscore);

                                            //뒤집은 카드 세개 중 하나 고르기
                                            FlipChoiceCard(CardManager.instance.field[CardManager.instance.field.Count - 1]);


                                            break;

                                        }
                                    case 4:
                                        {
                                            GameObject[] temp = new GameObject[3];

                                            //폭탄
                                            temp = FlipBombCard(gameObject, handscore);

                                            EmptyFieldPosition(OrignFieldPosition(temp));

                                            GameObject[] temp2 = FlipBombCard(CardManager.instance.field[CardManager.instance.field.Count - 1], handscore);

                                            EmptyFieldPosition(OrignFieldPosition(temp2));

                                            MoveBombFieldScoreField(temp, gameObject, handscore);

                                            MoveBombFieldScoreField(temp2, CardManager.instance.field[CardManager.instance.field.Count - 1], handscore);

                                            break;
                                        }

                                }
                            }

                            break;
                        }

                    }
            }

            CardManager.instance.ResetPosition(hand);

            if (hand == CardManager.instance.myHand)
            {
                CardManager.instance.DrawCard(CardManager.instance.opponentHand, 1);

            }
            if (hand == CardManager.instance.opponentHand)
            {
                CardManager.instance.DrawCard(CardManager.instance.myHand, 1);

            }

            GameManager.instance.isMyTurn = isPlayer;
        }

        else
        {
            print("실행안됨");
        }
    }

    GameObject GetHitCard(GameObject obj)
    {
        int index = 0;
        for (int i = 0; i < CardManager.instance.field.Count - 1; i++) // 필드에서 돌림
        {
            if (CardManager.instance.field[i].CompareTag(obj.tag))
            {
                index = i;
                obj.transform.position = new Vector3(CardManager.instance.field[i].transform.position.x + 0.5f, CardManager.instance.field[i].transform.position.y, CardManager.instance.field[i].transform.position.z - 0.1f);
                //obj.transform.DOMove(new Vector3(CardManager.instance.field[i].transform.position.x + 0.5f, CardManager.instance.field[i].transform.position.y, CardManager.instance.field[i].transform.position.z - 0.1f), 0.5f).SetEase(Ease.OutQuint);
            }
        }
        return CardManager.instance.field[index];
    }

    void FlipChoiceCard(GameObject obj)
    {
        for (int i = 0; i < CardManager.instance.field.Count - 1; i++) // 카운트에서 1빼는 이유 -> 비교할 태그가 있음
        {
            if (CardManager.instance.field[i].CompareTag(obj.tag))// 태그가 같을 때 
            {
                CardManager.instance.ChoiceObj.Add(CardManager.instance.field[i]);
            }

        }
        CardManager.instance.ChoiceObj.Remove(obj);

        //같은 카드 다음 포지션은 같은 태그의 갯수 * 0.5 만큼 x축을 더해준다.
        obj.transform.position = new Vector3(Math.Max(CardManager.instance.ChoiceObj[0].transform.position.x, CardManager.instance.ChoiceObj[1].transform.position.x) + 0.5f,
            CardManager.instance.ChoiceObj[0].transform.position.y, CardManager.instance.ChoiceObj[0].transform.position.z - 0.1f * 3);

        CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(obj)]--;

        if (GameManager.instance.isMyTurn)
        {
            MoveFieldScoreField(obj, CardManager.instance.myHandScore);
        }

        else
        {
            MoveFieldScoreField(obj, CardManager.instance.opponentHandScore);
        }

        CardManager.instance.field.Remove(obj);

        OpenChoicePanel = GameObject.Find("Canvas").transform.Find("Panel").gameObject;
        OpenChoicePanel.SetActive(true);
        OpenChoicePanel.transform.GetChild(0).GetComponent<Image>().sprite = CardManager.instance.ChoiceObj[0].GetComponent<SpriteRenderer>().sprite;
        OpenChoicePanel.transform.GetChild(1).GetComponent<Image>().sprite = CardManager.instance.ChoiceObj[1].GetComponent<SpriteRenderer>().sprite;
    }

    GameObject[] FlipBombCard(GameObject obj, List<GameObject> score)//뒤집은 카든
    {
        for (int i = 0; i < CardManager.instance.field.Count - 1; i++) // 카운트에서 1빼는 이유 -> 비교할 태그가 있음
        {
            if (CardManager.instance.field[i].CompareTag(obj.tag)) // 태그가 같을 때 
            {
                CardManager.instance.BombObj.Add(CardManager.instance.field[i]);
            }

        }
        if (CardManager.instance.BombObj.Contains(obj))
        {
            CardManager.instance.BombObj.Remove(obj);
        }


        print(obj.name);

        obj.transform.position = new Vector3(Math.Max(Math.Max(CardManager.instance.BombObj[0].transform.position.x, CardManager.instance.BombObj[1].transform.position.x), CardManager.instance.BombObj[2].transform.position.x) + 0.5f,
                                                                                    CardManager.instance.BombObj[0].transform.position.y, CardManager.instance.BombObj[0].transform.position.z - 0.1f * 3);// 필드에 먼저 놔둠
        //obj.transform.DOMove(new Vector3(Math.Max(Math.Max(BombObj[0].transform.position.x, BombObj[1].transform.position.x), BombObj[2].transform.position.x) + 0.5f,
        //                                                                            BombObj[0].transform.position.y, BombObj[0].transform.position.z - 0.1f * 3), 0.5f).SetEase(Ease.OutQuint);// 필드에 먼저 놔둠

        GameObject[] temp = new GameObject[3];

        for (int i = 0; i < temp.Length; i++)
        {
            temp[i] = CardManager.instance.BombObj[i];
        }

        return temp;
    }

    void Chu(List<GameObject> score)
    {

        EmptyFieldPosition(gameObject);
        CardManager.instance.field[CardManager.instance.field.Count - 1].transform.DOMove(new Vector3(gameObject.transform.position.x + 0.5f, gameObject.transform.position.y, gameObject.transform.position.z - 0.1f), 0.5f).SetEase(Ease.OutQuint); //맞춘 오브젝트 옆으로 이동


        //점수판으로 이동
        CardManager.instance.field[CardManager.instance.field.Count - 1].transform.DOMove(CardManager.instance.ScoreField(CardManager.instance.field[CardManager.instance.field.Count - 1], score), 0.5f).SetEase(Ease.OutQuint); // 점수판위치로 이동
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
        obj.transform.DOMove(CardManager.instance.fieldPosition[CardManager.instance.emptyIndex[0]], 0.5f).SetEase(Ease.OutQuint); // 마지막 필드포지션은 빈곳에 넣음
        CardManager.instance.emptyIndex.RemoveAt(0);
    }

    void MoveFieldScoreField(GameObject moveObj, List<GameObject> score)
    {
        moveObj.transform.DOMove(CardManager.instance.ScoreField(moveObj, score), 0.5f).SetEase(Ease.OutQuint); // 점수판 위치 이동

        score.Add(moveObj); // 점수에 더해주기 
        CardManager.instance.field.Remove(moveObj); // 필드에서 지우기
    }

    void MoveBombFieldScoreField(GameObject[] bombObj, GameObject card, List<GameObject> score)
    {
        for (int i = 0; i < bombObj.Length; i++)
        {
            bombObj[i].transform.DOMove(CardManager.instance.ScoreField(bombObj[i], score), 0.5f).SetEase(Ease.OutQuint); // 점수 필드로 위치 옮김
            //내 점수리스트 add 
            score.Add(bombObj[i]);//내 점수필드 리스트에 추가

            //필드에서 삭제
            CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(bombObj[i])]--;//필드에서 제거
            CardManager.instance.field.Remove(bombObj[i]);//필드에서 제거
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

        if (!CardManager.instance.emptyIndex.Contains(index)) // 원래 가지고 있는 값이 아니면 // 값이 없으면 음수로 들어가서 음수는 빼줘야함
        {
            if (index >= 0)
            {
                CardManager.instance.emptyIndex.Add(index);
            }
        }
    }

    GameObject OrignFieldPosition(GameObject[] obj)
    {
        int index = -1;
        for (int i = 0; i < obj.Length; i++)
        {

            for (int j = 0; j < CardManager.instance.fieldPosition.Length; j++)
            {
                if (obj[i].transform.position == CardManager.instance.fieldPosition[j])
                {
                    index = i;
                }
            }
        }
        return obj[index];
    }

    public void SelectNum(int num)
    {
        OpenChoicePanel = GameObject.Find("Canvas").transform.Find("Panel").gameObject;
        OpenChoicePanel.SetActive(false);

        GameObject[] temp = new GameObject[2];

        for (int i = 0; i < CardManager.instance.ChoiceObj.Count; i++)
        {
            temp[i] = CardManager.instance.ChoiceObj[i];
        }

        if (!GameManager.instance.isMyTurn)
        {
            MoveFieldScoreField(CardManager.instance.ChoiceObj[num], CardManager.instance.myHandScore);
        }

        else
        {
            MoveFieldScoreField(CardManager.instance.ChoiceObj[num], CardManager.instance.opponentHandScore);
        }

        CardManager.instance.ChoiceObj[1 - num].transform.position = OrignFieldPosition(temp).transform.position;

        CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(CardManager.instance.ChoiceObj[num])]--;
        CardManager.instance.field.Remove(CardManager.instance.ChoiceObj[num]);

        CardManager.instance.ChoiceObj.Clear();

    }
}
