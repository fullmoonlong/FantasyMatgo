using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardClick : MonoBehaviour
{
    public string type = "";

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
            case "4(Clone)":
            case "11(Clone)":
                type = "홍단";
                break;

            case "21(Clone)":
            case "33(Clone)":
            case "38(Clone)":
                type = "청단";
                break;

            case "15(Clone)":
            case "18(Clone)":
            case "27(Clone)":
            case "45(Clone)":
                type = "초단";
                break;

            case "5(Clone)":
            case "12(Clone)":
            case "29(Clone)":
                type = "새";
                break;

            default:
                type = "피";
                break;
        }
        print(this.type);
    }
    public void OnMouseUp()
    {
        Debug.Log(GameManager.instance.isMyTurn);
        if (GameManager.instance.isMyTurn == true)//만약 플레이어 턴이면
        {
            Debug.Log(gameObject.tag);
            for(int i = 0; i < CardManager.instance.field.Count; i++)
            {
                if (gameObject.transform.parent.name.Substring(gameObject.transform.parent.name.Length - 4) == "Hand")
                {
                    if (gameObject.tag == CardManager.instance.field[i].tag)//카드가 맞다면
                    {
                        //점수필드에 카드 놓기
                        gameObject.transform.position =
                            new Vector3(CardManager.instance.field[i].transform.position.x + 0.5f,
                            CardManager.instance.field[i].transform.position.y,
                            CardManager.instance.field[i].transform.position.z);
                    }
                }
            }
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

            //필드에 맞는 카드가 있는지 확인
            //카드가 맞다면 
            //점수필드에 카드 놓기
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
