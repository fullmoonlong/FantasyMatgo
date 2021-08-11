using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections;
using DG.Tweening;
using System.Collections.Generic;
public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion

    public Text turnText; // ������ ������ ǥ���ϴ� �ؽ�Ʈ
    public Text myScoreText;
    public Text opScoreText;
    public Text gameOverText;
    public GameObject gameOverPanel;
    public GameObject myFirstArtifactPanel;
    public GameObject mySecondArtifactPanel;
    public GameObject myThirdArtifactPanel;
    public GameObject opponentFirstArtifactPanel;
    public GameObject opponentSecondArtifactPanel;
    public GameObject opponentThirdArtifactPanel;
    public GameObject AttackPanel;

    public int maxTurnCount; // ������� ����� �� ��
    public int artifactNumMe;
    public int artifactNumOpponent;
    public bool isMyTurn; // ���� �����ϴ� bool ����
    public bool isGameEnd = false;
    public bool oneTime;
    public bool first;
    public bool battleFirst;
    public bool isMyFirstArtifact;
    public bool isMySecondArtifact;
    public bool isMyThirdArtifact;
    public bool isOppoFirstArtifact;
    public bool isOppoSecondArtifact;
    public bool isOppoThirdArtifact;
    public bool isSetting;
    public bool isMoving;
    public bool isBattle;
    public bool isBonus;
    public bool isShake;
    public bool isChoice;
    public bool isAttack;

    public Image[] artifactMe;
    public Image[] artifactOp;

    public GameObject[] artifact;

    List<Shop> CurArtifactList;

    public void Start()
    {
        isMyTurn = true;
        oneTime = false;
        first = true;
        isBattle = false;

        artifactNumMe = 0;
        artifactNumOpponent = 0;

        isMyFirstArtifact = false;
        isMySecondArtifact = false;

        isOppoFirstArtifact = false;
        isOppoSecondArtifact = false;

        isSetting = false;
        isMoving = false;

        maxTurnCount = 7;

        battleFirst = true;

        isBonus = false;

        isShake = false;

        isChoice = false;

        isAttack = false;

        StartCoroutine(CompleteSetting());
    }

    public void Update()
    {
        ScoreTextSet();
        TurnTextSet();

        //if(!isMyTurn)
        if (CardManager.instance.myHand.Count == 0 && CardManager.instance.opponentHand.Count == 0
            && mySecondArtifactPanel.activeSelf == false
            && opponentSecondArtifactPanel.activeSelf == false
            && myThirdArtifactPanel.activeSelf == false
            && opponentThirdArtifactPanel.activeSelf == false && !AttackPanel.activeSelf && !isAttack)
        {
            Invoke("Retry", 1f);
        }

        if (PlayerPrefs.GetInt(BattleSystem.instance.player.name + "Game_Hp") <= 0 || PlayerPrefs.GetInt(BattleSystem.instance.op.name + "Game_Hp") <= 0)
        {
            print("���� ����");
            if (PlayerPrefs.GetInt(BattleSystem.instance.player.name + "Game_Hp") > 0)
            {
                Profile.instance.currentHp += PlayerPrefs.GetInt(BattleSystem.instance.player.name + "Game_Hp");
                PlayerPrefs.SetInt("HP", Profile.instance.currentHp);
            }
            
            PlayerPrefs.DeleteKey(BattleSystem.instance.player.name + "Game_Hp");
            PlayerPrefs.DeleteKey(BattleSystem.instance.op.name + "Game_Hp");

            Invoke("GotoMain", 1f);
        }


        if (oneTime)
        {
            if (maxTurnCount > 0)
            {
                if (isMyTurn)
                {
                    //print("�ι�°");
                    CardManager.instance.ResetPosition(CardManager.instance.myHand);
                    CardManager.instance.DrawCard(CardManager.instance.myHand, 1);
                    oneTime = false;
                }

                else
                {
                    //print("�ι�°");
                    CardManager.instance.ResetPosition(CardManager.instance.opponentHand);
                    CardManager.instance.DrawCard(CardManager.instance.opponentHand, 1);
                    oneTime = false;
                }

                maxTurnCount -= 1;
            }

        }

    }

    public IEnumerator CompleteSetting()
    {
        yield return new WaitForSeconds(1f);
        isSetting = true;
    }

    public IEnumerator CompleteMoving()
    {
        yield return new WaitForSeconds(1f);
        isMoving = false;
    }

    public IEnumerator FixedMade(int king, int opking, int red, int blue, int normal, int animal, bool[] isking, bool[] isflag, bool[] isanimal, GameObject who, PlayerScript ui, BattleHUD hud)
    {
        print("attack");
        yield return new WaitForSeconds(1f);
        AttackPanel.SetActive(false);

        for (int i = 0; i < 3; i++)
        {
            if (!isking[i] && king == i + 3)
            {
                isAttack = true;
                print("�� ����");
                BattleSystem.instance.LightAttack(king, opking);
                StartCoroutine(AttackAction(who, ui, hud));
                isking[i] = true;
            }

            int flag = 0;
            switch (i)
            {
                case 0:
                    flag = red;
                    break;
                case 1:
                    flag = blue;
                    break;
                case 2:
                    flag = normal;
                    break;
                default:
                    break;
            }

            if (!isflag[i] && flag == 3)
            {
                isAttack = true;
                print("�÷��� ����");
                BattleSystem.instance.damage = 3;
                StartCoroutine(AttackAction(who, ui, hud));
                isflag[i] = true;
            }
        }

        if (!isanimal[0] && animal == 3)
        {
            isAttack = true;
            print("���� ����");
            BattleSystem.instance.damage = 5;
            StartCoroutine(AttackAction(who, ui, hud));
            isanimal[0] = true;
        }
    }
    public IEnumerator AttackAction(GameObject who, PlayerScript ui, BattleHUD hud)
    {
        AttackPanel.SetActive(true);
        Sequence mysequence = DOTween.Sequence();
        BattleSystem.instance.attackImage.transform.position = who.transform.position;

        mysequence.Append(BattleSystem.instance.attackImage.transform.DOScale(Vector3.one * 0.3f, 0.3f)
            .SetEase(Ease.InOutBack)).Join(who.transform.DOShakePosition(1f, 5f))
          .AppendInterval(1.2f).Append(BattleSystem.instance.attackImage.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InOutBack)).OnComplete(() => BattleSystem.instance.Damaged(ui, hud));

        yield return new WaitForSeconds(4f);
        AttackPanel.SetActive(false);
        isAttack = false;
    }

    public void ScoreCheck(bool isPlayer)
    {
        MatgoScore.instance.MyCardCountToScore();
        MatgoScore.instance.OpCardCountToScore();
        MatgoScore.instance.ScoreCalculate();
        if (isPlayer)
        {
            print("����");
            if (MatgoScore.myScore >= 3)
            {
                if (isMyFirstArtifact == false)
                {
                    ChooseMyArtifact("first");
                }
                isMyFirstArtifact = true;
            }

            if (MatgoScore.myScore >= 6)
            {
                if (isMySecondArtifact == false)
                {
                    ChooseMyArtifact("second");
                }
                isMySecondArtifact = true;
            }

            if (MatgoScore.myScore >= 7)
            {
                ChooseMyArtifact("third");
            }
        }

        else
        {
            if (MatgoScore.opScore >= 3)
            {
                if (isOppoFirstArtifact == false)
                {
                    ChooseOpponentArtifact("first");
                }
                isOppoFirstArtifact = true;
            }

            if (MatgoScore.opScore >= 6)
            {
                if (isOppoSecondArtifact == false)
                {
                    ChooseOpponentArtifact("second");
                }
                isOppoSecondArtifact = true;
            }

            if (MatgoScore.opScore >= 7)
            {
                ChooseOpponentArtifact("third");
            }
        }
    }

    public void ScoreTextSet()
    {
        myScoreText.text = "1P (6��) ���� : " + MatgoScore.myScore.ToString();
        opScoreText.text = "2P(12��) ���� : " + MatgoScore.opScore.ToString();
    }

    public void TurnTextSet()
    {
        if (isMyTurn == true)
        {
            turnText.text = "1P �� (6��)";
        }
        else
        {
            turnText.text = "2P �� (12��)";
        }
    }

    public void ArrangeArtifact() // ��� 6�� ����
    {
        //CurArtifactList = ShopManager.instance.AllShopList.FindAll(x => x.IsEq);

        for(int i=0;i<artifact.Length;i++)
        {
            artifact[i].SetActive(i < CurArtifactList.Count);
        }
    }
    public void ChooseMyArtifact(string order)
    {
        if (order == "first")
        {
            myFirstArtifactPanel.SetActive(true);
        }
        if (order == "second")
        {
            mySecondArtifactPanel.SetActive(true);
        }
        if (order == "third")
        {
            myThirdArtifactPanel.SetActive(true);
        }

        /// �� ������ ��Ƽ��Ʈ�� 4�� �̻� �Դ� �ý��� ����
        ///
    }

    public void ChooseOpponentArtifact(string order)
    {
        if (order == "first")
        {
            opponentFirstArtifactPanel.SetActive(true);
        }
        if (order == "second")
        {
            opponentSecondArtifactPanel.SetActive(true);
        }
        if (order == "third")
        {
            opponentThirdArtifactPanel.SetActive(true);
        }
        /// �� ������ ��Ƽ��Ʈ�� 4�� �̻� �Դ� �ý��� ����
        /// 
    }

    public void ApplyMyArtifact(int num)
    {
        GameObject btn = EventSystem.current.currentSelectedGameObject;
        artifactMe[artifactNumMe].sprite = btn.GetComponent<Image>().sprite;
        artifactMe[artifactNumMe].gameObject.SetActive(true);
        //CurArtifactList[num].IsHaving = false; // ������ ����
        //artifactMe[artifactNumMe].sprite = artifact[num].GetComponent<Image>().sprite;

        if (myFirstArtifactPanel.activeInHierarchy == true)
        {
            myFirstArtifactPanel.SetActive(false);
        }
        else if (mySecondArtifactPanel.activeInHierarchy == true)
        {
            mySecondArtifactPanel.SetActive(false);
        }
        else if (myThirdArtifactPanel.activeInHierarchy == true)
        {
            myThirdArtifactPanel.SetActive(false);
        }
        artifactNumMe++;

        if(!mySecondArtifactPanel.activeSelf && !myThirdArtifactPanel.activeSelf)
        {
            AttackPanel.SetActive(true);

            StartCoroutine(FixedMade(CardManager.instance.kingEmptyIndex, CardManager.instance.enemyKingEmptyIndex, CardManager.instance.redFlagEmptyIndex, CardManager.instance.blueFlagEmptyIndex, CardManager.instance.normalFlagEmptyIndex,
                CardManager.instance.animalEmptyIndex, BattleSystem.instance.kingAttack, BattleSystem.instance.flagAttack, BattleSystem.instance.animalAttack, BattleSystem.instance.op, BattleSystem.instance.opUi, BattleSystem.instance.opHUD));
        }

     }

    public void ApplyOpponentArtifact()
    {
        GameObject btn = EventSystem.current.currentSelectedGameObject;
        artifactOp[artifactNumOpponent].sprite = btn.GetComponent<Image>().sprite;
        artifactOp[artifactNumOpponent].gameObject.SetActive(true);

        if (opponentFirstArtifactPanel.activeInHierarchy == true)
        {
            opponentFirstArtifactPanel.SetActive(false);
        }
        else if (opponentSecondArtifactPanel.activeInHierarchy == true)
        {
            opponentSecondArtifactPanel.SetActive(false);
        }
        else if (opponentThirdArtifactPanel.activeInHierarchy == true)
        {
            opponentThirdArtifactPanel.SetActive(false);
        }
        artifactNumOpponent++;


        if (!opponentSecondArtifactPanel.activeSelf && !opponentThirdArtifactPanel.activeSelf)
        {
            AttackPanel.SetActive(true);

            StartCoroutine(FixedMade(CardManager.instance.enemyKingEmptyIndex, CardManager.instance.kingEmptyIndex, CardManager.instance.enemyRedFlagEmptyIndex, CardManager.instance.enemyBlueFlagEmptyIndex, CardManager.instance.enemyNormalFlagEmptyIndex,
           CardManager.instance.enemyAnimalEmptyIndex, BattleSystem.instance.enemyKingAttack, BattleSystem.instance.enemyFlagAttack, BattleSystem.instance.enemyAnimalAttack, BattleSystem.instance.player, BattleSystem.instance.playerUi, BattleSystem.instance.playerHUD));

        }

    }

    public void GameOver()
    {
        if (!gameOverPanel.activeSelf)
        {
            gameOverText.text = "Game Over";
            gameOverPanel.SetActive(true);
            isGameEnd = true;
        }
    }

    public void GotoMain()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void Retry()
    {
        SceneManager.LoadScene("Game");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}