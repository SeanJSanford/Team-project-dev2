using System.Collections.Generic;
using UnityEngine;

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

/*
========================================================
Project: Team Code Sick
Script: PlayerStats.cs

Primary Developer:
- Avery Wilson

System Category:
- Gameplay Stat System
- Runtime Player Stats
- Combat/Scaling Architecture
- Systems Integration

Purpose:
- Central runtime stat system used by the player.
- Handles scalable stat calculations, modifiers,
  damage handling, regeneration, and runtime scaling.

Core Responsibilities:
- Base stat storage
- Runtime stat modifier handling
- Damage calculations
- Healing/regeneration
- Combat stat scaling
- Runtime stat queries
- Shared gameplay stat integration

Connected Team Systems:
- Avery: WeaponStats / stat integration architecture
- Dai: PlayerMovement MoveSpeed integration
- Heather: Future item/equipment stat integration
- Sean: Combat scaling, weapon interaction, and enemy combat interactions
- Nilo: Gameplay progression oversight

Design Philosophy:
This script acts as the central runtime stat framework
for the player.

Other gameplay systems should READ stat values from here
instead of storing duplicate gameplay values.

This keeps:
- Combat systems modular
- Scaling predictable
- Future systems expandable
- Stat balancing centralized

Responsibilities intentionally excluded:
- Player movement logic
- Weapon firing logic
- Inventory management
- UI rendering
- Enemy AI behavior
- Visual effects

Why This Separation Exists:
Separating stat calculations from gameplay behavior:
- Prevents duplicate stat logic
- Simplifies balancing
- Improves scalability
- Supports future progression systems
- Reduces hardcoded dependencies

Development Notes:
- Built as a scalable replacement for hardcoded
  player upgrade values.
- Designed to support future perk, loot, corruption,
  and progression systems using shared modifier logic.
- Uses IDamageable so enemy/projectile systems can
  damage the player through the shared combat pipeline.

Future Expansion Ideas:
- Lifesteal
- Dodge chance
- Cooldown reduction
- Status resistances
- Buff/debuff tracking
- Event callbacks
- Multiplayer stat syncing
- Procedural scaling systems
========================================================
*/