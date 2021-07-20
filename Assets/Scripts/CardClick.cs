using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardClick : MonoBehaviour
{
    public string type = "";
    List<int> sameTagInex;
    List<int> drawsameTagInex;
    int fieldCount;
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
        sameTagInex = new List<int>();
        fieldCount = CardManager.instance.field.Count;
        drawsameTagInex = new List<int>();
    }

    public void OnMouseUp()
    {
        Debug.Log(GameManager.instance.isMyTurn);
        if (GameManager.instance.isMyTurn == true)//만약 플레이어 턴이면
        {
            Debug.Log(gameObject.tag);
            for(int i = 0; i < fieldCount; i++)
            {
                if (CardManager.instance.myHand.Contains(gameObject)) // 내손에 이 게임오브젝트가 있을 때
                {
                    if (gameObject.tag == CardManager.instance.field[i].tag)//카드가 맞다면
                    {
                        //러프 필드에 카드 놓기
                        gameObject.transform.position =
                            new Vector3(CardManager.instance.field[i].transform.position.x + 0.5f,
                            CardManager.instance.field[i].transform.position.y,
                            CardManager.instance.field[i].transform.position.z);
                        
                        CardManager.instance.field.Add(gameObject); //내손에 있는 카드 필드에 넣기

                        int index = CardManager.instance.myHand.IndexOf(gameObject);//내손에서 낸 카드는 리스트에서 삭제
                        CardManager.instance.myHand.RemoveAt(index);

                        sameTagInex.Add(i);//필드에 넣은 오브젝트 인덱스 저장
                    }
                }
            }

            print("start");
            CardManager.instance.DrawCard(CardManager.instance.field, 1);// 카드 드로우
            print("end");

            for (int i = 0; i < fieldCount; i++) // 또 같은거 있는지 체크
            {
                //전체 태그에서 뽑은 태그와 똑같은게 있다면
                if (CardManager.instance.field[i].tag == CardManager.instance.field[CardManager.instance.field.Count - 1].tag) 
                {
                    if(CardManager.instance.field[CardManager.instance.field.Count - 1].tag == gameObject.tag)
                    {
                        sameTagInex.Add(i);
                    }
                    else
                    {
                        drawsameTagInex.Add(i);
                    }
               
                }
            }
            CheckAction(sameTagInex);
            CheckAction(drawsameTagInex);

            //얻은 카드들은 플레이어 점수 필드에 넣음

            /// WIP ---- 만약 필드위에 같은 태그의 카드가 두개 있다면 어떻게 다룰지 생각해야함.
            /// 또한 카드가 두개이상일경우 첫째카드말고 마지막으로 붙어있는카드 옆에 카드를 배치해야함.

            //점수계산
            //계속, 전투 고를 수 있다면
            //계속 시
            //추가 규칙 시행
            //멈춤 시
            //전투 시작 

            //카드가 맞기 않다면
            //필드에 카드놓기

            //필드에 카드하나 뒤집기
        }

        else//만약 상대 턴이면
        {
        }

    }

    public void CheckAction(List<int> list)
    {
        switch (list.Count)
        {
            case 1://내손에 두개 가져가기
                CardManager.instance.myHandScore.Add(gameObject); // 내 점수 필드에 넣음
                CardManager.instance.SetNextPosition(CheckTag(gameObject)); // 포지션 미리 지정해 주기
                CardManager.instance.myHandScore.Add(CardManager.instance.field[list[0]]); // 필드꺼 내 점수 필드에 넣음
                CardManager.instance.SetNextPosition(CheckTag(CardManager.instance.field[list[0]]));
                CardManager.instance.field.Remove(CardManager.instance.field[list[0]]);//필드 삭제
                print("끝");
                break;

            case 2:
                CardManager.instance.ChoicePanel();//카드 둘중에 하나 고르기
                                                   //CardManager.instance.myHandScore.Add(gameObject); // 내 점수 필드에 넣음
                                                   //CardManager.instance.ScoreSetPosition(CardManager.instance.myHandScore, CheckTag(gameObject));
                                                   //CardManager.instance.myHandScore.Add(sameObject[choiceNum]);//하나 고른 카드 내 손에 넣기
                                                   //CardManager.instance.ScoreSetPosition(CardManager.instance.myHandScore, CheckTag(sameObject[choiceNum]));
                break;
            case 3:
                //뻑
                //못가져감
                break;

            case 4:
                //폭탄
                //카드 전부 내 손에 넣기
                break;
            default:
                break;
        }
    }
    Vector3 CheckTag(GameObject obj) //오브젝트 태그 체크
    {
        switch (obj.tag)
        {
            case "광":
                print("광");
                return CardManager.instance.scoreKingPosition;
            case "홍단":
          
            case "청단":
            
            case "초단":
                print("flag");
                return CardManager.instance.scoreFlagPosition;

            case "새":
                print("동물");
                return CardManager.instance.scoreAnimalPosition;

            default:
                print("pee");
                return CardManager.instance.scoreSoldierPosition;
        }
    }
 
    public int ScoreLight()
    {
        //광 4장 왕비 1장 15점

        //광 4장 4
        //광 3장 왕비 1장 4
        //광 3장 3
        //광 2장 왕비 1장 2

        //아니면 리턴 0
        return 0;
    }
    public int ScoreAnimal()
    {
        //새동물 3종 5점
        //동물카드 7장 3
        //동물카드 6장 2
        //동물카드 5장 1
        //7장 이후 하나당 1점씩
        return 0;
    }
    public int ScoreFlag()
    {
        //홍단 3개 3점
        //청단 3개
        //초단 3개
        //깃발카드 6장 2점
        //깃발카드 5장 1점
        //7장 이후 하나당 1점씩
        return 0;
    }

    public int ScoreArmy()
    {
        //병사카드 10장 1점
        //10장이후 하나당 1점
        return 0;
    }
}
