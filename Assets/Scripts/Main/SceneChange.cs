using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChange : MonoBehaviour
{
    public GameObject enterPanel;
    // Start is called before the first frame update
    public void GameSceneLoad()
    {
        Profile.instance.currentHp = PlayerPrefs.GetInt("HP");
        print(Profile.instance.currentHp);
        print(PlayerPrefs.GetInt("License"));
        if(PlayerPrefs.HasKey("Equip0") && PlayerPrefs.HasKey("Equip1") && PlayerPrefs.HasKey("Equip2"))
        {
            if (Profile.instance.currentHp >= 1000 + PlayerPrefs.GetInt("License") * 2)
            {
                switch (PlayerPrefs.GetInt("License"))
                {
                    case 0:
                        Profile.instance.currentHp -= 100;
                        PlayerPrefs.SetInt("Player" + "Game_Hp", 100);
                        PlayerPrefs.SetInt("Op" + "Game_Hp", 100);
                        break;

                    case 1:
                        Profile.instance.currentHp -= 120;
                        PlayerPrefs.SetInt("Player" + "Game_Hp", 120);
                        PlayerPrefs.SetInt("Op" + "Game_Hp", 120);
                        break;

                    case 2:
                        Profile.instance.currentHp -= 140;
                        PlayerPrefs.SetInt("Player" + "Game_Hp", 140);
                        PlayerPrefs.SetInt("Op" + "Game_Hp", 140);
                        break;

                    case 3:
                        Profile.instance.currentHp -= 160;
                        PlayerPrefs.SetInt("Player" + "Game_Hp", 160);
                        PlayerPrefs.SetInt("Op" + "Game_Hp", 160);
                        break;

                    case 4:
                        Profile.instance.currentHp -= 180;
                        PlayerPrefs.SetInt("Player" + "Game_Hp", 180);
                        PlayerPrefs.SetInt("Op" + "Game_Hp", 180);
                        break;

                    case 5:
                        Profile.instance.currentHp -= 200;
                        PlayerPrefs.SetInt("Player" + "Game_Hp", 200);
                        PlayerPrefs.SetInt("Op" + "Game_Hp", 200);
                        break;
                }

                Profile.instance.SetHp(Profile.instance.currentHp);
                SceneManager.LoadScene("Game");
            }

            else
            {
                enterPanel.SetActive(true);
                enterPanel.transform.GetChild(0).GetComponent<Text>().text = "체력이 부족합니다.";
            }
        }

        else
        {
            enterPanel.SetActive(true);
            enterPanel.transform.GetChild(0).GetComponent<Text>().text = "아티팩트가 장착되지 않았습니다.";
        }
      
    }

}
