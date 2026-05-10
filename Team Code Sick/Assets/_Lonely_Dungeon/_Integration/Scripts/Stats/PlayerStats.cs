using System.Collections.Generic;
using UnityEngine;

/*
 * PlayerStats
 * 
 * Purpose:
 * Central runtime stat system for the player.
 * 
 * This system stores:
 * - Base stats
 * - Runtime stat modifiers
 * - Health handling
 * - Damage handling
 * - Regeneration
 * 
 * Why This Exists:
 * Instead of hardcoding separate upgrade methods for every stat,
 * PlayerStats uses:
 * - StatType
 * - StatModifier
 * - StatModifierType
 * 
 * to create a scalable modifier-based stat architecture.
 * 
 * This allows systems such as:
 * - Weapons
 * - Perks
 * - Loot
 * - Character archetypes
 * - Curses
 * - Buffs/debuffs
 * - Economy upgrades
 * 
 * to all interact with the same stat system.
 * 
 * Connected Systems:
 * - WeaponStats
 * - PlayerHealthUI
 * - StatModifier
 * - StatType
 * - Enemy combat systems
 * - Future perk systems
 * - Future corruption systems
 * 
 * Design Notes:
 * This script manages PLAYER RUNTIME STATS ONLY.
 * 
 * It should NOT:
 * - Handle movement logic
 * - Fire weapons
 * - Manage UI
 * - Handle inventory systems
 * - Spawn effects
 * 
 * Other systems should READ stat values from here.
 * 
 * Example:
 * WeaponStats requests:
 * - Damage
 * - AttackRate
 * - ProjectileRange
 * 
 * Future Expansion Ideas:
 * - Lifesteal
 * - Dodge chance
 * - Cooldown reduction
 * - Status resistances
 * - Buff/debuff tracking
 * - Event callbacks for UI updates
 */

public class PlayerStats : MonoBehaviour, IDamageable
{
    [Header("Runtime Health")]

    // Current runtime health value.
    [SerializeField] private float currentHealth = 100f;

    [Header("Base Stats")]

    // =========================
    // SURVIVABILITY
    // =========================

    [SerializeField] private float baseMaxHealth = 100f;
    [SerializeField] private float baseHealthRegen = 0f;
    [SerializeField] private float baseArmor = 0f;

    // =========================
    // OFFENSIVE STATS
    // =========================

    [SerializeField] private float baseDamage = 10f;
    [SerializeField] private float baseCritChance = 0.05f;
    [SerializeField] private float baseCritDamage = 1.5f;
    [SerializeField] private float baseAttackRate = 1f;

    // =========================
    // PROJECTILE STATS
    // =========================

    [SerializeField] private float baseProjectileAmount = 1f;
    [SerializeField] private float baseProjectileSize = 1f;
    [SerializeField] private float baseProjectileRange = 20f;

    // =========================
    // AREA EFFECT STATS
    // =========================

    [SerializeField] private float baseAoeDamage = 0f;
    [SerializeField] private float baseAoeSize = 1f;

    // =========================
    // ELEMENTAL STATS
    // =========================

    [SerializeField] private float baseElementalDamage = 0f;
    [SerializeField] private float baseElementalResistance = 0f;
    [SerializeField] private float baseElementalProcChance = 0f;

    // =========================
    // MOVEMENT STATS
    // =========================

    [SerializeField] private float baseMoveSpeed = 6f;
    [SerializeField] private float baseDashAmount = 1f;

    // =========================
    // LOOT / ECONOMY STATS
    // =========================

    [SerializeField] private float baseItemDropChance = 0f;
    [SerializeField] private float baseItemDropRarity = 0f;
    [SerializeField] private float baseGoldDropChance = 0f;

    // =========================
    // GAME SCALING
    // =========================

    [SerializeField] private float baseCorruptionLevel = 0f;

    /*
     * Runtime Modifier List
     * 
     * Stores all active stat modifiers currently affecting the player.
     * 
     * Example Sources:
     * - Perks
     * - Loot
     * - Temporary buffs
     * - Character passives
     * - Curse effects
     */
    private readonly List<StatModifier> modifiers = new List<StatModifier>();

    // =========================
    // READ-ONLY PROPERTIES
    // =========================

    public float CurrentHealth => currentHealth;

    public float MaxHealth =>
        GetStatValue(StatType.MaxHealth);

    public float Damage =>
        GetStatValue(StatType.Damage);

    public float AttackRate =>
        GetStatValue(StatType.AttackRate);

    public float MoveSpeed =>
        GetStatValue(StatType.MoveSpeed);

    private void Start()
    {
        // Player starts with full health.
        currentHealth = MaxHealth;
    }

    private void Update()
    {
        // Handles passive health regeneration.
        RegenerateHealth();
    }

