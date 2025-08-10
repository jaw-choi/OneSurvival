using UnityEngine;

public class WeaponDatabaseLoader : MonoBehaviour
{
    public static WeaponDatabase Instance { get; private set; }

    void Awake()
    {
        Instance = Resources.Load<WeaponDatabase>("Weapon/WeaponDatabase");

        if (Instance == null)
        {
            // ������ �޶����� �� �־� ��ü �˻� �õ�
            var all = Resources.LoadAll<WeaponDatabase>("");
            if (all != null && all.Length > 0)
            {
                Instance = all[0];
                Debug.LogWarning($"WeaponDatabase�� �⺻ ��ο��� �� ã�� : {Instance.name}");
            }
        }

        if (Instance == null)
            Debug.LogError("WeaponDatabase.asset�� Resources ������ �����ϴ�. ");
    }

}

