using UnityEngine;
// EnemyBase.cs (일부만 발췌)
// ===== 추가 시작 =====
using UnityEngine.U2D; // 필요 시(다른 곳에서 Atlas 참조 안 하면 없어도 됨)
// ===== 추가 끝 =====

public class EnemyBase : MonoBehaviour
{
    [Header("Data")]
    public EnemyData data;

    public EnemyMovement enemyMove;

    [HideInInspector] public float currentHP;
    // protected Animator animator;   // <-- Animator 미사용으로 주석 처리 권장
    protected SpriteRenderer spriteRenderer;
    protected Rigidbody2D rb;
    protected Collider2D col;

    public bool isDead { get; private set; } = false;
    private bool isDying = false; 

    public float despawnDistance = 20f;

    // ===== 추가 시작 =====
    // 프레임 애니메이터 참조(신규)
    protected AtlasFrameAnimator frameAnimator;
    private HitFlashKnockback hitFx;

    // 이동 속도 임계치(이 값 이상이면 Walk, 미만이면 Idle)
    [Header("Anim Switch")]
    public float walkSpeedThreshold = 0.05f;
    // ===== 추가 끝 =====

    protected virtual void Awake()
    {
        // animator = GetComponent<Animator>(); // <-- 사용 안 함
        enemyMove = GetComponent<EnemyMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        // ===== 추가 시작 =====
        frameAnimator = GetComponent<AtlasFrameAnimator>();
        if (frameAnimator != null)
            frameAnimator.onCompleted += OnAnimCompleted;
        hitFx = GetComponent<HitFlashKnockback>();
    }
    protected virtual void OnEnable()
    {
        isDead = false;
        isDying = false;
        enemyMove.speed = data.moveSpeed;
        if (data == null)
        {
            Debug.LogWarning($"[EnemyBase] data is null on {name}. Check prefab/pool assignment.");
            currentHP = 0f;
        }
        else
        {
            currentHP = data.maxHP;
        }



        if (spriteRenderer != null && data != null && data.enemySprite != null)
            spriteRenderer.sprite = data.enemySprite;

        if (col != null) col.enabled = true;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.simulated = true;
        }

        // ===== 추가 시작 =====
        // 시작 상태: Idle
        if (frameAnimator != null)
            frameAnimator.SetState(AtlasFrameAnimator.State.Walk);
        // ===== 추가 끝 =====
    }
    private void OnAnimCompleted(AtlasFrameAnimator.State s)
    {
        if (s == AtlasFrameAnimator.State.Dead)
        {
            frameAnimator.completedFired = false;
            gameObject.SetActive(false);
        }
    }
    protected virtual void OnDestroy()
    {
        if (frameAnimator != null)
            frameAnimator.onCompleted -= OnAnimCompleted;
    }

    void Update()
    {
        if (!GameManager.Instance || GameManager.Instance.PlayerTransform == null) return;
        if (isDead) return;
        if (GameManager.Instance.IsGameOver) return;


        // ===== 추가 시작 =====
        // 속도 기반으로 Idle/Walk 전환
        if (frameAnimator != null)
        {
            if (isDead)
            {
                if (frameAnimator.state != AtlasFrameAnimator.State.Dead)
                    frameAnimator.SetState(AtlasFrameAnimator.State.Dead);
            }
            else if (rb != null)
            {
                if (frameAnimator.state != AtlasFrameAnimator.State.Walk)
                    frameAnimator.SetState(AtlasFrameAnimator.State.Walk);
            }
        }
        // ===== 추가 끝 =====


        float distSqr = (transform.position - GameManager.Instance.PlayerTransform.position).sqrMagnitude;
        if (distSqr > despawnDistance * despawnDistance)
        {
            gameObject.SetActive(false);
            //TODO:
            //계속 도망 다니면 active false만 되다가 끝나기 때문에
            //해당시간 enemy 및 이후 enemy의 respawn 끝나는 시간도 active false된 enemy에 비례해서 늘리기
        }
    }
    public virtual void TakeDamage(float dmg)
    {
        if (isDead) return;
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Hit);

        currentHP -= dmg;
        if (currentHP <= 0)
            Die();
    }
    public virtual void TakeDamage(float dmg, Transform attacker)
    {
        if (isDead || isDying) return;
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Hit);

        // 공격자 반대 방향
        Vector2 dir = Vector2.zero;
        if (attacker != null)
            dir = (transform.position - attacker.position).normalized;

        currentHP -= dmg;

        if (currentHP <= 0)
        {
            // 연출 → 끝나면 죽음
            isDying = true;
            Die();
        }
        else
        {
            // 일반 피격: 연출만
            hitFx?.OnHit(dir);
        }
    }
    public virtual void OnWarped()
    {
        Debug.Log(this.name + " has warped");
    }
    protected virtual void Die()
    {
        isDead = true;
        if(!GameManager.Instance.IsGameOver)
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Dead);

        // if (animator != null) animator.SetTrigger("Die"); // Animator 미사용
        if (frameAnimator != null)
            frameAnimator.SetState(AtlasFrameAnimator.State.Dead);

        if (col != null) col.enabled = false;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.simulated = false;
        }

        if (EnemyKillCounter.Instance != null)
            EnemyKillCounter.Instance.AddKill();

        if (data != null && data.expGemPrefab != null)
        {
            GameObject gem = Instantiate(data.expGemPrefab, transform.position, Quaternion.identity);
            ExpGem expGem = gem.GetComponent<ExpGem>();
            if (expGem != null)
                expGem.SetExp(data.expDrop);
        }
        hitFx?.ResetColor();
        //OnDeathAnimationEnd();
        //gameObject.SetActive(false);
    }
    public void OnDeathAnimationEnd()
    {
        //gameObject.SetActive(false); // or return to pool
        //OnDeathAnimationEnd();
    }
}
