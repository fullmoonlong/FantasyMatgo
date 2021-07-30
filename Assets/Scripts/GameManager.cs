using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections;

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
    public GameObject myArtifactPanel;
    public GameObject oppoArtifactPanel;
    public int MaxturnCount; // 현재까지 진행된 턴 수
    public int artifactNumMe;
    public int artifactNumOpponent;
    public bool isMyTurn; // 턴을 판정하는 bool 변수
    public bool isGameEnd = false;
    public bool oneTime;
    public bool first;
    public bool isMyFirstArtifact;
    public bool isMySecondArtifact;
    public bool isMyThirdArtifact;
    public bool isOppoFirstArtifact;
    public bool isOppoSecondArtifact;
    public bool isOppoThirdArtifact;
    public bool isSetting;
    public bool isMoving;

    public Image[] artifactMe;
    public Image[] artifactOp;

    public void Start()
    {
        isMyTurn = true;
        oneTime = false;
        first = true;

        artifactNumMe = 0;
        artifactNumOpponent = 0;

        isMyFirstArtifact = true;
        isMySecondArtifact = true;

        isOppoFirstArtifact = true;
        isOppoSecondArtifact = true;

        isSetting = false;
        isMoving = false;

        MaxturnCount = 7;
        StartCoroutine(CompleteSetting());
    }

    public void Update()
    {
        ScoreTextSet();
        TurnTextSet();
        ScoreCheck();

        if (CardManager.instance.myHand.Count == 0 && CardManager.instance.opponentHand.Count == 0)
        {
            GameOver();
        }

        if (oneTime)
        {
            if(MaxturnCount > 0)
            {
                if (isMyTurn)
                {
                    print("두번째");
                    CardManager.instance.ResetPosition(CardManager.instance.myHand);

                    CardManager.instance.DrawCard(CardManager.instance.myHand, 1);
                    oneTime = false;
                }

                else
                {
                    print("두번째");
                    CardManager.instance.ResetPosition(CardManager.instance.opponentHand);
                    CardManager.instance.DrawCard(CardManager.instance.opponentHand, 1);
                    oneTime = false;
                }

                MaxturnCount -= 1;
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
    public void ScoreCheck()
    {
        if (MatgoScore.myScore >= 3)
        {
            if (isMyFirstArtifact == true)
            {
                ChooseMyArtifact();
            }
            isMyFirstArtifact = false;
        }
        if (MatgoScore.opScore >= 3)
        {
            if (isOppoFirstArtifact == true)
            {
                ChooseOpponentArtifact();
            }
            isOppoFirstArtifact = false;
        }
        if (MatgoScore.myScore >= 6)
        {
            if (isMySecondArtifact == true)
            {
                ChooseMyArtifact();
            }
            isMySecondArtifact = false;
        }
        if (MatgoScore.opScore >= 6)
        {
            if (isOppoSecondArtifact == true)
            {
                ChooseOpponentArtifact();
            }
            isOppoSecondArtifact = false;
        }
        if (MatgoScore.myScore >= 7)
        {
            ChooseMyArtifact();
        }
        if (MatgoScore.opScore >= 7)
        {
            ChooseOpponentArtifact();
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
        if (artifactNumMe < 3)
        {
            myArtifactPanel.SetActive(true);
        }
    }
    public void ChooseOpponentArtifact()
    {
        if (artifactNumOpponent < 3)
        {
            oppoArtifactPanel.SetActive(true);
        }
    }

    public void ApplyMyArtifact()
    {
        GameObject btn = EventSystem.current.currentSelectedGameObject;
        artifactMe[artifactNumMe].sprite = btn.GetComponent<Image>().sprite;
        artifactMe[artifactNumMe].gameObject.SetActive(true);
        myArtifactPanel.SetActive(false);
        artifactNumMe++;
    }

    public void ApplyOpponentArtifact()
    {
        GameObject btn = EventSystem.current.currentSelectedGameObject;
        artifactOp[artifactNumOpponent].sprite = btn.GetComponent<Image>().sprite;
        artifactOp[artifactNumOpponent].gameObject.SetActive(true);
        oppoArtifactPanel.SetActive(false);
        artifactNumOpponent++;
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
}