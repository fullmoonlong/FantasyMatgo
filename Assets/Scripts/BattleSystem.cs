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

    public int power;

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
                print("case 1");
                FlagAttack(CardManager.instance.redFlagEmptyIndex, CardManager.instance.blueFlagEmptyIndex, CardManager.instance.normalFlagEmptyIndex);
                break;

            default:
                break;
        }

        attackImage.transform.position = op.transform.position;

        mysequence.Append(attackImage.transform.DOScale(Vector3.one * 0.9f, 0.5f).SetEase(Ease.InOutBack))
            .AppendInterval(1.2f).Append(attackImage.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InOutBack)).OnComplete(() => Damaged(opUi, opHUD));

        if(attackCount != 3)
        {
            Invoke("OpTurn", 3f);
        }
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
                FlagAttack(CardManager.instance.enemyRedFlagEmptyIndex, CardManager.instance.enemyBlueFlagEmptyIndex, CardManager.instance.enemyNormalFlagEmptyIndex);
                break;
            default:
                break;
        }

        attackImage.transform.position = player.transform.position;

        mysequence.Append(attackImage.transform.DOScale(Vector3.one * 0.9f, 0.5f).SetEase(Ease.InOutBack))
          .AppendInterval(1.2f).Append(attackImage.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InOutBack)).OnComplete(() => Damaged(playerUi, playerHUD));

        attackCount++;

        Invoke("PlayerTurn", 3f);

    }

    void Damaged(PlayerScript ui, BattleHUD hud)
    {
        ui.currentHp -= power;
        print(power);
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
                power = 0;
                break;
            case 3:
                print("3점");
                power = 3;
                break;

            case 4:
                print("4점");
                power = 4;
                break;

            case 5:
                print("15점");
                power = 15;
                break;

            default:
                break;
        }

        if(mylight > 3 && oplight == 0)
        {
            times = 2;
        }
        power *= times;

        //Instantiate(lightObj, gameObject.transform.position, Quaternion.identity); // 애니메이션 

    }
    public void FlagAttack(int red, int blue, int brown)
    {
        power = 0;

        int result = red + blue + brown;
        if(red == 3)
        {
            power += 3;
            print("red");
        }
        else if (blue == 3)
        {
            power += 3;
            print("blue");
        }

        else if (brown == 3)
        {
            power += 3;
            print("brown");
        }

        else if (result == 7)
        {
            power += 3;
            print("result");
        }

        else if (result == 6)
        {
            power += 2;
            print("result");
        }

        else if(result > 7)
        {
            for(int i=7;i<result;i++)
            {
                power++;
            }
        }

        print(power);
    }
}
