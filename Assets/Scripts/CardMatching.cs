using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMatching : MonoBehaviour
{
    public static CardMatching instance;

    private void Awake()
    {
        instance = this;
    }

    public void FindMatch(string tag)
    {
        switch (tag)
        {
            case "Rat":
                Debug.Log("Rat");
                break;
            case "Cow":
                Debug.Log("Cow");
                break;
            case "Tiger":
                Debug.Log("Tiger");
                break;
            case "Rabbit":
                Debug.Log("Rabbit");
                break;
            case "Dragon":
                Debug.Log("Dragon");
                break;
            case "Snake":
                Debug.Log("Snake");
                break;
            case "Horse":
                Debug.Log("Horse");
                break;
            case "Sheep":
                Debug.Log("Sheep");
                break;
            case "Monkey":
                Debug.Log("Monkey");
                break;
            case "Cock":
                Debug.Log("Cock");
                break;
            case "Dog":
                Debug.Log("Dog");
                break;
            case "Pig":
                Debug.Log("Pig");
                break;
            default:
                break;
        }
    }
}
