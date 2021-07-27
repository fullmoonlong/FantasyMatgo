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
    public int turnCount; // 현재까지 진행된 턴 수
    public bool isMyTurn; // 턴을 판정하는 bool 변수

    public void Start()
    {
        isMyTurn = true;
    }

    public void Update()
    {
        ScoreTextSet();
        TurnTextSet();
        WinDecision();
    }

    private void WinDecision()
    {
        if (CardClick.instance.myScore >= 7)
        {
            gameOverText.text = "1P(6시) 승리!";
            GameOver();
        }
        else if (CardClick.instance.opponentScore >= 7)
        {
            gameOverText.text = "2P(12시) 승리!";
            GameOver();
        }
        else
        {
            gameOverPanel.SetActive(false);
        }
    }

    public void ScoreTextSet()
    {
        myScoreText.text = "1P (6시) 점수 : " + CardClick.instance.myScore.ToString();
        opScoreText.text = "2P(12시) 점수 : " + CardClick.instance.opponentScore.ToString();
    }

    public void TurnTextSet()
    {
        if(isMyTurn == true)
        {
            turnText.text = "1P 턴 (6시)";
        }
        else
        {
            turnText.text = "2P 턴 (12시)";
        }
    }

    public void GameOver()
    {
        if (gameOverPanel.activeInHierarchy == false)
        {
            gameOverPanel.SetActive(true);
        }
    }

    public void Retry()
    {
        SceneManager.LoadScene("Game");
    }
}