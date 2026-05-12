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

    [Header("Attack")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackPointDistance;
    public float attackRadius;
    public float damage;
    public LayerMask enemyLayer;

    [Header("UI")]
    public GameObject deathPanel;
    public GameObject GameOverlay;
    public GameObject GameManeger;
    public UI_Maneger_Script uiManeger;

    private bool isDead = false;

    private void Awake()
    {
        
        charController = GetComponent<CharacterController>();
        
    }

    private void Start()
    {
        Time.timeScale = 1f;

            playerHealth = player_stats.origin_health;
            type = player_stats.type;
            damage = player_stats.damage;
        attackRadius = 5;
        attackPointDistance = 2;

    }

    

    private void Update()
    {
        PlayerMovement();
        ApplyGravity();
        if (Input.GetKeyDown(KeyCode.Escape)) { OpenMenue(); }
        if (Input.GetMouseButtonDown(0)) { Attack(); }
    }

    private void PlayerMovement()
    {
        float horizInput = Input.GetAxis(horizontalInputName) * movementSpeed;
        float vertInput = Input.GetAxis(verticalInputName) * movementSpeed;

        Vector3 forwardMovement = transform.forward * vertInput;
        Vector3 rightMovement = transform.right * horizInput;
        Vector3 movment = forwardMovement + rightMovement;

        movment += knockbackVelocity;

        if (charController != null)
            charController.Move(movment * Time.deltaTime);

        knockbackVelocity = Vector3.Lerp(knockbackVelocity, Vector3.zero, knockbackDecay * Time.deltaTime);
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

        if (playerHealth <= 0f) Die();
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
     

        Collider[] hitEnemies = Physics.OverlapSphere(transform.position + transform.forward * attackPointDistance, attackRadius, enemyLayer);
        foreach (Collider enemyCollider in hitEnemies)
        {
            Enemy_Behavior enemy = enemyCollider.GetComponent<Enemy_Behavior>();
            if (enemy != null)
            {
                
                enemy.TakeDamage(damage);
                ShowAttackSphereDebug(transform.position + transform.forward * attackPointDistance, attackRadius);
            }
        }
    }
    private void ShowAttackSphereDebug(Vector3 center, float radius, float duration = 0.5f)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        // Collider entfernen, weil es nur eine Visualisierung sein soll
        var col = go.GetComponent<Collider>();
        if (col != null) Destroy(col);

        go.transform.position = center;
        go.transform.localScale = Vector3.one * radius * 2f;

        var mat = new Material(Shader.Find("Standard"));
        // stärkere Transparenz: Alpha deutlich reduzieren
        float alpha = 0.15f; // kleinerer Wert = transparenter
        mat.color = new Color(1f, 0f, 0f, alpha);

        // Transparent-Setup für Standard-Shader
        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mat.SetInt("_ZWrite", 0);
        mat.DisableKeyword("_ALPHATEST_ON");
        mat.EnableKeyword("_ALPHABLEND_ON");
        mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        mat.renderQueue = 3000;

        var rend = go.GetComponent<Renderer>();
        if (rend != null) rend.sharedMaterial = mat;

        Destroy(go, duration);
    }
}