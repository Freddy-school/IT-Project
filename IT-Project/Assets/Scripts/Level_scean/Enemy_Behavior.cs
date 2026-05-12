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
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Behavior : MonoBehaviour, IDamageDealer
{
    public GameObject Spawnpoint1;
    public GameObject Player_Pos;

    public Vector3 destination;
    public NavMeshAgent agent;

    [Header("Stats")]
    [SerializeField] protected float enemyHealth = 100f;
    [SerializeField] private float enemyDamage = 10f;
    [SerializeField] private string type;

    private bool isStunned = false;

    private void Start()
    {
        Spawnpoint1 = GameObject.Find("Spawnpoint1");
        Player_Pos = GameObject.Find("Player");

        agent = GetComponent<NavMeshAgent>();

        transform.position = Spawnpoint1.transform.position;

        agent.updateRotation = true;
        agent.updatePosition = true;
    }

    private void Update()
    {
        if (Player_Pos == null) return;

        if (!isStunned)
        {
            destination = Player_Pos.transform.position;
            agent.SetDestination(destination);
        }
    }


    public float GetDamage()
    {
        return enemyDamage;
    }

    public void TakeDamage(float damage)
    {
        enemyHealth -= damage;
        Debug.Log(type + " Health: " + enemyHealth);
    }


    public void Knockback(Vector3 direction, float force, float stunTime = 0.3f)
    {
        if (!gameObject.activeInHierarchy) return;

        StartCoroutine(KnockbackRoutine(direction, force, stunTime));
    }

    private IEnumerator KnockbackRoutine(Vector3 direction, float force, float stunTime)
    {
        isStunned = true;

        agent.isStopped = true;

        // leichte physische Bewegung
        float timer = 0f;
        while (timer < stunTime)
        {
            transform.position += direction.normalized * force * Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        }

        agent.isStopped = false;
        isStunned = false;
    }
}