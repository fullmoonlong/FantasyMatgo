using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneChange : MonoBehaviour
{
    public GameObject enterPanel;
    // Start is called before the first frame update
    public void SceneLoad(string name)
    {
        Profile.instance.currentHp = PlayerPrefs.GetInt("HP");
        print(Profile.instance.currentHp);
        print(PlayerPrefs.GetInt("License"));
        if (Profile.instance.currentHp >= 1000 + PlayerPrefs.GetInt("License") * 2)
        {
            switch (PlayerPrefs.GetInt("License"))
            {
                case 0:
                    Profile.instance.currentHp -= 1000;
                    PlayerPrefs.SetInt("Player" + "Game_Hp", 100);
                    PlayerPrefs.SetInt("Op" + "Game_Hp", 100);
                    break;

                case 1:
                    Profile.instance.currentHp -= 1200;
                    PlayerPrefs.SetInt("Player" + "Game_Hp", 120);
                    PlayerPrefs.SetInt("Op" + "Game_Hp", 120);
                    break;

                case 2:
                    Profile.instance.currentHp -= 1400;
                    PlayerPrefs.SetInt("Player" + "Game_Hp", 140);
                    PlayerPrefs.SetInt("Op" + "Game_Hp", 140);
                    break;

                case 3:
                    Profile.instance.currentHp -= 1600;
                    PlayerPrefs.SetInt("Player" + "Game_Hp", 160);
                    PlayerPrefs.SetInt("Op" + "Game_Hp", 160);
                    break;

                case 4:
                    Profile.instance.currentHp -= 1800;
                    PlayerPrefs.SetInt("Player" + "Game_Hp", 180);
                    PlayerPrefs.SetInt("Op" + "Game_Hp", 180);
                    break;

                case 5:
                    Profile.instance.currentHp -= 2000;
                    PlayerPrefs.SetInt("Player" + "Game_Hp", 200);
                    PlayerPrefs.SetInt("Op" + "Game_Hp", 200);
                    break;
            }

            Profile.instance.SetHp(Profile.instance.currentHp);
            SceneManager.LoadScene(name);
        }
      
        else
        {
            enterPanel.SetActive(true);
        }
    }

}
