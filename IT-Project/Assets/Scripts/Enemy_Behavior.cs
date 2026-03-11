using UnityEngine;
using UnityEngine.AI;

public class Enemy_Behavior : MonoBehaviour
{
    //Getting Components
    
    public GameObject Player_Obj;
    public NavMeshAgent agent;

    private void Update()
    {
        SetWayPoint();
    }


    void SetWayPoint()
    {
        Player_Obj.transform.position 
    }


}
