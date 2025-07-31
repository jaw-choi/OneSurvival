using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAutoAttack : MonoBehaviour
{
    public Weapon weapon;

    public void Initialize(Weapon weapon)
    {
        //this.weapon = weapon;
        StartCoroutine(AutoFireRoutine());
    }
    private IEnumerator AutoFireRoutine()
    {
        while (true)
        {
            List<Weapon> weapons = WeaponManager.Instance.GetAllWeapons();

            foreach (var weapon in weapons)
            {
                if (weapon.weaponData == null) continue;

                GameObject nearestEnemy = FindNearestEnemy();
                if (nearestEnemy != null)
                {
                    Vector2 dir = (nearestEnemy.transform.position - transform.position).normalized;
                    weapon.Fire(dir);
                }
            }

            yield return null; // �� �����Ӹ��� �ݺ�
        }
    }


    GameObject FindNearestEnemy()
    {
        // ���� ����. ������ EnemyManager�� �� ����Ʈ ����
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float minDist = float.MaxValue;
        GameObject nearest = null;
        foreach (var enemy in enemies)
        {
            float dist = Vector2.Distance(transform.position, enemy.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = enemy;
            }
        }
        return nearest;
    }
}
