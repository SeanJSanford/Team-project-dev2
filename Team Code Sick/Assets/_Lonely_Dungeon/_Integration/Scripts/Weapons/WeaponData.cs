using UnityEngine;

/*
 * WeaponData
 * 
 * Purpose:
 * ScriptableObject used to define a weapon's base stats
 * outside of runtime gameplay code.
 * 
 * Why Use ScriptableObjects:
 * ScriptableObjects allow weapons to be created and balanced
 * directly in the Unity Editor without creating separate
 * scripts for every weapon type.
 * 
 * This keeps weapon content modular and easier to scale.
 * 
 * Example Weapon Assets:
 * - SO_BasicPistol
 * - SO_Shotgun
 * - SO_SniperRifle
 * - SO_FireWand
 * 
 * Connected Systems:
 * - WeaponStats
 * - Shooting systems
 * - Inventory/equipment systems
 * - Loot systems
 * - Future perk systems
 * - Future rarity systems
 * 
 * Design Notes:
 * WeaponData stores STATIC weapon values only.
 * 
 * Runtime calculations should happen in:
 * - WeaponStats
 * - PlayerStats
 * - Perk systems
 * - Modifier systems
 * 
 * Example:
 * WeaponData:
 * - Damage = 10
 * 
 * PlayerStats:
 * - Damage Modifier = +5
 * 
 * WeaponStats:
 * - Final Damage = 15
 * 
 * This separation prevents gameplay logic
 * from being hardcoded into weapon assets.
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
 * - Projectile speed
 * - Status effect scaling
 */

// Adds a weapon asset option to Unity's Create menu.
[CreateAssetMenu(
    fileName = "SO_NewWeapon",
    menuName = "Lonely Dungeon/Weapons/Weapon Data"
)]

public class WeaponData : ScriptableObject
{
    [Header("Weapon Info")]

    // Name shown in UI and inventory systems.
    public string weaponName = "Basic Gun";

    [Header("Base Weapon Stats")]

    // Base weapon damage before player modifiers are applied.
    public int damage = 10;

    // Time between attacks.
    // Lower values = faster weapons.
    public float fireRate = 0.25f;

    // Maximum attack distance before modifiers.
    public float range = 20f;
}