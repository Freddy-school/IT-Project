using UnityEngine;

[CreateAssetMenu(fileName = "Player_Stats", menuName = "Scriptable Objects/Player_Stats")]
public class Player_Stats : Entity_Stats
{
   
        [SerializeField] public double origin_health = 100;
    
}
