using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    public Text hpText;
    // Start is called before the first frame update
    public  void SetHUD(PlayerScript player)
    {
        hpText.text = player.maxHp.ToString();
    }

    public void SetHp(int hp)
    {
        hpText.text = hp.ToString();
    }
}
