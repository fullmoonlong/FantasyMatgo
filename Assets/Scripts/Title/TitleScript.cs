using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class TitleScript : MonoBehaviour
{
    public Text TouchText;
    float time;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene("MainScene");
        }

        if( time < 0.5f)
        {
            TouchText.color = new Color(0, 0, 0, 1 - time);
        }

        else
        {
            TouchText.color = new Color(0, 0, 0, time);
            if(time > 1f)
            {
                time = 0;
            }
        }
        time += Time.deltaTime;
    }
}
