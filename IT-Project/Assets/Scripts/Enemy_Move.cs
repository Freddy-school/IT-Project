using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Move : MonoBehaviour
{
    [SerializeField] Transform Player_pos;
    [SerializeField] Transform Position_Self;
    [SerializeField] float Move_Speed;
    [SerializeField] float Enemy_Damege;
    void Start()
    {
        Player_pos = GetComponent<Transform>();
        Position_Self = GetComponent<Transform>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    void FindPlayer()
    {

    }
}
