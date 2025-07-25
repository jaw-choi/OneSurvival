using UnityEngine;

public class ExpGem : MonoBehaviour
{
    public int expAmount = 10;
    public float absorbRange = 2f;         // ��� ���� �Ÿ�
    public float absorbSpeed = 5f;         // ��� �̵� �ӵ�

    private Transform target;              // �÷��̾� ��ġ
    private bool isAbsorbing = false;

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
                PlayerExpManager exp = target.GetComponent<PlayerExpManager>();
                if (exp != null)
                    exp.AddExp(expAmount);

                Destroy(gameObject);
            }
        }
    }
}
