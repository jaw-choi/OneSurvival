using UnityEngine;

public class GarlicWeapon : MonoBehaviour
{
    private Weapon weapon;
    private Transform player;
    private float tickTimer;
    private float tickInterval = 0.5f; // 반초마다 데미지

    private SpriteRenderer visual;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        weapon = GetComponent<Weapon>();
        visual = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        if (weapon == null || weapon.weaponData == null)
            return;

        UpdateVisualRadius();
    }


    void UpdateVisualRadius()
    {
        float radius = weapon.weaponData.projectileData.aoeRadius * weapon.GetSpeedMultiplier();
        if (visual != null)
        {
            float size = radius * 2f;
            visual.transform.localScale = new Vector3(size, size, 1f);
        }
    }
}
