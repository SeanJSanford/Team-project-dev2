
public enum StatModifierType
{
    // Direct value increase.
    // Example: +5 Damage
    Flat,

    // Additive percentage increase.
    // Example: +25% Damage
    PercentAdd,

    // Multiplicative scaling applied after additive calculations.
    // Example: x1.5 Damage
    PercentMultiply
}

[System.Serializable]

public class StatModifier
{
    // Which gameplay stat is being modified.
    public StatType statType;

    // How the modifier affects the stat.
    public StatModifierType modifierType;

    // Numerical value of the modifier.
    public float value;

    /*
     * StatModifier Constructor
     * 
     * Creates a new stat modifier instance.
     * 
     * Example:
     * new StatModifier(
     *     StatType.Damage,
     *     StatModifierType.Flat,
     *     5f
     * );
     */
    public StatModifier(
        StatType statType,
        StatModifierType modifierType,
        float value)
    {
        this.statType = statType;
        this.modifierType = modifierType;
        this.value = value;
    }
}

/*
========================================================
Project: Team Code Sick
Script: StatModifier.cs
Related Enum: StatModifierType.cs

Primary Developer:
- Avery Wilson

System Category:
- Gameplay Stat System
- Runtime Modifier Framework
- Combat/Scaling Architecture

Purpose:
- Represents a single runtime stat modification.
- Defines scalable stat modifiers used throughout
  the gameplay systems architecture.
- Allows multiple gameplay systems to modify stats
  dynamically without hardcoded upgrade logic.

Core Responsibilities:
- Store the stat being modified
- Store the modifier type being applied
- Store the modifier value
- Define modifier calculation behavior
- Support scalable runtime stat calculations
- Enable modular gameplay scaling

How It Works:
A StatModifier contains:
- The StatType being modified
- The StatModifierType being applied
- The numerical value of the modifier

Example:
+5 Damage

new StatModifier(
    StatType.Damage,
    StatModifierType.Flat,
    5f
);

Example:
+25% Attack Rate

new StatModifier(
    StatType.AttackRate,
    StatModifierType.PercentAdd,
    0.25f
);

Connected Team Systems:
- Avery: PlayerStats / WeaponStats integration
- Heather: Future equipment and item bonuses
- Sean: Weapon balancing, combat tuning, and enemy combat scaling
- Dai: Movement stat integration
- Nilo: Gameplay progression oversight

Why This Exists:
Instead of creating separate upgrade logic
for every gameplay stat, this system uses:
- StatType
- StatModifierType
- StatModifier

to create a reusable modifier-based architecture.

This allows systems such as:
- Weapons
- Loot
- Perks
- Buffs/debuffs
- Character archetypes
- Corruption systems
- Difficulty scaling

to all interact with the same stat pipeline.

Modifier Types:
- Flat
    Direct value increases
    Example: +5 Damage

- PercentAdd
    Additive percentage bonuses
    Example: +25% Damage

- PercentMultiply
    Multiplicative scaling applied after additive bonuses
    Example: x1.5 Damage

Calculation Formula:
(Base + Flat Bonuses)
* (1 + PercentAdd Bonuses)
* PercentMultiply Bonuses

Design Philosophy:
StatModifier objects are intended to be modular
and reusable across multiple gameplay systems.

This prevents:
- Hardcoded stat upgrades
- Uncontrolled stat inflation
- Duplicate scaling logic
- Inconsistent balancing behavior

This allows:
- Temporary buffs
- Permanent upgrades
- Item bonuses
- Curse effects
- Character passives

without needing separate hardcoded methods
for every stat type.

Development Notes:
- Built as the foundation for scalable gameplay progression.
- Designed to support both temporary and permanent modifiers.
- Intended to unify all future gameplay scaling systems.

Future Expansion Ideas:
- Modifier durations
- Source tracking
- Stacking rules
- Conditional modifiers
- Network synchronization
- Unique modifier identifiers
- Modifier priorities
========================================================
*/