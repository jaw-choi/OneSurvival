using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    public HealthBar healthBarPrefab;
    private HealthBar healthBarInstance;


    void Start()
    {
        currentHealth = maxHealth;

        if (healthBarPrefab != null)
        {
            //Canvas worldCanvas = Object.FindFirstObjectByType<Canvas>(); // World Space Canvas
            GameObject canvasObj = GameObject.Find("WorldSpaceCanvas");
            Canvas worldCanvas = canvasObj?.GetComponent<Canvas>();
            healthBarInstance = Instantiate(healthBarPrefab, worldCanvas.transform);
            healthBarInstance.SetTarget(transform);
            healthBarInstance.SetHealth(currentHealth, maxHealth);
        }
    }

    public void TakeDamage(float amount)
    {
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
