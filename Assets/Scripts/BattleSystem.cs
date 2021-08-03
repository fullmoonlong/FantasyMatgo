using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class BattleSystem : MonoBehaviour
{
   
    #region Singleton
    public static BattleSystem instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion

    
    public GameObject attackImage;

    public GameObject startPanel;

    private PlayerScript playerUi;
    private PlayerScript opUi;

    public GameObject player;
    public GameObject op;

    public int damage;

    //public GameObject startPanel;

    public BattleHUD playerHUD;
    public BattleHUD opHUD;


    int attackCount;
    // Start is called before the first frame update
    // Update is called once per frame
    public void Update()
    {


        //흔들기 print("my turn");
    }

    public IEnumerator SettingBattle()
    {
        startPanel.SetActive(true);

        yield return new WaitForSeconds(2f);
        startPanel.SetActive(false);
        player.SetActive(true);
        op.SetActive(true);

        playerUi = player.GetComponent<PlayerScript>();
        opUi = op.GetComponent<PlayerScript>();

        playerHUD.SetHUD(playerUi);
        opHUD.SetHUD(opUi);

        Invoke("PlayerTurn",2f);
    }

    public void PlayerTurn()
    {
        Sequence mysequence = DOTween.Sequence();

        switch (attackCount)
        {
            case 0:
                attackCount = 0;
                print("attackcount: " + attackCount);
                LightAttack(CardManager.instance.kingEmptyIndex, CardManager.instance.enemyKingEmptyIndex);
                break;

            case 1:
                GoDoRiAttack(CardManager.instance.animalEmptyIndex);
                break;

            case 2:
                FlagAttack(CardManager.instance.redFlagEmptyIndex, CardManager.instance.blueFlagEmptyIndex, CardManager.instance.normalFlagEmptyIndex);
                break;


            case 3:
                SoldierAttack(CardManager.instance.soldierEmptyIndex);
                break;

            default:
                break;
        }

        attackImage.transform.position = op.transform.position;

        mysequence.Append(attackImage.transform.DOScale(Vector3.one * 0.9f, 0.5f).SetEase(Ease.InOutBack))
            .AppendInterval(1.2f).Append(attackImage.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InOutBack)).OnComplete(() => Damaged(opUi, opHUD));

        Invoke("OpTurn", 3f);
       
    }

    public void OpTurn()
    {
        Sequence mysequence = DOTween.Sequence();

        switch (attackCount)
        {
            case 0:
                LightAttack(CardManager.instance.enemyKingEmptyIndex, CardManager.instance.kingEmptyIndex);
                break;

            case 1:
                GoDoRiAttack(CardManager.instance.enemyAnimalEmptyIndex);
                break;

            case 2:
                FlagAttack(CardManager.instance.enemyRedFlagEmptyIndex, CardManager.instance.enemyBlueFlagEmptyIndex, CardManager.instance.enemyNormalFlagEmptyIndex);
                break;


            case 3:
                SoldierAttack(CardManager.instance.enemySoldierEmptyIndex);
                break;

            default:
                break;
        }

        attackImage.transform.position = player.transform.position;

        mysequence.Append(attackImage.transform.DOScale(Vector3.one * 0.9f, 0.5f).SetEase(Ease.InOutBack))
          .AppendInterval(1.2f).Append(attackImage.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InOutBack)).OnComplete(() => Damaged(playerUi, playerHUD));


        if (attackCount != 3)
        {
            Invoke("PlayerTurn", 3f);
        }

        attackCount++;
    }

    void Damaged(PlayerScript ui, BattleHUD hud)
    {
        ui.currentHp -= damage;
        print(damage);
        hud.SetHp(ui.currentHp);
    }
    public void LightAttack(int mylight, int oplight)
    {
        int times = 1;
        //kingEmptyIndex
        switch (mylight)
        {
            case 0:   
            case 1:
            case 2:
                print("nothing");
                damage = 0;
                break;
            case 3:
                print("3점");
                damage = 3;
                break;

            case 4:
                print("4점");
                damage = 4;
                break;

            case 5:
                print("15점");
                damage = 15;
                break;

            default:
                break;
        }

        if(mylight > 3 && oplight == 0)
        {
            times = 2;
        }
        damage *= times;

        //Instantiate(lightObj, gameObject.transform.position, Quaternion.identity); // 애니메이션 

    }
    public void FlagAttack(int red, int blue, int brown)
    {
        damage = 0;

        int result = red + blue + brown;
        if(red == 3)
        {
            damage += 3;
            print("red");
        }
        if (blue == 3)
        {
            damage += 3;
            print("blue");
        }

        if (brown >= 3)
        {
            damage += 3;
            print("brown");
        }

        if (result == 7)
        {
            damage += 3;
            print("result");
        }

        if (result == 6)
        {
            damage += 2;
            print("result");
        }

        if(result > 7)
        {
            damage += 3;
            for(int i=7;i<result;i++)
            {
                damage++;
            }
        }

        print(damage);
    }

    public void GoDoRiAttack(int bird)
    {
        damage = 0;
        if (bird == 3)
        {
            damage = 5;
        }
    }

    public void SoldierAttack(int soldier)
    {
        damage = 0;
        if(soldier >= 10)
        {
            damage = 1;
            for(int i = 10; i<soldier;i++)
            {
                damage++;
            }
        }
    }
}
