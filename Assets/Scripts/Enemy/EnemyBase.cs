using UnityEngine;


public class EnemyBase : MonoBehaviour
{
    [Header("Data")]
    public EnemyData data;

    [HideInInspector] public float currentHP;
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;
    protected Rigidbody2D rb;
    protected Collider2D col;

    public bool CanWarp = true; // 보스/엘리트 등은 false로 제외
    public bool isDead { get; private set; } = false;

    public float despawnDistance = 20f;
    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    protected virtual void OnEnable()
    {
        isDead = false;
        // ===== 수정 시작 =====
        // Guard against missing data from pooled instances
        if (data == null)
        {
            Debug.LogWarning($"[EnemyBase] data is null on {name}. Check prefab/pool assignment.");
            currentHP = 0f;
        }
        else
        {
            currentHP = data.maxHP;
        }
        // ===== 수정 끝 =====

        if (spriteRenderer != null && data != null && data.enemySprite != null)
            spriteRenderer.sprite = data.enemySprite;

        if (animator != null && data != null && data.animatorController != null)
            animator.runtimeAnimatorController = data.animatorController;

        if (col != null)
            col.enabled = true;

        if (rb != null)
        {
            // ===== 수정 시작 =====
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.simulated = true;
            // ===== 수정 끝 =====
        }
    }
    void Update()
    {
        if (!GameManager.Instance || GameManager.Instance.PlayerTransform == null)
            return;

        // 죽었으면 처리 안 함
        if (isDead) return;

        // 보스나 꺼지면 안 되는 적이면 건너뛰기
        if (!CanWarp) return;

        // 거리 체크
        float distSqr = (transform.position - GameManager.Instance.PlayerTransform.position).sqrMagnitude;
        if (distSqr > despawnDistance * despawnDistance)
        {
            gameObject.SetActive(false); // 풀로 반환
        }
    }
    public virtual void TakeDamage(float dmg)
    {
        if (isDead) return;
        currentHP -= dmg;
        if (currentHP <= 0)
            Die();
    }

    public virtual void OnWarped()
    {
        Debug.Log(this.name + " has warped");
    }

    protected virtual void Die()
    {
        isDead = true;
        if (animator != null) animator.SetTrigger("Die");
        if (col != null) col.enabled = false;

        // ===== 수정 시작 =====
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.simulated = false;
        }
        // ===== 수정 끝 =====

        if (EnemyKillCounter.Instance != null)
            EnemyKillCounter.Instance.AddKill();

        if (data != null && data.expGemPrefab != null)
        {
            GameObject gem = Instantiate(data.expGemPrefab, transform.position, Quaternion.identity);
            ExpGem expGem = gem.GetComponent<ExpGem>();
            if (expGem != null)
                expGem.SetExp(data.expDrop);
        }
    }

    // Animation Event
    public void OnDeathAnimationEnd()
    {
        gameObject.SetActive(false); // or return to pool
    }
}
