using UnityEngine;

public class Player_Stats : Entity_Stats
{
    private void OnEnable()
    {
        type = EntityType.Player; 
        origin_health = 100;
    }
}
