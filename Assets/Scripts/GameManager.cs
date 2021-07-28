using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    public GameObject artifactPanelMe;
    public GameObject artifactPanelOp;
    public int turnCount; // 현재까지 진행된 턴 수
    public bool isMyTurn; // 턴을 판정하는 bool 변수
    public bool isGameEnd = false;

    public void Start()
    {
        gameOverPanel.SetActive(false);
        artifactPanelMe.SetActive(false);
        artifactPanelOp.SetActive(false);
        isMyTurn = true;
    }

    public void Update()
    {
        ScoreTextSet();
        TurnTextSet();
        ScoreCheck();
    }

    private void ScoreCheck()
    {
        if (MatgoScore.myScore >= 3)
        {
            ChooseMyArtifact();
        }
        if (MatgoScore.opScore >= 3)
        {
            ChooseOpponentArtifact();
        }
        if (MatgoScore.myScore >= 6)
        {
            ChooseMyArtifact();
        }
        if (MatgoScore.opScore >= 6)
        {
            ChooseOpponentArtifact();
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
        artifactPanelMe.SetActive(true);
    }
    public void ChooseOpponentArtifact()
    {
        artifactPanelOp.SetActive(true);
    }

    public void GameOver()
    {
        if (gameOverPanel.activeInHierarchy == false)
        {
            gameOverPanel.SetActive(true);
            isGameEnd = true;
        }
    }

    public void Retry()
    {
        SceneManager.LoadScene("Game");
    }
}