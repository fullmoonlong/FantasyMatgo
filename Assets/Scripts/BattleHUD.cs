using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    public Text hpText;
    // Start is called before the first frame update
    public void SetHUD(PlayerScript player)
    {
        player.currentHp = PlayerPrefs.GetInt(player.name + "Game_Hp");
        hpText.text = player.currentHp.ToString();
    }

    public void SetHp(int hp)
    {
        print(gameObject.name);
        PlayerPrefs.SetInt(gameObject.name + "Game_Hp", hp);
        //hpText.text = PlayerPrefs.GetInt("HP").ToString();
        hpText.text = hp.ToString();
    }
}
