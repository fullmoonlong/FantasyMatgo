using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

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
    public GameObject artifactPanel;
    public int turnCount; // 현재까지 진행된 턴 수
    public bool isMyTurn; // 턴을 판정하는 bool 변수
    public bool isGameEnd = false;
    public bool oneTime;
    public bool first;

    private string Who;
    public Image[] ArtifactMe;
    public Image[] ArtifactOp;

    public int ArtifactNumMe;
    public int ArtifactNumOp;

    public void Start()
    {
        isMyTurn = true;
        oneTime = false;
        first = true;

        Who = "Player";
            
        ArtifactNumMe = 0;
        ArtifactNumOp = 0;
    }

    public void Update()
    {
        ScoreTextSet();
        TurnTextSet();

        if (CardManager.instance.myHand.Count == 0 || CardManager.instance.opponentHand.Count == 0)
        {
            GameOver();
        }

        if (oneTime)
        {
            if (isMyTurn)
            {
                CardManager.instance.DrawCard(CardManager.instance.myHand, 1);
                oneTime = false;
            }

            else
            {
                CardManager.instance.DrawCard(CardManager.instance.opponentHand, 1);
                oneTime = false;
            }
        }
    }

    public void ScoreCheck()
    {
        if (MatgoScore.myScore >= 3)
        {
            Who = "Player";
            ChooseArtifact();
        }
        if (MatgoScore.opScore >= 3)
        {
            Who = "Opponent";
            ChooseArtifact();
        }
        if (MatgoScore.myScore >= 6)
        {
            Who = "Player";
            ChooseArtifact();
        }
        if (MatgoScore.opScore >= 6)
        {
            Who = "Opponent";
            ChooseArtifact();
        }
        if (MatgoScore.myScore >= 7)
        {
            Who = "Player";
            ChooseArtifact();
        }
        if (MatgoScore.opScore >= 7)
        {
            Who = "Opponent";
            ChooseArtifact();
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

    public void ChooseArtifact()
    {
        artifactPanel.SetActive(true);
    }

    public void ApplyArtifact()
    {
        artifactPanel.SetActive(false);

        GameObject btn = EventSystem.current.currentSelectedGameObject;
        
        if(ArtifactNumMe <= 3 && ArtifactNumOp <= 3)
        {
            if (Who == "Player")
            {
                
                ArtifactMe[ArtifactNumMe].sprite = btn.GetComponent<Image>().sprite;
                ArtifactMe[ArtifactNumMe].gameObject.SetActive(true);
                ArtifactNumMe++;
            }

            else if (Who == "Opponent")
            {
                ArtifactOp[ArtifactNumOp].sprite = btn.GetComponent<Image>().sprite;
                ArtifactMe[ArtifactNumMe].gameObject.SetActive(true);
                ArtifactNumOp++;
            }
        }
       
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