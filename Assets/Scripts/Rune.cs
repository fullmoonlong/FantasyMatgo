using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Rune", menuName = "Rune")]
public class Rune : ScriptableObject
{
    public string   runeName;       // 룬의 이름
    public string   type;           // 룬이 지니는 속성
    public string   description;    // 룬에 대한 설명
    public int      price;          // 룬의 가격
    public Sprite   image;          // 룬의 디자인
}
