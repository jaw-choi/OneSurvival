using UnityEngine;

public class ExpGem : MonoBehaviour
{
    public int expAmount = 1;
    public float absorbRange = 2f;         // ��� ���� �Ÿ�
    public float absorbSpeed = 5f;         // ��� �̵� �ӵ�

    private Transform target;              // �÷��̾� ��ġ
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

            // �÷��̾� �������� �̵�
            Vector2 dir = (target.position - transform.position).normalized;
            transform.position += (Vector3)(dir * absorbSpeed * Time.deltaTime);

            // ��������� ���
            if (dist < 0.2f)
            {
                if (PlayerExpManager.Instance != null)
                    PlayerExpManager.Instance.AddExp(expAmount);

                Destroy(gameObject);
            }
        }
    }
}
