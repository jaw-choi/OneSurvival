using UnityEngine;

public static class PermanentStatManager
{
    public static int GetLevel(PermanentStatType stat)
    {
        return PlayerPrefs.GetInt(stat.ToString(), 0);
    }

    public static void SetLevel(PermanentStatType stat, int level)
    {
        PlayerPrefs.SetInt(stat.ToString(), level);
        PlayerPrefs.Save();
    }

    public static float GetValue(PermanentStatType stat)
    {
        int level = GetLevel(stat);

        switch (stat)
        {
            case PermanentStatType.Damage: return level * 0.05f; // 5 level
            case PermanentStatType.MoveSpeed: return level * 0.05f;// 2 level
            case PermanentStatType.ProjectileSpeed: return level * 0.1f;// 2 level
            case PermanentStatType.ExpBonus: return level * 0.03f;// 5 level
            case PermanentStatType.GoldBonus: return level * 0.1f;// 5 level
            case PermanentStatType.MaxHP: return level * 0.1f;// 5 level
            case PermanentStatType.Regen: return level * 0.1f;// 5 level
            default: return 0f;
        }
    }
}
