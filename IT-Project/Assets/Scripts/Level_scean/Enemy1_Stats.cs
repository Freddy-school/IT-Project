using UnityEngine;

[CreateAssetMenu(fileName = "Enemy1_Stats", menuName = "Scriptable Objects/Enemy1_Stats")]
public class Enemy1_Stats : Enemy_Stats
{
    public Enemy1_Stats()
    {
        type = EntityType.Enemy1;
        origin_health = 50;
    }
    
}
