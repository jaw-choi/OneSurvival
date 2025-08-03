using UnityEngine;

public class WeaponDatabaseLoader : MonoBehaviour
{
    public static WeaponDatabase Instance { get; private set; }

    void Awake()
    {
        Instance = Resources.Load<WeaponDatabase>("WeaponDatabase");
        if (Instance == null)
            Debug.LogError("WeaponDatabase.asset�� Resources ������ �����ϴ�!");
    }
}

    