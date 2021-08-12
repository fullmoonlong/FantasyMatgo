using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missions : MonoBehaviour
{
    #region Singleton
    public static Missions instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion

    private List<string> missions;
    private string currentMission;
    private bool isMissionSelected;
    private bool isGwangMissionDone;
    private bool isDanMissionDone;
    private bool isPeeMissionDone;
    private bool isGodoriMissionDone;

    public int myMissionDoneCount;
    public int opponentMissionDoneCount;

    private void Start()
    {
        currentMission = "";
        isMissionSelected = false;
        isGwangMissionDone = false;
        isDanMissionDone = false;
        isPeeMissionDone = false;
        isGodoriMissionDone = false;
        missions = new List<string>();
        missions.Add("GwangPair");
        missions.Add("FivePee");
        missions.Add("DanPair");
        missions.Add("GodoriPair");
    }

    private void Update()
    {
        MissionDone();
        if (GameManager.instance.maxTurnCount == 5)
        {
            if (isMissionSelected == true)
            {
                return;
            }
            currentMission = missions[Random.Range(0, missions.Count)];
            isMissionSelected = true;
            Debug.Log(currentMission);
        }
    }

    private void MissionDone()
    {
        Debug.Log("WHA");
        switch (currentMission)
        {
            case "GwangPair":
                if (CardManager.instance.kingEmptyIndex >= 2 && isGwangMissionDone == false)
                {
                    myMissionDoneCount++;
                    BattleSystem.instance.damage = 2;
                    Debug.Log("MeDoneGwangMission");
                    isGwangMissionDone = true;
                }
                else if (CardManager.instance.enemyKingEmptyIndex >= 2 && isGwangMissionDone == false)
                {
                    opponentMissionDoneCount++;
                    BattleSystem.instance.damage = 2;
                    Debug.Log("OPDoneGwangMission");
                    isGwangMissionDone = true;
                }
                break;

            case "FivePee":
                if (CardManager.instance.soldierEmptyIndex >= 5 && isPeeMissionDone == false)
                {
                    myMissionDoneCount++;
                    BattleSystem.instance.damage = 2;
                    Debug.Log("MeDonePeeMission");
                    isPeeMissionDone = true;
                }
                else if (CardManager.instance.enemySoldierEmptyIndex >= 5 && isPeeMissionDone == false)
                {
                    opponentMissionDoneCount++;
                    BattleSystem.instance.damage = 2;
                    Debug.Log("OPDonePeeMission");
                    isPeeMissionDone = true;
                }
                break;

            case "DanPair":
                if (CardManager.instance.normalFlagEmptyIndex >= 2 && isDanMissionDone == false)
                {
                    myMissionDoneCount++;
                    BattleSystem.instance.damage = 2;
                    Debug.Log("MeDoneFlagMission");
                    isDanMissionDone = true;
                }
                else if (CardManager.instance.enemyNormalFlagEmptyIndex >= 2 && isDanMissionDone == false)
                {
                    opponentMissionDoneCount++;
                    BattleSystem.instance.damage = 2;
                    Debug.Log("OPDoneFlagMission");
                    isDanMissionDone = true;
                }
                break;

            case "GodoriPair":
                if (CardManager.instance.animalEmptyIndex >= 2 && isGodoriMissionDone == false)
                {
                    myMissionDoneCount++;
                    BattleSystem.instance.damage = 2;
                    Debug.Log("MeDoneGodoriMission");
                    isGodoriMissionDone = true;
                }
                else if (CardManager.instance.enemyAnimalEmptyIndex >= 2 & isGodoriMissionDone == false)
                {
                    opponentMissionDoneCount++;
                    BattleSystem.instance.damage = 2;
                    Debug.Log("OPDoneGodoriMission");
                    isGodoriMissionDone = true;
                }
                break;

            default:
                break;
        }
    }
}
