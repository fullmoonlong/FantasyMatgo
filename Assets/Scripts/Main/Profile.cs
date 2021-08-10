using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  
public class Profile : MonoBehaviour
{
    #region Singleton
    public static Profile instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion
    public Image playerImage;
    public Text playerName;
    public int currentHp;
    public Text hpText;
    private void Start()
    {
        if(!PlayerPrefs.HasKey("HP"))
        {
            PlayerPrefs.SetInt("HP", 10000);
        }

        currentHp = PlayerPrefs.GetInt("HP");
    }

    public void SetHUD()
    {
        currentHp = PlayerPrefs.GetInt("HP");
        hpText.text = "HP : " + currentHp.ToString();
    }

    public void SetHp(int hp)
    {
        PlayerPrefs.SetInt("HP", hp);
        hpText.text = "HP : " + hp.ToString();
    }
}
