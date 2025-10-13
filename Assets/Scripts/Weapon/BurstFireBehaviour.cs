using System.Collections;
using UnityEngine;

public class BurstFireBehaviour : IWeaponFireBehaviour
{
    private MonoBehaviour coroutineOwner;
    private Transform ownerTransform;
    private Weapon weapon;

    public BurstFireBehaviour(Weapon weapon)
    {
        this.weapon = weapon;
        coroutineOwner = weapon;
        ownerTransform = weapon.transform;
    }

    public void Fire(Vector2 position, Vector2 direction, WeaponData data)
    {
        coroutineOwner.StartCoroutine(FireBurst(direction));
        Vector2 center = ownerTransform.position;
        if (data.projectileData.hitEffectPrefab != null)
            Object.Instantiate(data.projectileData.hitEffectPrefab, center, Quaternion.identity);
    }

    private IEnumerator FireBurst(Vector2 direction)
    {
        for (int i = 0; i < weapon.weaponData.burstCount; i++)
        {
            Vector2 currentPosition = ownerTransform.position;

            GameObject proj = Object.Instantiate(weapon.weaponData.projectilePrefab, currentPosition, Quaternion.identity);

            Projectile projectile = proj.GetComponent<Projectile>();
            projectile.SetData(weapon.weaponData.projectileData, weapon); // 강화 수치 적용
            projectile.Init(direction.normalized); // 방향만 전달

            Debug.Log($"[{Time.time:F2}] 발사 {i + 1} / {weapon.weaponData.burstCount}");
            yield return new WaitForSeconds(weapon.weaponData.timeBetweenBurstShots);
        }

    }

}
