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
    [SerializeField] private float knockbackForce = 0.1f;
    [SerializeField] private float knockbackTime = 0.1f;

    private Color baseColor;
    private MaterialPropertyBlock mpb;

    void Awake()
    {
        if (!sr) sr = GetComponentInChildren<SpriteRenderer>();
        if (!rb) rb = GetComponent<Rigidbody2D>();
        mpb = new MaterialPropertyBlock();

        // 현재 스프라이트의 색을 기본색으로 보관
        baseColor = (sr != null) ? sr.color : Color.white;
        ApplyColor(baseColor);
    }

    public void OnHit(Vector2 hitDir)
    {
        StopAllCoroutines();
        StartCoroutine(CoFlashAndKnockback(hitDir.normalized));
    }

    public void PlayHitStop(float duration, float slowScale = 0f)
    {
        StopCoroutineSafe(HitStop(duration, slowScale));
        StartCoroutine(HitStop(duration, slowScale));
    }

    public void ResetColor()
    {
        ApplyColor(baseColor);
    }
    public void OnHitWithCallback(Vector2 hitDir, System.Action onComplete)
    {
        StopAllCoroutines();
        StartCoroutine(CoFlashAndKnockbackWithCallback(hitDir.normalized, onComplete));
    }
    private IEnumerator CoFlashAndKnockbackWithCallback(Vector2 dir, System.Action onComplete)
    {
        // 원래 하던 플래시 + 넉백 (기존 CoFlashAndKnockback 내용 재사용)
        // 1) 플래시
        ApplyColor(flashColor);
        yield return new WaitForSeconds(flashDuration);
        ApplyColor(baseColor);

        // 2) 넉백
        if (rb)
        {
            var prevVel = rb.linearVelocity;         // 원하면 복원용
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(dir * knockbackForce, ForceMode2D.Impulse);
            yield return new WaitForSeconds(knockbackTime);
            rb.linearVelocity = Vector2.zero;        // 관성 제거(권장). 복원하려면 prevVel로 대체
        }
        else
        {
            Vector3 start = transform.position;
            Vector3 end = start + (Vector3)(dir * 0.3f);
            float t = 0f;
            while (t < knockbackTime)
            {
                t += Time.deltaTime;
                transform.position = Vector3.Lerp(start, end, t / knockbackTime);
                yield return null;
            }
        }

        onComplete?.Invoke();
    }
    private IEnumerator CoFlashAndKnockback(Vector2 dir)
    {
        // 1) 플래시
        ApplyColor(flashColor);
        yield return new WaitForSeconds(flashDuration);
        ApplyColor(baseColor);

        // 2) 넉백
        if (rb)
        {
            //rb.linearVelocity = Vector2.zero;
            //rb.AddForce(dir * knockbackForce, ForceMode2D.Impulse);
            //yield return new WaitForSeconds(knockbackTime);

            var prevVel = rb.linearVelocity;
            rb.linearVelocity = Vector2.zero;

            rb.AddForce(dir * knockbackForce, ForceMode2D.Impulse);
            yield return new WaitForSeconds(knockbackTime);

            rb.linearVelocity = prevVel; // 원래 속도로 복원

        }
        else
        {
            Vector3 start = transform.position;
            Vector3 end = start + (Vector3)(dir * 0.3f);
            float t = 0f;
            while (t < knockbackTime)
            {
                t += Time.deltaTime;
                transform.position = Vector3.Lerp(start, end, t / knockbackTime);
                yield return null;
            }
        }
    }

    private void ApplyColor(Color c)
    {
        if (!sr) return;
        sr.GetPropertyBlock(mpb);
        mpb.SetColor("_Color", c);
        sr.SetPropertyBlock(mpb);
    }

    private IEnumerator HitStop(float duration, float slowScale = 0f)
    {
        float original = Time.timeScale;
        Time.timeScale = slowScale; // 0 = 정지, 0.05 = 초슬로우
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = original;
    }

    private void StopCoroutineSafe(IEnumerator co) { /* 자리표시자: 필요시 구현 */ }
}
