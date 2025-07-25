public class WeaponInstance
{
    public WeaponData weaponData;
    public int currentLevel;

    public float GetCurrentDamage()
    {
        return weaponData.projectileData.damage * weaponData.damageMultiplierPerLevel[currentLevel];
    }

    public float GetCurrentSpeed()
    {
        return weaponData.projectileData.speed * weaponData.speedMultiplierPerLevel[currentLevel];
    }

    public int GetCurrentPierce()
    {
        return weaponData.projectileData.pierceCount + (int)weaponData.pierceBonusPerLevel[currentLevel];
    }
}
