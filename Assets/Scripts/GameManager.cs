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

    public Text turnText; // ������ ������ ǥ���ϴ� �ؽ�Ʈ
    public int turnCount; // ������� ����� �� ��
    public bool isMyTurn; // ���� �����ϴ� bool ����
    public bool initialTurn = false;
    private float drawShow;

    private void Start()
    {
        isMyTurn = true;
        Time.timeScale = 0.0f;
    }

    private void Update()
    {
        if (isMyTurn == true)
        {
            drawShow += Time.deltaTime;
            if (drawShow > 3f)
            {
                if (initialTurn == true)
                {

                }
            }
        }
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

    public void GameStart()
    {
        Time.timeScale = 1.0f;
        GameObject.Find("GameStart").SetActive(false);
    }
}