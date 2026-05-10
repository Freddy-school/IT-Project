using UnityEngine;

public class Enemy1_Behavior : Enemy_Behavior
{
    [SerializeField] private Enemy1_Stats enemy1_stats;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyHealth = enemy1_stats.origin_health;  
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
