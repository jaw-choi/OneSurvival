using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image fillImage;
    private Transform target;
    public Vector3 offset = new Vector3(0, 1.5f, 0);

    public void SetTarget(Transform targetTransform)
    {
        target = targetTransform;
    }

    public void SetHealth(float current, float max)
    {
        fillImage.fillAmount = current / max;
    }

    void LateUpdate()
    {
        if (target != null)
            transform.position = target.position + offset;
    }
}
