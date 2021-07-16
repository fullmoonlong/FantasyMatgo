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

    private void Start()
    {
        Invoke("FirstTurn", 5f);
    }

    void FirstTurn()
    {
        turnText.text = "Player Turn";
        turnText.enabled = true;
        isMyTurn = true;
        CardManager.instance.DrawCard(CardManager.instance.myHand, 1);
    }
}
