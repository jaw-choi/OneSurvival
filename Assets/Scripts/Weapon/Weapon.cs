using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponData weaponData;
    private IWeaponFireBehaviour fireBehaviour;
    //private float cooldownTimer;

    void Awake()
    {
        // 나중에 무기 종류에 따라 동적으로 변경 가능
        fireBehaviour = new BurstFireBehaviour(this); // 또는 SingleShotBehaviour 등
        //fireBehaviour = new SingleShotBehaviour(this); // 또는 SingleShotBehaviour 등
    }
    //void Update()
    //{
    //    cooldownTimer -= Time.deltaTime;
    //    if (cooldownTimer <= 0f)
    //    {
    //        Fire();
    //        cooldownTimer = weaponData.attackCooldown;
    //    }
    //}

    public void Fire(Vector2 direction)
    {
        fireBehaviour.Fire(transform.position, direction, weaponData);
        //if (weaponData.projectilePrefab != null)
        //{
        //    GameObject projectile = Instantiate(
        //        weaponData.projectilePrefab,
        //        transform.position,
        //        transform.rotation
        //    );

        //    Projectile projScript = projectile.GetComponent<Projectile>();
        //    if (projScript != null)
        //    {
        //        projScript.SetData(weaponData.damage, weaponData.pierceCount);
        //    }
        //}
    }
    public void SetFireBehaviour(IWeaponFireBehaviour behaviour)
    {
        fireBehaviour = behaviour;
    }
}
