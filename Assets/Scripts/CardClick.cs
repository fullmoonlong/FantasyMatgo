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
            if (GameManager.instance.isSetting) // 셋팅이 끝나고 카드 움직이 끝났을 때 시작
            {
                #region PlayerTurn	
                if (GameManager.instance.isMyTurn == true)//만약 플레이어 턴이면	
                {
                    if(CardManager.instance.myHand.Contains(gameObject)) // 내손에 내가 클릭한 게임오브젝트가 있어야함
                    {
                        CardManager.instance.curHand = CardManager.instance.myHand;
                        CardManager.instance.curHandScore = CardManager.instance.myHandScore;

                        WhoTurn(CardManager.instance.curHand, CardManager.instance.curHandScore); // 턴 진행
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
                        CardManager.instance.curHand = CardManager.instance.opponentHand;
                        CardManager.instance.curHandScore = CardManager.instance.opponentHandScore;

                        WhoTurn(CardManager.instance.curHand, CardManager.instance.curHandScore);
               
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

   public void OnOffPanel(bool status)
    {
        BattleSystem.instance.actionPanel.SetActive(status);
    }

    public void FixedSetting()
    {
        if (!GameManager.instance.isShake && !GameManager.instance.isChoice && !GameManager.instance.isBonus) //흔들기, 선택, 보느스 카드가 아닐때만
        {
            print("come");
            GameManager.instance.ScoreCheck(GameManager.instance.isMyTurn); // 점수 체크 뒤 아티팩트 on off 결정

            if (!GameManager.instance.ArtifactPanel.activeSelf) // 만약 아티팩트창이 안 켜져 있다면
            {
                GameManager.instance.AttackPanel.SetActive(true); // 공격 판넬 띄움 -> 공격하는 도중에는 클릭을 ui로 막으려고 판넬 on 

                if (GameManager.instance.isMyTurn)
                {
                    StartCoroutine(GameManager.instance.FixedMade(CardManager.instance.kingEmptyIndex, CardManager.instance.enemyKingEmptyIndex,
                    CardManager.instance.redFlagEmptyIndex, CardManager.instance.blueFlagEmptyIndex, CardManager.instance.normalFlagEmptyIndex,
                    CardManager.instance.animalEmptyIndex, CardManager.instance.thingEmptyIndex,
                    BattleSystem.instance.kingAttack, BattleSystem.instance.flagAttack, BattleSystem.instance.animalAttack,
                    BattleSystem.instance.op, BattleSystem.instance.opponentInfo, BattleSystem.instance.opHUD)); // 고정 데미지 주기

                }

                else
                {
                    StartCoroutine(GameManager.instance.FixedMade(CardManager.instance.enemyKingEmptyIndex, CardManager.instance.kingEmptyIndex,
                                   CardManager.instance.enemyRedFlagEmptyIndex, CardManager.instance.enemyBlueFlagEmptyIndex,
                                   CardManager.instance.enemyNormalFlagEmptyIndex, CardManager.instance.enemyAnimalEmptyIndex,
                                   CardManager.instance.enemyThingEmptyIndex, BattleSystem.instance.enemyKingAttack,
                                   BattleSystem.instance.enemyFlagAttack, BattleSystem.instance.enemyAnimalAttack,
                                   BattleSystem.instance.player, BattleSystem.instance.playerInfo, BattleSystem.instance.playerHUD));

                }
            }
        }

        GameManager.instance.isBonus = false;
    }
    void WhoTurn(List<GameObject> hand, List<GameObject> handscore)
    {
        OnOffPanel(true);

        CardManager.instance.curObj = gameObject;

        CardManager.instance.DeleteOutline(CardManager.instance.curObj); // 오브젝트의 아웃라인을 제거함

        if (gameObject.name == "Bomb(Clone)") // 선택한 카드가 폭탄 카드일 때
        {
            CardManager.instance.hitCardCount = 1;

            hand.Remove(gameObject); //내 손에서 게임 오브젝트 지움

            CardAction(gameObject, hand, handscore);

            gameObject.transform.DOMove(new Vector3(-1f, 0f, 0f), 0.5f).SetEase(Ease.OutQuint); //맞춘 오브젝트 옆으로 이동
        }

        else if(gameObject.tag == "Bonus") // 선택한 카드가 보너스 카드일 때
        {
            GameManager.instance.isBonus = true;
            print("bonus card");

            StartCoroutine(MoveFieldScoreField(gameObject, handscore)); // 보너스 카드를 점수 패로 옮김

            hand.Remove(gameObject); // 손에서 게임 오브젝트 제거
            //보너스니까 한장 뽑기
            CardManager.instance.DrawCard(hand, 1); // 한장 더 드로우

            GameManager.instance.isBonus = false;
        }

        else // 나머지 일반 카드일 때
        {
            CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(gameObject)]++; // 내가 낸 카드 ++ 해줌
            CardManager.instance.hitCardCount = CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(gameObject)];
            CardManager.instance.field.Add(gameObject); //내손에 있는 카드 필드에 넣기

            if (HandCheck(hand, gameObject).Length == 2 && CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(gameObject)] == 1) // 내손에 같은 카드가 3장일 때 흔들기
            {
                OnOffPanel(false);
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

                    StartCoroutine(FlipCardHit(clickObj, hand, handscore));

                    break;
                }

            case 2:
                {
                    print("일단 맞음");
                    hittedCard = GetHitCard(clickObj); // 자리만 옮기기
                    print(hittedCard.name);

                    if (HandCheck(hand, clickObj).Length == 2) // 내손에 같은 카드가 3장일 때 
                    {
                        print("real bomb");
                        GameObject[] temp = HandCheck(hand, clickObj);

                        hand.Remove(temp[0]);
                        hand.Remove(temp[1]);

                        SetNextPosition(clickObj, temp[0]);
                        SetNextPosition(temp[0], temp[1]);

                        CardManager.instance.field.Add(temp[0]);
                        CardManager.instance.field.Add(temp[1]);

                        StartCoroutine(MoveFieldScoreField(temp[0], handscore));
                        StartCoroutine(MoveFieldScoreField(temp[1], handscore));

                        CardManager.instance.DrawBombCard(hand);
                    }

                    CardManager.instance.FlipCard();


                    StartCoroutine(FlipCardHit(clickObj, hand, handscore));

                    break;
                }


            case 3:
                {
                    print("2개중 고르기");
                    GetHitTwoCard(clickObj);
                    CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(clickObj)]--;

                    CardManager.instance.FlipCard();
                    StartCoroutine(FlipCardHit(clickObj, hand, handscore));

                    break;

                }
            case 4:
                {
                    //폭탄
                    print("폭탄");
                    GameObject[] temp = new GameObject[3];

                    temp = FlipBombCard(clickObj);

                    CardManager.instance.addEmptyIndex.Add(CheckEmptyPosition(OrignFieldPosition(temp)));

                    MoveBombFieldScoreField(temp, clickObj, handscore);

                    CardManager.instance.FlipCard();
                    StartCoroutine(FlipCardHit(clickObj, hand, handscore));

                    break;
                }
        }
    }

    IEnumerator FlipCardHit(GameObject clickObj, List<GameObject> hand, List<GameObject> handscore)
    {
        yield return new WaitForSeconds(1f);

        switch (CardManager.instance.hitCardCount)
        {
            case 1:
                {
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
                    if (!CardManager.instance.isFlip)
                    {
                        CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(hittedCard)]--;
                        CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(clickObj)]--;

                        CardManager.instance.addEmptyIndex.Add(CheckEmptyPosition(hittedCard));

                        StartCoroutine(MoveFieldScoreField(hittedCard, handscore));
                        StartCoroutine(MoveFieldScoreField(clickObj, handscore));
                        
                        //EmptyFieldPosition(hittedCard);
                        break;
                    }
                    
                    else
                    {
                        if (CardManager.instance.field[CardManager.instance.field.Count - 1].CompareTag(clickObj.tag)) // 같은거 맞음
                        {
                            //뻑
                            SetNextPosition(clickObj, CardManager.instance.field[CardManager.instance.field.Count - 1]);
                            //clickObj.transform.DOMove(new Vector3(CardManager.instance.field[CardManager.instance.field.Count - 1].transform.position.x + 0.5f, CardManager.instance.field[CardManager.instance.field.Count - 1].transform.position.y, CardManager.instance.field[CardManager.instance.field.Count - 1].transform.position.z - 0.1f), 0.5f).OnComplete(() => CardManager.instance.DeleteOutline(clickObj));

                            print("뻑");

                            FixedSetting();
                            break;
                        }

                        else
                        {
                            CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(hittedCard)]--;
                            CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(clickObj)]--;

                            CardManager.instance.addEmptyIndex.Add(CheckEmptyPosition(hittedCard));

                            StartCoroutine(MoveFieldScoreField(hittedCard, handscore));
                            StartCoroutine(MoveFieldScoreField(clickObj, handscore));

                            //EmptyFieldPosition(hittedCard);

                            FlipAction(hand, handscore);
                        }
                    }

                    break;
                }

            case 3:
                {
                    if (!CardManager.instance.isFlip)
                    {
                        //StartCoroutine(MoveFieldScoreField(clickObj, handscore));
                        OpenChoiceCardPanel();
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

                            CardManager.instance.addEmptyIndex.Add(CheckEmptyPosition(OrignFieldPosition(temp)));

                            MoveBombLastScoreField(temp, CardManager.instance.field[CardManager.instance.field.Count - 1], handscore);
                            
                            //EmptyFieldPosition(OrignFieldPosition(temp));
                            break;
                        }

                        else
                        {
                            print("2개중 하나 고름");
                            //StartCoroutine(MoveFieldScoreField(clickObj, handscore));
                            OpenChoiceCardPanel();
                            CardManager.instance.beforeFlip = true;
                        }
                        break;
                    }
                }

            case 4:
                {
                    if (!CardManager.instance.isFlip)
                    {
                        FixedSetting();
                        break;
                    }

                    else
                    {
                        if (!(CardManager.instance.field[CardManager.instance.field.Count - 1].CompareTag(clickObj.tag))) // 다른거 맞음
                        {
                            FlipAction(hand, handscore);
                        }
                        else
                        {
                            FixedSetting();
                        }
                        break;
                    }
                }
        }
        
        
    }

    void NoMatchField(GameObject obj)
    {
        obj.transform.DOMove(CardManager.instance.fieldPosition[CardManager.instance.emptyIndex[0]], 0.5f).SetEase(Ease.OutQuint); // 마지막 필드포지션은 빈곳에 넣음
       
        CardManager.instance.emptyIndex.RemoveAt(0);

        CardManager.instance.EmptyIndexSort();//빈곳 인덱스 오름차순 정렬
    }

    void FlipNoMatchField(GameObject obj)
    {
       
        obj.transform.DOMove(CardManager.instance.fieldPosition[CardManager.instance.emptyIndex[0]], 0.5f).SetEase(Ease.OutQuint).OnComplete(() => FixedSetting());// 마지막 필드포지션은 빈곳에 넣음
       
        CardManager.instance.emptyIndex.RemoveAt(0);

        CardManager.instance.EmptyIndexSort();//빈곳 인덱스 오름차순 정렬
    }

    void FlipAction(List<GameObject> hand, List<GameObject> handscore)
    {
        if(CardManager.instance.field[CardManager.instance.field.Count - 1].tag == "Bonus")
        {
            print("bonus card!");
            NoMatchField(CardManager.instance.field[CardManager.instance.field.Count - 1]); //빈공간에 내가 둘 카드 둠
            
            CardManager.instance.addEmptyIndex.Add(CheckEmptyPosition(CardManager.instance.field[CardManager.instance.field.Count - 1]));

            StartCoroutine(MoveFieldScoreField(CardManager.instance.field[CardManager.instance.field.Count - 1], handscore)); // 피로 옮김
            
            //EmptyFieldPosition(CardManager.instance.field[CardManager.instance.field.Count - 1]); // 빈공간 만들어줌

            CardManager.instance.FlipCard();

            FlipAction(hand, handscore);
        }
       
        else
        {
            print(CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(CardManager.instance.field[CardManager.instance.field.Count - 1])]);
            switch (CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(CardManager.instance.field[CardManager.instance.field.Count - 1])]) // 뒤집은 카드의 같은 태그 개수 별
            {
                case 1:
                    {
                        print("+ 뒤집은것도 안맞음");
                        FlipNoMatchField(CardManager.instance.field[CardManager.instance.field.Count - 1]); //빈공간에 내가 둘 카드 둠
                        break;//턴 넘기기
                    }

                case 2:
                    {
                        print(" + 뒤집은거 맞음");
                        /*다른거 두개 맞았을 때 -> 무조건 같은 태그 오브젝트 1개 존재*/
                        hittedCard = GetHitCard(CardManager.instance.field[CardManager.instance.field.Count - 1]); // 뒤집은 카드와 맞은 카드 찾음
                                                                                                                   //print(hittedCard.name);
                        CardManager.instance.addEmptyIndex.Add(CheckEmptyPosition(hittedCard));

                        CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(hittedCard)]--;
                        CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(CardManager.instance.field[CardManager.instance.field.Count - 1])]--;

                        //점수판으로 위치 옮기기
                        StartCoroutine(MoveFieldScoreField(hittedCard, handscore));
                        StartCoroutine(MoveLastScoreField(CardManager.instance.field[CardManager.instance.field.Count - 1], handscore));
                        //EmptyFieldPosition(hittedCard);
                        
                        break;
                    }

                case 3:
                    {
                        /*다른거 세개 맞았을 때 -> 무조건 2개 존재*/
                        print(" + 둘 중 하나 고름"); ;
                        GetHitTwoCard(CardManager.instance.field[CardManager.instance.field.Count - 1]);

                        CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(CardManager.instance.field[CardManager.instance.field.Count - 1])]--;

                        OpenChoiceCardPanel();
                        break;
                    }

                case 4:
                    {
                        //뒤집은 칻으가 폭탄
                        print(" + 폭탄");

                        GameObject[] temp = new GameObject[3];

                        temp = FlipBombCard(CardManager.instance.field[CardManager.instance.field.Count - 1]);


                        MoveBombLastScoreField(temp, CardManager.instance.field[CardManager.instance.field.Count - 1], handscore);
                        CardManager.instance.addEmptyIndex.Add(CheckEmptyPosition(OrignFieldPosition(temp)));
                        //EmptyFieldPosition(index);
                        break;
                    }

            }
        }
      
      
    }

    //카드를 맞추는 함수
    GameObject GetHitCard(GameObject obj)
    {
        int index = 0;
        for (int i = 0; i < CardManager.instance.field.Count - 1; i++) // 필드에서 돌림
        {
            if (CardManager.instance.field[i].CompareTag(obj.tag))
            {
                index = i;
            }
        }

        obj.transform.position = new Vector3(CardManager.instance.field[index].transform.position.x + 0.5f, 
            CardManager.instance.field[index].transform.position.y, 
            CardManager.instance.field[index].transform.position.z - 0.1f);

        return CardManager.instance.field[index];
    }

    // 2개 있는 카드옆으로 게임 오브젝트를 이동시키는 함수
    void GetHitTwoCard(GameObject obj)
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
       
        GameObject[] temp = new GameObject[3];

        for (int i = 0; i < temp.Length; i++)
        {
            temp[i] = CardManager.instance.bombObj[i];
        }

        return temp;
    }

    //쪽 함수 맞춘 카드를 점수패로 이동시킴
    void Chu(GameObject clickObj, List<GameObject> score)
    {
       
        CardManager.instance.field[CardManager.instance.field.Count - 1].transform.DOMove(new Vector3(clickObj.transform.position.x + 0.5f, clickObj.transform.position.y, clickObj.transform.position.z - 0.1f), 0.5f).SetEase(Ease.OutQuint); //맞춘 오브젝트 옆으로 이동
       
        CardManager.instance.field[CardManager.instance.field.Count - 1].transform.DOMove(CardManager.instance.ScoreField(CardManager.instance.field[CardManager.instance.field.Count - 1], score), 0.5f).SetEase(Ease.OutQuint); // 점수판위치로 이동

        CardManager.instance.addEmptyIndex.Add(CheckEmptyPosition(clickObj));

        clickObj.transform.DOMove(CardManager.instance.ScoreField(clickObj, score), 0.5f).SetEase(Ease.OutQuint).OnComplete(()=> FixedSetting()); // 점수판위치로 이동
      
        score.Add(CardManager.instance.field[CardManager.instance.field.Count - 1]);
        CardManager.instance.field.Remove(CardManager.instance.field[CardManager.instance.field.Count - 1]);

        score.Add(clickObj);
        
        CardManager.instance.field.Remove(clickObj);
    }
   
    //게임오브젝트를 점수필드로 이동시키는 함수
    public IEnumerator MoveFieldScoreField(GameObject moveObj, List<GameObject> score)
    {
        yield return new WaitForSeconds(0.5f);
        if(CardManager.instance.GetCardTagNum(moveObj) != 13)
        {
            CardManager.instance.DeleteOutline(moveObj);
        }
        
        int index = CheckEmptyPosition(moveObj);
        moveObj.transform.DOMove(CardManager.instance.ScoreField(moveObj, score), 0.5f).SetEase(Ease.OutQuint); // 점수판 위치 이동
        
        score.Add(moveObj); // 점수에 더해주기 


        if (CardManager.instance.field.Contains(moveObj))
        {
            CardManager.instance.field.Remove(moveObj); // 필드에서 지우기
        }
    }

    //MoveFieldScoreField와 거의 같음, 플립이 끝나면 턴을 종료해주기 위해 .OnComplete(() => FixedSetting()); 만 추가
    public IEnumerator MoveLastScoreField(GameObject moveObj, List<GameObject> score)
    {
        yield return new WaitForSeconds(0.5f);
        if (CardManager.instance.GetCardTagNum(moveObj) != 13)
        {
            CardManager.instance.DeleteOutline(moveObj);
        }

        //int index = CheckEmptyPosition(moveObj);

        moveObj.transform.DOMove(CardManager.instance.ScoreField(moveObj, score), 0.5f).SetEase(Ease.OutQuint).OnComplete(() => FixedSetting());// 점수판 위치 이동

        //print(score.Count);
        score.Add(moveObj); // 점수에 더해주기 

        if (CardManager.instance.field.Contains(moveObj))
        {
            CardManager.instance.field.Remove(moveObj); // 필드에서 지우기
        }
    }
    //폭탄인 4개의 카드를 모두 점수패로 옮기는 함수
    void MoveBombFieldScoreField(GameObject[] bombObj, GameObject card, List<GameObject> score)
    {
        for (int i = 0; i < bombObj.Length; i++)
        {
            CardManager.instance.DeleteOutline(bombObj[i]); // 아웃라인 삭제
           
            bombObj[i].transform.DOMove(CardManager.instance.ScoreField(bombObj[i], score), 0.5f).SetEase(Ease.OutQuint); // 점수 필드로 위치 옮김
        
            //내 점수리스트 add 
            score.Add(bombObj[i]);//내 점수필드 리스트에 추가

            //필드에서 삭제
            CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(bombObj[i])]--;//필드에서 제거
            CardManager.instance.field.Remove(bombObj[i]);//필드에서 제거
        }

        if (CardManager.instance.GetCardTagNum(card) != 13) // 보너스카드나 밤 카드가 아닐 때만
        {
            CardManager.instance.DeleteOutline(card);  // 아웃라인 삭제
        }

        card.transform.DOMove(CardManager.instance.ScoreField(card, score), 0.5f).SetEase(Ease.OutQuint);
    
        //내 점수리스트 add 
        score.Add(card);//내 점수필드 리스트에 추가

        //필드에서 삭제
        CardManager.instance.field.Remove(card);//필드에서 제거
        CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(card)]--;//필드에서 제거

    }

    //MoveBombFieldScoreField와 거의 같음, 플립이 끝나면 턴을 종료해주기 위해 .OnComplete(() => FixedSetting()); 만 추가
    void MoveBombLastScoreField(GameObject[] bombObj, GameObject card, List<GameObject> score)
    {
        for (int i = 0; i < bombObj.Length; i++)
        {
            CardManager.instance.DeleteOutline(bombObj[i]);
          
            bombObj[i].transform.DOMove(CardManager.instance.ScoreField(bombObj[i], score), 0.5f).SetEase(Ease.OutQuint); // 점수 필드로 위치 옮김
         
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

        card.transform.DOMove(CardManager.instance.ScoreField(card, score), 0.5f).SetEase(Ease.OutQuint).OnComplete(() => FixedSetting());
    
        //내 점수리스트 add 
        score.Add(card);//내 점수필드 리스트에 추가

        //필드에서 삭제
        CardManager.instance.field.Remove(card);//필드에서 제거
        CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(card)]--;//필드에서 제거

    }

    //게임 오브젝트의 포지션이 필드포지션과 같다면 필드 포지션 인덱스를 리턴
    public int CheckEmptyPosition(GameObject obj) //받은 게임오브젝트의 필드 자리를 비워줌
    {
        int index = Array.IndexOf(CardManager.instance.fieldPosition, obj.transform.position);

        return index;
    }

    //가지고 있는 인덱스들의 자리를 비워줌
    public void EmptyFieldPosition(List<int> index)
    {
        for(int i=0;i<index.Count;i++)
        {
            if (!CardManager.instance.emptyIndex.Contains(index[i])) // 원래 가지고 있는 값이 아니면 // 값이 없으면 음수로 들어가서 음수는 빼줘야함
            {
                if (index[i] >= 0)
                {
                    CardManager.instance.emptyIndex.Add(index[i]);
                    CardManager.instance.EmptyIndexSort();//빈곳 인덱스 오름차순 정렬
                }
            }
        }

        CardManager.instance.addEmptyIndex.Clear();
    }

    //게임 오브젝트가 필드 포지션과 같다면 포지션을 리턴함
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

    //카드를 선택할 판넬을 켜는 함수
    void OpenChoiceCardPanel()
    {
        OnOffPanel(false);
        GameManager.instance.isChoice = true;

        OpenChoicePanel = GameObject.Find("Canvas").transform.Find("Panel").gameObject;
        OpenChoicePanel.SetActive(true);
        OpenChoicePanel.transform.GetChild(1).GetComponent<Image>().sprite = CardManager.instance.choiceObj[0].GetComponent<SpriteRenderer>().sprite;
        OpenChoicePanel.transform.GetChild(2).GetComponent<Image>().sprite = CardManager.instance.choiceObj[1].GetComponent<SpriteRenderer>().sprite;
    }

    //카드 두개중 하나를 고른 뒤 진행할 함수
    public void SelectNum(int num)
    {
        OnOffPanel(true);

        OpenChoicePanel = GameObject.Find("Canvas").transform.Find("Panel").gameObject;

        OpenChoicePanel.SetActive(false);

        GameObject[] temp = new GameObject[2];

        for (int i = 0; i < CardManager.instance.choiceObj.Count; i++)
        {
            temp[i] = CardManager.instance.choiceObj[i];
        }

        GameManager.instance.isChoice = false;

        if (CardManager.instance.beforeFlip) // flipaction을 하기 전 이면
        {
            StartCoroutine(MoveFieldScoreField(CardManager.instance.curObj, CardManager.instance.curHandScore));
            StartCoroutine(MoveFieldScoreField(CardManager.instance.choiceObj[num], CardManager.instance.curHandScore)); // 냈던 오브젝트와 선택한 카드만 점수패로 이동
            FlipAction(CardManager.instance.curHand, CardManager.instance.curHandScore); // flipaction 실행
            CardManager.instance.beforeFlip = false;
        }

        else
        {
            StartCoroutine(MoveFieldScoreField(CardManager.instance.field[CardManager.instance.field.Count - 1], CardManager.instance.curHandScore));
            StartCoroutine(MoveLastScoreField(CardManager.instance.choiceObj[num], CardManager.instance.curHandScore));// 냈던 오브젝트와 선택한 카드만 점수패로 이동
        }
        CardManager.instance.choiceObj[1 - num].transform.position = OrignFieldPosition(temp).transform.position; // 선택 안한 카드는 오리진 필드 포지션으로 이동

        CardManager.instance.sameTagCount[CardManager.instance.GetCardTagNum(CardManager.instance.choiceObj[num])]--;
        CardManager.instance.field.Remove(CardManager.instance.choiceObj[num]);

        CardManager.instance.choiceObj.Clear();
    }

    // 손에 게임오브젝트와 같은 태그가 몇개 있는지 셈
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

    ///흔들기 버튼을 클릭했을 때 함수
    public void ApplyShake()
    {
        ShakePanel = GameObject.Find("Canvas").transform.Find("ShakePanel").gameObject;
        
        //흔들기 전투력 증가

        ShakePanel.SetActive(false);

        CardManager.instance.curHand.Remove(CardManager.instance.curObj);//내손에서 지우기

        GameManager.instance.isShake = false;

        CardAction(CardManager.instance.curObj, CardManager.instance.curHand, CardManager.instance.curHandScore);

    }

    //<마지막 정리 함수>
    public void EndArrange(List<GameObject> hand, bool isPlayer)
    {
        GameManager.instance.isMyTurn = isPlayer; // 턴 바꾸기

        GameManager.instance.NextTurnDraw(); // 다음 턴 사람 드로우

        EmptyFieldPosition(CardManager.instance.addEmptyIndex);  // 비울 필드 인덱스 비워주기
    }
    
}
