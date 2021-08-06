using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Rune", menuName = "Rune")]
public class Rune : ScriptableObject
{
    public string   runeName;       // ���� �̸�
    public string   type;           // ���� ���ϴ� �Ӽ�
    public string   description;    // �鿡 ���� ����
    public int      price;          // ���� ����
    public Sprite   image;          // ���� ������
}
