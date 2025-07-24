using UnityEngine;

public interface IWeaponFireBehaviour
{
    void Fire(Vector2 position, Vector2 direction, WeaponData data);
}
