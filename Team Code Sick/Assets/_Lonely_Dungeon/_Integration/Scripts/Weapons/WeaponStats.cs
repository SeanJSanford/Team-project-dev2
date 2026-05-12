using UnityEngine;

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

/*
========================================================
Project: Team Code Sick
Script: WeaponStats.cs

Primary Developer:
- Avery Wilson

System Category:
- Weapon System
- Runtime Combat Scaling
- Gameplay Systems Integration

Purpose:
- Calculates final runtime weapon values by combining:
    - WeaponData base stats
    - PlayerStats runtime modifiers

Core Responsibilities:
- Calculate final damage values
- Calculate final fire rate values
- Calculate final weapon range
- Handle equipped weapon references
- Bridge weapon data with runtime stat scaling

Connected Team Systems:
- Avery: PlayerStats / modifier architecture
- Sean: Shooting/combat systems
- Dai: Player movement/combat integration
- Heather: Future inventory/equipment systems
- Nilo: Gameplay progression oversight

Why This Exists:
WeaponData stores STATIC weapon values.

PlayerStats stores RUNTIME scaling values.

WeaponStats combines both systems so combat
systems can retrieve final gameplay-ready values.

This separation keeps:
- Weapon assets modular
- Runtime scaling flexible
- Combat systems clean
- Gameplay balancing centralized

Current Runtime Formulas:

Final Damage:
Weapon Damage + Player Damage Stat

Final Fire Rate:
Weapon Fire Rate / Player AttackRate Stat

Final Range:
Weapon Range + ProjectileRange Modifier

Example:
WeaponData:
- Damage = 10

PlayerStats:
- +5 Damage Modifier

WeaponStats:
- Final Damage = 15

Design Philosophy:
This script intentionally handles ONLY
weapon-related runtime calculations.

Responsibilities intentionally excluded:
- Projectile spawning
- Shooting input
- Hit detection
- Recoil handling
- Combat visuals
- UI updates

Those systems should request final values
from WeaponStats instead.

Why This Separation Exists:
Separating runtime calculations from
combat execution systems:
- Simplifies balancing
- Improves scalability
- Prevents duplicated calculations
- Supports future progression systems

Development Notes:
- Built as the runtime bridge between:
    - WeaponData
    - PlayerStats
    - Combat systems

- Designed to support future:
    - Perks
    - Loot scaling
    - Weapon rarity
    - Upgrade systems
    - Procedural weapon systems

- Intended to integrate cleanly with:
    - PlayerCombat
    - Inventory systems
    - Future equipment systems
    - Modifier frameworks

Current Features:
- Runtime damage calculation
- Runtime fire rate scaling
- Runtime range scaling
- Equipped weapon swapping
- Modifier-based scaling integration

Future Expansion Ideas:
- Crit calculations
- Reload systems
- Ammo systems
- Projectile spread
- Recoil handling
- Elemental scaling
- Procedural modifiers
- Weapon evolution systems
- Alternate fire modes
========================================================
*/