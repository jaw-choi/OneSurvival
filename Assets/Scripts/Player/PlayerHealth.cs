using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    public HealthBar healthBarPrefab;
    private HealthBar healthBarInstance;

    private float regenTimer = 0f;
    private float regenInterval = 1f; // 20초마다 회복
    void Start()
    {
        currentHealth = PlayerStats.Instance.TotalMaxHealth;
        maxHealth = PlayerStats.Instance.TotalMaxHealth;

        if (healthBarPrefab != null)
        {
            GameObject canvasObj = GameObject.Find("WorldSpaceCanvas");
            Canvas worldCanvas = canvasObj?.GetComponent<Canvas>();
            healthBarInstance = Instantiate(healthBarPrefab, worldCanvas.transform);
            healthBarInstance.SetTarget(transform);
            healthBarInstance.SetHealth(currentHealth, maxHealth);
        }
    }

    void Update()
    {
        HandleRegen();
    }
    void HandleRegen()
    {
        float regenAmount = PlayerStats.Instance.TotalRegen;
        if (regenAmount <= 0f) return;

        regenTimer += Time.deltaTime;
        if (regenTimer >= regenInterval)
        {
            regenTimer = 0f;
            Heal(regenAmount);
        }
    }

    public void Heal(float amount)
    {
        if (currentHealth <= 0) return; // 죽은 상태면 회복 안 함

        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        if (healthBarInstance != null)
        {
            healthBarInstance.SetHealth(currentHealth, maxHealth);
        }
    }
    public void TakeDamage(float amount)
    {
        GoldManager.Instance.AddGold(50); // 골드 증가
        GoldManager.Instance.SpendGold(30); // 골드 감소
        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0);
        Debug.Log("currentHealth" + currentHealth);
        if (healthBarInstance != null)
        {
            healthBarInstance.SetHealth(currentHealth, maxHealth);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        //Debug.Log("Player Dead");
        //gameObject.SetActive(false);

        Destroy(gameObject);
        if (healthBarInstance != null)
            Destroy(healthBarInstance.gameObject);

        GameManager.Instance.GameOver();
    }
}
