using UnityEngine;
using UnityEngine.AI;

public class Enemy_Behavior : MonoBehaviour, IDamageDealer
{
    public GameObject Spawnpoint1;
    public GameObject Player_Pos;
    public Vector3 destination;
    public NavMeshAgent agent;

    [Header("Atributes")]
    [SerializeField] private float enemyHealth;
    [SerializeField] private float enemyDamage;

    private void Start()
    {
        Spawnpoint1 = GameObject.Find("Spawnpoint1");
        Player_Pos = GameObject.Find("Player");
        agent = GetComponent<NavMeshAgent>();
        transform.position = Spawnpoint1.transform.position;
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
}