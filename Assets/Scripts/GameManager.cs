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
    public GameObject artifactPanelMe;
    public GameObject artifactPanelOp;
    public int turnCount; // ������� ����� �� ��
    public bool isMyTurn; // ���� �����ϴ� bool ����
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