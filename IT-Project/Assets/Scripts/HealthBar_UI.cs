using UnityEngine;
using UnityEngine.UI;

public class HealthBar_UI : MonoBehaviour
{
    public float Width, Height;
    float Health, MaxHealth;

    [SerializeField]
    private RectTransform healthbar;

    [SerializeField] private Player_Stats player_stats;
    [SerializeField] private Player_Controller player;

    private void Start()
    {
        MaxHealth = player_stats.origin_health;
    }

    private void Update()
    {
        Health = player.ReturnHealth();
    }

    public void SetMaxHealth(float maxHealth)
    {
        MaxHealth = maxHealth;
    }

    void setHealth(float health)
    {
        Health = health;
        float newWidth = (Health / MaxHealth) * Width;

        healthbar.sizeDelta = new Vector2 (newWidth, Height);
    }



}
