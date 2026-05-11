
public enum StatType
{
    // =========================
    // SURVIVABILITY
    // =========================

    // Maximum player health capacity.
    MaxHealth,

    // Health regenerated over time.
    HealthRegen,

    // Flat damage reduction.
    Armor,

    // =========================
    // OFFENSIVE STATS
    // =========================

    // Base outgoing damage modifier.
    Damage,

    // Chance for attacks to critically strike.
    CritChance,

    // Multiplier applied to critical hits.
    CritDamage,

    // Attack speed modifier.
    // Higher values = faster attacks.
    AttackRate,

    // =========================
    // PROJECTILE STATS
    // =========================

    // Additional projectiles fired.
    ProjectileAmount,

    // Projectile scale modifier.
    ProjectileSize,

    // Projectile travel distance/range modifier.
    ProjectileRange,

    // =========================
    // AREA EFFECT STATS
    // =========================

    // Additional area damage scaling.
    AoeDamage,

    // Area radius modifier.
    AoeSize,

    // =========================
    // ELEMENTAL STATS
    // =========================

    // General elemental damage modifier.
    ElementalDamage,

    // Resistance against elemental damage/effects.
    ElementalResistance,

    // Chance to apply elemental effects.
    ElementalProcChance,

    // =========================
    // MOVEMENT STATS
    // =========================

    // Movement speed modifier.
    MoveSpeed,

    // Number of available dashes.
    DashAmount,

    // =========================
    // LOOT / ECONOMY STATS
    // =========================

    // Increased chance for enemies to drop items.
    ItemDropChance,

    // Increased chance for higher rarity drops.
    ItemDropRarity,

    // Increased chance for gold/currency drops.
    GoldDropChance,

    // =========================
    // GAME SCALING
    // =========================

    // Global difficulty/corruption scaling value.
    CorruptionLevel
}

/*
========================================================
Project: Team Code Sick
Script: StatType.cs

Primary Developer:
- Avery Wilson

System Category:
- Gameplay Stat System
- Shared Stat Definitions
- Runtime Scaling Architecture

Purpose:
- Defines all gameplay stats used throughout
  the runtime stat framework.
- Provides a centralized list of scalable
  gameplay attributes shared between systems.

Core Responsibilities:
- Standardize gameplay stat references
- Prevent hardcoded stat strings
- Support modular stat calculations
- Allow scalable gameplay expansion
- Unify gameplay systems under one stat structure

Connected Team Systems:
- Avery: PlayerStats / WeaponStats integration
- Dai: Movement speed integration
- Sean: Weapon/combat balancing and enemy combat interactions
- Heather: Future item/equipment bonuses
- Nilo: Gameplay progression oversight

Why This Exists:
Instead of hardcoding separate variables and upgrade
paths for every gameplay system, StatType provides
a centralized enum used by:

- PlayerStats
- WeaponStats
- StatModifier
- Loot systems
- Equipment systems
- Perk systems
- Buff/debuff systems
- Future progression systems

This allows gameplay systems to interact using
shared stat identifiers.

Example:
GetStatValue(StatType.Damage)

rather than:
GetDamage()
GetWeaponDamage()
GetBonusDamage()
etc.

Design Philosophy:
This system is intentionally centralized and scalable.

Benefits:
- Reduces duplicate logic
- Simplifies balancing
- Supports future expansion
- Keeps gameplay systems modular
- Improves readability and maintainability

Current Supported Categories:
- Survivability
- Offensive stats
- Projectile stats
- Area-effect stats
- Elemental stats
- Movement stats
- Loot/economy stats
- Game scaling stats

Development Notes:
- Designed as the foundation for the modifier system.
- Intended to support future RPG/progression mechanics.
- Allows gameplay systems to scale without creating
  hardcoded upgrade methods.
- Built to act as the shared language between:
    - Combat systems
    - Movement systems
    - Loot systems
    - Progression systems

Future Expansion Ideas:
- Status effect stats
- Cooldown reduction
- Lifesteal
- Shield systems
- Summon stats
- Resource generation
- Corruption scaling
- Multiplayer scaling stats
========================================================
*/