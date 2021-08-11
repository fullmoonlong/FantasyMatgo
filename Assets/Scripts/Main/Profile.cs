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
    public int currentCoin;
    public Text coinText;
    private void Start()
    {
        //PlayerPrefs.DeleteAll();
        if(!PlayerPrefs.HasKey("HP"))
        {
            PlayerPrefs.SetInt("HP", 10000);
        }

        if(!PlayerPrefs.HasKey("Coin"))
        {
            PlayerPrefs.SetInt("Coin", 1000);
        }
        currentHp = PlayerPrefs.GetInt("HP");
        currentCoin = PlayerPrefs.GetInt("Coin");
    }

    public void SetHUD()
    {
        currentHp = PlayerPrefs.GetInt("HP");
        hpText.text = "HP : " + currentHp.ToString();

        currentCoin = PlayerPrefs.GetInt("Coin");
        coinText.text = currentCoin.ToString();
    }

    public void SetHp(int hp)
    {
        PlayerPrefs.SetInt("HP", hp);
        hpText.text = "HP : " + hp.ToString();
    }

    public void UseCoin(int value)
    {
        currentCoin = PlayerPrefs.GetInt("Coin");
        PlayerPrefs.SetInt("Coin", currentCoin - value);
    }
}
