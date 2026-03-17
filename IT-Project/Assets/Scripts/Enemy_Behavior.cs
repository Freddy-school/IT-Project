using System.Collections;
using UnityEngine;

public class Enemy_Behavior : MonoBehaviour
{
    [SerializeField] GameObject Spawnpoint1;

    private void Start()
    {
        
        transform.position = Spawnpoint1.transform.position;
    }
}