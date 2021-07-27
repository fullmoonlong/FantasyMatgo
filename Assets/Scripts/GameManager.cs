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

    public Text turnText; // ������ ������ ǥ���ϴ� �ؽ�Ʈ
    public Text myScoreText;
    public Text opScoreText;
    public Text gameOverText;
    public GameObject gameOverPanel;
    public int turnCount; // ������� ����� �� ��
    public bool isMyTurn; // ���� �����ϴ� bool ����

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
            gameOverText.text = "1P(6��) �¸�!";
            GameOver();
        }
        else if (CardClick.instance.opponentScore >= 7)
        {
            gameOverText.text = "2P(12��) �¸�!";
            GameOver();
        }
        else
        {
            gameOverPanel.SetActive(false);
        }
    }

    public void ScoreTextSet()
    {
        myScoreText.text = "1P (6��) ���� : " + CardClick.instance.myScore.ToString();
        opScoreText.text = "2P(12��) ���� : " + CardClick.instance.opponentScore.ToString();
    }

    public void TurnTextSet()
    {
        if(isMyTurn == true)
        {
            turnText.text = "1P �� (6��)";
        }
        else
        {
            turnText.text = "2P �� (12��)";
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