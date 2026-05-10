using UnityEngine;

/*
 * IntegrationWeaponStats
 * 
 * Purpose:
 * Combines weapon data with player stat modifiers
 * to calculate final combat values during gameplay.
 * 
 * How It Works:
 * - Reads base values from WeaponData
 * - Reads modifiers from IntegrationPlayerStats
 * - Returns final runtime combat values
 * 
 * Example:
 * Weapon Damage + Player Damage Bonus
 * Weapon Fire Rate modified by FireRateModifier
 * 
 * Why This Exists:
 * WeaponData should store STATIC weapon values.
 * PlayerStats should store PLAYER modifiers.
 * 
 * This class acts as the bridge between both systems
 * and calculates the final usable combat stats.
 * 
 * Connected Systems:
 * - IntegrationPlayerStats
 * - WeaponData
 * - Shooting systems
 * - Enemy combat systems
 * - Future perk systems
 * - Future elemental systems
 * 
 * Design Notes:
 * This script only CALCULATES weapon values.
 * 
 * It should NOT:
 * - Handle shooting logic
 * - Spawn projectiles
 * - Handle hit detection
 * - Handle UI
 * 
 * Those systems should request values from this script.
 * 
 * Example Flow:
 * Shooting System ->
 * GetTotalDamage() ->
 * Apply damage to target
 * 
 * Future Expansion Ideas:
 * - Crit chance
 * - Crit damage
 * - Elemental scaling
 * - Projectile modifiers
 * - Attack speed caps
 * - Weapon rarity scaling
 * - Perk/card interactions
 * - Corruption modifiers
 */

public class IntegrationWeaponStats : MonoBehaviour
{
    [Header("References")]

    // Current equipped weapon definition.
    [SerializeField] private WeaponData currentWeapon;

    // Reference to player stat modifiers.
    [SerializeField] private IntegrationPlayerStats playerStats;

    /*
     * Public Read-Only Property
     * 
     * Allows other systems to safely read
     * the currently equipped weapon.
     */
    public WeaponData CurrentWeapon => currentWeapon;

    private void Start()
    {
        // Automatically attempts to find the player's stat system
        // if one was not assigned manually.
        if (playerStats == null)
        {
            playerStats = GetComponent<IntegrationPlayerStats>();
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
     * If no weapon is equipped:
     * Uses player base damage as fallback.
     */
    public int GetTotalDamage()
    {
        // Fallback behavior if no weapon exists.
        if (currentWeapon == null)
        {
            return playerStats != null ? playerStats.BaseDamage : 1;
        }

        int playerDamage =
            playerStats != null ? playerStats.BaseDamage : 0;

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

        float modifier =
            playerStats != null ? playerStats.FireRateModifier : 1f;

        return currentWeapon.fireRate / modifier;
    }

    /*
     * GetRange
     * 
     * Returns the weapon's effective attack range.
     * 
     * Currently uses weapon data directly.
     * 
     * Future systems may modify this through:
     * - Perks
     * - Character archetypes
     * - Temporary buffs
     * - Weapon upgrades
     */
    public float GetRange()
    {
        if (currentWeapon == null)
            return 10f;

        return currentWeapon.range;
    }

    /*
     * EquipWeapon
     * 
     * Assigns a new weapon definition
     * as the player's currently equipped weapon.
     * 
     * Future systems may expand this to:
     * - Trigger animations
     * - Refresh UI
     * - Swap weapon models
     * - Trigger sound effects
     */
    public void EquipWeapon(WeaponData newWeapon)
    {
        currentWeapon = newWeapon;

        Debug.Log("Equipped weapon: " + newWeapon.weaponName);
    }
}