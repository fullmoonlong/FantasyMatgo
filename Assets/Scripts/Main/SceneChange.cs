using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneChange : MonoBehaviour
{
    // Start is called before the first frame update
    public Profile player;
    public void SceneLoad(string name)
    {
        player.currentHp -= 10;
        player.SetHp(player.currentHp);
        SceneManager.LoadScene(name);
    }

}
