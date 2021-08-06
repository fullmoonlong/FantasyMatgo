using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
public class CardClick : MonoBehaviour
{
    #region SINGLETON	
    public static CardClick instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion
    public string type = "";
    GameObject hittedCard;
    private GameObject OpenChoicePanel;
    private GameObject ShakePanel;
    private GameObject AttackPanel;
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
                    else if (scoreList == CardManager.instance.opponentHandScore)
                    {
                        CardManager.instance.enemyGwangCount++;
                    }
                    break;
                case "홍단":
                    if (scoreList == CardManager.instance.myHandScore)
                    {
                        CardManager.instance.redFlagCount++;
                    }
                    else if (scoreList == CardManager.instance.opponentHandScore)
                    {
                        CardManager.instance.enemyRedFlagCount++;
                    }
                    break;
                case "청단":
                    if (scoreList == CardManager.instance.myHandScore)
                    {
                        CardManager.instance.blueFlagCount++;
                    }
                    else if (scoreList == CardManager.instance.opponentHandScore)
                    {
                        CardManager.instance.enemyBlueFlagCount++;
                    }
                    break;
                case "초단":
                    if (scoreList == CardManager.instance.myHandScore)
                    {
                        CardManager.instance.normalFlagCount++;
                    }
                    else if (scoreList == CardManager.instance.opponentHandScore)
                    {
                        CardManager.instance.enemyNormalFlagCount++;
                    }
                    break;
                case "새":
                    if (scoreList == CardManager.instance.myHandScore)
                    {
                        CardManager.instance.animalCount++;
                    }
                    else if (scoreList == CardManager.instance.opponentHandScore)
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
                    else if (scoreList == CardManager.instance.opponentHandScore)
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

    IEnumerator FixedMade(int king, int opking, int red, int blue, int normal, int animal, bool[] isking, bool[] isflag, bool isanimal, GameObject who, PlayerScript ui, BattleHUD hud)
    {
     
        yield return new WaitForSeconds(1f);
        for (int i=0;i<3;i++)
        {
            if (!isking[i] && king == i + 3)
            {
                AttackPanel = GameObject.Find("Canvas").transform.Find("AttackPanel").gameObject;
                AttackPanel.SetActive(true);
                print("광 공격");
                BattleSystem.instance.LightAttack(king, opking);

                StartCoroutine(AttackAction(who, ui, hud));
                BattleSystem.instance.kingAttack[i] = true;
            }
        
            int flag = 0;
            switch (i)
            {
                case 0:
                    flag = red;
                    break;
                case 1:
                    flag = blue;
                    break;
                case 2:
                    flag = normal;
                    break;
                default:
                    break;
            }

            if (!isflag[i] && flag == 3)
            {
                AttackPanel = GameObject.Find("Canvas").transform.Find("AttackPanel").gameObject;
                AttackPanel.SetActive(true);
                BattleSystem.instance.damage = 3;
                StartCoroutine(AttackAction(who, ui, hud));
                BattleSystem.instance.flagAttack[i] = true;
            }
        }

        if (!isanimal && animal == 3)
        {
            AttackPanel = GameObject.Find("Canvas").transform.Find("AttackPanel").gameObject;
            AttackPanel.SetActive(true);
            BattleSystem.instance.damage = 5;
            StartCoroutine(AttackAction(who, ui, hud));
            BattleSystem.instance.animalAttack = true;
        }
    }
    IEnumerator AttackAction(GameObject who, PlayerScript ui, BattleHUD hud)
    {
        AttackPanel = GameObject.Find("Canvas").transform.Find("AttackPanel").gameObject;
        Sequence mysequence = DOTween.Sequence();
        BattleSystem.instance.attackImage.transform.position = who.transform.position;

        mysequence.Append(BattleSystem.instance.attackImage.transform.DOScale(Vector3.one * 0.3f, 0.3f)
            .SetEase(Ease.InOutBack)).Join(who.transform.DOShakePosition(1f, 5f))
          .AppendInterval(1.2f).Append(BattleSystem.instance.attackImage.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InOutBack)).OnComplete(() => BattleSystem.instance.Damaged(ui, hud));

