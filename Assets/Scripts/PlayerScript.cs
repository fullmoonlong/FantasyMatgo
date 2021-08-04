using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public int currentHp;

    private void Start()
    {
       
        if (!PlayerPrefs.HasKey("HP"))
        {
            PlayerPrefs.SetInt("HP", 100);
            currentHp = 100;
        }
     
        currentHp = PlayerPrefs.GetInt("HP");
    }
}
