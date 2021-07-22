using UnityEngine;
using UnityEngine.UI;

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
    public int turnCount; // 현재까지 진행된 턴 수
    public bool isMyTurn; // 턴을 판정하는 bool 변수
    public bool initialTurn = false;
    private float drawShow;

    private void Start()
    {
        isMyTurn = true;
    }

    private void Update()
    { 
        WinDecision();
    }

    private void WinDecision()
    {
        if (CardClick.score >= 7)
        {
            if (isMyTurn == true)
            {
                Debug.Log("I Won! (6'o clock)");
            }
            else
            {
                Debug.Log("Opponent Won! (12'o clock)");
            }
        }
    }
}