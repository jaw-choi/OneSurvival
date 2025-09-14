using UnityEngine;

public class GarlicWeapon : MonoBehaviour
{
    private Weapon weapon;

    private SpriteRenderer visual;

    void Awake()
    {
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
