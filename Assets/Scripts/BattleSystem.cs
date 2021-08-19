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
    public GameObject flyAttackImage;
    public List<GameObject> attackMotionImage;
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
        animalAttack = new bool[6];
        enemyAnimalAttack = new bool[6];
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

        for (int i = 0; i < 6; i++)
        {
            animalAttack[i] = false;
            enemyAnimalAttack[i] = false;
        }

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

    public void TotalDamaged(PlayerScript ui, BattleHUD hud, int _damage)
    {
        ui.currentHp -= _damage;

        hud.SetHp(ui.currentHp);
    }

    public void LightAttack(int mylight, int oplight)
    {
        int times = 1;

        switch (mylight)
        {
            case 3:
                damage += 3;

                if (GameManager.instance.isMyTurn == false)
                {
                    playerTotalDamage += 3;
                    Debug.Log("큐브 3뎀축적(플레이어)");
                    attackMotionImage.Add(Instantiate(flyAttackImage, CardManager.instance.scoreKingPosition[2], Quaternion.identity));
                }
                else
                {
                    opponentTotalDamage += 3;
                    Debug.Log("큐브 3뎀축적(적)");
                    attackMotionImage.Add(Instantiate(flyAttackImage, CardManager.instance.scoreEnemyKingPosition[2], Quaternion.identity));
                }

                break;

            case 4:
                damage += 4;
                if (GameManager.instance.isMyTurn == false)
                {
                    playerTotalDamage += 4;
                    Debug.Log("큐브 4뎀축적(플레이어)");
                    attackMotionImage.Add(Instantiate(flyAttackImage, CardManager.instance.scoreKingPosition[2], Quaternion.identity));
                }
                else
                {
                    opponentTotalDamage += 4;
                    Debug.Log("큐브 4뎀축적(적)");
                    attackMotionImage.Add(Instantiate(flyAttackImage, CardManager.instance.scoreEnemyKingPosition[2], Quaternion.identity));
                }

                break;

            case 5:
                damage += 5;

                if (GameManager.instance.isMyTurn == false)
                {
                    playerTotalDamage += 5;
                    Debug.Log("큐브 5뎀축적(플레이어)");
                    attackMotionImage.Add(Instantiate(flyAttackImage, CardManager.instance.scoreKingPosition[2], Quaternion.identity));
                }
                else
                {
                    opponentTotalDamage += 5;
                    Debug.Log("큐브 5뎀축적(적)");
                    attackMotionImage.Add(Instantiate(flyAttackImage, CardManager.instance.scoreEnemyKingPosition[2], Quaternion.identity));
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
        if (red == 3)
        {
            damage += 3;
            if (GameManager.instance.isMyTurn == false)
            {
                playerTotalDamage += 3;
                Debug.Log("홍단 3뎀축적(플레이어)");
                attackMotionImage.Add(Instantiate(flyAttackImage, CardManager.instance.scoreRedFlagPosition[1], Quaternion.identity));
            }
            else
            {
                opponentTotalDamage += 3;
                Debug.Log("홍단 3뎀축적(적)");
                attackMotionImage.Add(Instantiate(flyAttackImage, CardManager.instance.scoreEnemyRedFlagPosition[1], Quaternion.identity));
            }
            //print("red");
        }
        if (blue == 3)
        {
            damage += 3;
            if (GameManager.instance.isMyTurn == false)
            {
                playerTotalDamage += 3;
                Debug.Log("청단 3뎀축적(플레이어)");
                attackMotionImage.Add(Instantiate(flyAttackImage, CardManager.instance.scoreBlueFlagPosition[1], Quaternion.identity));
            }
            else
            {
                opponentTotalDamage += 3;
                Debug.Log("청단 3뎀축적(적)");
                attackMotionImage.Add(Instantiate(flyAttackImage, CardManager.instance.scoreEnemyBlueFlagPosition[1], Quaternion.identity));
            }
            //print("blue");
        }

        if (brown >= 3)
        {
            damage += 3;
            if (GameManager.instance.isMyTurn == false)
            {
                playerTotalDamage += 3;
                Debug.Log("초단 3뎀축적(플레이어)");
                attackMotionImage.Add(Instantiate(flyAttackImage, CardManager.instance.scoreNormalFlagPosition[1], Quaternion.identity));
            }
            else
            {
                opponentTotalDamage += 3;
                Debug.Log("초단 3뎀축적(적)");
                attackMotionImage.Add(Instantiate(flyAttackImage, CardManager.instance.scoreEnemyNormalFlagPosition[1], Quaternion.identity));
            }
            //print("brown");
        }

    }

    public void ResultFlag(int result)
    {
        if (result == 6)
        {
            damage += 2;
            if (GameManager.instance.isMyTurn == false)
            {
                playerTotalDamage += 2;
                Debug.Log("총단갯수6개 2뎀축적(플레이어)");
            }
            else
            {
                opponentTotalDamage += 2;
                Debug.Log("총단갯수6개 2뎀축적(적)");
            }
            //print("result");
        }

        if (result == 7)
        {
            damage += 3;
            if (GameManager.instance.isMyTurn == false)
            {
                playerTotalDamage += 3;
                Debug.Log("총단갯수7개 3뎀축적(플레이어)");
            }
            else
            {
                opponentTotalDamage += 3;
                Debug.Log("총단갯수7개 3뎀축적(적)");
            }
            //print("result");
        }

        if (result > 7)
        {
            damage += 3;
            for (int i = 7; i < result; i++)
            {
                damage++;
                if (GameManager.instance.isMyTurn == false)
                {
                    playerTotalDamage++;
                    Debug.Log("총단갯수7개초과 개당 1뎀축적(플레이어)");
                }
                else
                {
                    opponentTotalDamage++;
                    Debug.Log("총단갯수7개초과 개당 1뎀축적(적)");
                }
            }
        }


        if (GameManager.instance.isMyTurn == false)
        {
            attackMotionImage.Add(Instantiate(flyAttackImage, CardManager.instance.scoreBlueFlagPosition[1], Quaternion.identity));
        }

        if (GameManager.instance.isMyTurn)
        {
            attackMotionImage.Add(Instantiate(flyAttackImage, CardManager.instance.scoreEnemyBlueFlagPosition[1], Quaternion.identity));
        }

    }

    public void GoDoRiAttack()
    {
        damage += 5;
        if (GameManager.instance.isMyTurn == false)
        {
            playerTotalDamage += 5;
            Debug.Log("암흑오브(고도리) 5뎀축적(플레이어)");
            attackMotionImage.Add(Instantiate(flyAttackImage, CardManager.instance.scoreAnimalPosition[1], Quaternion.identity));
        }
        else
        {
            opponentTotalDamage += 5;
            Debug.Log("암흑오브(고도리) 5뎀축적(적)");
            attackMotionImage.Add(Instantiate(flyAttackImage, CardManager.instance.scoreEnemyAnimalPosition[1], Quaternion.identity));
        }

    }

    public void ResultAnimal(int result)
    {
        if (result > 4)
        {
            for (int i = 4; i < result; i++)
            {
                damage++;
                if (GameManager.instance.isMyTurn == false)
                {
                    playerTotalDamage++;
                    Debug.Log("오브(고도리,열끗(물건))4개 초과당 개당 1뎀축적(플레이어)");
                }
                else
                {
                    opponentTotalDamage++;
                    Debug.Log("오브(고도리,열끗(물건))4개 초과당 개당 1뎀축적(적)");
                }
            }
        }


        if (GameManager.instance.isMyTurn == false)
        {
            attackMotionImage.Add(Instantiate(flyAttackImage, CardManager.instance.scoreThingPosition[2], Quaternion.identity));
        }

        if (GameManager.instance.isMyTurn)
        {
            attackMotionImage.Add(Instantiate(flyAttackImage, CardManager.instance.scoreEnemyThingPosition[2], Quaternion.identity));
        }

        print(damage);
    }
    public void SoldierAttack(int soldier)
    {
        if (soldier >= 10)
        {
            if (GameManager.instance.isMyTurn == false)
            {
                playerTotalDamage += (CardManager.instance.soldierEmptyIndex - 9);
                Debug.Log("피 10개이상 개당 1뎀축적(플레이어)");
            }
            else
            {
                opponentTotalDamage += (CardManager.instance.enemySoldierEmptyIndex - 9);
                Debug.Log("피 10개이상 개당 1뎀축적(적)");
            }
        }

    }
}
