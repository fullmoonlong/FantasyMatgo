using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public int maxHp;
    public int currentHp;
    public int totalDamage;
    
    private void Awake()
    {
        maxHp = 100;
        if (!PlayerPrefs.HasKey(gameObject.name + "Game_Hp"))
        {
            PlayerPrefs.SetInt(gameObject.name + "Game_Hp", maxHp);
        }

        print(gameObject.name);
        currentHp = PlayerPrefs.GetInt(gameObject.name + "Game_Hp");
        totalDamage = 0;
    }
}
