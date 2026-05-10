using UnityEngine;

/*
 * WeaponData
 * 
 * Purpose:
 * ScriptableObject used to define weapon stats
 * outside of runtime gameplay code.
 * 
 * Why Use ScriptableObjects:
 * This allows weapons to be created directly
 * in the Unity Editor without hardcoding
 * separate weapon scripts for every gun.
 * 
 * Example Weapon Assets:
 * - SO_BasicPistol
 * - SO_Shotgun
 * - SO_SniperRifle
 * - SO_FireWand
 * 
 * Connected Systems:
 * - IntegrationWeaponStats
 * - Shooting systems
 * - Inventory/equipment systems
 * - Future loot systems
 * - Future rarity systems
 * 
 * Design Notes:
 * WeaponData stores STATIC weapon values only.
 * 
 * Runtime calculations should happen in:
 * - IntegrationWeaponStats
 * - Perk systems
 * - Character stat systems
 * 
 * This separation keeps weapon balancing
 * easier and prevents gameplay logic
 * from being hardcoded into assets.
 * 
 * Example:
 * WeaponData:
 * - Damage = 10
 * 
 * IntegrationWeaponStats:
 * - Final Damage = Weapon Damage + Player Damage
 * 
 * Future Expansion Ideas:
 * - Crit chance
 * - Crit damage
 * - Projectile count
 * - Projectile spread
 * - Reload speed
 * - Ammo systems
 * - Elemental damage
 * - Weapon rarity
 * - Upgrade scaling
 * - Recoil
 * - Knockback
 * - Area damage
 */

// Adds a weapon creation option inside Unity's Create menu.
[CreateAssetMenu(
    fileName = "SO_NewWeapon",
    menuName = "Integration/Weapons/Weapon Data"
)]

public class WeaponData : ScriptableObject
{
    [Header("Weapon Info")]

    // Name displayed in UI/inventory systems.
    public string weaponName = "Basic Gun";

    [Header("Base Weapon Stats")]

    // Base damage added into final runtime calculations.
    public int damage = 10;

    // Time between attacks.
    // Lower values = faster weapons.
    public float fireRate = 0.25f;

    // Maximum effective attack range.
    public float range = 20f;
}