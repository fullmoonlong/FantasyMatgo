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
    public GameObject ArtifactPanel;
    public GameObject AttackPanel;

    public int maxTurnCount; // ������� ����� �� ��
    public int artifactNumMe;
    public int artifactNumOpponent;
    public bool isMyTurn; // ���� �����ϴ� bool ����
    public bool isGameEnd = false;
    public bool oneTime;
    public bool first;

    public bool[] isMyArtifact;
    public bool[] isOpArtifact;

    public bool isSetting;
    public bool isMoving;
    public bool isBattle;
    public bool isBonus;
    public bool isShake;
    public bool isChoice;
    public bool isAttack;

    public Image[] artifactMe;
    public Image[] artifactOp;

    //public GameObject[] artifact;

    List<Shop> CurArtifactList;

    int MyOpenNum;
    int OpOpenNum;

    List<int> AllArtifact;
    public void Start()
    {
        isMyTurn = true;
        oneTime = false;
        first = true;
        isBattle = false;

        artifactNumMe = 0;
        artifactNumOpponent = 0;

        isMyArtifact = new bool[3];
        isOpArtifact = new bool[3];
        for (int i=0;i<isMyArtifact.Length;i++)
        {
            isMyArtifact[i] = true;
            isOpArtifact[i] = true;
        }

        isSetting = false;
        isMoving = false;

        maxTurnCount = 7;

        isBonus = false;

        isShake = false;

        isChoice = false;

        isAttack = false;

        MyOpenNum = 0;
        OpOpenNum = 0;


        AllArtifact = new List<int>();
        ArrangeArtifact();

        StartCoroutine(CompleteSetting());
    }

    public void Update()
    {
        ScoreTextSet();
        TurnTextSet();

        //if(!isMyTurn)
        if (CardManager.instance.myHand.Count == 0 && CardManager.instance.opponentHand.Count == 0 && !AttackPanel.activeSelf && !ArtifactPanel.activeSelf && !isAttack)
        {
            EndPhaseCalc.instance.DamageCalculation();

            //Invoke("Retry", 1f);
        }

        if (PlayerPrefs.GetInt(BattleSystem.instance.player.name + "Game_Hp") <= 0 || PlayerPrefs.GetInt(BattleSystem.instance.op.name + "Game_Hp") <= 0)
        {
            //print("���� ����");
            if (PlayerPrefs.GetInt(BattleSystem.instance.player.name + "Game_Hp") > 0)
            {
                PlayerPrefs.SetInt("HP", PlayerPrefs.GetInt("HP") + PlayerPrefs.GetInt(BattleSystem.instance.player.name + "Game_Hp"));
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
                    ////print("�ι�°");
                    CardManager.instance.ResetPosition(CardManager.instance.myHand);
                    CardManager.instance.DrawCard(CardManager.instance.myHand, 1);
                    oneTime = false;
                }

                else
                {
                    ////print("�ι�°");
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
        yield return new WaitForSeconds(1f);
        AttackPanel.SetActive(false);

        for (int i = 0; i < 3; i++)
        {
            if (!isking[i] && king == i + 3)
            {
                isAttack = true;
                //print("�� ����");
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
                //print("�÷��� ����");
                BattleSystem.instance.damage = 3;
                StartCoroutine(AttackAction(who, ui, hud));
                isflag[i] = true;
            }
        }

        if (!isanimal[0] && animal == 3)
        {
            isAttack = true;
            //print("���� ����");
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

        if(AllArtifact.Count > 0) // ��Ƽ��Ʈ�� ���� ��
        {
            if (isPlayer)
            {
                ArtifactPanel.transform.GetChild(1).GetComponent<Text>().text = "My Artifact";
                print("����");
                if (MatgoScore.myScore >= 3)
                {
                    if (isMyArtifact[0])
                    {
                        MyOpenNum++;
                        ArtifactPanel.SetActive(true);
                    }
                    isMyArtifact[0] = false;
                }

                if (MatgoScore.myScore >= 6)
                {
                    if (isMyArtifact[1])
                    {
                        MyOpenNum++;
                        ArtifactPanel.SetActive(true);
                    }
                    isMyArtifact[1] = false;
                }

                if (MatgoScore.myScore >= 7)
                {
                    if (isMyArtifact[2])
                    {
                        MyOpenNum++;
                        ArtifactPanel.SetActive(true);
                    }

                    else
                    {
                        //���� ���� �������� �� Ŀ���ٸ�
                        MyOpenNum++;
                        ArtifactPanel.SetActive(true);
                    }
                    isMyArtifact[2] = false;
                }
            }

            else
            {
                ArtifactPanel.transform.GetChild(1).GetComponent<Text>().text = "Op Artifact";
                if (MatgoScore.opScore >= 3)
                {
                    if (isOpArtifact[0])
                    {
                        OpOpenNum++;
                        ArtifactPanel.SetActive(true);
                    }
                    isOpArtifact[0] = false;
                }
                if (MatgoScore.opScore >= 6)
                {
                    if (isOpArtifact[1])
                    {
                        OpOpenNum++;
                        ArtifactPanel.SetActive(true);
                    }
                    isOpArtifact[1] = false;
                }
                if (MatgoScore.opScore >= 7)
                {
                    if (isOpArtifact[2])
                    {
                        OpOpenNum++;
                        ArtifactPanel.SetActive(true);
                    }

                    else
                    {
                        OpOpenNum++;
                        ArtifactPanel.SetActive(true);
                    }
                    isOpArtifact[2] = false;
                }
            }
        }
       
    }
    public void ApplyMyArtifact(int num)
    {

        GameObject btn = EventSystem.current.currentSelectedGameObject;

        for(int i=0;i<AllArtifact.Count;i++)
        {
            print("��Ƽ��Ʈ �� : " + AllArtifact[i]);
        }
        AllArtifact.RemoveAt(num);

        for (int i = 0; i < AllArtifact.Count; i++)
        {
            print("���� �� : " + AllArtifact[i]);
        }
        //CurArtifactList[num].IsHaving = false; // ������ ����
        //artifactMe[artifactNumMe].sprite = artifact[num].GetComponent<Image>().sprite;
        //ArtifactPanel.transform.GetChild(0).GetChild(num)
        //AllArtifact.RemoveAt(num);

        //print(AllArtifact.Count);

        if (isMyTurn)
        {
            print(btn.GetComponent<Image>().sprite.name);
            MyOpenNum--;

            artifactMe[artifactNumMe].sprite = btn.GetComponent<Image>().sprite;
            artifactMe[artifactNumMe].gameObject.SetActive(true);
            artifactNumMe++;

            if (MyOpenNum == 0 || AllArtifact.Count == 0)
            {
                ArtifactPanel.SetActive(false);
            }
        }

        if (!isMyTurn)
        {
            print(btn.GetComponent<Image>().sprite.name);
            OpOpenNum--;
            artifactOp[artifactNumOpponent].sprite = btn.GetComponent<Image>().sprite;
            artifactOp[artifactNumOpponent].gameObject.SetActive(true);
            artifactNumOpponent++;
            if (OpOpenNum == 0 || AllArtifact.Count == 0)
            {
                ArtifactPanel.SetActive(false);
            }
        }

        if (!ArtifactPanel.activeSelf)
        {
            AttackPanel.SetActive(true);

            if(isMyTurn)
            {
                StartCoroutine(FixedMade(CardManager.instance.kingEmptyIndex, CardManager.instance.enemyKingEmptyIndex, CardManager.instance.redFlagEmptyIndex, CardManager.instance.blueFlagEmptyIndex, CardManager.instance.normalFlagEmptyIndex,
                CardManager.instance.animalEmptyIndex, BattleSystem.instance.kingAttack, BattleSystem.instance.flagAttack, BattleSystem.instance.animalAttack, BattleSystem.instance.op, BattleSystem.instance.opponentInfo, BattleSystem.instance.opHUD));
                CardClick.instance.EndArrange(CardManager.instance.myHand, false);
            }
            else
            {
                StartCoroutine(FixedMade(CardManager.instance.enemyKingEmptyIndex, CardManager.instance.kingEmptyIndex, CardManager.instance.enemyRedFlagEmptyIndex, CardManager.instance.enemyBlueFlagEmptyIndex, CardManager.instance.enemyNormalFlagEmptyIndex,
                CardManager.instance.enemyAnimalEmptyIndex, BattleSystem.instance.enemyKingAttack, BattleSystem.instance.enemyFlagAttack, BattleSystem.instance.enemyAnimalAttack, BattleSystem.instance.player, BattleSystem.instance.playerInfo, BattleSystem.instance.playerHUD));
                CardClick.instance.EndArrange(CardManager.instance.opponentHand, true);
            }
        }

        print(AllArtifact.Count);
        for (int i = 0; i < 6; i++)
        {
            ArtifactPanel.transform.GetChild(0).GetChild(i).gameObject.SetActive(i < AllArtifact.Count);
        }

        for (int i = 0; i < AllArtifact.Count; i++)
        {
            ArtifactPanel.transform.GetChild(0).GetChild(i).GetComponent<Image>().sprite = ShopManager.instance.AllShopSprite[AllArtifact[i]];

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
        for(int i=0;i<3;i++)
        {
            ArtifactPanel.transform.GetChild(0).GetChild(i).GetComponent<Image>().sprite = ShopManager.instance.AllShopSprite[PlayerPrefs.GetInt("Equip" + i)];
            AllArtifact.Add(PlayerPrefs.GetInt("Equip" + i));
        }

        //�ϴ� ��� ����
        for(int i=3;i<6;i++)
        {
            ArtifactPanel.transform.GetChild(0).GetChild(i).GetComponent<Image>().sprite = ShopManager.instance.AllShopSprite[i];
            AllArtifact.Add(i);
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