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
    //private bool isMonthMissionDone;
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
        //isMonthMissionDone = false;
        isDanMissionDone = false;
        isPeeMissionDone = false;
        isGodoriMissionDone = false;
        missions = new List<string>();
        missions.Add("GwangPair");
        missions.Add("FivePee");
        missions.Add("MonthPair");
        missions.Add("DanPair");
        missions.Add("GodoriPair");
    }

    private void Update()
    {
        if (GameManager.instance.maxTurnCount == 5)
        {
            if (isMissionSelected == true)
            {
                return;
            }

            currentMission = missions[Random.Range(0, missions.Count)];
            isMissionSelected = true;
            Debug.Log(currentMission);
            MissionDone();
        }
    }

    private void MissionDone()
    {
        Debug.Log("WHA");
        switch (currentMission)
        {
            case "GwangPair":
                if (CardManager.instance.gwangCount >= 2 && isGwangMissionDone == false)
                {
                    myMissionDoneCount++;
                    Debug.Log("MeDoneGwangMission");
                    isGwangMissionDone = true;
                }
                else if (CardManager.instance.enemyGwangCount >= 2 && isGwangMissionDone == false)
                {
                    opponentMissionDoneCount++;
                    Debug.Log("OPDoneGwangMission");
                    isGwangMissionDone = true;
                }
                break;
            case "FivePee":
                if (CardManager.instance.peeCount >= 5 && isPeeMissionDone == false)
                {
                    myMissionDoneCount++;
                    Debug.Log("MeDonePeeMission");
                    isPeeMissionDone = true;
                }
                else if (CardManager.instance.enemyPeeCount >= 5 && isPeeMissionDone == false)
                {
                    opponentMissionDoneCount++;
                    Debug.Log("OPDonePeeMission");
                    isPeeMissionDone = true;
                }
                break;
            case "DanPair":
                if (CardManager.instance.normalFlagCount >= 2 && isDanMissionDone == false)
                {
                    myMissionDoneCount++;
                    Debug.Log("MeDoneFlagMission");
                    isDanMissionDone = true;
                }
                else if (CardManager.instance.enemyNormalFlagCount >= 2 && isDanMissionDone == false)
                {
                    opponentMissionDoneCount++;
                    Debug.Log("OPDoneFlagMission");
                    isDanMissionDone = true;
                }
                break;
            case "GodoriPair":
                if (CardManager.instance.animalCount >= 2 && isGodoriMissionDone == false)
                {
                    myMissionDoneCount++;
                    Debug.Log("MeDoneAnimalMission");
                    isGodoriMissionDone = true;
                }
                else if (CardManager.instance.enemyAnimalCount >= 2 & isGodoriMissionDone == false)
                {
                    opponentMissionDoneCount++;
                    Debug.Log("OPDoneAnimalMission");
                    isGodoriMissionDone = true;
                }
                break;
            default:
                break;
        }
    }
}
