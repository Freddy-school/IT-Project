using UnityEngine;


//Versuche die Nullreference zu fixen
[CreateAssetMenu(fileName = "Player_Stats", menuName = "Scriptable Objects/Player Stats")]
public class Player_Stats : Entity_Stats
{

    private void OnEnable()
    {
        type = EntityType.Player; 
        origin_health = 100;
        damage = 25;
    }
}
