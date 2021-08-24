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
    private void Start()
    {
        switch (name)
        {
            //큐브  
            case "0(Clone)":
            case "8(Clone)":
            case "28(Clone)":
            case "40(Clone)":
            case "44(Clone)":
                type = "큐브";
                break;

            case "1(Clone)":
            case "5(Clone)":
            case "9(Clone)":
                type = "붉은 크리스탈";
                break;

            case "21(Clone)":
            case "33(Clone)":
            case "37(Clone)":
                type = "파란 크리스탈";
                break;

            case "13(Clone)":
            case "17(Clone)":
            case "25(Clone)":
            case "41(Clone)":
                type = "초록 크리스탈";
                break;

            case "4(Clone)":
            case "12(Clone)":
            case "29(Clone)":
                type = "암흑 오브";
                break;

            case "16(Clone)":
            case "20(Clone)":
            case "24(Clone)":
            case "32(Clone)":
            case "36(Clone)":
            case "45(Clone)":
                type = "파란 오브";
                break;

            default:
                type = "일반";
                break;
        }
    }

    public void OnMouseUp()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (GameManager.instance.isSetting && !GameManager.instance.isMoving) // 셋팅이 끝나고 카드 움직이 끝났을 때 시작
            {
                #region PlayerTurn	
                if (GameManager.instance.isMyTurn == true)//만약 플레이어 턴이면	
                {
                    if(CardManager.instance.myHand.Contains(gameObject)) // 내손에 내가 클릭한 게임오브젝트가 있어야함
                    {
                        WhoTurn(CardManager.instance.myHand, CardManager.instance.myHandScore); // 턴 진행

                        if (!GameManager.instance.isShake && !GameManager.instance.isChoice && !GameManager.instance.isBonus) //흔들기, 선택, 보느스 카드가 아닐때만
                        {
                            CardManager.instance.ArrangeHandPosition(CardManager.instance.myHand); // 내손에 있는 카드 포지션 sort

                            GameManager.instance.ScoreCheck(true); // 점수 체크 뒤 아티팩트 on off 결정

                            if (!GameManager.instance.ArtifactPanel.activeSelf) // 만약 아티팩트창이 안 켜져 있다면
                            {
                                GameManager.instance.AttackPanel.SetActive(true); // 공격 판넬 띄움 -> 공격하는 도중에는 클릭을 ui로 막으려고 판넬 on 
                             
                                StartCoroutine(GameManager.instance.FixedMade(CardManager.instance.kingEmptyIndex, CardManager.instance.enemyKingEmptyIndex, 
                                    CardManager.instance.redFlagEmptyIndex, CardManager.instance.blueFlagEmptyIndex, CardManager.instance.normalFlagEmptyIndex,
                                CardManager.instance.animalEmptyIndex, CardManager.instance.thingEmptyIndex, 
                                BattleSystem.instance.kingAttack, BattleSystem.instance.flagAttack, BattleSystem.instance.animalAttack, 
                                BattleSystem.instance.op, BattleSystem.instance.opponentInfo, BattleSystem.instance.opHUD)); // 고정 데미지 주기

                                EndArrange(CardManager.instance.myHand, false); // 턴 넘김
                            }
                        }

                        GameManager.instance.isBonus = false;
                    }

                    else
                    {
                        print("실행안됨");
                    }
                  
                }
                #endregion

                #region OpponentTurn	
                else//만약 상대 턴이면	
                {
                    if (CardManager.instance.opponentHand.Contains(gameObject)) // 내손에 이 게임오브젝트가 있을 때
                    {
                        WhoTurn(CardManager.instance.opponentHand, CardManager.instance.opponentHandScore);
               
                        if (!GameManager.instance.isShake && !GameManager.instance.isChoice && !GameManager.instance.isBonus)
                        {
                            CardManager.instance.ArrangeHandPosition(CardManager.instance.opponentHand);
                            GameManager.instance.ScoreCheck(false);

                            if (!GameManager.instance.ArtifactPanel.activeSelf)
                            {
                                GameManager.instance.AttackPanel.SetActive(true);
                               
                                StartCoroutine(GameManager.instance.FixedMade(CardManager.instance.enemyKingEmptyIndex, CardManager.instance.kingEmptyIndex, 
                                    CardManager.instance.enemyRedFlagEmptyIndex, CardManager.instance.enemyBlueFlagEmptyIndex, 
                                    CardManager.instance.enemyNormalFlagEmptyIndex, CardManager.instance.enemyAnimalEmptyIndex, 
                                    CardManager.instance.enemyThingEmptyIndex, BattleSystem.instance.enemyKingAttack, 
                                    BattleSystem.instance.enemyFlagAttack, BattleSystem.instance.enemyAnimalAttack, 
                                    BattleSystem.instance.player, BattleSystem.instance.playerInfo, BattleSystem.instance.playerHUD));

                                EndArrange(CardManager.instance.opponentHandScore, true);
                            }

                        }
                        GameManager.instance.isBonus = false;
                    }

                    else
                    {
                        print("실행안됨");
                    }
                 
                }
                #endregion

            }

        }
    }

   
    void WhoTurn(List<GameObject> hand, List<GameObject> handscore)
    {
        if (gameObject.name == "Bomb(Clone)") // 선택한 카드가 폭탄 카드일 때
        {
            CardManager.instance.hitCardCount = 1;

            hand.Remove(gameObject); //내 손에서 게임 오브젝트 지움

            CardAction(gameObject, hand, handscore);

            GameManager.instance.isMoving = true;
            gameObject.transform.DOMove(new Vector3(-1f, 0f, 0f), 0.5f).SetEase(Ease.OutQuint); //맞춘 오브젝트 옆으로 이동
            StartCoroutine(GameManager.instance.CompleteMoving()); // 오브젝트 이동하는 동안 코루틴 켜서 클릭 못하게 만듬
        }

        else if(gameObject.tag == "Bonus") // 선택한 카드가 보너스 카드일 때
        {
            GameManager.instance.isBonus = true;
            print("bonus card");

            MoveFieldScoreField(gameObject, handscore); // 보너스 카드를 점수 패로 옮김

            hand.Remove(gameObject); // 손에서 게임 오브젝트 제거
            //보너스니까 한장 뽑기
            CardManager.instance.DrawCard(hand, 1); // 한장 더 드로우

        }

        else // 나머지 일반 카드일 때
        {
            CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(gameObject)]++; // 내가 낸 카드 ++ 해줌
            CardManager.instance.hitCardCount = CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(gameObject)];
            CardManager.instance.field.Add(gameObject); //내손에 있는 카드 필드에 넣기

            if (HandCheck(hand, gameObject).Length == 2 && CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(gameObject)] == 1) // 내손에 같은 카드가 3장일 때 흔들기
            {
                print("흔들기");
                GameManager.instance.isShake = true;
                ShakePanel = GameObject.Find("Canvas").transform.Find("ShakePanel").gameObject;
                ShakePanel.SetActive(true);
                CardManager.instance.curObj = gameObject;
            }

            else
            {
                hand.Remove(gameObject);//내손에서 지우기

                CardAction(gameObject, hand, handscore); // 카드 행동 시작
            }
        }
     
    }

    void CardAction(GameObject clickObj, List<GameObject> hand, List<GameObject> handscore) // 맞춘 카드마다 다른 행동
    {
        switch (CardManager.instance.hitCardCount) // 2, 3, 4개 맞췄을 시 다름
        {
            //카드가 안맞았을 때
            case 1:
                {
                    print("일단 안맞음");
                    /*카드의 위치는 빈공간으로 간다.*/

                    // 폭탄 카드나 보너스 카드일 때는 따로 카드를 필드에 둘 필요가 없기때문에 nomatchfield함수를 사용하지 않는다.

                    if (clickObj.name != "Bomb(Clone)" && clickObj.tag != "Bonus") //일반 카드는 맞지 않으면 nomatchfield
                    {
                        NoMatchField(clickObj);
                    }
                    
                    CardManager.instance.FlipCard();

                    if (!CardManager.instance.isFlip) // 플립할 카드가 없다면 멈춤
                    {
                        break;
                    }

                    else
                    {
                        if (CardManager.instance.field[CardManager.instance.field.Count - 1].CompareTag(clickObj.tag)) // 뒤집은 카드랑 내가 냈던 카드랑 같으면
                        {
                            print("쪽");
                            CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(clickObj)]--; // 내가 낸 카드
                            CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(CardManager.instance.field[CardManager.instance.field.Count - 1])]--; //뒤집은 카드 필드에서 빼기 //뒤집은 카드랑

                            Chu(clickObj, handscore); //쪽 함수 실행
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
                            SetNextPosition(clickObj, CardManager.instance.field[CardManager.instance.field.Count - 1]);
                            CardManager.instance.DeleteOutline(clickObj);
                            print("뻑");
                            break;
                        }

                        else
                        {
                            if (HandCheck(hand, clickObj).Length == 2) // 내손에 같은 카드가 3장일 때 
                            {
                                GameObject[] temp = HandCheck(hand, clickObj);

                                hand.Remove(temp[0]);
                                hand.Remove(temp[1]);

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



    void NoMatchField(GameObject obj)
    {
        GameManager.instance.isMoving = true;
        obj.transform.DOMove(CardManager.instance.fieldPosition[CardManager.instance.emptyIndex[0]], 0.5f).SetEase(Ease.OutQuint); // 마지막 필드포지션은 빈곳에 넣음
        StartCoroutine(GameManager.instance.CompleteMoving());

        CardManager.instance.emptyIndex.RemoveAt(0);

        CardManager.instance.EmptyIndexSort();//빈곳 인덱스 오름차순 정렬
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
        CardManager.instance.choiceObj.Clear();

        for (int i = 0; i < CardManager.instance.field.Count - 1; i++) // 카운트에서 1빼는 이유 -> 비교할 태그가 있음
        {
            if (CardManager.instance.field[i].CompareTag(obj.tag))// 태그가 같을 때 
            {
                CardManager.instance.choiceObj.Add(CardManager.instance.field[i]);
            }

        }
        CardManager.instance.choiceObj.Remove(obj);

        //같은 카드 다음 포지션은 같은 태그의 갯수 * 0.5 만큼 x축을 더해준다.
        float max = CardManager.instance.choiceObj[0].transform.position.x;
        for(int i=0;i<CardManager.instance.choiceObj.Count;i++)
        {
            if(CardManager.instance.choiceObj[i].transform.position.x > max)
            {
                max = CardManager.instance.choiceObj[i].transform.position.x;
            }
        }

        obj.transform.position = new Vector3(max + 0.5f, CardManager.instance.choiceObj[0].transform.position.y, CardManager.instance.choiceObj[0].transform.position.z - 0.3f);
    }

    void AfterFlipChoiceCard()
    {
        GameManager.instance.isChoice = true;

        OpenChoicePanel = GameObject.Find("Canvas").transform.Find("Panel").gameObject;
        OpenChoicePanel.SetActive(true);
        OpenChoicePanel.transform.GetChild(1).GetComponent<Image>().sprite = CardManager.instance.choiceObj[0].GetComponent<SpriteRenderer>().sprite;
        OpenChoicePanel.transform.GetChild(2).GetComponent<Image>().sprite = CardManager.instance.choiceObj[1].GetComponent<SpriteRenderer>().sprite;
    }
    GameObject[] FlipBombCard(GameObject obj)//뒤집은 카든
    {
        CardManager.instance.bombObj.Clear();

        for (int i = 0; i < CardManager.instance.field.Count - 1; i++) // 카운트에서 1빼는 이유 -> 비교할 태그가 있음
        {
            if (CardManager.instance.field[i].CompareTag(obj.tag)) // 태그가 같을 때 
            {
                CardManager.instance.bombObj.Add(CardManager.instance.field[i]);
            }

        }
        if (CardManager.instance.bombObj.Contains(obj))
        {
            CardManager.instance.bombObj.Remove(obj);
        }


        obj.transform.position = new Vector3(Math.Max(Math.Max(CardManager.instance.bombObj[0].transform.position.x, CardManager.instance.bombObj[1].transform.position.x), CardManager.instance.bombObj[2].transform.position.x) + 0.5f,
                                                                                    CardManager.instance.bombObj[0].transform.position.y, CardManager.instance.bombObj[0].transform.position.z - 0.1f * 3);// 필드에 먼저 놔둠
        //obj.transform.DOMove(new Vector3(Math.Max(Math.Max(BombObj[0].transform.position.x, BombObj[1].transform.position.x), BombObj[2].transform.position.x) + 0.5f,
        //                                                                            BombObj[0].transform.position.y, BombObj[0].transform.position.z - 0.1f * 3), 0.5f).SetEase(Ease.OutQuint);// 필드에 먼저 놔둠

        GameObject[] temp = new GameObject[3];

        for (int i = 0; i < temp.Length; i++)
        {
            temp[i] = CardManager.instance.bombObj[i];
        }

        return temp;
    }

    void Chu(GameObject clickObj, List<GameObject> score)
    {
        EmptyFieldPosition(clickObj);
        GameManager.instance.isMoving = true;
        CardManager.instance.field[CardManager.instance.field.Count - 1].transform.DOMove(new Vector3(clickObj.transform.position.x + 0.5f, clickObj.transform.position.y, clickObj.transform.position.z - 0.1f), 0.5f).SetEase(Ease.OutQuint); //맞춘 오브젝트 옆으로 이동
        StartCoroutine(GameManager.instance.CompleteMoving());

        GameManager.instance.isMoving = true;
        CardManager.instance.field[CardManager.instance.field.Count - 1].transform.DOMove(CardManager.instance.ScoreField(CardManager.instance.field[CardManager.instance.field.Count - 1], score), 0.5f).SetEase(Ease.OutQuint); // 점수판위치로 이동
        StartCoroutine(GameManager.instance.CompleteMoving());

        GameManager.instance.isMoving = true;
        clickObj.transform.DOMove(CardManager.instance.ScoreField(clickObj, score), 0.5f).SetEase(Ease.OutQuint); // 점수판위치로 이동
        StartCoroutine(GameManager.instance.CompleteMoving());

        score.Add(CardManager.instance.field[CardManager.instance.field.Count - 1]);
        CardManager.instance.field.Remove(CardManager.instance.field[CardManager.instance.field.Count - 1]);

        score.Add(clickObj);
        CardManager.instance.field.Remove(clickObj);
    }

    public void MoveFieldScoreField(GameObject moveObj, List<GameObject> score)
    {
        if(CardManager.instance.GetCardTagNum(moveObj) != 13)
        {
            CardManager.instance.DeleteOutline(moveObj);
        }
        
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
            CardManager.instance.DeleteOutline(bombObj[i]);
            GameManager.instance.isMoving = true;
            bombObj[i].transform.DOMove(CardManager.instance.ScoreField(bombObj[i], score), 0.5f).SetEase(Ease.OutQuint); // 점수 필드로 위치 옮김
            StartCoroutine(GameManager.instance.CompleteMoving());
            //내 점수리스트 add 
            score.Add(bombObj[i]);//내 점수필드 리스트에 추가

            //필드에서 삭제
            CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(bombObj[i])]--;//필드에서 제거
            CardManager.instance.field.Remove(bombObj[i]);//필드에서 제거
        }

        if (CardManager.instance.GetCardTagNum(card) != 13)
        {
            CardManager.instance.DeleteOutline(card);
        }

        GameManager.instance.isMoving = true;
        card.transform.DOMove(CardManager.instance.ScoreField(card, score), 0.5f).SetEase(Ease.OutQuint);
        StartCoroutine(GameManager.instance.CompleteMoving());
        //내 점수리스트 add 
        score.Add(card);//내 점수필드 리스트에 추가

        //필드에서 삭제
        CardManager.instance.field.Remove(card);//필드에서 제거
        CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(card)]--;//필드에서 제거

    }

    public void EmptyFieldPosition(GameObject obj) //받은 게임오브젝트의 필드 자리를 비워줌
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

        for (int i = 0; i < CardManager.instance.choiceObj.Count; i++)
        {
            temp[i] = CardManager.instance.choiceObj[i];
        }

        if (GameManager.instance.isMyTurn)
        {
            MoveFieldScoreField(CardManager.instance.choiceObj[num], CardManager.instance.myHandScore);
            CardManager.instance.ArrangeHandPosition(CardManager.instance.myHand);

            GameManager.instance.ScoreCheck(true);

            if (!GameManager.instance.ArtifactPanel.activeSelf)
            {
                print("panel no");
                GameManager.instance.AttackPanel.SetActive(true);
                StartCoroutine(GameManager.instance.FixedMade(CardManager.instance.kingEmptyIndex, CardManager.instance.enemyKingEmptyIndex, CardManager.instance.redFlagEmptyIndex, CardManager.instance.blueFlagEmptyIndex, CardManager.instance.normalFlagEmptyIndex,
                CardManager.instance.animalEmptyIndex, CardManager.instance.thingEmptyIndex, BattleSystem.instance.kingAttack, BattleSystem.instance.flagAttack, BattleSystem.instance.animalAttack, BattleSystem.instance.op, BattleSystem.instance.opponentInfo, BattleSystem.instance.opHUD));

                EndArrange(CardManager.instance.myHand, false);
            }

        }

        else
        {
            MoveFieldScoreField(CardManager.instance.choiceObj[num], CardManager.instance.opponentHandScore);
            CardManager.instance.ArrangeHandPosition(CardManager.instance.opponentHand);

            GameManager.instance.ScoreCheck(false);


            if (!GameManager.instance.ArtifactPanel.activeSelf)
            {
                print("panel no");
                GameManager.instance.AttackPanel.SetActive(true);
                StartCoroutine(GameManager.instance.FixedMade(CardManager.instance.enemyKingEmptyIndex, CardManager.instance.kingEmptyIndex, CardManager.instance.enemyRedFlagEmptyIndex, CardManager.instance.enemyBlueFlagEmptyIndex, CardManager.instance.enemyNormalFlagEmptyIndex,
                CardManager.instance.enemyAnimalEmptyIndex, CardManager.instance.enemyThingEmptyIndex, BattleSystem.instance.enemyKingAttack, BattleSystem.instance.enemyFlagAttack, BattleSystem.instance.enemyAnimalAttack, BattleSystem.instance.player, BattleSystem.instance.playerInfo, BattleSystem.instance.playerHUD));

                EndArrange(CardManager.instance.opponentHand, true);
            }

        }

        CardManager.instance.choiceObj[1 - num].transform.position = OrignFieldPosition(temp).transform.position;

        CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(CardManager.instance.choiceObj[num])]--;
        CardManager.instance.field.Remove(CardManager.instance.choiceObj[num]);

        CardManager.instance.choiceObj.Clear();
      
        GameManager.instance.isChoice = false;
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
            
            CardManager.instance.myHand.Remove(CardManager.instance.curObj);//내손에서 지우기
            CardAction(CardManager.instance.curObj, CardManager.instance.myHand, CardManager.instance.myHandScore);

            GameManager.instance.ScoreCheck(true);

            if (!GameManager.instance.ArtifactPanel.activeSelf)
            {
                print("panel no");
                GameManager.instance.AttackPanel.SetActive(true);

                StartCoroutine(GameManager.instance.FixedMade(CardManager.instance.kingEmptyIndex, CardManager.instance.enemyKingEmptyIndex, CardManager.instance.redFlagEmptyIndex, CardManager.instance.blueFlagEmptyIndex, CardManager.instance.normalFlagEmptyIndex,
                CardManager.instance.animalEmptyIndex, CardManager.instance.thingEmptyIndex, BattleSystem.instance.kingAttack, BattleSystem.instance.flagAttack, BattleSystem.instance.animalAttack, BattleSystem.instance.op, BattleSystem.instance.opponentInfo, BattleSystem.instance.opHUD));
                
                EndArrange(CardManager.instance.myHand, false);
            }

            
        }

        else
        {
            //print(CardManager.instance.curObj.name);
            CardManager.instance.opponentHand.Remove(CardManager.instance.curObj);//내손에서 지우기
            CardAction(CardManager.instance.curObj, CardManager.instance.opponentHand, CardManager.instance.opponentHandScore);

            GameManager.instance.ScoreCheck(false);

            if (!GameManager.instance.ArtifactPanel.activeSelf)
            {
                print("panel no");
                GameManager.instance.AttackPanel.SetActive(true);

                StartCoroutine(GameManager.instance.FixedMade(CardManager.instance.enemyKingEmptyIndex, CardManager.instance.kingEmptyIndex, CardManager.instance.enemyRedFlagEmptyIndex, CardManager.instance.enemyBlueFlagEmptyIndex, CardManager.instance.enemyNormalFlagEmptyIndex,
                CardManager.instance.enemyAnimalEmptyIndex, CardManager.instance.enemyThingEmptyIndex, BattleSystem.instance.enemyKingAttack, BattleSystem.instance.enemyFlagAttack, BattleSystem.instance.enemyAnimalAttack, BattleSystem.instance.player, BattleSystem.instance.playerInfo, BattleSystem.instance.playerHUD));

                EndArrange(CardManager.instance.opponentHand, true);
            }

           
        }
        GameManager.instance.isShake = false;
    }
    public void EndArrange(List<GameObject> hand, bool isPlayer)
    {
        CardManager.instance.ArrangeHandPosition(hand);
        if (GameManager.instance.first)
        {
            print("first in ");
            CardManager.instance.DrawCard(hand, 1);
            GameManager.instance.first = false;
        } // 처음만

        GameManager.instance.isMyTurn = isPlayer;
        GameManager.instance.oneTime = true;
    }
    
}
