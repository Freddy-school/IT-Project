using UnityEngine;


public class Enemy1_Behavior : Enemy_Behavior
{

    [SerializeField]
    private Enemy1_Stats enemy1_stats;



    protected override void Awake()
    {
        base.Awake();


        if (enemy1_stats == null)
        {
            Debug.LogError(
                "Enemy1 Stats fehlen!"
            );

            return;
        }



        enemyHealth =
            enemy1_stats.origin_health;


        enemyDamage =
            enemy1_stats.damage;


        type =
            enemy1_stats.type;
    }
}