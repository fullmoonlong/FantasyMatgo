using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public int maxHp;
    public int currentHp;

    void Awake()
    {
        maxHp = 100;
        //if (!PlayerPrefs.HasKey("Game_Hp"))
        //{
        //    PlayerPrefs.SetInt("Game_Hp", maxHp);
        //}

        //currentHp = PlayerPrefs.GetInt("Game_Hp");
        currentHp = maxHp;
    }
}
