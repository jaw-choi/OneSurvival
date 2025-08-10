using UnityEngine;

public class WeaponDatabaseLoader : MonoBehaviour
{
    public static WeaponDatabase Instance { get; private set; }

    void Awake()
    {
        Instance = Resources.Load<WeaponDatabase>("Weapon/WeaponDatabase");

        if (Instance == null)
        {
            // 폴더가 달라졌을 수 있어 전체 검색 시도
            var all = Resources.LoadAll<WeaponDatabase>("");
            if (all != null && all.Length > 0)
            {
                Instance = all[0];
                Debug.LogWarning($"WeaponDatabase를 기본 경로에서 못 찾음 : {Instance.name}");
            }
        }

        if (Instance == null)
            Debug.LogError("WeaponDatabase.asset이 Resources 하위에 없습니다. ");
    }

}

