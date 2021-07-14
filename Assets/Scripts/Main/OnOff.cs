using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnOff : MonoBehaviour
{
    public GameObject Panel1;
    public GameObject Panel2;
    // Start is called before the first frame update
    public void OnOffPanel()
    {
        Panel1.SetActive(false);
        Panel2.SetActive(true);
    }
}
