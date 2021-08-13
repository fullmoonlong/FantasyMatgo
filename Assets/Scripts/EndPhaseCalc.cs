using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPhaseCalc : MonoBehaviour
{
    #region Singleton
    public static EndPhaseCalc instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion

    public void DamageCalculation()
    {
        PlayerPrefs.SetInt("HP", PlayerPrefs.GetInt("HP") + PlayerPrefs.GetInt(BattleSystem.instance.player.name + "Game_Hp"));
        PlayerPrefs.SetInt("HP", PlayerPrefs.GetInt("HP") + PlayerPrefs.GetInt(BattleSystem.instance.op.name + "Game_Hp"));
    }
}
