using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //라이센스, 돈 관리
        if(!PlayerPrefs.HasKey("License"))
        {
            PlayerPrefs.SetInt("License", 0);
        }
        if(!PlayerPrefs.HasKey("Coin"))
        {
            PlayerPrefs.SetInt("Coin", 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
