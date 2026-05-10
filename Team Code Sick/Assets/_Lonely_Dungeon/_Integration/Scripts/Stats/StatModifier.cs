/*
 * StatModifierType
 * 
 * Purpose:
 * Defines how a stat modifier changes a stat value.
 * 
 * Why This Exists:
 * Different modifier types allow the game to scale stats
 * in more controlled and balanced ways.
 * 
 * Example:
 * Flat:
 * +5 Damage
 * 
 * PercentAdd:
 * +25% Damage
 * 
 * PercentMultiply:
 * Multiplicative scaling applied after additive bonuses
 * 
 * Order of Operations:
 * Final Value =
 * (Base + Flat Bonuses)
 * * (1 + PercentAdd Bonuses)
 * * PercentMultiply Bonuses
 * 
 * Example:
 * Base Damage = 10
 * Flat Bonus = +5
 * PercentAdd = +20%
 * PercentMultiply = x1.5
 * 
 * Final:
 * (10 + 5) * (1 + 0.2) * 1.5 = 27
 * 
 * Design Notes:
 * Separating modifier types prevents:
 * - Uncontrolled stat inflation
 * - Broken stacking behavior
 * - Hardcoded upgrade logic
 * 
 * Connected Systems:
 * - PlayerStats
 * - WeaponStats
 * - Perk systems
 * - Loot systems
 * - Buff/debuff systems
 * - Future progression systems
 */

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

/*
 * StatModifier
 * 
 * Purpose:
 * Represents a single runtime stat modification.
 * 
 * How It Works:
 * A StatModifier contains:
 * - The stat being modified
 * - The type of modifier being applied
 * - The value of the modifier
 * 
 * Example:
 * +5 Damage
 * 
 * new StatModifier(
 *     StatType.Damage,
 *     StatModifierType.Flat,
 *     5f
 * );
 * 
 * Example:
 * +25% Attack Rate
 * 
 * new StatModifier(
 *     StatType.AttackRate,
 *     StatModifierType.PercentAdd,
 *     0.25f
 * );
 * 
 * Connected Systems:
 * - PlayerStats
 * - WeaponStats
 * - Perk systems
 * - Loot systems
 * - Character archetypes
 * - Future buff/debuff systems
 * 
 * Design Notes:
 * StatModifier objects are intended to be modular
 * and reusable across multiple gameplay systems.
 * 
 * This allows:
 * - Temporary buffs
 * - Permanent upgrades
 * - Item bonuses
 * - Curse effects
 * - Character passives
 * 
 * without needing separate hardcoded methods
 * for every stat type.
 */

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