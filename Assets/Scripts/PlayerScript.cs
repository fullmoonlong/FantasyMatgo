using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public int maxHp;
    public int currentHp;

    void Awake()
    {
        //if (!PlayerPrefs.HasKey("HP"))
        //{
        //    PlayerPrefs.SetInt("HP", 100);
        //}
        maxHp = 100;

        //currentHp = PlayerPrefs.GetInt("HP");
        currentHp = maxHp;
    }
}
