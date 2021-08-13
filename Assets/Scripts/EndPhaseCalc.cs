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

    public void DamageCalculation(PlayerScript player)
    {
        Debug.Log(player.totalDamage);
        player.currentHp -= player.totalDamage;
    }
}
