using UnityEngine;
public class EnemyBase : MonoBehaviour
{
    [Header("Data")]
    public EnemyData data;              // 적 데이터 (체력, 속도, 드랍 등)
    public EnemyMovement enemyMove;     // 이동 스크립트 참조

    [HideInInspector] public float currentHP; // 현재 체력
    protected SpriteRenderer spriteRenderer;
    protected Rigidbody2D rb;
    protected Collider2D col;

    public bool isDead { get; private set; } = false; // 사망 여부
    private bool isDying = false;                     // 사망 연출 중 여부
    public float despawnDistance = 20f;               // 비활성화 거리 기준

    // 아틀라스 프레임 애니메이터 및 피격 연출
    protected AtlasFrameAnimator frameAnimator;
    private HitFlashKnockback hitFx;

    [Header("Anim Switch")]
    public float walkSpeedThreshold = 0.05f; // 걷기/대기 전환 속도 기준

    // 초기화: 컴포넌트 참조 및 애니메이터 이벤트 등록
    protected virtual void Awake()
    {
        enemyMove = GetComponent<EnemyMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        frameAnimator = GetComponent<AtlasFrameAnimator>();
        if (frameAnimator != null)
            frameAnimator.onCompleted += OnAnimCompleted;

        hitFx = GetComponent<HitFlashKnockback>();
    }

    // 활성화 시 상태 초기화
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

        if (spriteRenderer != null && data?.enemySprite != null)
            spriteRenderer.sprite = data.enemySprite;

        if (col != null) col.enabled = true;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.simulated = true;
        }

        // 시작 상태: Walk
        if (frameAnimator != null)
            frameAnimator.SetState(AtlasFrameAnimator.State.Walk);
    }

    // 애니메이션 완료 시 처리 (Dead 끝나면 비활성화)
    private void OnAnimCompleted(AtlasFrameAnimator.State s)
    {
        if (s == AtlasFrameAnimator.State.Dead)
        {
            frameAnimator.completedFired = false;
            gameObject.SetActive(false);
        }
    }

    // 파괴 시 이벤트 해제
    protected virtual void OnDestroy()
    {
        if (frameAnimator != null)
            frameAnimator.onCompleted -= OnAnimCompleted;
    }

    // 매 프레임 갱신
    void Update()
    {
        if (!GameManager.Instance || GameManager.Instance.PlayerTransform == null) return;
        if (isDead || GameManager.Instance.IsGameOver) return;

        // 속도 기반으로 Idle/Walk/Dead 상태 전환
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

        // 플레이어와 거리 체크 후 멀어지면 비활성화
        float distSqr = (transform.position - GameManager.Instance.PlayerTransform.position).sqrMagnitude;
        if (distSqr > despawnDistance * despawnDistance)
        {
            gameObject.SetActive(false);
        }
    }


    // 넉백과 함께 데미지 처리 (공격자 방향 포함)
    public virtual void TakeDamage(float dmg, Transform attacker = null)
    {
        if (isDead || isDying) return;
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Hit);

        // 공격자 반대 방향 계산
        Vector2 dir = Vector2.zero;
        if (attacker != null)
            dir = (transform.position - attacker.position).normalized;

        currentHP -= dmg;

        if (currentHP <= 0)
        {
            isDying = true;
            Die();
        }
        else
        {
            hitFx?.OnHit(dir); // 넉백 연출
        }
    }

    // 사망 처리: 애니메이션, 충돌 비활성화, 드랍 등
    protected virtual void Die()
    {
        isDead = true;
        if (!GameManager.Instance.IsGameOver)
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Dead);

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

        // 경험치 젬 드랍
        if (data?.expGemPrefab != null)
        {
            GameObject gem = Instantiate(data.expGemPrefab, transform.position, Quaternion.identity);
            ExpGem expGem = gem.GetComponent<ExpGem>();
            if (expGem != null)
                expGem.SetExp(data.expDrop);
        }

        hitFx?.ResetColor();
    }
}
