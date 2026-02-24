using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    /*
     READ ME:
     Das ist ein test script wo ich an besseren bewegungsoptionen arbeite, weil ich die actuelle nur so semi Finde
     */

    [Header("Objecte")]
    [SerializeField]    CharacterController controller;
    [SerializeField]    Transform transformCamera;
    [Header("Werte")]
    [SerializeField]    float speed = 6f;
    [SerializeField]    float rotationSpeed = 10f;
    [SerializeField]    float gravity = -9.81f;
    [SerializeField]    float yVelocity;

    void Update()
    {
        Move();
        ApplyGravity();
    }

    void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 inputDir = new Vector3(horizontal, 0f, vertical).normalized;

        if(inputDir.magnitude >= 0.1f)
        {
            Vector3 camForward = transformCamera.forward;
            Vector3 camRight = transformCamera.right;

            camForward.y = 0f;
            camRight.y = 0f;

            Vector3 moveDir = camForward * vertical + camRight * horizontal;

            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
    }

    void ApplyGravity()
    {
        if(controller.isGrounded && yVelocity < 0f)
        {
            yVelocity = -2f;
        }
        yVelocity += gravity * Time.deltaTime;

        controller.Move(Vector3.up * yVelocity * Time.deltaTime);
    }

}
