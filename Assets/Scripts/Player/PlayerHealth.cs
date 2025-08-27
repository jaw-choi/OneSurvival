using System.Collections;
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


    [Header("Hit Flash")]
    [SerializeField] private Renderer playerRenderer; // 플레이어 MeshRenderer/SpriteRenderer 등 할당
    [SerializeField] private Color hitColor = Color.black;
    [SerializeField] private float flashDuration = 0.2f;
    private Color originalColor;
    private bool isFlashing = false;    

    // Optional: small camera shake
    [SerializeField] private Camera mainCam;
    [SerializeField] private float shakeAmt = 0.05f;
    [SerializeField] private float shakeTime = 0.04f;
    private Coroutine _subscribeCo;
    private bool _subscribed;

    IEnumerator SubscribeWhenReady()
    {
        // 이미 구독돼 있으면 중복 방지
        if (_subscribed) yield break;

        // PlayerStats.Instance가 준비될 때까지 대기
        while (PlayerStats.Instance == null)
            yield return null;

        PlayerStats.Instance.OnStatsChanged += HandleStatsChanged;
        _subscribed = true;
    }

    void OnEnable()
    {
        // 혹시 이전 코루틴이 남아있다면 안전하게 정리
        if (_subscribeCo != null) StopCoroutine(_subscribeCo);
        _subscribeCo = StartCoroutine(SubscribeWhenReady());
    }

    void OnDisable()
    {
        // 구독 해제 및 코루틴 정리
        if (_subscribeCo != null) { StopCoroutine(_subscribeCo); _subscribeCo = null; }

        if (_subscribed && PlayerStats.Instance != null)
            PlayerStats.Instance.OnStatsChanged -= HandleStatsChanged;

        _subscribed = false;
    }
    void Start()
    {
        currentHealth = PlayerStats.Instance.TotalMaxHealth;
        maxHealth = PlayerStats.Instance.TotalMaxHealth;

        if (playerRenderer != null)
            originalColor = playerRenderer.material.color;

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
        //DEBUG
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
        //TODO: blood particle or animation play
        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0);
        Debug.Log("currentHealth" + currentHealth + "//" + maxHealth);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Hit);

        // 1) small camera shake
        if(SettingsManager.Instance.ScreenShake)
            DoCameraShake();
        // 2) Red Flash
        if (playerRenderer != null && !isFlashing)
            StartCoroutine(HitFlash());

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

    private void DoCameraShake()
    {
        if (mainCam == null || shakeAmt <= 0f || shakeTime <= 0f) return;
        StartCoroutine(CameraShakeRoutine());
    }

    private System.Collections.IEnumerator CameraShakeRoutine()
    {
        Vector3 origin = mainCam.transform.localPosition;
        float t = 0f;
        while (t < shakeTime)
        {
            // simple per-frame jitter
            mainCam.transform.localPosition = origin + (Vector3)Random.insideUnitCircle * shakeAmt;
            t += Time.deltaTime;
            yield return null;
        }
        mainCam.transform.localPosition = origin;
    }
    private IEnumerator HitFlash()
    {
        isFlashing = true;
        playerRenderer.material.color = hitColor;
        yield return new WaitForSeconds(flashDuration);
        playerRenderer.material.color = originalColor;
        isFlashing = false;
    }
}
