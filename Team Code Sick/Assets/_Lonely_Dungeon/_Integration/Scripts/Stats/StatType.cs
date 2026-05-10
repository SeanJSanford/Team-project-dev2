/*
 * StatType
 * 
 * Purpose:
 * Defines every supported runtime stat used by the game's
 * modifier-based stat system.
 * 
 * Why This Exists:
 * Instead of hardcoding separate variables and upgrade methods
 * for every possible stat, the game uses a shared stat enum
 * combined with StatModifier objects.
 * 
 * This allows systems such as:
 * - Perks
 * - Loot
 * - Character archetypes
 * - Curses
 * - Economy upgrades
 * - Buffs/debuffs
 * 
 * to modify stats dynamically through a single system.
 * 
 * Example:
 * playerStats.AddModifier(
 *     new StatModifier(
 *         StatType.Damage,
 *         StatModifierType.Flat,
 *         5f
 *     )
 * );
 * 
 * Connected Systems:
 * - PlayerStats
 * - WeaponStats
 * - StatModifier
 * - Perk systems
 * - Loot systems
 * - Corruption scaling
 * - Future status effect systems
 * 
 * Design Notes:
 * These are runtime gameplay stats,
 * NOT UI values or animation variables.
 * 
 * New stats should only be added here if:
 * - They affect gameplay systems
 * - They can be modified dynamically
 * - Multiple systems may interact with them
 * 
 * Future Expansion Ideas:
 * - CooldownReduction
 * - Lifesteal
 * - DodgeChance
 * - KnockbackResistance
 * - ProjectileSpeed
 * - StatusEffectDuration
 * - ExperienceGain
 * - Luck
 */

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