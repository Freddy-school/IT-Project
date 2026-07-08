/*using UnityEngine;
using UnityEngine.AI;

public class Enemy_Behavior : MonoBehaviour, IDamageDealer
{
    public GameObject Spawnpoint1;
    public GameObject Player_Pos;
    public Vector3 destination;
    public NavMeshAgent agent;
    public double currentHealth;

    [Header("Atributes")]
    [SerializeField] protected double enemyHealth;
    [SerializeField] private float enemyDamage;
    [SerializeField] private string type;
    

    private void Start()
    {
        Spawnpoint1 = GameObject.Find("Spawnpoint1");
        Player_Pos = GameObject.Find("Player");
        agent = GetComponent<NavMeshAgent>();
        transform.position = Spawnpoint1.transform.position;
        currentHealth = enemyHealth;
    }

    void Update()
    {
        destination = Player_Pos.transform.position;
        MoveAgent();
    }

    void MoveAgent()
    {
        agent.SetDestination(destination);
    }

    //Interface-Implementierung
    public float GetDamage()
    {
        return enemyDamage;
    }

    public void TakeDamage(float damage)
    {
       currentHealth -= (int)damage;
       Debug.Log(type + "Health: " + currentHealth);
    }
}*/
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Enemy_Behavior : MonoBehaviour, IDamageDealer
{
    public GameObject Player_Pos;

    protected Vector3 destination;
    protected NavMeshAgent agent;


    [Header("Stats")]
    [SerializeField] protected float enemyHealth;
    [SerializeField] protected float enemyDamage;
    [SerializeField] protected EntityType type;


    protected bool isStunned = false;



    public float GetDamage()
    {
        return enemyDamage;
    }



    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();


        if (agent == null)
        {
            Debug.LogError("Kein NavMeshAgent auf " + gameObject.name);
        }


        agent.updateRotation = true;
        agent.updatePosition = true;


        Player_Pos = GameObject.FindGameObjectWithTag("Charakter_Player");


        if (Player_Pos == null)
        {
            Debug.LogError( "Kein Player mit Tag 'Player' gefunden!" );
        }
    }





    protected virtual void Update()
    {
        if (Player_Pos == null)
            return;


        if (agent == null)
            return;

        if (!agent.isOnNavMesh)
        {
            Debug.LogWarning(gameObject.name + " ist nicht auf dem NavMesh!");

            return;
        }



        if (!isStunned)
        {
            destination = Player_Pos.transform.position;


            agent.SetDestination(destination);
        }
    }







    public void TakeDamage(float damage)
    {
        enemyHealth -= damage;


        Debug.Log(type + " Health: " + enemyHealth);


        if (enemyHealth <= 0)
        {
            Destroy(gameObject);
        }
    }







    public void Knockback(Vector3 direction,float force,float stunTime = 0.3f)
    {
        if (!gameObject.activeInHierarchy)
            return;


        StartCoroutine(
            KnockbackRoutine(
                direction,
                force,
                stunTime
            )
        );
    }






    IEnumerator KnockbackRoutine( Vector3 direction,float force,float stunTime)
    {
        isStunned = true;


        if (agent != null)
            agent.isStopped = true;



        float timer = 0;


        while (timer < stunTime)
        {
            transform.position +=
                direction.normalized *
                force *
                Time.deltaTime;


            timer += Time.deltaTime;

            yield return null;
        }



        if (agent != null)
            agent.isStopped = false;


        isStunned = false;
    }
}