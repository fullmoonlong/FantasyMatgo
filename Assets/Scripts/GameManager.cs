using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections;
using DG.Tweening;
using System.Collections.Generic;
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
    public GameObject ArtifactPanel;
    public GameObject AttackPanel;

    public int maxTurnCount; // 현재까지 진행된 턴 수
    public int artifactNumMe;
    public int artifactNumOpponent;
    public bool isMyTurn; // 턴을 판정하는 bool 변수
    public bool isGameEnd = false;
    public bool oneTime;
    public bool first;

    public bool[] isMyArtifact;
    public bool[] isOpArtifact;

    public bool isSetting;
    public bool isMoving;
    public bool isBattle;
    public bool isBonus;
    public bool isShake;
    public bool isChoice;
    public bool isAttack;

    public Image[] artifactMe;
    public Image[] artifactOp;

    int MyOpenNum;
    int OpOpenNum;

    List<int> AllArtifact;
    List<string> myArtifactList;
    List<string> opponentArtifactList;

    public void Start()
    {
        isMyTurn = true;
        oneTime = false;
        first = true;
        isBattle = false;

        artifactNumMe = 0;
        artifactNumOpponent = 0;

        isMyArtifact = new bool[3];
        isOpArtifact = new bool[3];
        for (int i = 0; i < isMyArtifact.Length; i++)
        {
            isMyArtifact[i] = true;
            isOpArtifact[i] = true;
        }

        isSetting = false;
        isMoving = false;

        maxTurnCount = 7;

        isBonus = false;

        isShake = false;

        isChoice = false;

        isAttack = false;

        MyOpenNum = 0;
        OpOpenNum = 0;


        AllArtifact = new List<int>();
        myArtifactList = new List<string>();
        opponentArtifactList = new List<string>();

        ArrangeArtifact();

        StartCoroutine(CompleteSetting());
    }

    public void Update()
    {
        ScoreTextSet();
        TurnTextSet();

        if (CardManager.instance.myHand.Count == 0 && CardManager.instance.opponentHand.Count == 0 && !AttackPanel.activeSelf && !ArtifactPanel.activeSelf && !isAttack)
        {
            MyArtifactDamage();
            OpponentArtifactDamage();
            StartCoroutine(EndPhaseCalc.instance.MyDamageCalculation());
        }

        if (PlayerPrefs.GetInt(BattleSystem.instance.player.name + "Game_Hp") <= 0 || PlayerPrefs.GetInt(BattleSystem.instance.op.name + "Game_Hp") <= 0)
        {
            if (PlayerPrefs.GetInt(BattleSystem.instance.player.name + "Game_Hp") > 0)
            {
                PlayerPrefs.SetInt("HP", PlayerPrefs.GetInt("HP") + PlayerPrefs.GetInt(BattleSystem.instance.player.name + "Game_Hp"));
            }
            PlayerPrefs.DeleteKey(BattleSystem.instance.player.name + "Game_Hp");
            PlayerPrefs.DeleteKey(BattleSystem.instance.op.name + "Game_Hp");

            Invoke("GotoMain", 1f);
        }


        if (oneTime)
        {
            if (maxTurnCount > 0)
            {
                if (isMyTurn)
                {
                    //print("두번째");
                    CardManager.instance.CardInitialPosition(CardManager.instance.myHand);
                    CardManager.instance.DrawCard(CardManager.instance.myHand, 1);
                    oneTime = false;
                }

                else
                {
                    //print("두번째");
                    CardManager.instance.CardInitialPosition(CardManager.instance.opponentHand);
                    CardManager.instance.DrawCard(CardManager.instance.opponentHand, 1);
                    oneTime = false;
                }

                maxTurnCount -= 1;
            }
        }
    }

    public IEnumerator CompleteSetting()
    {
        yield return new WaitForSeconds(1f);
        isSetting = true;
    }

    public IEnumerator CompleteMoving()
    {
        yield return new WaitForSeconds(1f);
        isMoving = false;
    }

    public IEnumerator FixedMade(int king, int opking, int red, int blue, int normal, int animal,int thing, bool[] isking, bool[] isflag, bool[] isanimal, GameObject who, PlayerScript ui, BattleHUD hud)
    {
        yield return new WaitForSeconds(1f);
        AttackPanel.SetActive(false);

        for (int i = 0; i < 3; i++)
        {
            if (!isking[i] && king == i + 3)
            {
                isAttack = true;
                //print("광 공격");
                BattleSystem.instance.LightAttack(king, opking);
                isking[i] = true;
            }

            int flag = 0;
            switch (i)
            {
                case 0:
                    flag = red;
                    break;
                case 1:
                    flag = blue;
                    break;
                case 2:
                    flag = normal;
                    break;
                default:
                    break;
            }

            if (!isflag[i] && flag == 3)
            {
                isAttack = true;
                //print("플래그 공격");
                BattleSystem.instance.FlagAttack(red, blue, normal);
                isflag[i] = true;
            }
        }

        int result = red + blue + normal;
        for (int i = 0; i < 5; i++)
        {
            if (!isflag[i + 3] && result == i + 6)
            {
                isAttack = true;
                BattleSystem.instance.ResultFlag(result);
                isflag[i + 3] = true;
            }
        }

        if (!isanimal[0] && animal == 3)
        {
            isAttack = true;
            //print("동물 공격");
            BattleSystem.instance.GoDoRiAttack();
            isanimal[0] = true;
        }

        result = animal + thing;

        for(int i=0;i<5;i++)
        {
            if (!isanimal[i + 1] && result == i + 5) 
            {
                isAttack = true;
                BattleSystem.instance.ResultAnimal(result);
                isanimal[i + 1] = true;
            }
        }

        if (isAttack)
        {
            AttackMotion(who, ui, hud);
        }
    }
    public void AttackMotion(GameObject who, PlayerScript ui, BattleHUD hud)
    {
        AttackPanel.SetActive(true);
        Sequence attacksequence = DOTween.Sequence();
        print(BattleSystem.instance.attackMotionImage.Count);
        for(int i=0;i<BattleSystem.instance.attackMotionImage.Count;i++)
        {
            attacksequence.Append(BattleSystem.instance.attackMotionImage[i].transform.DOMove(who.transform.position, 1f))
                .OnComplete(() => StartCoroutine(AttackAction(who, ui, hud)));
        }
    }
    public IEnumerator AttackAction(GameObject who, PlayerScript ui, BattleHUD hud)
    {
        //AttackPanel.SetActive(true);

        GameObject[] fireObj = GameObject.FindGameObjectsWithTag("fire");

        for (int i = 0; i < fireObj.Length; i++)
        {
            fireObj[i].SetActive(false);
        }

        Sequence mysequence = DOTween.Sequence();
        BattleSystem.instance.attackImage.transform.position = who.transform.position;
      
        mysequence.Append(BattleSystem.instance.attackImage.transform.DOScale(Vector3.one * 0.3f, 0.3f)
            .SetEase(Ease.InOutBack)).Join(who.transform.DOShakePosition(1f, 5f))
          .AppendInterval(1.2f).Append(BattleSystem.instance.attackImage.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InOutBack)).OnComplete(() => BattleSystem.instance.Damaged(ui, hud));

        yield return new WaitForSeconds(4f);
        
        for (int i = 0; i < BattleSystem.instance.attackMotionImage.Count; i++)
        {
            BattleSystem.instance.attackMotionImage.RemoveAt(i);
        }

        BattleSystem.instance.damage = 0;

        AttackPanel.SetActive(false);
        isAttack = false;
    }

    public void ScoreCheck(bool isPlayer)
    {
        MatgoScore.instance.MyCardCountToScore();
        MatgoScore.instance.OpCardCountToScore();
        MatgoScore.instance.ScoreCalculate();

        if (AllArtifact.Count > 0) // 아티팩트가 있을 때
        {
            if (isPlayer)
            {
                ArtifactPanel.transform.GetChild(1).GetComponent<Text>().text = "My Artifact";
                print("내턴");
                if (MatgoScore.myScore >= 3)
                {
                    if (isMyArtifact[0])
                    {
                        MyOpenNum++;
                        ArtifactPanel.SetActive(true);
                    }
                    isMyArtifact[0] = false;
                }

                if (MatgoScore.myScore >= 6)
                {
                    if (isMyArtifact[1])
                    {
                        MyOpenNum++;
                        ArtifactPanel.SetActive(true);
                    }
                    isMyArtifact[1] = false;
                }

                if (MatgoScore.myScore >= 7)
                {
                    if (isMyArtifact[2])
                    {
                        MyOpenNum++;
                        ArtifactPanel.SetActive(true);
                    }

                    else
                    {
                        //만약 이전 점수보다 더 커졌다면
                        MyOpenNum++;
                        ArtifactPanel.SetActive(true);
                    }
                    isMyArtifact[2] = false;
                }
            }

            else
            {
                ArtifactPanel.transform.GetChild(1).GetComponent<Text>().text = "Op Artifact";
                if (MatgoScore.opScore >= 3)
                {
                    if (isOpArtifact[0])
                    {
                        OpOpenNum++;
                        ArtifactPanel.SetActive(true);
                    }
                    isOpArtifact[0] = false;
                }
                if (MatgoScore.opScore >= 6)
                {
                    if (isOpArtifact[1])
                    {
                        OpOpenNum++;
                        ArtifactPanel.SetActive(true);
                    }
                    isOpArtifact[1] = false;
                }
                if (MatgoScore.opScore >= 7)
                {
                    if (isOpArtifact[2])
                    {
                        OpOpenNum++;
                        ArtifactPanel.SetActive(true);
                    }

                    else
                    {
                        OpOpenNum++;
                        ArtifactPanel.SetActive(true);
                    }
                    isOpArtifact[2] = false;
                }
            }
        }

    }

    public void ApplyMyArtifact(int num)
    {

        GameObject btn = EventSystem.current.currentSelectedGameObject;

        print(AllArtifact.Count);
        for (int i = 0; i < AllArtifact.Count; i++)
        {
            print("아티펙트 넘 : " + AllArtifact[i]);
        }

       
        for (int i = 0; i < AllArtifact.Count; i++)
        {
            print("변경 후 : " + AllArtifact[i]);
        }
        

        if (isMyTurn)
        {
            print(btn.GetComponent<Image>().sprite.name);
            MyOpenNum--;

            myArtifactList.Add(ShopManager.instance.AllShopList[AllArtifact[num]].Name);
            artifactMe[artifactNumMe].sprite = btn.GetComponent<Image>().sprite;
            artifactMe[artifactNumMe].gameObject.SetActive(true);
            artifactNumMe++;

            AllArtifact.RemoveAt(num);

            if (MyOpenNum == 0 || AllArtifact.Count == 0)
            {
                ArtifactPanel.SetActive(false);
            }
        }
        if (!isMyTurn)
        {
            print(btn.GetComponent<Image>().sprite.name);
            OpOpenNum--;

            opponentArtifactList.Add(ShopManager.instance.AllShopList[AllArtifact[num]].Name);
            artifactOp[artifactNumOpponent].sprite = btn.GetComponent<Image>().sprite;
            artifactOp[artifactNumOpponent].gameObject.SetActive(true);
            artifactNumOpponent++;

            AllArtifact.RemoveAt(num);

            if (OpOpenNum == 0 || AllArtifact.Count == 0)
            {
                ArtifactPanel.SetActive(false);
            }
        }
        

        if (!ArtifactPanel.activeSelf)
        {
            AttackPanel.SetActive(true);

            if (isMyTurn)
            {
                StartCoroutine(FixedMade(CardManager.instance.kingEmptyIndex, CardManager.instance.enemyKingEmptyIndex, CardManager.instance.redFlagEmptyIndex, CardManager.instance.blueFlagEmptyIndex, CardManager.instance.normalFlagEmptyIndex,
                CardManager.instance.animalEmptyIndex, CardManager.instance.thingEmptyIndex, BattleSystem.instance.kingAttack, BattleSystem.instance.flagAttack, BattleSystem.instance.animalAttack, BattleSystem.instance.op, BattleSystem.instance.opponentInfo, BattleSystem.instance.opHUD));
                CardClick.instance.EndArrange(CardManager.instance.myHand, false);
            }
            else
            {
                StartCoroutine(FixedMade(CardManager.instance.enemyKingEmptyIndex, CardManager.instance.kingEmptyIndex, CardManager.instance.enemyRedFlagEmptyIndex, CardManager.instance.enemyBlueFlagEmptyIndex, CardManager.instance.enemyNormalFlagEmptyIndex,
                CardManager.instance.enemyAnimalEmptyIndex, CardManager.instance.enemyThingEmptyIndex, BattleSystem.instance.enemyKingAttack, BattleSystem.instance.enemyFlagAttack, BattleSystem.instance.enemyAnimalAttack, BattleSystem.instance.player, BattleSystem.instance.playerInfo, BattleSystem.instance.playerHUD));
                CardClick.instance.EndArrange(CardManager.instance.opponentHand, true);
            }
        }

        print(AllArtifact.Count);
        for (int i = 0; i < 6; i++)
        {
            ArtifactPanel.transform.GetChild(0).GetChild(i).gameObject.SetActive(i < AllArtifact.Count);
        }

        for (int i = 0; i < AllArtifact.Count; i++)
        {
            ArtifactPanel.transform.GetChild(0).GetChild(i).GetComponent<Image>().sprite = ShopManager.instance.AllShopSprite[AllArtifact[i]];

        }

    }

    public void ScoreTextSet()
    {
        myScoreText.text = "1P 점수 : " + MatgoScore.myScore.ToString();
        opScoreText.text = "2P 점수 : " + MatgoScore.opScore.ToString();
    }

    public void TurnTextSet()
    {
        if (isMyTurn == true)
        {
            turnText.text = "1P 턴";
        }
        else
        {
            turnText.text = "2P 턴";
        }
    }

    public void ArrangeArtifact() // 장비 6개 장착
    {
        for (int i = 0; i < 3; i++)
        {
            ArtifactPanel.transform.GetChild(0).GetChild(i).GetComponent<Image>().sprite = ShopManager.instance.AllShopSprite[PlayerPrefs.GetInt("Equip" + i)];
            AllArtifact.Add(PlayerPrefs.GetInt("Equip" + i));
        }

        //일단 상대 예제
        for (int i = 3; i < 6; i++)
        {
            ArtifactPanel.transform.GetChild(0).GetChild(i).GetComponent<Image>().sprite = ShopManager.instance.AllShopSprite[i];
            AllArtifact.Add(i);
        }
    }

    public void MyArtifactDamage()
    {
        for (int i = 0; i < myArtifactList.Count; i++)
        {
            switch (myArtifactList[i])
            {
                case "기본 화염 지팡이":
                    if (CardManager.instance.redFlagEmptyIndex == 3)
                    {
                        BattleSystem.instance.playerTotalDamage += 4;
                    }
                    break;

                case "기본 전기 지팡이":
                    if (CardManager.instance.normalFlagEmptyIndex == 3)
                    {
                        BattleSystem.instance.playerTotalDamage += 4;
                    }
                    break;

                case "기본 얼음 지팡이":
                    if (CardManager.instance.blueFlagEmptyIndex == 3)
                    {
                        BattleSystem.instance.playerTotalDamage += 4;
                    }
                    break;

                case "기본 대검":
                    if (CardManager.instance.kingEmptyIndex == 5 && (CardManager.instance.redFlagEmptyIndex + CardManager.instance.blueFlagEmptyIndex + CardManager.instance.normalFlagEmptyIndex) == 5)
                    {
                        BattleSystem.instance.playerTotalDamage += 5;
                    }
                    break;

                case "기본 장검":
                    if ((CardManager.instance.redFlagEmptyIndex + CardManager.instance.blueFlagEmptyIndex + CardManager.instance.normalFlagEmptyIndex) == 5 && CardManager.instance.animalEmptyIndex == 2)
                    {
                        BattleSystem.instance.playerTotalDamage += 5;
                    }
                    break;

                case "기본 단검":
                    if (CardManager.instance.soldierEmptyIndex >= 10)
                    {
                        BattleSystem.instance.playerTotalDamage += (CardManager.instance.soldierEmptyIndex / 5);
                    }
                    break;

                case "기본 오브":
                    if ((CardManager.instance.redFlagEmptyIndex + CardManager.instance.blueFlagEmptyIndex + CardManager.instance.normalFlagEmptyIndex) == 5 && CardManager.instance.soldierEmptyIndex >= 15)
                    {
                        BattleSystem.instance.playerTotalDamage += 8;
                    }
                    break;

                case "기본 창":
                    if ((CardManager.instance.redFlagEmptyIndex + CardManager.instance.blueFlagEmptyIndex + CardManager.instance.normalFlagEmptyIndex) >= 7 &&
                        (CardManager.instance.redFlagEmptyIndex == 3 || CardManager.instance.blueFlagEmptyIndex == 3 || CardManager.instance.normalFlagEmptyIndex == 3))
                    {
                        BattleSystem.instance.playerTotalDamage += 5;
                    }
                    break;

                default:
                    break;
            }
        }

    }

    public void OpponentArtifactDamage()
    {
        for (int i = 0; i < opponentArtifactList.Count; i++)
        {
            switch (opponentArtifactList[i])
            {
                case "기본 화염 지팡이":
                    if (CardManager.instance.enemyRedFlagEmptyIndex == 3)
                    {
                        BattleSystem.instance.opponentTotalDamage += 4;
                    }
                    break;

                case "기본 전기 지팡이":
                    if (CardManager.instance.enemyNormalFlagEmptyIndex == 3)
                    {
                        BattleSystem.instance.opponentTotalDamage += 4;
                    }
                    break;

                case "기본 얼음 지팡이":
                    if (CardManager.instance.enemyBlueFlagEmptyIndex == 3)
                    {
                        BattleSystem.instance.opponentTotalDamage += 4;
                    }
                    break;

                case "기본 대검":
                    if (CardManager.instance.enemyKingEmptyIndex == 5 && (CardManager.instance.enemyRedFlagEmptyIndex + CardManager.instance.enemyBlueFlagEmptyIndex + CardManager.instance.enemyNormalFlagEmptyIndex) == 5)
                    {
                        BattleSystem.instance.opponentTotalDamage += 5;
                    }
                    break;

                case "기본 장검":
                    if ((CardManager.instance.enemyRedFlagEmptyIndex + CardManager.instance.enemyBlueFlagEmptyIndex + CardManager.instance.enemyNormalFlagEmptyIndex) == 5 && CardManager.instance.enemyAnimalEmptyIndex == 2)
                    {
                        BattleSystem.instance.opponentTotalDamage += 5;
                    }
                    break;

                case "기본 단검":
                    if (CardManager.instance.enemySoldierEmptyIndex >= 10)
                    {
                        BattleSystem.instance.opponentTotalDamage += (CardManager.instance.enemySoldierEmptyIndex / 5);
                    }
                    break;

                case "기본 오브":
                    if ((CardManager.instance.enemyRedFlagEmptyIndex + CardManager.instance.enemyBlueFlagEmptyIndex + CardManager.instance.enemyNormalFlagEmptyIndex) == 5 && CardManager.instance.enemySoldierEmptyIndex >= 15)
                    {
                        BattleSystem.instance.opponentTotalDamage += 8;
                    }
                    break;

                case "기본 창":
                    if ((CardManager.instance.enemyRedFlagEmptyIndex + CardManager.instance.enemyBlueFlagEmptyIndex + CardManager.instance.enemyNormalFlagEmptyIndex) >= 7 &&
                        (CardManager.instance.enemyRedFlagEmptyIndex == 3 || CardManager.instance.enemyBlueFlagEmptyIndex == 3 || CardManager.instance.enemyNormalFlagEmptyIndex == 3))
                    {
                        BattleSystem.instance.opponentTotalDamage += 5;
                    }
                    break;

                default:
                    break;
            }
        }
    }

    public void GameOver()
    {
        if (!gameOverPanel.activeSelf)
        {
            gameOverText.text = "Game Over";
            gameOverPanel.SetActive(true);
            isGameEnd = true;
        }
    }

    public void GotoMain()
    {
        SceneManager.LoadScene("MainScene");
    }

    public IEnumerator Retry()
    {
        yield return new WaitForSeconds(1f);
        EndPhaseCalc.instance.isPhaseOver = false;
        SceneManager.LoadScene("Game");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}