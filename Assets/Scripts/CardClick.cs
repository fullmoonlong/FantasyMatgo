using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class CardClick : MonoBehaviour
{
    public static int score = 0;
    public string type = "";
    List<int> sameTagCount;
    private List<GameObject> storage;

    public GameObject ChoicePanel;

    int myCardCount;
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
        sameTagCount = new List<int>(12) { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        storage = new List<GameObject>();
        myCardCount = 0;

        FieldSameCard();
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
    public void OnMouseUp()
    {
        if (GameManager.instance.isMyTurn == true)//만약 플레이어 턴이면
        {
            if (CardManager.instance.myHand.Contains(gameObject)) // 내손에 이 게임오브젝트가 있을 때
            {
                print("내 손에 있음");

                    sameTagCount[GetCardTagNum(gameObject)]++; // 내가 낸 카드 ++ 해줌
                    myCardCount = sameTagCount[GetCardTagNum(gameObject)];
                    CardManager.instance.field.Add(gameObject); //내손에 있는 카드 필드에 넣기
                    CardManager.instance.myHand.Remove(gameObject);//내손에서 지우기

                    print(myCardCount);
                    switch (myCardCount) // 2, 3, 4개 맞췄을 시 다름
                    {
                        //카드가 안맞았을 때
                        case 1:
                            print("카드 안맞음");
                            /*카드의 위치는 빈공간으로 간다.*/

                            //펑션으로 만들곳
                            //필드 빈공간에 게임오브젝트 넣어줌
                            CardManager.instance.EmptyIndexSort();//빈곳 인덱스 오름차순 정렬
                            gameObject.transform.position = CardManager.instance.fieldPosition[CardManager.instance.emptyIndex[0]]; // 마지막 필드포지션은 빈곳에 넣음
                            CardManager.instance.emptyIndex.RemoveAt(0);

                        //카드 플립함
                            CardManager.instance.FlipCard();
                            sameTagCount[GetCardTagNum(CardManager.instance.field[CardManager.instance.field.Count - 1])]++;

                            if (CardManager.instance.field[CardManager.instance.field.Count - 1].CompareTag(gameObject.tag)) // 뒤집은 카드랑 내가 냈던 카드랑 같으면
                            {
                                /*쪽*/
                                print("쪽");
                                sameTagCount[GetCardTagNum(CardManager.instance.field[CardManager.instance.field.Count - 1])]--;
                                sameTagCount[GetCardTagNum(gameObject)]--;

                                CardManager.instance.field[CardManager.instance.field.Count - 1].transform.position = new Vector3(gameObject.transform.position.x + 0.5f, gameObject.transform.position.y, gameObject.transform.position.z); //맞춘 오브젝트 옆으로 이동
                                                                                              
                                CardManager.instance.field[CardManager.instance.field.Count - 1].transform.position = CardManager.instance.ScoreField(CardManager.instance.field[CardManager.instance.field.Count - 1]); // 점수판위치로 이동

                                gameObject.transform.position = CardManager.instance.ScoreField(gameObject); // 점수판위치로 이동

                                //얻은 카드
                                CardManager.instance.myHandScore.Add(CardManager.instance.field[CardManager.instance.field.Count - 1]);
                                CardManager.instance.field.Remove(CardManager.instance.field[CardManager.instance.field.Count - 1]);

                                CardManager.instance.myHandScore.Add(gameObject);
                                CardManager.instance.field.Remove(gameObject);

                                GameManager.instance.isMyTurn = false;
                            }

                            else
                            {
                                //2개중 한개 고르기 // 뒤집은 카드의 갯수가 1개이면 안맞은거
                                switch (sameTagCount[GetCardTagNum(CardManager.instance.field[CardManager.instance.field.Count - 1])]) // 뒤집은 카드의 같은 태그 개수 별
                                {
                                    case 1:
                                        /*아무것도 못맞춤*/
                                        print("아무것도 못맞춤");
                                        //턴 넘기기
                                        GameManager.instance.isMyTurn = false;
                                        break;

                                    case 2:
                                    print("두개 맞춤");
                                        /*다른거 두개 맞았을 때 -> 무조건 같은 태그 오브젝트 1개 존재*/
                                        int index = 0;

                                        for (int i = 0; i < CardManager.instance.field.Count - 1; i++) // 카운트에서 1빼는 이유 -> 비교할 태그가 있음
                                        {
                                            if (CardManager.instance.field[i].CompareTag(CardManager.instance.field[CardManager.instance.field.Count - 1].tag))
                                            {
                                                //같은 카드 다음 포지션은 같은 태그의 갯수 * 0.5 만큼 x축을 더해준다.
                                                CardManager.instance.field[CardManager.instance.field.Count - 1].transform.position = new Vector3(CardManager.instance.field[i].transform.position.x + 0.5f, CardManager.instance.field[i].transform.position.y, CardManager.instance.field[i].transform.position.z - 0.1f);
                                                
                                                index = i;
                                            }
                                        }

                                        //맞춘거 다 보여주면
                                        /*맞춘 카드 두개 점수 패로 들고가기*/

                                        /*뒤집은 카드와 같은 태그의 카드 (1개)*/
                                        //필드위치 내점수 필드 위치로 바꾸기
                                        CardManager.instance.field[index].transform.position = CardManager.instance.ScoreField(CardManager.instance.field[index]);

                                        //내 점수리스트 add 
                                        CardManager.instance.myHandScore.Add(CardManager.instance.field[index]);//내 점수필드 리스트에 추가

                                        //필드에서 삭제
                                        CardManager.instance.field.Remove(CardManager.instance.field[index]);//필드에서 제거
                                        sameTagCount[GetCardTagNum(CardManager.instance.field[index])]--;//필드에서 제거


                                        /*뒤집은 카드*/
                                        //필드위치 내점수 필드 위치로 바꾸기
                                        CardManager.instance.field[CardManager.instance.field.Count - 1].transform.position = CardManager.instance.ScoreField(CardManager.instance.field[CardManager.instance.field.Count - 1]);
                                        
                                        //내 점수리스트 add 
                                        CardManager.instance.myHandScore.Add(CardManager.instance.field[CardManager.instance.field.Count - 1]);//내 점수필드 리스트에 추가

                                        //필드에서 삭제
                                        CardManager.instance.field.Remove(CardManager.instance.field[CardManager.instance.field.Count - 1]);//필드에서 제거
                                        sameTagCount[GetCardTagNum(CardManager.instance.field[CardManager.instance.field.Count - 1])]--;//필드에서 제거
                                        

                                        GameManager.instance.isMyTurn = false;
                                        break;
  /*아직덜만듬*/                        
                                    case 3:

                                        /*다른거 세개 맞았을 때 -> 무조건 2개 존재*/
                                        print("둘중하나 고름");
                                        //FlipChoiceCard();

                                        GameManager.instance.isMyTurn = false;

                                        break;

                                    case 4:
                                        //폭탄
                                        print("폭탄");
                                        //FlipBombCard();

                                        GameManager.instance.isMyTurn = false;
                                        break;
                                }
                            }
                            break;
                        case 2:
                            print("일단 맞았음");
                            GetHitCard(gameObject);

                            CardManager.instance.FlipCard();
                            sameTagCount[GetCardTagNum(CardManager.instance.field[CardManager.instance.field.Count - 1])]++;

                            if (CardManager.instance.field[CardManager.instance.field.Count - 1].CompareTag(gameObject.tag)) // 같은거 맞음
                            {
                                //뻑
                                CardManager.instance.field[CardManager.instance.field.Count - 1].transform.position = new Vector3(gameObject.transform.position.x + 0.5f, gameObject.transform.position.y, gameObject.transform.position.z - 0.1f);//뒤집은 카드는 내가 냈던 카드 옆으로 위치 이동
                                print("뻑");

                                GameManager.instance.isMyTurn = false;
                            }

                            else
                            {
                                print(sameTagCount[GetCardTagNum(CardManager.instance.field[CardManager.instance.field.Count - 1])]);
                                switch (sameTagCount[GetCardTagNum(CardManager.instance.field[CardManager.instance.field.Count - 1])]) // 뒤집은 카드의 같은 태그 개수 별
                                {
                                    case 1:
                                        print("내가 낸 카드만 맞음");
                                        //게임 오브젝트가 가지고 있는 모든 같은 태그 카드 들고가기
                                        //GetHitCard(gameObject);

                                        GameManager.instance.isMyTurn = false;

                                        break;
                                    case 2:
                                        print("내가 낸 카드, 뒤집은 카드도 맞음");
                                        //게임 오브젝트가 가지고 있는 모든 같은 태그 카드 들고가기
                                        //GetHitCard(gameObject);

                                        //뒤집은 카드랑 같은 태그 카드 들고가기
                                        //GetHitCard(CardManager.instance.field[CardManager.instance.field.Count - 1]);

                                        GameManager.instance.isMyTurn = false;
                                        break;
                                    case 3:
                                        print("내가 낸 카드, 뒤집은 카드도 맞음 2개중 하나 골라야함");
                                        //게임오브젝트가 가지고 있는 모든 같은 태그 카드 들고가기
                                        //GetHitCard(gameObject);

                                        //2개중 한개 고르기
                                        //FlipChoiceCard();

                                        GameManager.instance.isMyTurn = false;
                                        break;
                                    case 4:
                                        print("내가 낸 카드, 뒤집은 카드 폭탄");
                                        //GetHitCard(gameObject);
                                        //폭탄
                                        //FlipBombCard();

                                        GameManager.instance.isMyTurn = false;
                                        break;
                                }
                            }
                        break;

                        case 3:
                            CardManager.instance.FlipCard();
                            sameTagCount[GetCardTagNum(CardManager.instance.field[CardManager.instance.field.Count - 1])]++;

                            if (CardManager.instance.field[CardManager.instance.field.Count - 1].CompareTag(gameObject.tag)) // 같은거 맞음
                            {
                                //폭탄
                                FlipBombCard();
                                print("폭탄입니다.");
                                GameManager.instance.isMyTurn = false;
                            }

                            else
                            {
                                //2개중 한개 고르기 // 뒤집은 카드의 갯수가 1개이면 안맞은거
                                switch (sameTagCount[GetCardTagNum(CardManager.instance.field[CardManager.instance.field.Count - 1])]) // 뒤집은 카드의 같은 태그 개수 별
                                {
                                    case 1:
                                        //내가 낸 카드에서 내카드 빼고 두개중 한개 고르기
                                        print("내가 낸 카드에서 내카드 빼고 두개중 한개 고르기");
                                        FlipChoiceCard();

                                        //그냥 빈 자리에 두기
                                        CardManager.instance.EmptyIndexSort();//빈곳 인덱스 오름차순 정렬
                                        gameObject.transform.position = CardManager.instance.fieldPosition[CardManager.instance.emptyIndex[0]]; // 마지막 필드포지션은 빈곳에 넣음
                                        CardManager.instance.emptyIndex.RemoveAt(0);

                                        sameTagCount[GetCardTagNum(gameObject)]++; // 필드에서 추가

                                        CardManager.instance.field.Add(gameObject); //내손에 있는 카드 필드에 넣기
                                        CardManager.instance.myHand.Remove(gameObject);//내손에서 지우기

                                        GameManager.instance.isMyTurn = false;
                                        break;

                                    case 2:
                                        //뒤집은 카드랑 같은 태그 카드 들고가기
                                        FlipChoiceCard();

                                        GetHitCard(CardManager.instance.field[CardManager.instance.field.Count - 1]);

                                        break;
                                    case 3:
                                        //2개중 한개 고르기
                                        break;
                                    case 4:
                                        //폭탄
                                        FlipChoiceCard();
                                        FlipBombCard();
                                        break;
                                }
                            }
                            break;

                        case 4:
                            //폭탄
                            print("폭탄");
                            CardManager.instance.FlipCard();
                            sameTagCount[GetCardTagNum(CardManager.instance.field[CardManager.instance.field.Count - 1])]++;

                            if (!(CardManager.instance.field[CardManager.instance.field.Count - 1].CompareTag(gameObject.tag))) // 다른거 맞음
                            {
                                switch (sameTagCount[GetCardTagNum(CardManager.instance.field[CardManager.instance.field.Count - 1])]) // 뒤집은 카드의 같은 태그 개수 별
                                {

                                    case 2:
                                        //뒤집은 카드랑 같은 태그 카드 들고가기
                                        break;
                                    case 3:
                                        //뒤집은 카드랑 같은 태그 2개중 한개 고르기
                                        break;
                                    case 4:
                                        //폭탄
                                        break;
                                }
                            }

                            break;
                    
                }
            }

            else
            {
                print("실행안됨");
            }
        }

        else//만약 상대 턴이면
        {
            if (CardManager.instance.opponentHand.Contains(gameObject)) // 내손에 이 게임오브젝트가 있을 때
            {
                if (sameTagCount[GetCardTagNum(gameObject)] > 0) // 같은 카드가 1개 이상있을 때 -> 무조건 맞출 수 있음
                {
                    sameTagCount[GetCardTagNum(gameObject)]++; // 내가 낸 카드 ++ 해줌

                    CardManager.instance.field.Add(gameObject); //내손에 있는 카드 필드에 넣기
                    CardManager.instance.opponentHand.Remove(gameObject);//내손에서 지우기

                    myCardCount = sameTagCount[GetCardTagNum(gameObject)];

                    CardManager.instance.FlipCard();

                    sameTagCount[GetCardTagNum(CardManager.instance.field[CardManager.instance.field.Count - 1])]++;


                    switch (myCardCount) // 2, 3, 4개 맞췄을 시 다름
                    {
                        //카드가 안맞았을 때
                        case 1:
                            print("카드 안맞음");
                            /*카드의 위치는 빈공간으로 간다.*/

                            //펑션으로 만들곳
                            //필드 빈공간에 게임오브젝트 넣어줌
                            CardManager.instance.EmptyIndexSort();//빈곳 인덱스 오름차순 정렬
                            gameObject.transform.position = CardManager.instance.fieldPosition[CardManager.instance.emptyIndex[0]]; // 마지막 필드포지션은 빈곳에 넣음
                            CardManager.instance.emptyIndex.RemoveAt(0);

                            sameTagCount[GetCardTagNum(gameObject)]++; // 필드에서 추가

                            if (CardManager.instance.field[CardManager.instance.field.Count - 1].CompareTag(gameObject.tag)) // 뒤집은 카드랑 내가 냈던 카드랑 같으면
                            {
                                /*쪽*/
                                print("쪽");
                                sameTagCount[GetCardTagNum(CardManager.instance.field[CardManager.instance.field.Count - 1])]--;
                                sameTagCount[GetCardTagNum(gameObject)]--;

                                CardManager.instance.field[CardManager.instance.field.Count - 1].transform.position = new Vector3(gameObject.transform.position.x + 0.5f, gameObject.transform.position.y, gameObject.transform.position.z); //맞춘 오브젝트 옆으로 이동

                                CardManager.instance.field[CardManager.instance.field.Count - 1].transform.position = CardManager.instance.ScoreField(CardManager.instance.field[CardManager.instance.field.Count - 1]); // 점수판위치로 이동

                                gameObject.transform.position = CardManager.instance.ScoreField(gameObject); // 점수판위치로 이동

                                //얻은 카드
                                CardManager.instance.opponentHandScore.Add(CardManager.instance.field[CardManager.instance.field.Count - 1]);
                                CardManager.instance.field.Remove(CardManager.instance.field[CardManager.instance.field.Count - 1]);

                                CardManager.instance.opponentHandScore.Add(gameObject);
                                CardManager.instance.field.Remove(gameObject);

                                GameManager.instance.isMyTurn = false;
                            }

                            else
                            {
                                //2개중 한개 고르기 // 뒤집은 카드의 갯수가 1개이면 안맞은거
                                switch (sameTagCount[GetCardTagNum(CardManager.instance.field[CardManager.instance.field.Count - 1])]) // 뒤집은 카드의 같은 태그 개수 별
                                {
                                    case 1:
                                        /*아무것도 못맞춤*/
                                        //턴 넘기기
                                        GameManager.instance.isMyTurn = true;
                                        break;

                                    case 2:
                                        /*다른거 두개 맞았을 때 -> 무조건 같은 태그 오브젝트 1개 존재*/
                                        int index = 0;

                                        for (int i = 0; i < CardManager.instance.field.Count - 1; i++) // 카운트에서 1빼는 이유 -> 비교할 태그가 있음
                                        {
                                            if (CardManager.instance.field[i].CompareTag(CardManager.instance.field[CardManager.instance.field.Count - 1].tag))
                                            {
                                                //같은 카드 다음 포지션은 같은 태그의 갯수 * 0.5 만큼 x축을 더해준다.
                                                CardManager.instance.field[CardManager.instance.field.Count - 1].transform.position = new Vector3(CardManager.instance.field[i].transform.position.x + 0.5f, CardManager.instance.field[i].transform.position.y, CardManager.instance.field[i].transform.position.z - 0.1f);

                                                index = i;
                                            }
                                        }

                                        //맞춘거 다 보여주면
                                        /*맞춘 카드 두개 점수 패로 들고가기*/

                                        /*뒤집은 카드와 같은 태그의 카드 (1개)*/
                                        //필드위치 내점수 필드 위치로 바꾸기
                                        CardManager.instance.field[index].transform.position = CardManager.instance.ScoreField(CardManager.instance.field[index]);

                                        //내 점수리스트 add 
                                        CardManager.instance.opponentHandScore.Add(CardManager.instance.field[index]);//내 점수필드 리스트에 추가

                                        //필드에서 삭제
                                        CardManager.instance.field.Remove(CardManager.instance.field[index]);//필드에서 제거
                                        sameTagCount[GetCardTagNum(CardManager.instance.field[index])]--;//필드에서 제거


                                        /*뒤집은 카드*/
                                        //필드위치 내점수 필드 위치로 바꾸기
                                        CardManager.instance.field[CardManager.instance.field.Count - 1].transform.position = CardManager.instance.ScoreField(CardManager.instance.field[CardManager.instance.field.Count - 2]);

                                        //내 점수리스트 add 
                                        CardManager.instance.opponentHandScore.Add(CardManager.instance.field[CardManager.instance.field.Count - 1]);//내 점수필드 리스트에 추가

                                        //필드에서 삭제
                                        CardManager.instance.field.Remove(CardManager.instance.field[CardManager.instance.field.Count - 1]);//필드에서 제거
                                        sameTagCount[GetCardTagNum(CardManager.instance.field[CardManager.instance.field.Count - 1])]--;//필드에서 제거


                                        GameManager.instance.isMyTurn = true;
                                        break;
                                    /*아직덜만듬*/
                                    case 3:
                                        /*다른거 세개 맞았을 때 -> 무조건 2개 존재*/
                                        FlipChoiceCard();

                                        GameManager.instance.isMyTurn = true;

                                        break;

                                    case 4:
                                        //폭탄
                                        FlipBombCard();

                                        GameManager.instance.isMyTurn = true;
                                        break;
                                }
                            }
                            break;
                        case 2:
                            if (CardManager.instance.field[CardManager.instance.field.Count - 1].CompareTag(gameObject.tag)) // 같은거 맞음
                            {
                                //뻑
                                CardManager.instance.field[CardManager.instance.field.Count - 1].transform.position = new Vector3(gameObject.transform.position.x + 0.5f, gameObject.transform.position.y, gameObject.transform.position.z);//뒤집은 카드는 내가 냈던 카드 옆으로 위치 이동
                                print("뻑");

                                GameManager.instance.isMyTurn = true;
                            }

                            else
                            {
                                switch (sameTagCount[GetCardTagNum(CardManager.instance.field[CardManager.instance.field.Count - 1])]) // 뒤집은 카드의 같은 태그 개수 별
                                {
                                    case 1:
                                        //게임 오브젝트가 가지고 있는 모든 같은 태그 카드 들고가기
                                        GetHitCard(gameObject);

                                        GameManager.instance.isMyTurn = true;

                                        break;
                                    case 2:
                                        //게임 오브젝트가 가지고 있는 모든 같은 태그 카드 들고가기
                                        GetHitCard(gameObject);

                                        //뒤집은 카드랑 같은 태그 카드 들고가기
                                        GetHitCard(CardManager.instance.field[CardManager.instance.field.Count - 1]);

                                        GameManager.instance.isMyTurn = true;
                                        break;
                                    case 3:
                                        //게임오브젝트가 가지고 있는 모든 같은 태그 카드 들고가기
                                        GetHitCard(gameObject);

                                        //2개중 한개 고르기
                                        FlipChoiceCard();

                                        GameManager.instance.isMyTurn = true;
                                        break;
                                    case 4:
                                        //폭탄
                                        FlipBombCard();

                                        GameManager.instance.isMyTurn = true;
                                        print("폭탄");
                                        break;
                                }
                            }
                            break;

                        case 3:
                            if (CardManager.instance.field[CardManager.instance.field.Count - 1].CompareTag(gameObject.tag)) // 같은거 맞음
                            {
                                //폭탄
                                FlipBombCard();

                                GameManager.instance.isMyTurn = false;
                            }

                            else
                            {
                                //2개중 한개 고르기 // 뒤집은 카드의 갯수가 1개이면 안맞은거
                                switch (sameTagCount[GetCardTagNum(CardManager.instance.field[CardManager.instance.field.Count - 1])]) // 뒤집은 카드의 같은 태그 개수 별
                                {
                                    case 1:
                                        //두개중 한개 고르기
                                        FlipChoiceCard();

                                        //그냥 빈 자리에 두기
                                        CardManager.instance.EmptyIndexSort();//빈곳 인덱스 오름차순 정렬
                                        gameObject.transform.position = CardManager.instance.fieldPosition[CardManager.instance.emptyIndex[0]]; // 마지막 필드포지션은 빈곳에 넣음
                                        CardManager.instance.emptyIndex.RemoveAt(0);

                                        sameTagCount[GetCardTagNum(gameObject)]++; // 필드에서 추가

                                        CardManager.instance.field.Add(gameObject); //내손에 있는 카드 필드에 넣기
                                        CardManager.instance.opponentHand.Remove(gameObject);//내손에서 지우기

                                        GameManager.instance.isMyTurn = false;
                                        break;

                                    case 2:
                                        //뒤집은 카드랑 같은 태그 카드 들고가기
                                        FlipChoiceCard();

                                        GetHitCard(CardManager.instance.field[CardManager.instance.field.Count - 1]);

                                        break;
                                    case 3:
                                        //2개중 한개 고르기
                                        break;
                                    case 4:
                                        //폭탄
                                        FlipChoiceCard();
                                        FlipBombCard();
                                        break;
                                }
                            }
                            break;

                        case 4:
                            //폭탄

                            if (!(CardManager.instance.field[CardManager.instance.field.Count - 1].CompareTag(gameObject.tag))) // 다른거 맞음
                            {
                                switch (sameTagCount[GetCardTagNum(CardManager.instance.field[CardManager.instance.field.Count - 1])]) // 뒤집은 카드의 같은 태그 개수 별
                                {

                                    case 2:
                                        //뒤집은 카드랑 같은 태그 카드 들고가기
                                        break;
                                    case 3:
                                        //뒤집은 카드랑 같은 태그 2개중 한개 고르기
                                        break;
                                    case 4:
                                        //폭탄
                                        break;
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
    }

    void FieldSameCard()
    {

        
        for (int j = 0; j < 12; j++) // 태그 12개 다 돌리기
        {
            for (int i = 0; i < CardManager.instance.field.Count; i++) // 필드 돌리기
            {
                if (j == GetCardTagNum(CardManager.instance.field[i])) // 필드 i의 태그가 j와 같다면
                {
                    sameTagCount[j]++; // 태그 값 더하기
                    if (sameTagCount[j] == 1) // 필드 태그가 하나밖에 없다면
                    {
                        storage.Add(CardManager.instance.field[i]);
                    }
                }

            }
        }

        int count = 0;

        for (int i = 0; i < storage.Count; i++)
        {
            for (int j = 0; j < CardManager.instance.field.Count; j++)
            {
                if (storage[i].CompareTag(CardManager.instance.field[j].tag) && storage[i].transform.position != CardManager.instance.field[j].transform.position)// 태그는 같고 포지션은 다를 때
                {
                    count++;
                    print(count);

                    //원래 친구 포지션
                    print(CardManager.instance.field[j].transform.position);

                    //같은 카드 다음 포지션은 같은 태그의 갯수 * 0.5 만큼 x축을 더해준다.
                    CardManager.instance.field[j].transform.position = new Vector3(storage[i].transform.position.x + 0.5f * count, storage[i].transform.position.y, storage[i].transform.position.z - 0.1f * count);

                    //만약 카드 카운트가 0 이상이면 -> 같은 카드의 자리가 비기 때문에
                    if (count > 0)
                    {
                        CardManager.instance.emptyIndex.Add(j);
                        print(CardManager.instance.emptyIndex);
                    }
                }
                //엠티 카운트가 없으면 어저지,,
            }
            count = 0;
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

    void GetHitCard(GameObject obj)
    {
        //GameObject[] sameTagObj = GameObject.FindGameObjectsWithTag(obj.tag);

        //sameTagCount[GetCardTagNum(sameTagObj[0])]--;
        //sameTagCount[GetCardTagNum(sameTagObj[1])]--;

        //sameTagObj[0].transform.position = CardManager.instance.ScoreField(sameTagObj[0]); // 점수판위치로 이동

        //sameTagObj[1].transform.position = CardManager.instance.ScoreField(sameTagObj[1]); // 점수판위치로 이동

        ////얻은 카드
        //CardManager.instance.myHandScore.Add(sameTagObj[0]);
        //CardManager.instance.field.Remove(sameTagObj[0]);

        //CardManager.instance.myHandScore.Add(sameTagObj[1]);
        //CardManager.instance.field.Remove(sameTagObj[1]);

        for (int i = 0; i < CardManager.instance.field.Count - 1; i++) // 필드에서 돌림
        {
            if (CardManager.instance.field[i].CompareTag(obj.tag))
            {
                obj.transform.position = new Vector3(CardManager.instance.field[i].transform.position.x + 0.5f, CardManager.instance.field[i].transform.position.y, CardManager.instance.field[i].transform.position.z - 0.1f);
            }
        }
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
        CardManager.instance.field[CardManager.instance.field.Count - 1].transform.position = new Vector3(Math.Max(sameTagObj[0].transform.position.x, sameTagObj[1].transform.position.x) + 0.5f, sameTagObj[0].transform.position.y, sameTagObj[0].transform.position.z - 0.1f * 3);

        //뒤집은 카드랑 같은 태그 2개중 한개 고르기
        print("둘중 하나 고르기");
        //ChoicePanel.SetActive(true);

        //고른카드 필드에서 제거
        //필드위치 내점수 필드 위치로 바꾸기
        //내 점수리스트 add 
        //필드에서 삭제
    }

    void FlipBombCard()
    {
        int count = 0;
        //무조건 3개
        GameObject[] BombObj = new GameObject[3];

        for (int i = 0; i < CardManager.instance.field.Count - 1; i++) // 카운트에서 1빼는 이유 -> 비교할 태그가 있음
        {
            if (CardManager.instance.field[i].CompareTag(CardManager.instance.field[CardManager.instance.field.Count - 1].tag)) // 태그가 같을 때 
            {
                BombObj[count] = CardManager.instance.field[i];
                count++;
            }

        }
        CardManager.instance.field[CardManager.instance.field.Count - 1].transform.position = new Vector3(Math.Max(Math.Max(BombObj[0].transform.position.x, BombObj[1].transform.position.x), BombObj[2].transform.position.x) + 0.5f,
                                                                                    BombObj[0].transform.position.y, BombObj[0].transform.position.z - 0.1f * 3);// 필드에 먼저 놔둠


        //맞춘거 다 보여주면
        /*맞춘 카드 두개 점수 패로 들고가기*/

        for (int i = 0; i < BombObj.Length; i++)
        {
            BombObj[i].transform.position = CardManager.instance.ScoreField(CardManager.instance.field[CardManager.instance.field.Count - 1]); // 점수 필드로 위치 옮김

            //내 점수리스트 add 
            CardManager.instance.myHandScore.Add(BombObj[i]);//내 점수필드 리스트에 추가

            //필드에서 삭제
            CardManager.instance.field.Remove(BombObj[i]);//필드에서 제거
            sameTagCount[GetCardTagNum(BombObj[i])]--;//필드에서 제거
        }



        CardManager.instance.field[CardManager.instance.field.Count - 1].transform.position = CardManager.instance.ScoreField(CardManager.instance.field[CardManager.instance.field.Count - 1]);

        //내 점수리스트 add 
        CardManager.instance.myHandScore.Add(CardManager.instance.field[CardManager.instance.field.Count - 1]);//내 점수필드 리스트에 추가

        //필드에서 삭제
        CardManager.instance.field.Remove(CardManager.instance.field[CardManager.instance.field.Count - 1]);//필드에서 제거
        sameTagCount[GetCardTagNum(CardManager.instance.field[CardManager.instance.field.Count - 1])]--;//필드에서 제거
    }
}
