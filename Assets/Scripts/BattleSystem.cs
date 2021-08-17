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

    public PlayerScript playerInfo;
    public PlayerScript opponentInfo;

    public GameObject player;
    public GameObject op;

    public bool isKingCacled = false;
    public int damage;
    public int playerTotalDamage;
    public int opponentTotalDamage;

    //public GameObject startPanel;

    public BattleHUD playerHUD;
    public BattleHUD opHUD;

    int attackCount;

    public bool[] kingAttack;
    public bool[] flagAttack;
    public bool[] animalAttack;

    public bool[] enemyKingAttack;
    public bool[] enemyFlagAttack;
    public bool[] enemyAnimalAttack;
    // Start is called before the first frame update
    // Update is called once per frame

    private void Start()
    {
        kingAttack = new bool[3];
        flagAttack = new bool[8];
        enemyKingAttack = new bool[3];
        enemyFlagAttack = new bool[8];
        animalAttack = new bool[1];
        enemyAnimalAttack = new bool[1];
        for (int i = 0; i < 3; i++)
        {
            kingAttack[i] = false;

            enemyKingAttack[i] = false;

        }

        for (int i = 0; i < 8; i++)
        {
            flagAttack[i] = false;
            enemyFlagAttack[i] = false;
        }
        animalAttack[0] = false;
        enemyAnimalAttack[0] = false;
        SettingBattle();
    }

    public void SettingBattle()
    {
        player.SetActive(true);
        op.SetActive(true);

        playerInfo = player.GetComponent<PlayerScript>();
        opponentInfo = op.GetComponent<PlayerScript>();

        playerHUD.SetHUD(playerInfo);
        opHUD.SetHUD(opponentInfo);
    }

    public void Damaged(PlayerScript ui, BattleHUD hud)
    {
        ui.currentHp -= damage;

        hud.SetHp(ui.currentHp);
    }

    public void LightAttack(int mylight, int oplight)
    {
        int times = 1;

        switch (mylight)
        {
            case 3:
                print("3점");
                damage = 3;
                if (isKingCacled == false)
                {
                    if (GameManager.instance.isMyTurn == false)
                    {
                        playerTotalDamage += 3;
                    }
                    else
                    {
                        opponentTotalDamage += 3;
                    }
                    isKingCacled = true;
                }
                break;

            case 4:
                print("4점");
                damage = 4;
                if (isKingCacled == false)
                {
                    if (GameManager.instance.isMyTurn == false)
                    {
                        playerTotalDamage += 4;
                    }
                    else
                    {
                        opponentTotalDamage += 4;
                    }
                    isKingCacled = true;
                }
                break;

            case 5:
                print("5점");
                damage = 5;
                if (isKingCacled == false)
                {
                    if (GameManager.instance.isMyTurn == false)
                    {
                        playerTotalDamage += 5;
                    }
                    else
                    {
                        opponentTotalDamage += 5;
                    }
                    isKingCacled = true;
                }
                break;

            default:
                break;
        }

        if (mylight > 3 && oplight == 0)
        {
            times = 2;
        }

        damage *= times;
    }

    public void FlagAttack(int red, int blue, int brown)
    {
        damage = 0;
        if (red == 3)
        {
            damage += 3;
            if (GameManager.instance.isMyTurn == true)
            {
                playerTotalDamage += 3;
            }
            else
            {
                opponentTotalDamage += 3;
            }
            //print("red");
        }
        if (blue == 3)
        {
            damage += 3;
            if (GameManager.instance.isMyTurn == true)
            {
                playerTotalDamage += 3;
            }
            else
            {
                opponentTotalDamage += 3;
            }
            //print("blue");
        }

        if (brown >= 3)
        {
            damage += 3;
            if (GameManager.instance.isMyTurn == true)
            {
                playerTotalDamage += 3;
            }
            else
            {
                opponentTotalDamage += 3;
            }
            //print("brown");
        }

    }

    public void ResultFlag(int result)
    {
        damage = 0;

        if (result == 6)
        {
            damage += 2;
            if (GameManager.instance.isMyTurn == true)
            {
                playerTotalDamage += 2;
            }
            else
            {
                opponentTotalDamage += 2;
            }
            //print("result");
        }

        if (result == 7)
        {
            damage += 3;
            if (GameManager.instance.isMyTurn == true)
            {
                playerTotalDamage += 3;
            }
            else
            {
                opponentTotalDamage += 3;
            }
            //print("result");
        }

        if (result > 7)
        {
            damage += 3;
            for (int i = 7; i < result; i++)
            {
                damage++;
                if (GameManager.instance.isMyTurn == true)
                {
                    playerTotalDamage++;
                }
                else
                {
                    opponentTotalDamage++;
                }
            }
        }

        print(damage);
    }

    public void GoDoRiAttack()
    {
        damage = 0;
        damage = 5;
        if (GameManager.instance.isMyTurn == true)
        {
            playerTotalDamage += 5;
        }
        else
        {
            opponentTotalDamage += 5;
        }

    }

    public void SoldierAttack(int soldier)
    {
        if (soldier >= 10)
        {
            if (GameManager.instance.isMyTurn == true)
            {
                playerTotalDamage = (CardManager.instance.soldierEmptyIndex - 9);
            }
            else
            {
                opponentTotalDamage = (CardManager.instance.enemySoldierEmptyIndex - 9);
            }
        }
    }
}
