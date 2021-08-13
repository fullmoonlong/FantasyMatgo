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
    public Text licenseText;
    private void Start()
    {
        //PlayerPrefs.DeleteAll();
        if(!PlayerPrefs.HasKey("HP"))
        {
            PlayerPrefs.SetInt("HP", 1000);
        }

        if(!PlayerPrefs.HasKey("Coin"))
        {
            PlayerPrefs.SetInt("Coin", 1000);
        }

        if (!PlayerPrefs.HasKey("License"))
        {
            PlayerPrefs.SetInt("License", 0);
        }
        if (!PlayerPrefs.HasKey("Coin"))
        {
            PlayerPrefs.SetInt("Coin", 0);
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

        licenseText.text = "License : "  + LicenseName();

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

    public void AddCoin(int value)
    {
        currentCoin = PlayerPrefs.GetInt("Coin");
        PlayerPrefs.SetInt("Coin", currentCoin + value);
    }

    public string LicenseName()
    {
        switch (PlayerPrefs.GetInt("License"))
        {
            case 0:
                return "N2";
            case 1:
                return "N1";

            case 2:
                return "MAGIC";

            case 3:
                return "RARE";

            case 4:
                return "UNIQUE";

            case 5:
                return "EPIC";

            default:
                return "";
        }

    }
}
