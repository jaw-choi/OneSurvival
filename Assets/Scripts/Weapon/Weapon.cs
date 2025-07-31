using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponData weaponData { get; private set; }

    private IWeaponFireBehaviour fireBehaviour;
    private int currentLevel;
    private float lastFireTime;

    public void Initialize(WeaponData data, int startLevel)
    {
        weaponData = data;
        currentLevel = startLevel;
        SetFireBehaviourByType(weaponData.fireType);
        lastFireTime = -999f; // ó���� ��� �߻� �����ϵ���
    }

    public bool CanFire()
    {
        return Time.time >= lastFireTime + weaponData.attackCooldown;
    }

    public void Fire(Vector2 direction)
    {
        if (fireBehaviour != null && CanFire())
        {
            fireBehaviour.Fire(transform.position, direction, weaponData);
            lastFireTime = Time.time;
        }
    }

    public void Upgrade()
    {
        currentLevel++;
        Debug.Log($"{weaponData.weaponName} ���� ������ �� Lv.{currentLevel}");
    }

    private void SetFireBehaviourByType(WeaponFireType fireType)
    {
        switch (fireType)
        {
            case WeaponFireType.Burst:
                fireBehaviour = new BurstFireBehaviour(this);
                break;
            //case WeaponFireType.Single:
            //    fireBehaviour = new SingleShotFireBehaviour(this);
            //    break;
            //case WeaponFireType.Area:
            //    fireBehaviour = new AreaFireBehaviour(this);
            //    break;
            //// �ʿ��� Ÿ�� ��� �߰�
            default:
                fireBehaviour = new BurstFireBehaviour(this);
                break;
        }
    }
}
