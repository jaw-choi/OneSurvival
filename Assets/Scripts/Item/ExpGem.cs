using UnityEngine;

public class ExpGem : MonoBehaviour
{
    public int expAmount = 1;
    public float absorbRange = 2f;         // 흡수 시작 거리
    public float absorbSpeed = 5f;         // 흡수 이동 속도

    private Transform target;              // 플레이어 위치
    private bool isAbsorbing = false;

    public void SetExp(int amount)
    {
        expAmount = amount;
    }
    void Start()
    {
        target = GameManager.Instance.PlayerTransform;
    }

    void Update()
    {
        if (target == null) return;

        float dist = Vector2.Distance(transform.position, target.position);

        if (isAbsorbing || dist <= absorbRange)
        {
            isAbsorbing = true;

            // 플레이어 방향으로 이동
            Vector2 dir = (target.position - transform.position).normalized;
            transform.position += (Vector3)(dir * absorbSpeed * Time.deltaTime);

            // 가까워지면 흡수
            if (dist < 0.2f)
            {
                if (PlayerExpManager.Instance != null)
                    PlayerExpManager.Instance.AddExp(expAmount);

                Destroy(gameObject);
            }
        }
    }
}
