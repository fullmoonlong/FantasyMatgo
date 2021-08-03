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
    private bool isMissionDone;

    public int missionDoneCount;

    private void Start()
    {
        currentMission = "";
        isMissionSelected = false;
        isMissionDone = false;
        missions = new List<string>();
        missions.Add("GwangPair");
        missions.Add("PeePair");
        missions.Add("MonthPair");
        missions.Add("DanPair");
        missions.Add("GodoriPair");
    }

    private void Update()
    {
        if (GameManager.instance.maxTurnCount == 5)
        {
            if(isMissionSelected == true)
            {
                return;
            }

            currentMission = missions[Random.Range(0, missions.Count)];
            isMissionSelected = true;
        }
    }

    private void MissionDone()
    {
        switch(currentMission)
        {
            case "GwangPair":
                if (CardManager.instance.gwangCount == 2 && isMissionDone == false)
                {
                    missionDoneCount++;
                    isMissionDone = true;
                }
                break;
            case "PeePair":
                missionDoneCount++;
                break;
            case "MonthPair":
                missionDoneCount++;
                break;
            case "DanPair":
                missionDoneCount++;
                break;
            case "GodoriPair":
                missionDoneCount++;
                break;
            default:
                break;
        }
    }
}
