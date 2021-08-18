using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
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

    public IEnumerator MyDamageCalculation()
    {
        Debug.Log("Calculation Start");
        if (isPhaseOver == false)
        {
            // player damage phase
            Debug.Log("정산 전 내 체력: " + PlayerPrefs.GetInt(BattleSystem.instance.player.name + "Game_Hp"));
            //PlayerPrefs.SetInt(BattleSystem.instance.player.name + "Game_Hp",
            //    PlayerPrefs.GetInt(BattleSystem.instance.player.name + "Game_Hp") - BattleSystem.instance.opponentTotalDamage);

            //BattleSystem.instance.player.GetComponent<BattleHUD>().SetHp(PlayerPrefs.GetInt(BattleSystem.instance.player.name + "Game_Hp"));

            //Debug.Log(BattleSystem.instance.opponentTotalDamage); // 내가 받을 데미지
            //Debug.Log(BattleSystem.instance.opponentTotalDamage - (MatgoScore.instance.opponentGwangScore + MatgoScore.instance.opponentBlueFlagScore + MatgoScore.instance.opponentRedFlagScore + MatgoScore.instance.opponentNormalFlagScore + MatgoScore.instance.opponentAnimalScore));
            
            GameManager.instance.AttackPanel.SetActive(true);
            Sequence mysequence = DOTween.Sequence();
            BattleSystem.instance.attackImage.transform.position = BattleSystem.instance.op.transform.position;

            mysequence.Append(BattleSystem.instance.attackImage.transform.DOScale(Vector3.one * 0.3f, 0.3f)
                .SetEase(Ease.InOutBack)).Join(BattleSystem.instance.op.transform.DOShakePosition(1f, 5f))
              .AppendInterval(1.2f).Append(BattleSystem.instance.attackImage.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InOutBack)).OnComplete(() => BattleSystem.instance.TotalDamaged(BattleSystem.instance.opponentInfo, BattleSystem.instance.opHUD, BattleSystem.instance.playerTotalDamage));
            

            yield return new WaitForSeconds(4f);
            Debug.Log("정산 후 내 체력: " + PlayerPrefs.GetInt(BattleSystem.instance.player.name + "Game_Hp"));
            GameManager.instance.AttackPanel.SetActive(false);
            GameManager.instance.isAttack = false;

            StartCoroutine(OpDamageCalculation());
        }
    }

    public IEnumerator OpDamageCalculation()
    {
        Debug.Log("정산 전 내 체력: " + PlayerPrefs.GetInt(BattleSystem.instance.op.name + "Game_Hp"));
        GameManager.instance.AttackPanel.SetActive(true);
        Sequence mysequence = DOTween.Sequence();
        BattleSystem.instance.attackImage.transform.position = BattleSystem.instance.player.transform.position;

        mysequence.Append(BattleSystem.instance.attackImage.transform.DOScale(Vector3.one * 0.3f, 0.3f)
            .SetEase(Ease.InOutBack)).Join(BattleSystem.instance.player.transform.DOShakePosition(1f, 5f))
          .AppendInterval(1.2f).Append(BattleSystem.instance.attackImage.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InOutBack)).OnComplete(() => BattleSystem.instance.TotalDamaged(BattleSystem.instance.playerInfo, BattleSystem.instance.playerHUD, BattleSystem.instance.opponentTotalDamage));


        
        yield return new WaitForSeconds(4f);
        Debug.Log("정산 후 내 체력: " + PlayerPrefs.GetInt(BattleSystem.instance.op.name + "Game_Hp"));

        GameManager.instance.AttackPanel.SetActive(false);
        GameManager.instance.isAttack = false;

        isPhaseOver = true;
        StartCoroutine(GameManager.instance.Retry());
    }
}