    /*
     * GetStatValue
     * 
     * Calculates the final runtime value for a stat.
     * 
     * Formula:
     * (Base + Flat Bonuses)
     * * (1 + PercentAdd Bonuses)
     * * PercentMultiply Bonuses
     * 
     * This keeps stat scaling modular and predictable.
     */
    public float GetStatValue(StatType statType)
    {
        float baseValue = GetBaseStatValue(statType);

        float flatBonus = 0f;
        float percentAddBonus = 0f;
        float percentMultiplyBonus = 1f;

        // Loop through all active modifiers.
        foreach (StatModifier modifier in modifiers)
        {
            // Skip unrelated stat types.
            if (modifier.statType != statType)
                continue;

            switch (modifier.modifierType)
            {
                case StatModifierType.Flat:
                    flatBonus += modifier.value;
                    break;

                case StatModifierType.PercentAdd:
                    percentAddBonus += modifier.value;
                    break;

                case StatModifierType.PercentMultiply:
                    percentMultiplyBonus *= modifier.value;
                    break;
            }
        }

        float finalValue =
            (baseValue + flatBonus)
            * (1f + percentAddBonus)
            * percentMultiplyBonus;

        // Prevent negative stats.
        return Mathf.Max(0f, finalValue);
    }

    /*
     * GetBaseStatValue
     * 
     * Returns the player's unmodified base stat value.
     * 
     * These values are used before runtime modifiers are applied.
     */
    private float GetBaseStatValue(StatType statType)
    {
        switch (statType)
        {
            case StatType.MaxHealth: return baseMaxHealth;
            case StatType.HealthRegen: return baseHealthRegen;
            case StatType.Armor: return baseArmor;

            case StatType.Damage: return baseDamage;
            case StatType.CritChance: return baseCritChance;
            case StatType.CritDamage: return baseCritDamage;
            case StatType.AttackRate: return baseAttackRate;

            case StatType.ProjectileAmount: return baseProjectileAmount;
            case StatType.ProjectileSize: return baseProjectileSize;
            case StatType.ProjectileRange: return baseProjectileRange;

            case StatType.AoeDamage: return baseAoeDamage;
            case StatType.AoeSize: return baseAoeSize;

            case StatType.ElementalDamage: return baseElementalDamage;
            case StatType.ElementalResistance: return baseElementalResistance;
            case StatType.ElementalProcChance: return baseElementalProcChance;

            case StatType.MoveSpeed: return baseMoveSpeed;
            case StatType.DashAmount: return baseDashAmount;

            case StatType.ItemDropChance: return baseItemDropChance;
            case StatType.ItemDropRarity: return baseItemDropRarity;
            case StatType.GoldDropChance: return baseGoldDropChance;

            case StatType.CorruptionLevel: return baseCorruptionLevel;

            default: return 0f;
        }
    }

    /*
     * AddModifier
     * 
     * Adds a runtime stat modifier to the player.
     * 
     * Example:
     * +5 Damage
     * +25% AttackRate
     */
    public void AddModifier(StatModifier modifier)
    {
        modifiers.Add(modifier);
    }

    /*
     * RemoveModifier
     * 
     * Removes a runtime modifier from the player.
     * 
     * Used for:
     * - Temporary buffs
     * - Expired effects
     * - Curse removal
     */
    public void RemoveModifier(StatModifier modifier)
    {
        modifiers.Remove(modifier);
    }

    /*
     * TakeDamage
     * 
     * Called through IDamageable by enemy/projectile systems.
     * 
     * Current Damage Formula:
     * Incoming Damage - Armor
     */
    public void TakeDamage(int amount)
    {
        float armor = GetStatValue(StatType.Armor);

        float reducedDamage =
            Mathf.Max(1f, amount - armor);

        currentHealth -= reducedDamage;

        currentHealth =
            Mathf.Clamp(currentHealth, 0f, MaxHealth);

        // Trigger death behavior if HP reaches zero.
        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    /*
     * Heal
     * 
     * Restores health up to MaxHealth.
     */
    public void Heal(float amount)
    {
        currentHealth += amount;

        currentHealth =
            Mathf.Clamp(currentHealth, 0f, MaxHealth);
    }

    /*
     * RegenerateHealth
     * 
     * Applies passive health regeneration over time.
     */
    private void RegenerateHealth()
    {
        float regen =
            GetStatValue(StatType.HealthRegen);

        if (regen <= 0f || currentHealth <= 0f)
            return;

        Heal(regen * Time.deltaTime);
    }

    /*
     * Die
     * 
     * Handles player death behavior.
     * 
     * Current Behavior:
     * - Disables the player GameObject
     * 
     * Future Systems:
     * - Respawning
     * - Game over handling
     * - Multiplayer revive systems
     * - Run failure tracking
     */
    private void Die()
    {
        Debug.Log("Player died.");

        // Temporary death behavior.
        gameObject.SetActive(false);
    }
}