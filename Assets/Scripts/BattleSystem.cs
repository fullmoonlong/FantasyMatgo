using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public GameObject actionPanel;

    public PlayerScript playerInfo;
    public PlayerScript opponentInfo;

    public GameObject player;
    public GameObject op;

    [HideInInspector] public int damage;
    [HideInInspector] public int playerTotalDamage;
    [HideInInspector] public int opponentTotalDamage;

    public BattleHUD playerHUD;
    public BattleHUD opHUD;

    [HideInInspector] public bool[] kingAttack;
    [HideInInspector] public bool[] flagAttack;
    [HideInInspector] public bool[] animalAttack;

    [HideInInspector] public bool[] enemyKingAttack;
    [HideInInspector] public bool[] enemyFlagAttack;
    [HideInInspector] public bool[] enemyAnimalAttack;
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

                    attackMotionImage.Add(Instantiate(flyAttackImage, CardManager.instance.scoreKingPosition[2], Quaternion.identity));
                }
                else
                {
                    opponentTotalDamage += 3;

                    attackMotionImage.Add(Instantiate(flyAttackImage, CardManager.instance.scoreEnemyKingPosition[2], Quaternion.identity));
                }

                break;

            case 4:
                damage += 4;
                if (GameManager.instance.isMyTurn == false)
                {
                    playerTotalDamage += 4;
                
                    attackMotionImage.Add(Instantiate(flyAttackImage, CardManager.instance.scoreKingPosition[2], Quaternion.identity));
                }
                else
                {
                    opponentTotalDamage += 4;
                   
                    attackMotionImage.Add(Instantiate(flyAttackImage, CardManager.instance.scoreEnemyKingPosition[2], Quaternion.identity));
                }

                break;

            case 5:
                damage += 5;

                if (GameManager.instance.isMyTurn == false)
                {
                    playerTotalDamage += 5;
              
                    attackMotionImage.Add(Instantiate(flyAttackImage, CardManager.instance.scoreKingPosition[2], Quaternion.identity));
                }
                else
                {
                    opponentTotalDamage += 5;
                 
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
            
                attackMotionImage.Add(Instantiate(flyAttackImage, CardManager.instance.scoreRedFlagPosition[1], Quaternion.identity));
            }
            else
            {
                opponentTotalDamage += 3;
            
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
             
                attackMotionImage.Add(Instantiate(flyAttackImage, CardManager.instance.scoreBlueFlagPosition[1], Quaternion.identity));
            }
            else
            {
                opponentTotalDamage += 3;
            
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
               
                attackMotionImage.Add(Instantiate(flyAttackImage, CardManager.instance.scoreNormalFlagPosition[1], Quaternion.identity));
            }
            else
            {
                opponentTotalDamage += 3;
            
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
            }
            else
            {
                opponentTotalDamage += 2;
            }
        }

        if (result == 7)
        {
            damage += 3;
            if (GameManager.instance.isMyTurn == false)
            {
                playerTotalDamage += 3;
            }
            else
            {
                opponentTotalDamage += 3;
            }
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
                }
                else
                {
                    opponentTotalDamage++;
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

            attackMotionImage.Add(Instantiate(flyAttackImage, CardManager.instance.scoreAnimalPosition[1], Quaternion.identity));
        }
        else
        {
            opponentTotalDamage += 5;

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
                }
                else
                {
                    opponentTotalDamage++;
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
    }
    public void SoldierAttack(int soldier)
    {
        if (soldier >= 10)
        {
            if (GameManager.instance.isMyTurn == false)
            {
                playerTotalDamage += (CardManager.instance.soldierEmptyIndex - 9);
            }
            else
            {
                opponentTotalDamage += (CardManager.instance.enemySoldierEmptyIndex - 9);
            }
        }

    }
}
