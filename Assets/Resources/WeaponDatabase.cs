using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponDatabase", menuName = "Scriptable Objects/WeaponDatabase")]
public class WeaponDatabase : ScriptableObject
{
    public List<WeaponData> weaponList;

    public List<WeaponData> GetAll()
    {
        return weaponList;
    }

    public WeaponData GetWeaponByName(string name)
    {
        return weaponList.Find(w => w.weaponName == name);
    }
}
