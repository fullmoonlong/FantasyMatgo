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
    List<int> sameTagIndex;
    List<int> drawSameTagIndex;
    int fieldCount;
    List<int> sameTagCount;
    private List<GameObject> storage;

    List<List<GameObject>> fieldjj;

    bool IsPair = false;
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
        sameTagIndex = new List<int>();
        fieldCount = CardManager.instance.field.Count;
        drawSameTagIndex = new List<int>();
        sameTagCount = new List<int>(12) { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        storage = new List<GameObject>();
        
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
        IsPair = false;
        fieldCount = CardManager.instance.field.Count;

        if (GameManager.instance.isMyTurn == true)//만약 플레이어 턴이면
        {
            if (CardManager.instance.myHand.Contains(gameObject)) // 내손에 이 게임오브젝트가 있을 때
            {
                IsPair = false;
                // 맞는 카드가 있는지 포문 돌림
                for (int i = 0; i < fieldCount; i++)
                {

                    if (gameObject.CompareTag(CardManager.instance.field[i].tag))//카드가 맞다면
                    {
                        print(IsPair);
                        IsPair = true;

                        //러프 필드에 카드 놓기
                        gameObject.transform.position =
                            new Vector3(CardManager.instance.field[i].transform.position.x + 0.5f,
                            CardManager.instance.field[i].transform.position.y,
                            CardManager.instance.field[i].transform.position.z);

                        CardManager.instance.field.Add(gameObject); //내손에 있는 카드 필드에 넣기

                        CardManager.instance.myHand.Remove(gameObject);

                        sameTagCount[GetCardTagNum(gameObject)]++;//맞는 태그 ++

                        //sameTagIndex.Add(i);//필드에 넣은 오브젝트 인덱스 저장
                        //태그의 위치에 같은 카드 1 입력

                    }
                }

                if (!IsPair)
                {
                    print("플레이어 안맞음");
                    IsPair = false;
                    CardManager.instance.field.Add(gameObject); //내손에 있는 카드 필드에 넣기
                    CardManager.instance.EmptyIndexSort();//빈곳 인덱스 오름차순 정렬
                    CardManager.instance.field[CardManager.instance.field.Count - 1].transform.position = CardManager.instance.fieldPosition[CardManager.instance.emptyIndex[0]]; // 마지막 필드포지션은 빈곳에 넣음
                    CardManager.instance.emptyIndex.RemoveAt(0);
                    CardManager.instance.myHand.Remove(gameObject); // 내손에서 지움
                }

                CardManager.instance.FlipCard();// 카드 드로우

                fieldCount = CardManager.instance.field.Count;// 필드 카운트 업뎃
                
                IsPair = false;
                for (int i = 0; i < fieldCount -1; i++) // 또 같은거 있는지 체크
                {
                    //전체 태그에서 뽑은 태그와 똑같은게 있다면
                    if (CardManager.instance.field[i].tag == CardManager.instance.field[CardManager.instance.field.Count - 1].tag)
                    {
                        if(CardManager.instance.field[i].tag != gameObject.tag) // 다른 카드 맞춤
                        {
                            IsPair = true;
                            CardManager.instance.field[CardManager.instance.field.Count - 1].transform.position = new Vector3(CardManager.instance.field[i].transform.position.x + 0.5f , CardManager.instance.field[i].transform.position.y, CardManager.instance.field[i].transform.position.z);
                            //마지막 카드랑, 태그 카드 내 점수패로 들고옴

                        }
                        
                        //if (CardManager.instance.field[CardManager.instance.field.Count - 1].tag == gameObject.tag)
                        //{
                        //    sameTagIndex.Add(i);

                        //}
                        //else
                        //{
                        //    drawSameTagIndex.Add(i);
                        //}

                    }
                }

                if(!IsPair)
                {
                    IsPair = false;
                    CardManager.instance.field[CardManager.instance.field.Count - 1].transform.position = CardManager.instance.fieldPosition[CardManager.instance.emptyIndex[0]]; // 마지막 필드포지션은 빈곳에 넣음
                    CardManager.instance.emptyIndex.RemoveAt(0);
                    CardManager.instance.myHand.Remove(gameObject); // 내손에서 지움
                }

                GameManager.instance.isMyTurn = false;
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
                print(fieldCount);
                IsPair = false;
                for (int i = 0; i < fieldCount; i++)
                {

                    if (gameObject.CompareTag(CardManager.instance.field[i].tag))//카드가 맞다면
                    {
                        print(IsPair);
                        IsPair = true;

                        //러프 필드에 카드 놓기
                        gameObject.transform.position =
                            new Vector3(CardManager.instance.field[i].transform.position.x + 0.5f,
                            CardManager.instance.field[i].transform.position.y,
                            CardManager.instance.field[i].transform.position.z);

                        CardManager.instance.field.Add(gameObject); //내손에 있는 카드 필드에 넣기

                        CardManager.instance.opponentHand.Remove(gameObject);

                        sameTagCount[GetCardTagNum(gameObject)]++;//맞는 태그 ++

                        //sameTagIndex.Add(i);//필드에 넣은 오브젝트 인덱스 저장
                        //태그의 위치에 같은 카드 1 입력

                    }
                }
                print(IsPair);
                if (!IsPair)
                {
                    print("상대 안맞음");
                    IsPair = false;
                    CardManager.instance.field.Add(gameObject); //내손에 있는 카드 필드에 넣기
                    CardManager.instance.EmptyIndexSort();//빈곳 인덱스 오름차순 정렬
                    CardManager.instance.field[CardManager.instance.field.Count - 1].transform.position = CardManager.instance.fieldPosition[CardManager.instance.emptyIndex[0]]; // 마지막 필드포지션은 빈곳에 넣음
                    CardManager.instance.emptyIndex.RemoveAt(0);
                    CardManager.instance.opponentHand.Remove(gameObject); // 내손에서 지움
                }

                CardManager.instance.FlipCard();// 카드 드로우
                fieldCount = CardManager.instance.field.Count;// 필드 카운트 업뎃

                IsPair = false;
                for (int i = 0; i < fieldCount; i++) // 또 같은거 있는지 체크
                {
                    //전체 태그에서 뽑은 태그와 똑같은게 있다면
                    if (CardManager.instance.field[i].tag == CardManager.instance.field[CardManager.instance.field.Count - 1].tag)
                    {
                        IsPair = true;
                        CardManager.instance.field[CardManager.instance.field.Count - 1].transform.position = new Vector3(CardManager.instance.field[i].transform.position.x + 0.5f, CardManager.instance.field[i].transform.position.y, CardManager.instance.field[i].transform.position.z);
                        //마지막 카드랑, 태그 카드 내 점수패로 들고옴
                    }
                }

                if (!IsPair)
                {
                    IsPair = false;
                    CardManager.instance.field[CardManager.instance.field.Count - 1].transform.position = CardManager.instance.fieldPosition[CardManager.instance.emptyIndex[0]]; // 마지막 필드포지션은 빈곳에 넣음
                    CardManager.instance.emptyIndex.RemoveAt(0);
                    CardManager.instance.opponentHand.Remove(gameObject); // 내손에서 지움
                }

                GameManager.instance.isMyTurn = true;
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
                    CardManager.instance.field[j].transform.position = new Vector3(storage[i].transform.position.x + 0.5f * count, storage[i].transform.position.y, storage[i].transform.position.z + 0.1f * count);

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

    //public void CheckAction(int num, int tag)
    //{
    //    switch (num)
    //    {
    //        case 2://내손에 두개 가져가기
    //            CardManager.instance.myHandScore.Add(gameObject); // 내 점수 필드에 넣음
    //            CardManager.instance.SetNextPosition(CheckTag()); // 포지션 미리 지정해 주기
    //            CardManager.instance.myHandScore.Add(CardManager.instance.field[list[0]]); // 필드꺼 내 점수 필드에 넣음
    //            CardManager.instance.SetNextPosition(CheckTag());
    //            CardManager.instance.field.Remove(CardManager.instance.field[list[0]]);//필드 삭제
    //            print("끝");
    //            break;

    //        case 3:
    //            CardManager.instance.ChoicePanel();//카드 둘중에 하나 고르기
    //                                               //CardManager.instance.myHandScore.Add(gameObject); // 내 점수 필드에 넣음
    //                                               //CardManager.instance.ScoreSetPosition(CardManager.instance.myHandScore, CheckTag(gameObject));
    //                                               //CardManager.instance.myHandScore.Add(sameObject[choiceNum]);//하나 고른 카드 내 손에 넣기
    //                                               //CardManager.instance.ScoreSetPosition(CardManager.instance.myHandScore, CheckTag(sameObject[choiceNum]));
    //            break;
    //    }
    //}

    public void CheckBomb()
    {
        if (sameTagIndex.Count == 4)
        {
            CardManager.instance.myHandScore.Add(gameObject); // 내 점수 필드에 넣음
            CardManager.instance.SetNextPosition(CheckTag()); // 포지션 미리 지정해 주기
            CardManager.instance.myHandScore.Add(CardManager.instance.field[sameTagIndex[0]]); // 필드꺼 내 점수 필드에 넣음
            CardManager.instance.SetNextPosition(CheckTag());
            CardManager.instance.myHandScore.Add(CardManager.instance.field[sameTagIndex[1]]); // 필드꺼 내 점수 필드에 넣음
            CardManager.instance.SetNextPosition(CheckTag());
            CardManager.instance.myHandScore.Add(CardManager.instance.field[sameTagIndex[2]]); // 필드꺼 내 점수 필드에 넣음
            CardManager.instance.SetNextPosition(CheckTag());
            CardManager.instance.field.Remove(CardManager.instance.field[sameTagIndex[0]]);//필드 삭제
            CardManager.instance.field.Remove(CardManager.instance.field[sameTagIndex[1]]);//필드 삭제
            CardManager.instance.field.Remove(CardManager.instance.field[sameTagIndex[2]]);//필드 삭제
        }
    }
    Vector3 CheckTag() //오브젝트 태그 체크
    {
        switch (type)
        {
            case "광":
                return CardManager.instance.scoreKingPosition;
            case "홍단":

            case "청단":

            case "초단":
                return CardManager.instance.scoreFlagPosition;

            case "새":
                return CardManager.instance.scoreAnimalPosition;

            default:
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
