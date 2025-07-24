using System.Collections;
using UnityEngine;

public class BurstFireBehaviour : IWeaponFireBehaviour
{
    private MonoBehaviour coroutineOwner;
    private Transform ownerTransform;

    public BurstFireBehaviour(MonoBehaviour owner)
    {
        this.coroutineOwner = owner;
        ownerTransform = owner.transform;
    }

    public void Fire(Vector2 position, Vector2 direction, WeaponData data)
    {
        coroutineOwner.StartCoroutine(FireBurst(direction, data));
    }

    private IEnumerator FireBurst(Vector2 direction, WeaponData data)
    {
        for (int i = 0; i < data.burstCount; i++)
        {
            Vector2 currentPosition = ownerTransform.position;

            GameObject proj = Object.Instantiate(data.projectilePrefab, currentPosition, Quaternion.identity);
            proj.GetComponent<Projectile>().Init(direction.normalized * data.projectileSpeed);
            Debug.Log($"[{Time.time:F2}] น฿ป็ {i + 1} / {data.burstCount}");
            yield return new WaitForSeconds(data.timeBetweenBurstShots);
        }
    }

}
