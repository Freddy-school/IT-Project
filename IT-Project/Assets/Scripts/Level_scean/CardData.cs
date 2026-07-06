using UnityEngine;

[CreateAssetMenu(
    fileName = "New Card",
    menuName = "Cards/Card")]
public class CardData : ScriptableObject
{
    public string cardName;

    public int cost;

    public int damage;

    public Sprite sprite;
}