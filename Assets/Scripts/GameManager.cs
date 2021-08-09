using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion

    public Text turnText; // 누구의 턴인지 표시하는 텍스트
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

    public int maxTurnCount; // 현재까지 진행된 턴 수
    public int artifactNumMe;
    public int artifactNumOpponent;
    public bool isMyTurn; // 턴을 판정하는 bool 변수
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
    public bool isAttack;
    public bool isShake;
    public bool isChoice;

    public Image[] artifactMe;
    public Image[] artifactOp;

    public void Start()
    {
        isMyTurn = true;
        oneTime = false;
        first = true;
        isBattle = false;

        artifactNumMe = 0;
        artifactNumOpponent = 0;

        isMyFirstArtifact = true;
        isMySecondArtifact = true;

        isOppoFirstArtifact = true;
        isOppoSecondArtifact = true;

        isSetting = false;
        isMoving = false;

        maxTurnCount = 7;

        battleFirst = true;

        isBonus = false;

        isAttack = true;

        isShake = false;

        isChoice = false;

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
            && opponentThirdArtifactPanel.activeSelf == false)
        {
            GameOver();
        }

        if (oneTime)
        {
            if (maxTurnCount > 0)
            {
                if (isMyTurn)
                {
                    //print("두번째");
                    CardManager.instance.ResetPosition(CardManager.instance.myHand);
                    CardManager.instance.DrawCard(CardManager.instance.myHand, 1);
                    oneTime = false;
                }

                else
                {
                    //print("두번째");
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

    public IEnumerator FixedMade(int king, int opking, int red, int blue, int normal, int animal, bool[] isking, bool[] isflag, bool isanimal, GameObject who, PlayerScript ui, BattleHUD hud)
    {
        print("attack");
        yield return new WaitForSeconds(1f);
        AttackPanel.SetActive(false);
        print("check");
        for (int i = 0; i < 3; i++)
        {
            if (!isking[i] && king == i + 3)
            {
                print("광 공격");
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
                print("플래그 공격");
                BattleSystem.instance.damage = 3;
                StartCoroutine(AttackAction(who, ui, hud));
                isflag[i] = true;
            }
        }

        if (!isanimal && animal == 3)
        {
            print("동물 공격");
            BattleSystem.instance.damage = 5;
            StartCoroutine(AttackAction(who, ui, hud));
            isanimal = true;
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
    }

    public void OnOffPanel(bool on)
    {
        print("panel false");
        AttackPanel.SetActive(on);
        print("panel false");
    }
    public void ScoreCheck(bool isPlayer)
    {
        MatgoScore.instance.MyCardCountToScore();
        MatgoScore.instance.OpCardCountToScore();
        MatgoScore.instance.ScoreCalculate();
        if (isPlayer)
        {
            print("내턴");
            if (MatgoScore.myScore >= 3)
            {
                if (isMyFirstArtifact == true)
                {
                    ChooseMyArtifact();
                }
                isMyFirstArtifact = false;
            }

            if (MatgoScore.myScore >= 6)
            {
                if (isMySecondArtifact == true)
                {
                    ChooseMyArtifact();
                }
                isMySecondArtifact = false;
            }

            if (MatgoScore.myScore >= 7)
            {
                ChooseMyArtifact();
            }
        }
     
        else
        {
            if (MatgoScore.opScore >= 3)
            {
                if (isOppoFirstArtifact == true)
                {
                    ChooseOpponentArtifact();
                }
                isOppoFirstArtifact = false;
            }

            if (MatgoScore.opScore >= 6)
            {
                if (isOppoSecondArtifact == true)
                {
                    ChooseOpponentArtifact();
                }
                isOppoSecondArtifact = false;
            }

            if (MatgoScore.opScore >= 7)
            {
                ChooseOpponentArtifact();
            }
        }
    }

    public void ScoreTextSet()
    {
        myScoreText.text = "1P (6시) 점수 : " + MatgoScore.myScore.ToString();
        opScoreText.text = "2P(12시) 점수 : " + MatgoScore.opScore.ToString();
    }

    public void TurnTextSet()
    {
        if (isMyTurn == true)
        {
            turnText.text = "1P 턴 (6시)";
        }
        else
        {
            turnText.text = "2P 턴 (12시)";
        }
    }

    public void ChooseMyArtifact()
    {
        if (artifactNumMe == 0)
        {
            myFirstArtifactPanel.SetActive(true);
        }
        else if (artifactNumMe == 1)
        {
            mySecondArtifactPanel.SetActive(true);
        }
        else if (artifactNumMe == 2)
        {
            myThirdArtifactPanel.SetActive(true);
        }
        /// 이 밑으로 아티팩트를 4개 이상 먹는 시스템 구현
        ///

    }
    public void ChooseOpponentArtifact()
    {
        if (artifactNumOpponent == 0)
        {
            opponentFirstArtifactPanel.SetActive(true);
        }
        if (artifactNumOpponent == 1)
        {
            opponentSecondArtifactPanel.SetActive(true);
        }
        if (artifactNumOpponent == 2)
        {
            opponentThirdArtifactPanel.SetActive(true);
        }
        /// 이 밑으로 아티팩트를 4개 이상 먹는 시스템 구현
        /// 
    }

    public void ApplyMyArtifact()
    {
        GameObject btn = EventSystem.current.currentSelectedGameObject;
        artifactMe[artifactNumMe].sprite = btn.GetComponent<Image>().sprite;
        artifactMe[artifactNumMe].gameObject.SetActive(true);

        if (myFirstArtifactPanel.activeInHierarchy == true)
        {
            myFirstArtifactPanel.SetActive(false);
        }
        if (mySecondArtifactPanel.activeInHierarchy == true)
        {
            mySecondArtifactPanel.SetActive(false);
        }
        if (myThirdArtifactPanel.activeInHierarchy == true)
        {
            myThirdArtifactPanel.SetActive(false);
        }
        artifactNumMe++;

        AttackPanel.SetActive(true);

        StartCoroutine(FixedMade(CardManager.instance.kingEmptyIndex, CardManager.instance.enemyKingEmptyIndex, CardManager.instance.redFlagEmptyIndex, CardManager.instance.blueFlagEmptyIndex, CardManager.instance.normalFlagEmptyIndex,
                CardManager.instance.animalEmptyIndex, BattleSystem.instance.kingAttack, BattleSystem.instance.flagAttack, BattleSystem.instance.animalAttack, BattleSystem.instance.op, BattleSystem.instance.opUi, BattleSystem.instance.opHUD));
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
        if (opponentSecondArtifactPanel.activeInHierarchy == true)
        {
            opponentSecondArtifactPanel.SetActive(false);
        }
        if (opponentThirdArtifactPanel.activeInHierarchy == true)
        {
            opponentThirdArtifactPanel.SetActive(false);
        }
        artifactNumOpponent++;

        AttackPanel.SetActive(true);

        StartCoroutine(FixedMade(CardManager.instance.enemyKingEmptyIndex, CardManager.instance.kingEmptyIndex, CardManager.instance.enemyRedFlagEmptyIndex, CardManager.instance.enemyBlueFlagEmptyIndex, CardManager.instance.enemyNormalFlagEmptyIndex,
           CardManager.instance.enemyAnimalEmptyIndex, BattleSystem.instance.enemyKingAttack, BattleSystem.instance.enemyFlagAttack, BattleSystem.instance.enemyAnimalAttack, BattleSystem.instance.player, BattleSystem.instance.playerUi, BattleSystem.instance.playerHUD));


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
    public void Retry()
    {
        SceneManager.LoadScene("Game");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}