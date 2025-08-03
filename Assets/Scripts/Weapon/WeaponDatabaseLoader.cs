using UnityEngine;

public class WeaponDatabaseLoader : MonoBehaviour
{
    public static WeaponDatabase Instance { get; private set; }

    void Awake()
    {
        Instance = Resources.Load<WeaponDatabase>("WeaponDatabase");
        if (Instance == null)
            Debug.LogError("WeaponDatabase.asset이 Resources 폴더에 없습니다!");
    }
}

    