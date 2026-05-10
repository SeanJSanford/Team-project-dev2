using UnityEngine;
using System.Collections;

/*
 * EnemyHealth
 * 
 * Purpose:
 * Handles enemy health, damage, hit feedback,
 * health bar updates, death behavior, and loot drops.
 * 
 * Connected Systems:
 * - IDamageable
 * - Player combat systems
 * - LootDropper
 * - Future enemy AI systems
 * - Future damage popup systems
 * 
 * Current Features:
 * - Damage handling
 * - Health bar scaling
 * - Flash-on-hit feedback
 * - Loot drops on death
 * - Enemy destruction
 * 
 * Future Expansion Ideas:
 * - Armor/resistances
 * - Status effects
 * - Enemy phases
 * - Damage numbers
 * - Death animations
 * - Enemy scaling
 */

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 40;
    [SerializeField] private int currentHealth;

    [Header("Visual Feedback")]
    [SerializeField] private Renderer enemyRenderer;

    [SerializeField] private Color hitColor = Color.red;

    [SerializeField] private float flashDuration = 0.1f;

    [Header("UI")]
    [SerializeField] private Transform healthBarFill;

    private Color originalColor;
    private Vector3 originalHealthBarScale;

    private LootDropper lootDropper;

    private void Start()
    {
        currentHealth = maxHealth;

        lootDropper = GetComponent<LootDropper>();

        if (enemyRenderer == null)
        {
            enemyRenderer = GetComponentInChildren<Renderer>();
        }

        if (enemyRenderer != null)
        {
            originalColor = enemyRenderer.material.color;
        }

        if (healthBarFill != null)
        {
            originalHealthBarScale = healthBarFill.localScale;
        }
    }

    /*
     * TakeDamage
     * 
     * Called by combat systems through IDamageable.
     */
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        currentHealth = Mathf.Clamp(
            currentHealth,
            0,
            maxHealth
        );

        UpdateHealthBar();

        Debug.Log(
            $"{gameObject.name} took {amount} damage. " +
            $"HP: {currentHealth}/{maxHealth}"
        );

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(FlashOnHit());
        }
    }

    /*
     * UpdateHealthBar
     * 
     * Scales the debug health bar based on current HP.
     */
    private void UpdateHealthBar()
    {
        if (healthBarFill == null)
            return;

        float healthPercent =
            (float)currentHealth / maxHealth;

        healthBarFill.localScale = new Vector3(
            originalHealthBarScale.x * healthPercent,
            originalHealthBarScale.y,
            originalHealthBarScale.z
        );
    }

    /*
     * FlashOnHit
     * 
     * Briefly flashes the enemy red when damaged.
     */
    private IEnumerator FlashOnHit()
    {
        if (enemyRenderer == null)
            yield break;

        enemyRenderer.material.color = hitColor;

        yield return new WaitForSeconds(flashDuration);

        enemyRenderer.material.color = originalColor;
    }

    /*
     * Die
     * 
     * Handles enemy death behavior.
     */
    private void Die()
    {
        Debug.Log($"{gameObject.name} died.");

        // Drop loot if available.
        //lootDropper?.DropLoot();

        Destroy(gameObject);
    }
}