using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private string horizontalInputName = "Horizontal";
    [SerializeField] private string verticalInputName = "Vertical";

    [Header("Movement")]
    [SerializeField] private float movementSpeed = 6f;

    private CharacterController charController;

    [Header("Gravity")]
    [SerializeField] private float gravity = -9.81f;
    private float yVelocity;

    [Header("Player Stats")]
    [SerializeField] private int playerHealth = 100;

    private void Awake()
    {
        charController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        PlayerMovement();  
        ApplyGravity();    
    }

    private void PlayerMovement()
    {
        float horizInput = Input.GetAxis(horizontalInputName) * movementSpeed;
        float vertInput = Input.GetAxis(verticalInputName) * movementSpeed;

        Vector3 forwardMovement = transform.forward * vertInput;
        Vector3 rightMovement = transform.right * horizInput;

        
        charController.Move((forwardMovement + rightMovement) * Time.deltaTime);
    }

    private void ApplyGravity()
    {
        if (charController.isGrounded && yVelocity < 0f)
        {
            yVelocity = -2f;
        }

        yVelocity += gravity * Time.deltaTime;
        charController.Move(Vector3.up * yVelocity * Time.deltaTime);
    }

    
    private void OnTriggerEnter(Collider other)
    {
        IDamageDealer damageDealer = other.GetComponent<IDamageDealer>();

        if (damageDealer != null)
        {
            float damageTaken = damageDealer.GetDamage();
            TakeDamage(damageTaken);
        }
    }

    public void TakeDamage(float damage)
    {
        playerHealth -= (int)damage;
        Debug.Log("Player Health: " + playerHealth);
    }
}