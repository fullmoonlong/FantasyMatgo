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

    public bool isPhaseOver = false;

    public void DamageCalculation()
    {
        Debug.Log("Calculation Start");
        if (isPhaseOver == false)
        {
            // player damage phase
            Debug.Log("정산 전 내 피: " + PlayerPrefs.GetInt(BattleSystem.instance.player.name + "Game_Hp"));
            PlayerPrefs.SetInt(BattleSystem.instance.player.name + "Game_Hp",
                PlayerPrefs.GetInt(BattleSystem.instance.player.name + "Game_Hp") - BattleSystem.instance.opponentTotalDamage);

            BattleSystem.instance.player.GetComponent<BattleHUD>().SetHp(PlayerPrefs.GetInt(BattleSystem.instance.player.name + "Game_Hp"));

            Debug.Log(BattleSystem.instance.opponentTotalDamage); // 내가 받을 데미지
            Debug.Log("정산 후 내 피: " + PlayerPrefs.GetInt(BattleSystem.instance.player.name + "Game_Hp"));

            //opponent damage phase
            Debug.Log("정산 전 적 피 : " + PlayerPrefs.GetInt(BattleSystem.instance.op.name + "Game_Hp"));
            PlayerPrefs.SetInt(BattleSystem.instance.op.name + "Game_Hp",
                PlayerPrefs.GetInt(BattleSystem.instance.op.name + "Game_Hp") - BattleSystem.instance.playerTotalDamage);
            
            BattleSystem.instance.op.GetComponent<BattleHUD>().SetHp(PlayerPrefs.GetInt(BattleSystem.instance.op.name + "Game_Hp"));

            Debug.Log(BattleSystem.instance.playerTotalDamage); // 상대가 받을 데미지
            Debug.Log("정산 후 적 피 : " + PlayerPrefs.GetInt(BattleSystem.instance.op.name + "Game_Hp"));

            isPhaseOver = true;
        }
    }
}
