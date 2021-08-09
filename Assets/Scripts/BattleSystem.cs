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

    public PlayerScript playerUi;
    public PlayerScript opUi;

    public GameObject player;
    public GameObject op;

    public int damage;

    //public GameObject startPanel;

    public BattleHUD playerHUD;
    public BattleHUD opHUD;

    int attackCount;

    public bool[] kingAttack;
    public bool[] flagAttack;
    public bool animalAttack;

    public bool[] enemyKingAttack;
    public bool[] enemyFlagAttack;
    public bool enemyAnimalAttack;
    // Start is called before the first frame update
    // Update is called once per frame

    private void Start()
    {
        kingAttack = new bool[3];
        flagAttack = new bool[3];
        enemyKingAttack = new bool[3];
        enemyFlagAttack = new bool[3];
        for (int i=0;i<3;i++)
        {
            kingAttack[i] = false;
            flagAttack[i] = false;
            enemyKingAttack[i] = false;
            enemyFlagAttack[i] = false;
        }
        animalAttack = false;
        enemyAnimalAttack = false;
        SettingBattle();
    }
    public void Update()
    {


        //흔들기 //print("my turn");
    }

    public void SettingBattle()
    {
        player.SetActive(true);
        op.SetActive(true);

        playerUi = player.GetComponent<PlayerScript>();
        opUi = op.GetComponent<PlayerScript>();

        playerHUD.SetHUD(playerUi);
        opHUD.SetHUD(opUi);

        //Invoke("PlayerTurn",2f);
    }

    public void PlayerTurn(int type)
    {
        Sequence mysequence = DOTween.Sequence();

        switch (type)
        {
            case 0:
                //print("attackcount: " + attackCount);
                //LightAttack(CardManager.instance.kingEmptyIndex, CardManager.instance.enemyKingEmptyIndex);
                LightAttack(CardManager.instance.kingEmptyIndex, CardManager.instance.enemyKingEmptyIndex);
                break;

            case 1:
                GoDoRiAttack(CardManager.instance.animalEmptyIndex);
                break;

            case 2:
                damage = 3;
                //FlagAttack(CardManager.instance.redFlagEmptyIndex, CardManager.instance.blueFlagEmptyIndex, CardManager.instance.normalFlagEmptyIndex);
                break;


            case 3:
                SoldierAttack(CardManager.instance.soldierEmptyIndex);
                break;

            default:
                break;
        }

        attackImage.transform.position = op.transform.position;

        mysequence.Append(attackImage.transform.DOScale(Vector3.one * 0.3f, 0.3f).SetEase(Ease.InOutBack)).Join(op.transform.DOShakePosition(1f,5f))
            .AppendInterval(1.2f).Append(attackImage.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InOutBack)).OnComplete(() => Damaged(opUi, opHUD));

        //Invoke("OpTurn", 3f);
    }

    public void OpTurn(int type)
    {
        Sequence mysequence = DOTween.Sequence();

        switch (type)
        {
            case 0:
          
                LightAttack(CardManager.instance.enemyKingEmptyIndex, CardManager.instance.kingEmptyIndex);
                break;

            case 1:
                GoDoRiAttack(CardManager.instance.enemyAnimalEmptyIndex);
                break;

            case 2:
                damage = 3;
                //FlagAttack(CardManager.instance.redFlagEmptyIndex, CardManager.instance.blueFlagEmptyIndex, CardManager.instance.normalFlagEmptyIndex);
                break;


            case 3:
                SoldierAttack(CardManager.instance.enemySoldierEmptyIndex);
                break;

            default:
                break;
        }


        attackImage.transform.position = player.transform.position;

        mysequence.Append(attackImage.transform.DOScale(Vector3.one * 0.3f, 0.3f).SetEase(Ease.InOutBack)).Join(player.transform.DOShakePosition(1f,5f))
          .AppendInterval(1.2f).Append(attackImage.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InOutBack)).OnComplete(() => Damaged(playerUi, playerHUD));


        //if (attackCount != 3)
        //{
        //    Invoke("PlayerTurn", 3f);
        //}

        //attackCount++;
    }

    public void Damaged(PlayerScript ui, BattleHUD hud)
    {
        ui.currentHp -= damage;
        //print(damage);
        //PlayerPrefs.SetInt("HP", ui.currentHp);
        hud.SetHp(ui.currentHp);
    }
    public void LightAttack(int mylight, int oplight)
    {
        int times = 1;
        //kingEmptyIndex
        switch (mylight)
        {
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
        print("damage : " + damage);
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
            //print("red");
        }
        if (blue == 3)
        {
            damage += 3;
            //print("blue");
        }

        if (brown >= 3)
        {
            damage += 3;
            //print("brown");
        }

        if (result == 7)
        {
            damage += 3;
            //print("result");
        }

        if (result == 6)
        {
            damage += 2;
            //print("result");
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
        print(damage);
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
        print(damage);
    }
}
