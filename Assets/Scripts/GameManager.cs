using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion

    [SerializeField] public GameObject[] cardObjectList;

    public bool myTurn;
    public int turnCount;
}
