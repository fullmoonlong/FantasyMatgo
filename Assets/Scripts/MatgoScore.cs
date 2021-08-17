using UnityEngine;

public class MatgoScore : MonoBehaviour
{
    #region Singleton
    public static MatgoScore instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion

    public static int myScore = 0;
    public static int opScore = 0;

    public int myGwangScore = 0;
    public int myRedFlagScore = 0;
    public int myBlueFlagScore = 0;
    public int myNormalFlagScore = 0;
    public int myAnimalScore = 0;
    public int myPeeScore = 0;

    public int opponentGwangScore = 0;
    public int opponentRedFlagScore = 0;
    public int opponentBlueFlagScore = 0;
    public int opponentNormalFlagScore = 0;
    public int opponentAnimalScore = 0;
    public int opponentPeeScore = 0;

    private void Start()
    {
        myScore = 0;
        opScore = 0;
    }

    private void Update()
    {
        MyCardCountToScore();
        OpCardCountToScore();
        ScoreCalculate();

        if (GameManager.instance.isGameEnd)
        {
            return;
        }
    }

    public void ScoreCalculate()
    {
        #region SCORESUM
        myScore = myGwangScore +
            myRedFlagScore +
            myBlueFlagScore +
            myNormalFlagScore +
            myAnimalScore +
            myPeeScore;
        opScore = opponentGwangScore +
            opponentRedFlagScore +
            opponentBlueFlagScore +
            opponentNormalFlagScore +
            opponentAnimalScore +
            opponentPeeScore;
        #endregion
    }

    public void MyCardCountToScore()
    {
        #region MyScore
        if (CardManager.instance.kingEmptyIndex == 3 && CardManager.instance.isGwang3)
        {
            CardManager.instance.isGwang3 = false;
            myGwangScore = 3;
            //Debug.Log("광3개");
        }
        if (CardManager.instance.kingEmptyIndex == 4 && CardManager.instance.isGwang4)
        {
            CardManager.instance.isGwang4 = false;
            myGwangScore = 4;
            //Debug.Log("광4개");
        }
        if (CardManager.instance.kingEmptyIndex == 5 && CardManager.instance.isGwang5)
        {
            CardManager.instance.isGwang5 = false;
            myGwangScore = 15;
            //Debug.Log("광5개");
        }
        if (CardManager.instance.redFlagEmptyIndex == 3 && CardManager.instance.isRedFlag)
        {
            CardManager.instance.isRedFlag = false;
            myRedFlagScore = 3;
            ////print("홍단");
        }
        if (CardManager.instance.blueFlagEmptyIndex == 3 && CardManager.instance.isBlueFlag)
        {
            CardManager.instance.isBlueFlag = false;
            myBlueFlagScore = 3;
            ////print("청단");
        }
        if (CardManager.instance.normalFlagEmptyIndex == 3 && CardManager.instance.isNormalFlag)
        {
            CardManager.instance.isNormalFlag = false;
            myNormalFlagScore = 3;
            //Debug.Log("초단");
        }
        if (CardManager.instance.soldierEmptyIndex >= 10)
        {
            myPeeScore = (CardManager.instance.soldierEmptyIndex - 9);
            BattleSystem.instance.SoldierAttack(CardManager.instance.soldierEmptyIndex);
            //Debug.Log("PEE over 10 from now +1 every pee");
        }
        if (CardManager.instance.animalEmptyIndex == 3 && CardManager.instance.isAnimal)
        {
            CardManager.instance.isAnimal = false;
            myAnimalScore = 5;
            //Debug.Log("고도리");
        }
        #endregion
    }

    public void OpCardCountToScore()
    {
        #region OpponentScore
        if (CardManager.instance.enemyKingEmptyIndex == 3 && CardManager.instance.isOpGwang3)
        {
            CardManager.instance.isOpGwang3 = false;
            opponentGwangScore = 3;
            //Debug.Log("광3개");
        }
        if (CardManager.instance.enemyKingEmptyIndex == 4 && CardManager.instance.isOpGwang4)
        {
            CardManager.instance.isOpGwang4 = false;
            opponentGwangScore = 4;
            //Debug.Log("광4개");
        }
        if (CardManager.instance.enemyKingEmptyIndex == 5 && CardManager.instance.isOpGwang5)
        {
            CardManager.instance.isOpGwang5 = false;
            opponentGwangScore = 15;
            //Debug.Log("광5개");
        }
        if (CardManager.instance.enemyRedFlagEmptyIndex == 3 && CardManager.instance.isOpRedFlag)
        {
            CardManager.instance.isOpRedFlag = false;
            opponentRedFlagScore = 3;
            ////print("홍단");
        }
        if (CardManager.instance.enemyBlueFlagEmptyIndex == 3 && CardManager.instance.isOpBlueFlag)
        {
            CardManager.instance.isOpBlueFlag = false;
            opponentBlueFlagScore = 3;
            ////print("청단");
        }
        if (CardManager.instance.enemyNormalFlagEmptyIndex == 3 && CardManager.instance.isOpNormalFlag)
        {
            CardManager.instance.isOpNormalFlag = false;
            opponentNormalFlagScore = 3;
            //Debug.Log("초단");
        }
        if (CardManager.instance.enemySoldierEmptyIndex >= 10)
        {
            opponentPeeScore = (CardManager.instance.enemySoldierEmptyIndex - 9);
            BattleSystem.instance.SoldierAttack(CardManager.instance.enemySoldierEmptyIndex);
            //Debug.Log("PEE over 10 from now +1 every pee");
        }
        if (CardManager.instance.enemyAnimalEmptyIndex == 3 && CardManager.instance.isOpAnimal)
        {
            CardManager.instance.isOpAnimal = false;
            opponentAnimalScore = 5;
            //Debug.Log("고도리");
        }
        #endregion
    }

}
