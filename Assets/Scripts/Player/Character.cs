using UnityEngine;

public class Character : MonoBehaviour
{
    public static float Speed
    {
        get { return PlayerStats.Instance.playerID == 0 ? 1.1f : 1f; }
    }
    public static float WeaponDamage
    {
        get { return PlayerStats.Instance.playerID == 1 ? 1.1f : 1f; }
    }

    public static float MaxHP
    {
        get { return PlayerStats.Instance.playerID == 2 ? 1.1f : 1f; }
    }
    public static float RegenHP
    {
        get { return PlayerStats.Instance.playerID == 3 ? 1.1f : 1f; }
    }
}
