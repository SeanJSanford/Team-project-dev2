using UnityEngine;

/*
 * WeaponStats
 * 
 * Purpose:
 * Calculates the player's final runtime weapon values
 * by combining weapon data with player stat modifiers.
 * 
 * Why This Exists:
 * WeaponData stores base weapon values,
 * while PlayerStats stores player progression modifiers.
 * 
 * This script combines both systems together
 * so combat systems can request finalized values.
 * 
 * Example:
 * Final Damage =
 * Weapon Damage + Player Base Damage
 * 
 * Final Fire Rate =
 * Weapon Fire Rate modified by FireRateModifier
 * 
 * Connected Systems:
 * - WeaponData
 * - PlayerStats
 * - Shooting systems
 * - Enemy combat systems
 * - Future perk/card systems
 * - Future loot progression systems
 * 
 * Design Notes:
 * This script ONLY handles stat calculations.
 * 
 * It should NOT:
 * - Fire weapons
 * - Spawn projectiles
 * - Handle recoil
 * - Handle hit detection
 * - Handle UI
 * 
 * Those systems should request values from here instead.
 * 
 * Future Expansion Ideas:
 * - Crit chance
 * - Crit damage
 * - Projectile modifiers
 * - Elemental scaling
 * - Weapon rarity
 * - Perk modifiers
 * - Corruption scaling
 * - Multi-weapon support
 */

public class WeaponStats : MonoBehaviour
{
    [Header("References")]

    // Current equipped weapon definition.
    [SerializeField] private WeaponData currentWeapon;

    // Reference to the player's stat system.
    [SerializeField] private PlayerStats playerStats;

    /*
     * Public Read-Only Property
     * 
     * Allows other systems to safely read
     * the currently equipped weapon.
     */
    public WeaponData CurrentWeapon => currentWeapon;

    private void Start()
    {
        // Automatically attempts to find PlayerStats
        // on the same GameObject if no reference was assigned.
        if (playerStats == null)
        {
            playerStats = GetComponent<PlayerStats>();
        }
    }

    /*
     * GetTotalDamage
     * 
     * Returns the final runtime damage value.
     * 
     * Formula:
     * Weapon Damage + Player Base Damage
     * 
     * If no weapon exists:
     * Uses player base damage as fallback.
     */
    public int GetTotalDamage()
    {
        // Fallback behavior if no weapon is equipped.
        if (currentWeapon == null)
        {
            return playerStats != null ? playerStats.BaseDamage : 1;
        }

        // Read player stat modifiers safely.
        int playerDamage =
            playerStats != null ? playerStats.BaseDamage : 0;

        // Final damage calculation.
        return playerDamage + currentWeapon.damage;
    }

    /*
     * GetFireRate
     * 
     * Returns the final attack speed value.
     * 
     * Formula:
     * Weapon Fire Rate / Player Fire Rate Modifier
     * 
     * Higher modifiers result in faster attacks.
     */
    public float GetFireRate()
    {
        // Fallback fire rate if no weapon exists.
        if (currentWeapon == null)
            return 0.5f;

        // Read fire rate modifiers from PlayerStats.
        float modifier =
            playerStats != null ? playerStats.FireRateModifier : 1f;

        // Final fire rate calculation.
        return currentWeapon.fireRate / modifier;
    }

    /*
     * GetRange
     * 
     * Returns the weapon's effective attack range.
     * 
     * Currently uses the weapon's base range directly.
     * 
     * Future systems may modify this value through:
     * - Perks
     * - Character archetypes
     * - Buffs/debuffs
     * - Weapon upgrades
     */
    public float GetRange()
    {
        // Fallback range if no weapon exists.
        if (currentWeapon == null)
            return 10f;

        return currentWeapon.range;
    }

    /*
     * EquipWeapon
     * 
     * Sets a new weapon as the player's active weapon.
     * 
     * Future versions may also:
     * - Update UI
     * - Swap weapon models
     * - Trigger animations
     * - Play sounds
     * - Refresh stat displays
     */
    public void EquipWeapon(WeaponData newWeapon)
    {
        currentWeapon = newWeapon;

        Debug.Log("Equipped weapon: " + newWeapon.weaponName);
    }
}