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
    }

    private void Update()
    {
        if (isMyTurn == true)
        {
            drawShow += Time.deltaTime;
            if (drawShow > 3f)
            {
                if(initialTurn == true)
                {

                }
            }
        }
    }
}
