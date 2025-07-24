using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponData weaponData;
    private IWeaponFireBehaviour fireBehaviour;
    //private float cooldownTimer;

    void Awake()
    {
        // ���߿� ���� ������ ���� �������� ���� ����
        fireBehaviour = new BurstFireBehaviour(this); // �Ǵ� SingleShotBehaviour ��
        //fireBehaviour = new SingleShotBehaviour(this); // �Ǵ� SingleShotBehaviour ��
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
