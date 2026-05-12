using UnityEngine;

public class Player_Controller : MonoBehaviour, IDamageable
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
    [SerializeField] private Player_Stats player_stats;
    [SerializeField] private float playerHealth;
    private EntityType type;

    [Header("Player Knockback")]
    [SerializeField] Vector3 knockbackVelocity;
    [SerializeField] float knockbackDecay = 5f;
    [SerializeField] float KnockbackStrength;

    [Header("UI")]
    public GameObject deathPanel;
    public GameObject GameOverlay;
    public GameObject GameManeger;
    public UI_Maneger_Script uiManeger;

    private bool isDead = false;
    

    private void Start()
    {
        charController = GetComponent<CharacterController>();
        Time.timeScale = 1f;
        playerHealth = player_stats.origin_health;
        type = player_stats.type;
    }

    private void Update()
    {
        PlayerMovement();
        ApplyGravity();
        if(Input.GetKeyDown(KeyCode.Escape)) { OpenMenue(); }
        if(Input.GetMouseButtonDown(0)) {Attack(); }
        
    }

    private void PlayerMovement()
    {
        float horizInput = Input.GetAxis(horizontalInputName) * movementSpeed;
        float vertInput = Input.GetAxis(verticalInputName) * movementSpeed;

        Vector3 forwardMovement = transform.forward * vertInput;
        Vector3 rightMovement = transform.right * horizInput;

        Vector3 movment = forwardMovement + rightMovement;

        //Knockback intigration
        movment += knockbackVelocity;


        charController.Move(movment * Time.deltaTime);

        //knockback abnehmen
        knockbackVelocity = Vector3.Lerp(knockbackVelocity, Vector3.zero, knockbackDecay*Time.deltaTime);
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

            ApplyEnemyKnockback(other.transform, damageTaken);
        }

        if (playerHealth <= 0f)
        {
            Die();
        }
    }

    public void TakeDamage(float damage)
    {
        
        playerHealth -= /*(int)*/damage;
        Debug.Log(type + " Health: " + playerHealth);


    }

    public void ApplyKnockback(Vector3 direction, float force)
    {
        knockbackVelocity = direction.normalized * force;
    }

    private void ApplyEnemyKnockback(Transform enemy, float damageTaken)
    {
        Vector3 direction = (transform.position - enemy.position).normalized;

        KnockbackStrength = damageTaken * 0.3f;
        ApplyKnockback(direction, KnockbackStrength);
    }

    void Die()
    {
        if(isDead) return;
        isDead = true;

        Debug.Log("You Died");
        Time.timeScale = 0f;
        //GameManager.Instance.GameOver();
        ShowDeathUI();
    }

    void ShowDeathUI()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if(deathPanel != null)
        {
            deathPanel.SetActive(true);
            GameOverlay.SetActive(false);
        }
    }

    void OpenMenue()
    {
        Debug.Log("Test1");
        uiManeger.OpenOptions();
    }

    //temporär jetzt hier später vielleicht bei der ui oder so
    private void HideDeathUI()
    {
        if (deathPanel != null)
        {
            deathPanel.SetActive(false);

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    public void Attack()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, 2f);
        Debug.Log("Enemy Hit"); 
    }

}