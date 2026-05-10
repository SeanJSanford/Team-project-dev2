using UnityEngine;

/*
 * IntegrationPlayerStats
 * 
 * Purpose:
 * Central runtime stat container for the player.
 * 
 * This system stores and manages:
 * - Health
 * - Damage
 * - Fire rate modifiers
 * - Movement modifiers
 * 
 * Why This Exists:
 * Instead of multiple systems storing separate player values,
 * this script acts as the main source of truth for player stats.
 * 
 * Other gameplay systems should READ from this script
 * instead of duplicating player stat values.
 * 
 * Connected Systems:
 * - Weapons
 * - UI
 * - Enemy combat
 * - Perk systems
 * - Loot upgrades
 * - Future economy/progression systems
 * - Dev menu testing tools
 * 
 * Combat Flow Example:
 * Enemy attacks ->
 * TakeDamage() ->
 * HP updates ->
 * UI updates ->
 * Death check occurs
 * 
 * Design Notes:
 * This script handles PLAYER STAT DATA ONLY.
 * 
 * It should NOT directly manage:
 * - Inventory UI
 * - Movement logic
 * - Weapon firing
 * - Animations
 * - Menus
 * 
 * Those systems should instead reference these values.
 * 
 * Future Expansion Ideas:
 * - Armor
 * - Crit chance
 * - Crit damage
 * - Health regeneration
 * - Dash count
 * - Elemental modifiers
 * - Corruption scaling
 * - Status effects
 * - Buff/debuff systems
 */

public class IntegrationPlayerStats : MonoBehaviour, IDamage
{
    [Header("Health")]

    // Maximum amount of health the player can have.
    [SerializeField] private int maxHP = 100;

    // Current runtime health value.
    [SerializeField] private int currentHP;

    [Header("Combat Stats")]

    // Base damage added into weapon calculations.
    [SerializeField] private int baseDamage = 10;

    // Fire rate multiplier used by weapon systems.
    // Higher values = faster attacks.
    [SerializeField] private float fireRateModifier = 1f;

    [Header("Movement Stats")]

    // Movement speed multiplier.
    [SerializeField] private float moveSpeedModifier = 1f;

    /*
     * Public Read-Only Properties
     * 
     * Other systems can safely READ these values
     * without directly modifying private fields.
     */

    public int MaxHP => maxHP;
    public int CurrentHP => currentHP;
    public int BaseDamage => baseDamage;
    public float FireRateModifier => fireRateModifier;
    public float MoveSpeedModifier => moveSpeedModifier;

    private void Start()
    {
        // Player starts with full health.
        currentHP = maxHP;
    }

    /*
     * TakeDamage
     * 
     * Called by combat systems through the IDamage interface.
     * 
     * Reduces health and checks for death.
     */
    public void TakeDamage(int amount)
    {
        currentHP -= amount;

        // Prevents HP from going below 0 or above maxHP.
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        Debug.Log("Player HP: " + currentHP + " / " + maxHP);

        // Trigger death behavior if health reaches 0.
        if (currentHP <= 0)
        {
            Die();
        }
    }

    /*
     * Heal
     * 
     * Restores player health up to the maximum HP limit.
     */
    public void Heal(int amount)
    {
        currentHP += amount;

        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        Debug.Log("Player healed. HP: " + currentHP + " / " + maxHP);
    }

    /*
     * AddDamage
     * 
     * Permanently increases the player's base damage.
     * 
     * Used for:
     * - Loot upgrades
     * - Perks
     * - Progression systems
     * - Debug testing
     */
    public void AddDamage(int amount)
    {
        baseDamage += amount;

        Debug.Log("Player damage increased to: " + baseDamage);
    }

    /*
     * AddMaxHP
     * 
     * Increases both max HP and current HP.
     * 
     * This prevents the player from feeling punished
     * after increasing their health capacity.
     */
    public void AddMaxHP(int amount)
    {
        maxHP += amount;
        currentHP += amount;

        Debug.Log("Player max HP increased to: " + maxHP);
    }

    /*
     * AddFireRateModifier
     * 
     * Increases the player's attack speed modifier.
     * 
     * Weapon systems use this value to calculate
     * how quickly the player can attack.
     */
    public void AddFireRateModifier(float amount)
    {
        fireRateModifier += amount;

        // Prevent invalid or negative fire rate values.
        fireRateModifier = Mathf.Max(0.1f, fireRateModifier);

        Debug.Log("Fire rate modifier: " + fireRateModifier);
    }

    /*
     * AddMoveSpeedModifier
     * 
     * Increases the player's movement speed multiplier.
     */
    public void AddMoveSpeedModifier(float amount)
    {
        moveSpeedModifier += amount;

        // Prevent invalid movement values.
        moveSpeedModifier = Mathf.Max(0.1f, moveSpeedModifier);

        Debug.Log("Move speed modifier: " + moveSpeedModifier);
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
     * - Game over screen
     * - Respawn systems
     * - Run failure tracking
     * - Multiplayer revive systems
     */
    private void Die()
    {
        Debug.Log("Player died.");

        // Temporary death behavior.
        gameObject.SetActive(false);
    }
}