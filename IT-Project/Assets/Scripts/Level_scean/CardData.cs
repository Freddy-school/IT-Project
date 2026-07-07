using UnityEngine;
using static PlayerCombat;

[CreateAssetMenu(fileName = "New Card",
    menuName = "Cards/Card")]
public class CardData : ScriptableObject
{
    public string cardName;

    public int damage;

    public float range;

    public PlayerCombat.AttackType attackType;

    public Sprite sprite;
    
   
}