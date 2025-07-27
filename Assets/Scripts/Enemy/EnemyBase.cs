using UnityEngine;

[RequireComponent(typeof(Animator), typeof(SpriteRenderer))]
public class EnemyBase : MonoBehaviour
{
    [Header("Data")]
    public EnemyData data;

    [HideInInspector] public float currentHP;
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;

    public bool isDead { get; private set; } = false;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected virtual void OnEnable()
    {
        isDead = false;
        currentHP = data.maxHP;
        if (spriteRenderer != null && data.enemySprite != null)
            spriteRenderer.sprite = data.enemySprite;

        if (animator != null && data.animatorController != null)
            animator.runtimeAnimatorController = data.animatorController;

        var col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = true;

        // Rigidbody2D 다시 활성화
        var rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.simulated = true;
        }
    }

    public virtual void TakeDamage(float dmg)
    {
        if (isDead) return;
        currentHP -= dmg;
        if (currentHP <= 0)
            Die();
    }

    protected virtual void Die()
    {
        isDead = true;
        animator.SetTrigger("Die");
        GetComponent<Collider2D>().enabled = false;

        var rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.simulated = false;
        }

        // Drop exp
        if (data.expGemPrefab != null)
        {
            GameObject gem = Instantiate(data.expGemPrefab, transform.position, Quaternion.identity);
            ExpGem expGem = gem.GetComponent<ExpGem>();
            if (expGem != null)
                expGem.SetExp(data.expDrop); // 경험치 전달
        }
    }

    // Animation Event에서 호출
    public void OnDeathAnimationEnd()
    {
        gameObject.SetActive(false); // 또는 Pool로 반환
    }
}
