using UnityEngine;

public class Enemy1_Behavior : Enemy_Behavior
{
    [SerializeField] private Enemy1_Stats enemy1_stats;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        base.Awake();
        enemyHealth = enemy1_stats.origin_health;
        enemyDamage = enemy1_stats.damage;
        type = enemy1_stats.type;
    }

    // Update is called once per frame
    void Update()
    {
        base.FixedUpdate();
    }
}
