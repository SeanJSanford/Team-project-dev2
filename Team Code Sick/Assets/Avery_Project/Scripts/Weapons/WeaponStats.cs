using UnityEngine;

public class WeaponStats : MonoBehaviour
{
    [SerializeField] private WeaponData currentWeapon;
    [SerializeField] private PlayerStats playerStats;

    public WeaponData CurrentWeapon => currentWeapon;

    private void Start()
    {
        if (playerStats == null)
        {
            playerStats = GetComponent<PlayerStats>();
        }
    }

    public int GetTotalDamage()
    {
        if (currentWeapon == null)
        {
            return playerStats != null ? playerStats.BaseDamage : 1;
        }

        int playerDamage = playerStats != null ? playerStats.BaseDamage : 0;
        return playerDamage + currentWeapon.damage;
    }

    public float GetFireRate()
    {
        if (currentWeapon == null)
            return 0.5f;

        float modifier = playerStats != null ? playerStats.FireRateModifier : 1f;
        return currentWeapon.fireRate / modifier;
    }

    public float GetRange()
    {
        if (currentWeapon == null)
            return 10f;

        return currentWeapon.range;
    }

    public void EquipWeapon(WeaponData newWeapon)
    {
        currentWeapon = newWeapon;
        Debug.Log("Equipped weapon: " + newWeapon.weaponName);
    }
}