        AttackPanel.SetActive(false);
        yield return new WaitForSeconds(3f);
    }

    public void OnMouseUp()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (GameManager.instance.isSetting && !GameManager.instance.isMoving)
            {
                #region PlayerTurn	
                if (GameManager.instance.isMyTurn == true)//만약 플레이어 턴이면	
                {
                    //print("player turn");
                    WhoTurn(CardManager.instance.myHand, CardManager.instance.myHandScore, false);
                    CalculateScore(CardManager.instance.myHandScore);
                    //GameManager.instance.ScoreCheck();
                    StartCoroutine(FixedMade(CardManager.instance.kingEmptyIndex, CardManager.instance.enemyKingEmptyIndex, CardManager.instance.redFlagEmptyIndex, CardManager.instance.blueFlagEmptyIndex, CardManager.instance.normalFlagEmptyIndex,
                        CardManager.instance.animalEmptyIndex, BattleSystem.instance.kingAttack, BattleSystem.instance.flagAttack, BattleSystem.instance.animalAttack, BattleSystem.instance.op, BattleSystem.instance.opUi, BattleSystem.instance.opHUD));
                    for (int i = 0; i < CardManager.instance.emptyIndex.Count; i++)
                    {
                        //print("완료 남은 카드 인덱스 " + i + " : " + CardManager.instance.emptyIndex[i]);
                    }

                }
                #endregion
                #region OpponentTurn	
                else//만약 상대 턴이면	
                {
                    //print("Opponent turn");
                    WhoTurn(CardManager.instance.opponentHand, CardManager.instance.opponentHandScore, true);
                    CalculateScore(CardManager.instance.opponentHandScore);
                    for (int i = 0; i < CardManager.instance.emptyIndex.Count; i++)
                    {
                        //print("완료 남은 카드 인덱스 " + i + " : " + CardManager.instance.emptyIndex[i]);
                    }

                }
                #endregion
            }

        }
    }

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
    }

    void WhoTurn(List<GameObject> hand, List<GameObject> handscore, bool isPlayer)
    {
        print(gameObject.name);
        //카드 뽑는 애니메이션
        if (hand.Contains(gameObject)) // 내손에 이 게임오브젝트가 있을 때
        {
            //print("hand in");
            //print(gameObject.name);
            if (gameObject.name == "Bomb(Clone)")
            {
                CardManager.instance.myCardCount = 1;

                CheckCardAction(gameObject, hand, handscore);

                hand.Remove(gameObject);
                Destroy(gameObject);
                GameManager.instance.isMoving = false;
                
                CardManager.instance.ResetPosition(hand);

                EndArrange(hand, isPlayer);
            }

            else if(gameObject.tag == "Bonus")
            {
                print("bonus card");
                //GameManager.instance.maxTurnCount++;
                MoveFieldScoreField(gameObject, handscore);
                hand.Remove(gameObject);

                CardManager.instance.ResetPosition(hand);

                if (GameManager.instance.first)
                {
                    CardManager.instance.DrawCard(hand, 1);
                }
                CardManager.instance.DrawCard(hand, 1);
                GameManager.instance.oneTime = false;
            }
            else
            {
                CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(gameObject)]++; // 내가 낸 카드 ++ 해줌
                CardManager.instance.myCardCount = CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(gameObject)];
                CardManager.instance.field.Add(gameObject); //내손에 있는 카드 필드에 넣기

                if (HandCheck(hand, gameObject).Length == 2 && CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(gameObject)] == 1) // 내손에 같은 카드가 3장일 때 흔들기
                {
                    print("흔들기");
                    ShakePanel = GameObject.Find("Canvas").transform.Find("ShakePanel").gameObject;
                    ShakePanel.SetActive(true);
                    CardManager.instance.curObj = gameObject;
                }

                else
                {
                    hand.Remove(gameObject);//내손에서 지우기
                    CardManager.instance.ResetPosition(hand);

                    CheckCardAction(gameObject, hand, handscore);

                    //GameManager.instance.isBonus = false;

                    EndArrange(hand, isPlayer);
                }
            }
        }

        else
        {
            print("실행안됨");
        }
    }

    void CheckCardAction(GameObject clickObj, List<GameObject> hand, List<GameObject> handscore) // 맞춘 카드마다 다른 행동
    {

        CardManager.instance.ResetPosition(hand);
        switch (CardManager.instance.myCardCount) // 2, 3, 4개 맞췄을 시 다름
        {
            //카드가 안맞았을 때
            case 1:
                {
                    print("일단 안맞음");
                    /*카드의 위치는 빈공간으로 간다.*/

                    if(clickObj.name != "Bomb(Clone)" && clickObj.tag != "Bonus")
                    {
                        NoMatchField(clickObj);
                    }
                    
                    CardManager.instance.FlipCard();

                    if (!CardManager.instance.isFlip)
                    {
                        break;
                    }

                    else
                    {
                        if (CardManager.instance.field[CardManager.instance.field.Count - 1].CompareTag(clickObj.tag)) // 뒤집은 카드랑 내가 냈던 카드랑 같으면
                        {
                            print("쪽");
                            CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(clickObj)]--; // 내가 낸 카드
                            CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(CardManager.instance.field[CardManager.instance.field.Count - 1])]--; //뒤집은 카드랑

                            Chu(clickObj, handscore);
                            break;
                        }

                        else
                        {
                            print("태그가 다름");
                            FlipAction(hand, handscore);
                        }
                    }

                    break;
                }

            case 2:
                {
                    print("일단 맞음");
                    hittedCard = GetHitCard(clickObj); // 자리만 옮기기
                    print(hittedCard.name);
                    CardManager.instance.FlipCard();

                    if (!CardManager.instance.isFlip)
                    {
                        CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(hittedCard)]--;
                        CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(clickObj)]--;

                        EmptyFieldPosition(hittedCard);

                        MoveFieldScoreField(hittedCard, handscore);
                        MoveFieldScoreField(clickObj, handscore);
                        break;
                    }

                    else
                    {
                        if (CardManager.instance.field[CardManager.instance.field.Count - 1].CompareTag(clickObj.tag)) // 같은거 맞음
                        {
                            //뻑
                            //print(clickObj.name);
                            SetNextPosition(clickObj, CardManager.instance.field[CardManager.instance.field.Count - 1]);
                            //CardManager.instance.field[CardManager.instance.field.Count - 1].transform.position = new Vector3(gameObject.transform.position.x + 0.5f, gameObject.transform.position.y, gameObject.transform.position.z - 0.1f);//뒤집은 카드는 내가 냈던 카드 옆으로 위치 이동                                                                                                                                                                                        //    gameObject.transform.position.z - 0.1f), 0.5f).SetEase(Ease.OutQuint);//뒤집은 카드는 내가 냈던 카드 옆으로 위치 이동
                            print("뻑");
                            break;
                        }

                        else
                        {
                            if (HandCheck(hand, clickObj).Length == 2) // 내손에 같은 카드가 3장일 때 
                            {
                                //print("handCheck");
                                GameObject[] temp = HandCheck(hand, clickObj);

                                hand.Remove(temp[0]);
                                hand.Remove(temp[1]);

                                CardManager.instance.ResetPosition(hand);

                                SetNextPosition(clickObj, temp[0]);
                                SetNextPosition(temp[0], temp[1]);

                                CardManager.instance.field.Add(temp[0]);
                                CardManager.instance.field.Add(temp[1]);

                                MoveFieldScoreField(temp[0], handscore);
                                MoveFieldScoreField(temp[1], handscore);

                                CardManager.instance.DrawBombCard(hand);
                            }

                            EmptyFieldPosition(hittedCard);

                            CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(hittedCard)]--;
                            CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(clickObj)]--;

                            MoveFieldScoreField(hittedCard, handscore);
                            MoveFieldScoreField(clickObj, handscore);


                            FlipAction(hand, handscore);
                        }
                    }

                    break;
                }


            case 3:
                {
                    print("2개중 고르기");
                    FlipChoiceCard(clickObj);
                    CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(clickObj)]--;

                    CardManager.instance.FlipCard();

                    if (!CardManager.instance.isFlip)
                    {
                        MoveFieldScoreField(clickObj, handscore);
                        AfterFlipChoiceCard();
                        break;
                    }

                    else
                    {
                        if (CardManager.instance.field[CardManager.instance.field.Count - 1].CompareTag(clickObj.tag)) // 같은거 맞음
                        {
                            //폭탄
                            print("폭탄입니다.");

                            GameObject[] temp = new GameObject[3];
                            temp = FlipBombCard(CardManager.instance.field[CardManager.instance.field.Count - 1]);

                            EmptyFieldPosition(OrignFieldPosition(temp));

                            MoveBombFieldScoreField(temp, CardManager.instance.field[CardManager.instance.field.Count - 1], handscore);
                            break;
                        }

                        else
                        {
                            print("2개중 하나 고름");
                            MoveFieldScoreField(clickObj, handscore);
                            AfterFlipChoiceCard();

                            FlipAction(hand, handscore);
                        }
                        break;
                    }

                }
            case 4:
                {
                    //폭탄
                    print("폭탄");
                    GameObject[] temp = new GameObject[3];

                    temp = FlipBombCard(clickObj);

                    EmptyFieldPosition(OrignFieldPosition(temp));

                    MoveBombFieldScoreField(temp, clickObj, handscore);

                    CardManager.instance.FlipCard();

                    if (!CardManager.instance.isFlip)
                    {
                        break;
                    }

                    else
                    {
                        if (!(CardManager.instance.field[CardManager.instance.field.Count - 1].CompareTag(clickObj.tag))) // 다른거 맞음
                        {
                            FlipAction(hand, handscore);
                        }

                        break;
                    }

                }

        }
    }


    void FlipAction(List<GameObject> hand, List<GameObject> handscore)
    {
        if(CardManager.instance.field[CardManager.instance.field.Count - 1].tag == "Bonus")
        {

            //print("bonus card!");
            NoMatchField(CardManager.instance.field[CardManager.instance.field.Count - 1]); //빈공간에 내가 둘 카드 둠

            EmptyFieldPosition(CardManager.instance.field[CardManager.instance.field.Count - 1]); // 빈공간 만들어줌

            MoveFieldScoreField(CardManager.instance.field[CardManager.instance.field.Count - 1], handscore); // 피로 옮김

            CardManager.instance.FlipCard();
            //GameManager.instance.isBonus = true;
            //CardManager.instance.ResetPosition(hand);

            //CardManager.instance.DrawCard(hand, 1);
            //GameManager.instance.oneTime = false;
            //CardManager.instance.FlipCard();

            FlipAction(hand, handscore);
        }
       
        else
        {
            switch (CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(CardManager.instance.field[CardManager.instance.field.Count - 1])]) // 뒤집은 카드의 같은 태그 개수 별
            {
                case 1:
                    {
                        print("+ 뒤집은것도 안맞음");
                        NoMatchField(CardManager.instance.field[CardManager.instance.field.Count - 1]); //빈공간에 내가 둘 카드 둠
                        break;//턴 넘기기
                    }

                case 2:
                    {
                        print(" + 뒤집은거 맞음");
                        /*다른거 두개 맞았을 때 -> 무조건 같은 태그 오브젝트 1개 존재*/
                        hittedCard = GetHitCard(CardManager.instance.field[CardManager.instance.field.Count - 1]); // 뒤집은 카드와 맞은 카드 찾음
                        //print(hittedCard.name);

                        EmptyFieldPosition(hittedCard);

                        CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(hittedCard)]--;
                        CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(CardManager.instance.field[CardManager.instance.field.Count - 1])]--;

                        //점수판으로 위치 옮기기
                        MoveFieldScoreField(hittedCard, handscore);
                        MoveFieldScoreField(CardManager.instance.field[CardManager.instance.field.Count - 1], handscore);
                        break;
                    }

                case 3:
                    {
                        /*다른거 세개 맞았을 때 -> 무조건 2개 존재*/
                        print(" + 둘 중 하나 고름"); ;
                        FlipChoiceCard(CardManager.instance.field[CardManager.instance.field.Count - 1]);

                        CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(CardManager.instance.field[CardManager.instance.field.Count - 1])]--;

                        MoveFieldScoreField(CardManager.instance.field[CardManager.instance.field.Count - 1], handscore);

                        AfterFlipChoiceCard();
                        break;
                    }

                case 4:
                    {
                        //뒤집은 칻으가 폭탄
                        print(" + 폭탄");

                        GameObject[] temp = new GameObject[3];

                        temp = FlipBombCard(CardManager.instance.field[CardManager.instance.field.Count - 1]);

                        EmptyFieldPosition(OrignFieldPosition(temp));

                        MoveBombFieldScoreField(temp, CardManager.instance.field[CardManager.instance.field.Count - 1], handscore);
                        break;
                    }

            }
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
                //print(CardManager.instance.field[index].name);
                //obj.transform.DOMove(new Vector3(CardManager.instance.field[i].transform.position.x + 0.5f, CardManager.instance.field[i].transform.position.y, CardManager.instance.field[i].transform.position.z - 0.1f), 0.5f).SetEase(Ease.OutQuint);
            }
        }
        return CardManager.instance.field[index];
    }
    void FlipChoiceCard(GameObject obj)
    {
        CardManager.instance.ChoiceObj.Clear();
        for (int i = 0; i < CardManager.instance.field.Count - 1; i++) // 카운트에서 1빼는 이유 -> 비교할 태그가 있음
        {
            if (CardManager.instance.field[i].CompareTag(obj.tag))// 태그가 같을 때 
            {
                CardManager.instance.ChoiceObj.Add(CardManager.instance.field[i]);
            }

        }
        CardManager.instance.ChoiceObj.Remove(obj);

        //print(CardManager.instance.ChoiceObj.Count);
        //같은 카드 다음 포지션은 같은 태그의 갯수 * 0.5 만큼 x축을 더해준다.
        float max = CardManager.instance.ChoiceObj[0].transform.position.x;
        for(int i=0;i<CardManager.instance.ChoiceObj.Count;i++)
        {
            if(CardManager.instance.ChoiceObj[i].transform.position.x > max)
            {
                max = CardManager.instance.ChoiceObj[i].transform.position.x;
            }
        }

        //obj.transform.position = new Vector3(Math.Max(CardManager.instance.ChoiceObj[0].transform.position.x, CardManager.instance.ChoiceObj[1].transform.position.x) + 0.5f,
        //    CardManager.instance.ChoiceObj[0].transform.position.y, CardManager.instance.ChoiceObj[0].transform.position.z - 0.1f * 3);

        obj.transform.position = new Vector3(max + 0.5f, CardManager.instance.ChoiceObj[0].transform.position.y, CardManager.instance.ChoiceObj[0].transform.position.z - 0.3f);
    }

    void AfterFlipChoiceCard()
    {
        OpenChoicePanel = GameObject.Find("Canvas").transform.Find("Panel").gameObject;
        OpenChoicePanel.SetActive(true);
        OpenChoicePanel.transform.GetChild(1).GetComponent<Image>().sprite = CardManager.instance.ChoiceObj[0].GetComponent<SpriteRenderer>().sprite;
        OpenChoicePanel.transform.GetChild(2).GetComponent<Image>().sprite = CardManager.instance.ChoiceObj[1].GetComponent<SpriteRenderer>().sprite;
    }
    GameObject[] FlipBombCard(GameObject obj)//뒤집은 카든
    {
        CardManager.instance.BombObj.Clear();

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

        //print(obj.name);

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

    void Chu(GameObject clickObj, List<GameObject> score)
    {

        EmptyFieldPosition(clickObj);
        GameManager.instance.isMoving = true;
        CardManager.instance.field[CardManager.instance.field.Count - 1].transform.DOMove(new Vector3(clickObj.transform.position.x + 0.5f, clickObj.transform.position.y, clickObj.transform.position.z - 0.1f), 0.5f).SetEase(Ease.OutQuint); //맞춘 오브젝트 옆으로 이동
        StartCoroutine(GameManager.instance.CompleteMoving());

        //점수판으로 이동


        GameManager.instance.isMoving = true;
        CardManager.instance.field[CardManager.instance.field.Count - 1].transform.DOMove(CardManager.instance.ScoreField(CardManager.instance.field[CardManager.instance.field.Count - 1], score), 0.5f).SetEase(Ease.OutQuint); // 점수판위치로 이동
        StartCoroutine(GameManager.instance.CompleteMoving());

        GameManager.instance.isMoving = true;
        clickObj.transform.DOMove(CardManager.instance.ScoreField(clickObj, score), 0.5f).SetEase(Ease.OutQuint); // 점수판위치로 이동
        StartCoroutine(GameManager.instance.CompleteMoving());

        //얻은 카드
        score.Add(CardManager.instance.field[CardManager.instance.field.Count - 1]);
        CardManager.instance.field.Remove(CardManager.instance.field[CardManager.instance.field.Count - 1]);

        score.Add(clickObj);
        CardManager.instance.field.Remove(clickObj);
    }

    void NoMatchField(GameObject obj)
    {
        //print("no match");
        

        //print("0 번 값: " + CardManager.instance.emptyIndex[0]);
        GameManager.instance.isMoving = true;
        obj.transform.DOMove(CardManager.instance.fieldPosition[CardManager.instance.emptyIndex[0]], 0.5f).SetEase(Ease.OutQuint); // 마지막 필드포지션은 빈곳에 넣음
        StartCoroutine(GameManager.instance.CompleteMoving());

        CardManager.instance.emptyIndex.RemoveAt(0);

        CardManager.instance.EmptyIndexSort();//빈곳 인덱스 오름차순 정렬
        for (int i = 0; i < CardManager.instance.emptyIndex.Count; i++)
        {
            //print("남은 카드 인덱스 " + i + " : " + CardManager.instance.emptyIndex[i]);
        }

    }

    public void MoveFieldScoreField(GameObject moveObj, List<GameObject> score)
    {
        GameManager.instance.isMoving = true;
        moveObj.transform.DOMove(CardManager.instance.ScoreField(moveObj, score), 0.5f).SetEase(Ease.OutQuint); // 점수판 위치 이동
        StartCoroutine(GameManager.instance.CompleteMoving());
        //print(score.Count);
        score.Add(moveObj); // 점수에 더해주기 
        //print(score.Count);
        if (CardManager.instance.field.Contains(moveObj))
        {
            CardManager.instance.field.Remove(moveObj); // 필드에서 지우기
        }
    }

    void MoveBombFieldScoreField(GameObject[] bombObj, GameObject card, List<GameObject> score)
    {
        for (int i = 0; i < bombObj.Length; i++)
        {
            GameManager.instance.isMoving = true;
            bombObj[i].transform.DOMove(CardManager.instance.ScoreField(bombObj[i], score), 0.5f).SetEase(Ease.OutQuint); // 점수 필드로 위치 옮김
            StartCoroutine(GameManager.instance.CompleteMoving());
            //내 점수리스트 add 
            score.Add(bombObj[i]);//내 점수필드 리스트에 추가

            //필드에서 삭제
            CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(bombObj[i])]--;//필드에서 제거
            CardManager.instance.field.Remove(bombObj[i]);//필드에서 제거
        }

        //card.transform.position = CardManager.instance.ScoreField(card, score);
        GameManager.instance.isMoving = true;
        card.transform.DOMove(CardManager.instance.ScoreField(card, score), 0.5f).SetEase(Ease.OutQuint);
        StartCoroutine(GameManager.instance.CompleteMoving());
        //내 점수리스트 add 
        score.Add(card);//내 점수필드 리스트에 추가

        //필드에서 삭제
        CardManager.instance.field.Remove(card);//필드에서 제거
        CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(card)]--;//필드에서 제거

    }

    public void EmptyFieldPosition(GameObject obj)
    {
        int index = Array.IndexOf(CardManager.instance.fieldPosition, obj.transform.position);

        if (!CardManager.instance.emptyIndex.Contains(index)) // 원래 가지고 있는 값이 아니면 // 값이 없으면 음수로 들어가서 음수는 빼줘야함
        {
            if (index >= 0)
            {
                CardManager.instance.emptyIndex.Add(index);
            }
        }

        CardManager.instance.EmptyIndexSort();//빈곳 인덱스 오름차순 정렬
        for (int i=0;i<CardManager.instance.emptyIndex.Count;i++)
        {
            //print("남은 카드 인덱스 " + i + " : " + CardManager.instance.emptyIndex[i]);
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

    private GameObject[] HandCheck(List<GameObject> hand, GameObject obj)
    {
        List<GameObject> temp = new List<GameObject>();
        foreach (GameObject checkObj in hand)
        {
            if (checkObj.tag == obj.tag)
            {
                temp.Add(checkObj);
            }
        }

        if (temp.Contains(obj))
        {
            temp.Remove(obj);
        }

        int count = temp.Count;
        GameObject[] result = new GameObject[count];
        for (int i = 0; i < count; i++)
        {
            result[i] = temp[i];
        }
        return result;
    }

    private void SetNextPosition(GameObject baseObj, GameObject obj)
    {
        obj.transform.position = new Vector3(baseObj.transform.position.x + 0.5f, baseObj.transform.position.y, baseObj.transform.position.z - 0.1f);
    }

    public void ApplyShake()
    {
        ShakePanel = GameObject.Find("Canvas").transform.Find("ShakePanel").gameObject;
        //흔들기 전투력 증가
        ShakePanel.SetActive(false);

        if (GameManager.instance.isMyTurn)
        {
            //print(CardManager.instance.curObj.name);
            CardManager.instance.ResetPosition(CardManager.instance.myHand);
            CardManager.instance.myHand.Remove(CardManager.instance.curObj);//내손에서 지우기
            CheckCardAction(CardManager.instance.curObj, CardManager.instance.myHand, CardManager.instance.myHandScore);

            EndArrange(CardManager.instance.myHand, false);
        }

        else
        {
            //print(CardManager.instance.curObj.name);
            CardManager.instance.ResetPosition(CardManager.instance.opponentHand);
            CardManager.instance.opponentHand.Remove(CardManager.instance.curObj);//내손에서 지우기
            CheckCardAction(CardManager.instance.curObj, CardManager.instance.opponentHand, CardManager.instance.opponentHandScore);

            EndArrange(CardManager.instance.opponentHand, true);
        }
    }
    void EndArrange(List<GameObject> hand, bool isPlayer)
    {
        if (GameManager.instance.first)
        {
            print("first in ");
            CardManager.instance.ResetPosition(hand);
            CardManager.instance.DrawCard(hand, 1);
            GameManager.instance.first = false;
        } // 처음만

        GameManager.instance.isMyTurn = isPlayer;
        GameManager.instance.oneTime = true;
    }
    
}
