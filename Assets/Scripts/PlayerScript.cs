﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public int maxHp;
    public int currentHp;

    void Awake()
    {
        maxHp = 10;
        if (!PlayerPrefs.HasKey(gameObject.name + "Game_Hp"))
        {
            PlayerPrefs.SetInt(gameObject.name + "Game_Hp", maxHp);
        }

        print(gameObject.name);
        currentHp = PlayerPrefs.GetInt(gameObject.name + "Game_Hp");
        //currentHp = maxHp;
    }
}
