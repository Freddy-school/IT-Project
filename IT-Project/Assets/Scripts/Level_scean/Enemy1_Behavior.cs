using UnityEngine;


public class Enemy1_Behavior : Enemy_Behavior
{

    [SerializeField]
    private Enemy1_Stats enemy1_stats;

    [SerializeField] Animator animator;



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

    private void Update()
    {
        if ( agent.velocity.magnitude >= 0.1f)
        {
            animator.SetBool("IsWalking", true);
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }
    }
}