// HitFlashKnockback.cs (추가/수정)
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class HitFlashKnockback : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color flashColor = new Color(1.5f, 1.5f, 1.5f, 1f);
    [SerializeField] private float flashDuration = 0.2f;

    [Header("Knockback")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float knockbackForce = 2f;
    [SerializeField] private float knockbackTime = 0.1f;

    // 추가
    [SerializeField] private EnemyMovement move;

    private Color baseColor;
    private MaterialPropertyBlock mpb;
    private bool isKnockback = false;

    void Awake()
    {
        if (!sr) sr = GetComponentInChildren<SpriteRenderer>();
        if (!rb) rb = GetComponent<Rigidbody2D>();
        if (!move) move = GetComponent<EnemyMovement>();   // ← 추가

        mpb = new MaterialPropertyBlock();
        baseColor = sr ? sr.color : Color.white;
        ApplyColor(baseColor);
    }

    public void OnHit(Vector2 hitDir)
    {
        if (isKnockback) return;
        StopAllCoroutines();
        StartCoroutine(CoFlashAndKnockback(hitDir.normalized));
    }

    private IEnumerator CoFlashAndKnockback(Vector2 dir)
    {
        isKnockback = true;

        // 이동 잠시 중단 + 현재 속도 보관
        float prevSpeed = move ? move.speed : 0f;
        if (move) move.enabled = false;

        // 플래시
        ApplyColor(flashColor);
        yield return new WaitForSeconds(flashDuration);
        ApplyColor(baseColor);

        // 넉백 (끝나면 속도 0으로 정리)
        if (rb)
        {
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(dir * knockbackForce, ForceMode2D.Impulse);
            yield return new WaitForSeconds(knockbackTime);
            rb.linearVelocity = Vector2.zero;
        }
        else
        {
            Vector3 start = transform.position;
            Vector3 end = start + (Vector3)(dir * 0.3f);
            float t = 0f;
            while (t < knockbackTime) { t += Time.deltaTime; transform.position = Vector3.Lerp(start, end, t / knockbackTime); yield return null; }
        }

        // 이동 재개 + 속도 복원
        if (move) { move.speed = prevSpeed; move.enabled = true; }

        isKnockback = false;
    }

    private void ApplyColor(Color c)
    {
        if (!sr) return;
        sr.GetPropertyBlock(mpb);
        mpb.SetColor("_Color", c);
        sr.SetPropertyBlock(mpb);
    }

    public void ResetColor()
    {
        ApplyColor(baseColor);
    }
}
