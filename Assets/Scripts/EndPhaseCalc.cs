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
        Debug.Log("Calculation Start");
        PlayerPrefs.SetInt("HP", PlayerPrefs.GetInt(BattleSystem.instance.player.name + "Game_Hp") - BattleSystem.instance.playerTotalDamage);
        Debug.Log(BattleSystem.instance.playerTotalDamage);
        Debug.Log("PlayerSET");
        PlayerPrefs.SetInt("HP", PlayerPrefs.GetInt(BattleSystem.instance.op.name + "Game_Hp") - BattleSystem.instance.opponentTotalDamage);
        Debug.Log(BattleSystem.instance.opponentTotalDamage);
        Debug.Log("OP SET");
    }
}
