using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public int maxHp;
    public int currentHp;

    private void Start()
    {
        maxHp = 100;
        currentHp = 100;
    }
}
