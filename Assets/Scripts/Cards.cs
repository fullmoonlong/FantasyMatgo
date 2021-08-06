using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Cards : ScriptableObject
{
    public string cardName;
    public string description;

    public Sprite artwork;
}
