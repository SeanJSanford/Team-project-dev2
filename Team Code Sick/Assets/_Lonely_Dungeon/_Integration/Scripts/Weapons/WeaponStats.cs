using UnityEngine;

/*
 * WeaponStats
 * 
 * Purpose:
 * Calculates the player's final runtime weapon values
 * by combining equipped weapon data with the modifier-based PlayerStats system.
 * 
 * Why This Exists:
 * WeaponData stores static weapon values such as base damage,
 * base fire rate, and base range.
 * 
 * PlayerStats stores runtime stat scaling through StatType and StatModifier.
 * This script combines both sources so combat systems can request final values.
 * 
 * Current Formulas:
 * Final Damage = Weapon Damage + Player Damage Stat
 * Final Fire Rate = Weapon Fire Rate / Player AttackRate Stat
 * Final Range = Weapon Range + Player ProjectileRange Stat
 * 
 * Connected Systems:
 * - WeaponData
 * - PlayerStats
 * - StatType
 * - StatModifier
 * - Shooting systems
 * - Enemy damage systems
 * - Future perk/card systems
 * - Future loot progression systems
 * 
 * Design Notes:
 * This script only calculates weapon-related stat values.
 * 
 * It should NOT:
 * - Fire weapons
 * - Spawn projectiles
 * - Handle recoil
 * - Handle hit detection
 * - Handle UI
 * 
 * Those systems should request values from here instead.
 */

public class WeaponStats : MonoBehaviour
{
    [Header("References")]

    // Current equipped weapon definition.
    [SerializeField] private WeaponData currentWeapon;

    // Player stat system used for runtime modifiers.
    [SerializeField] private PlayerStats playerStats;

    public WeaponData CurrentWeapon => currentWeapon;

    private void Start()
    {
        // Tries to find PlayerStats on the same GameObject
        // if it was not manually assigned in the Inspector.
        if (playerStats == null)
        {
            playerStats = GetComponent<PlayerStats>();
        }
    }

    /*
     * GetTotalDamage
     * 
     * Returns final damage using:
     * - currentWeapon.damage
     * - PlayerStats StatType.Damage
     */
    public int GetTotalDamage()
    {
        float weaponDamage = currentWeapon != null ? currentWeapon.damage : 0f;
        float playerDamage = playerStats != null ? playerStats.GetStatValue(StatType.Damage) : 1f;

        return Mathf.RoundToInt(playerDamage + weaponDamage);
    }

    /*
     * GetFireRate
     * 
     * Returns the delay between attacks.
     * 
     * Lower returned value = faster shooting.
     * Higher AttackRate stat = faster shooting.
     */
    public float GetFireRate()
    {
        if (currentWeapon == null)
            return 0.5f;

        float attackRate = playerStats != null
            ? playerStats.GetStatValue(StatType.AttackRate)
            : 1f;

        return currentWeapon.fireRate / attackRate;
    }

    /*
     * GetRange
     * 
     * Returns final range using:
     * - currentWeapon.range
     * - PlayerStats StatType.ProjectileRange
     */
    public float GetRange()
    {
        float weaponRange = currentWeapon != null ? currentWeapon.range : 10f;
        float rangeModifier = playerStats != null
            ? playerStats.GetStatValue(StatType.ProjectileRange)
            : 0f;

        return weaponRange + rangeModifier;
    }

    /*
     * EquipWeapon
     * 
     * Sets a new weapon as the player's active weapon.
     */
    public void EquipWeapon(WeaponData newWeapon)
    {
        currentWeapon = newWeapon;

        if (newWeapon != null)
        {
            Debug.Log("Equipped weapon: " + newWeapon.weaponName);
        }
    }
}