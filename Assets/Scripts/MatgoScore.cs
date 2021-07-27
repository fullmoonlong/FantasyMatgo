using UnityEngine;

public class MatgoScore : MonoBehaviour
{
    private void Update()
    {
        MyCardCountToScore();
        OpCardCountToScore();
    }
    public void MyCardCountToScore()
    {
        #region MyScore
        if (CardManager.instance.gwangCount == 3 && CardManager.instance.isGwang3)
        {
            CardManager.instance.isGwang3 = false;
            CardClick.instance.myScore += 3;
            Debug.Log("±¤3°³");
        }
        if (CardManager.instance.gwangCount == 4 && CardManager.instance.isGwang4)
        {
            CardManager.instance.isGwang4 = false;
            CardClick.instance.myScore += 1;
            Debug.Log("±¤4°³");
        }
        if (CardManager .instance.gwangCount == 5 && CardManager.instance.isGwang5)
        {
            CardManager.instance.isGwang5 = false;
            CardClick.instance.myScore += 11;
            Debug.Log("±¤5°³");
        }
        if (CardManager.instance.redFlagCount == 3 && CardManager.instance.isRedFlag)
        {
            CardManager.instance.isRedFlag = false;
            CardClick.instance.myScore += 3;
            print("È«´Ü");
        }
        if (CardManager.instance.blueFlagCount == 3 && CardManager.instance.isBlueFlag)
        {
            CardManager.instance.isBlueFlag = false;
            CardClick.instance.myScore += 3;
            print("Ã»´Ü");
        }
        if (CardManager.instance.normalFlagCount == 3 && CardManager.instance.isNormalFlag)
        {
            CardManager.instance.isNormalFlag = false;
            CardClick.instance.myScore += 3;
            Debug.Log("ÃÊ´Ü");
        }
        if (CardManager.instance.peeCount >= 10 & CardManager.instance.isPee)
        {
            CardManager.instance.isPee = false;
            CardClick.instance.myScore += 1;
            Debug.Log("PEE over 10 from now +1 every pee");
        }
        if (CardManager.instance.animalCount == 3 && CardManager.instance.isAnimal)
        {
            CardManager.instance.isAnimal = false;
            CardClick.instance.myScore += 5;
            Debug.Log("°íµµ¸®");
        }
        #endregion
    }

    public void OpCardCountToScore()
    {
        #region OpponentScore
        if (CardManager.instance.enemyGwangCount == 3 && CardManager.instance.isOpGwang3)
        {
            CardManager.instance.isOpGwang3 = false;
            CardClick.instance.opponentScore += 3;
            Debug.Log("±¤3°³");
        }
        if (CardManager.instance.enemyGwangCount == 4 && CardManager.instance.isOpGwang4)
        {
            CardManager.instance.isOpGwang4 = false;
            CardClick.instance.opponentScore += 1;
            Debug.Log("±¤4°³");
        }
        if (CardManager.instance.enemyGwangCount == 5 && CardManager.instance.isOpGwang5)
        {
            CardManager.instance.isOpGwang5 = false;
            CardClick.instance.opponentScore += 11;
            Debug.Log("±¤5°³");
        }
        if (CardManager.instance.enemyRedFlagCount == 3 && CardManager.instance.isOpRedFlag)
        {
            CardManager.instance.isOpRedFlag = false;
            CardClick.instance.opponentScore += 3;
            print("È«´Ü");
        }
        if (CardManager.instance.enemyBlueFlagCount == 3 && CardManager.instance.isOpBlueFlag)
        {
            CardManager.instance.isOpBlueFlag = false;
            CardClick.instance.opponentScore += 3;
            print("Ã»´Ü");
        }
        if (CardManager.instance.enemyNormalFlagCount == 3 && CardManager.instance.isOpNormalFlag)
        {
            CardManager.instance.isOpNormalFlag = false;
            CardClick.instance.opponentScore += 3;
            Debug.Log("ÃÊ´Ü");
        }
        if (CardManager.instance.enemyPeeCount >= 10)
        {
            CardManager.instance.isOpPee = false;
            CardClick.instance.opponentScore += 1;
            Debug.Log("PEE over 10 from now +1 every pee");
        }
        if (CardManager.instance.enemyAnimalCount == 3 && CardManager.instance.isOpAnimal)
        {
            CardManager.instance.isOpAnimal = false;
            CardClick.instance.opponentScore += 5;
            Debug.Log("°íµµ¸®");
        }
        #endregion
    }

}
