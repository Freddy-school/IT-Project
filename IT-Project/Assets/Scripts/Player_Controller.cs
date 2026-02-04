using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] Rigidbody rb;

    [SerializeField] float horizontalInput;
    [SerializeField] float verticalInput;
    Vector3 movment;

    [SerializeField] Animation animaton_Attack_1;

    //Variabeln die wir später maybe brauchen
    bool canMove = true;
    float moveSpeedModifikator = 1f;


    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        MovePlayer();
        Attack1();
    }

    void MovePlayer()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S))
        {
            rb.AddForce(horizontalInput * moveSpeed * Time.fixedDeltaTime, 0f, 0f, ForceMode.VelocityChange);
        }

    }

    void Attack1()
    {
        if (Input.GetKeyDown(KeyCode.L)) 
        { 
            
        } 
        

    }
}
