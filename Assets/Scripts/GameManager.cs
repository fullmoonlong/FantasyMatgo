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
