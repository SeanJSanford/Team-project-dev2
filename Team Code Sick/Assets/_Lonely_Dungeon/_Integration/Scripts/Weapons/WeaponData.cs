using UnityEngine;

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

/*
========================================================
Project: Team Code Sick
Script: WeaponData.cs

Primary Developer:
- Avery Wilson

System Category:
- Weapon System
- ScriptableObject Data Architecture
- Combat Scaling Framework

Purpose:
- Defines static weapon data outside of runtime gameplay code.
- Stores base weapon values used by combat and stat systems.

Core Responsibilities:
- Store base weapon damage
- Store weapon fire rate
- Store weapon range
- Provide reusable weapon assets
- Support scalable weapon creation workflows

Connected Team Systems:
- Avery: WeaponStats / PlayerStats integration
- Sean: Combat and shooting systems
- Heather: Future inventory/equipment systems
- Nilo: Gameplay progression oversight

Why Use ScriptableObjects:
ScriptableObjects allow weapons to be:
- Created directly in the Unity Editor
- Balanced without changing gameplay code
- Reused across multiple systems
- Expanded more efficiently

This prevents needing separate scripts
for every individual weapon type.

Example Weapon Assets:
- SO_BasicPistol
- SO_Shotgun
- SO_SniperRifle
- SO_FireWand

Design Philosophy:
WeaponData intentionally stores STATIC weapon values only.

Responsibilities intentionally excluded:
- Runtime damage calculations
- Combat logic
- Projectile spawning
- Crit calculations
- Modifier scaling
- Player stat calculations

Those systems remain separated into:
- WeaponStats
- PlayerStats
- Modifier systems
- Combat systems

Example Runtime Flow:
WeaponData:
- Base Damage = 10

PlayerStats:
- +5 Damage Modifier

WeaponStats:
- Final Damage = 15

This separation:
- Prevents hardcoded gameplay logic
- Improves scalability
- Simplifies balancing
- Keeps weapon assets modular

Development Notes:
- Built using Unity ScriptableObjects for scalability.
- Intended to support future procedural weapon systems.
- Forms the foundation for future weapon progression.
- Designed to integrate cleanly with:
    - WeaponStats
    - Inventory systems
    - Loot systems
    - Future equipment systems

Current Features:
- Weapon name
- Base damage
- Fire rate
- Attack range

Future Expansion Ideas:
- Crit chance
- Crit damage
- Projectile count
- Projectile spread
- Reload speed
- Ammo systems
- Elemental damage
- Weapon rarity
- Upgrade scaling
- Recoil
- Knockback
- Area damage
- Projectile speed
- Status effect scaling
========================================================
*/