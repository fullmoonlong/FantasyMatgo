using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardClick : MonoBehaviour
{
    public string type = "";

    private void Start()
    {
        switch (this.name)
        {
            //광
            case "0(Clone)":
            case "1(Clone)":
            case "2(Clone)":
            case "3(Clone)":
                type = "광";
                break;

            case "4(Clone)":
                type = "왕비";
                break;

            case "5(Clone)":
            case "6(Clone)":
            case "7(Clone)":
                type = "홍단";
                break;

            case "8(Clone)":
            case "9(Clone)":
            case "10(Clone)":
                type = "청단";
                break;

            case "11(Clone)":
            case "12(Clone)":
            case "13(Clone)":
                type = "초단";
                break;

            case "14(Clone)":
            case "15(Clone)":
            case "16(Clone)":
                type = "새";
                break;

        }
        print(this.type);
    }
    public void OnMouseUp()
    {
        if (GameManager.instance.isMyTurn)//만약 플레이어 턴이면
        {
            //CardManager.instance.myHand.Contains(this.gameObject);
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

    public int ScoreAmy()
    {
        //병사카드 10장 1점
        //10장이후 하나당 1점
        return 0;
    }
}
