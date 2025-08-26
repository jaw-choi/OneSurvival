using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    public HealthBar healthBarPrefab;
    private HealthBar healthBarInstance;

    private float regenTimer = 0f;
    private float regenInterval = 1f; // 20초마다 회복

    [SerializeField] private bool keepPercentOnMaxHpChange = true;
    void OnEnable()
    {
        if (PlayerStats.Instance != null)
            PlayerStats.Instance.OnStatsChanged += HandleStatsChanged;
    }

    void OnDisable()
    {
        if (PlayerStats.Instance != null)
            PlayerStats.Instance.OnStatsChanged -= HandleStatsChanged;
    }
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
        if (Input.GetKeyDown(KeyCode.F1))
        {
                currentHealth = 1;
                Debug.Log("F1 눌림 → 체력이 1로 설정됨");
        }
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
        //TODO:
        //GoldManager.Instance.AddGold(50); // 골드 증가
        //GoldManager.Instance.SpendGold(30); // 골드 감소
        
        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0);
        Debug.Log("currentHealth" + currentHealth + "//" + maxHealth);
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
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Lose);

        Destroy(gameObject);
        if (healthBarInstance != null)
            Destroy(healthBarInstance.gameObject);

        GameManager.Instance.GameOver();
    }
    void HandleStatsChanged()
    {
        float newMax = PlayerStats.Instance.TotalMaxHealth;
        if (Mathf.Approximately(newMax, maxHealth)) return;

        if (keepPercentOnMaxHpChange && maxHealth > 0.0001f)
        {
            float percent = currentHealth / maxHealth;
            maxHealth = newMax;
            currentHealth = Mathf.Clamp(percent * maxHealth, 0, maxHealth);
        }
        else
        {
            maxHealth = newMax;
            currentHealth = Mathf.Min(currentHealth, maxHealth);
        }

        if (healthBarInstance != null) healthBarInstance.SetHealth(currentHealth, maxHealth);
    }
}
