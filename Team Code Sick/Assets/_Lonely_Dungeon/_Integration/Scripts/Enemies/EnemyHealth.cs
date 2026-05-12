using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [Header("Health")]
    private EnemyStats enemyStats;

    [Header("Visual Feedback")]
    [SerializeField] private Renderer enemyRenderer;

    [SerializeField] private Color hitColor = Color.red;

    [SerializeField] private float flashDuration = 0.1f;

    [Header("UI")]
    [SerializeField] private Transform healthBarFill;

    private Color originalColor;
    private Vector3 originalHealthBarScale;

    private LootDropper lootDropper;

    private void Awake()
    {
        enemyStats = GetComponent<EnemyStats>();
    }

    private void Start()
    {

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
        if (enemyStats == null)
            return;

        enemyStats.ApplyDamage(amount);

        UpdateHealthBar();

        Debug.Log(
            $"{gameObject.name} took {amount} damage. " +
            $"HP: {enemyStats.CurrentHealth}/{enemyStats.MaxHealth}"
        );

        if (enemyStats.IsDead())
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
            (float)enemyStats.CurrentHealth /
            enemyStats.MaxHealth;

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

/*
========================================================
Project: Team Code Sick
Script: EnemyHealth.cs

Primary Developer:
- Avery Wilson

System Category:
- Enemy Stats
- Enemy Health System
- Combat Integration

Purpose:
- Handles enemy health values, incoming damage,
  hit feedback, health bar updates, death behavior,
  and future loot-drop integration.

Connected Team Systems:
- Avery: Stats architecture / health logic / integration
- Sean: Shooting, melee combat, and enemy damage interactions
- Heather: LootDropper / item drop integration
- Dai: Movement/combat gameplay interaction testing
- Nilo: Overall integration and gameplay direction

Current Features:
- Uses IDamageable for shared combat compatibility
- Tracks max/current enemy health
- Updates debug health bar scaling
- Flashes enemy on hit
- Destroys enemy on death

Design Philosophy:
EnemyHealth intentionally manages ONLY:
- Health values
- Damage intake
- Death handling
- Combat feedback

Responsibilities intentionally excluded:
- Enemy AI behavior
- Movement logic
- Attack logic
- Loot generation logic
- Navigation systems

These responsibilities remain separated into
their own gameplay systems to reduce overlap
and improve scalability.

Development Notes:
- Replaces the earlier _EnemyAI prototype health logic.
- Uses the shared IDamageable combat interface.
- LootDropper integration is temporarily commented out
  until the loot pipeline is fully finalized.
- Built to support modular enemy architecture.

Current Status:
- Active runtime enemy health framework
- Shared combat integration component
- Supports future scalable enemy systems

Future Expansion Ideas:
- Armor/resistance systems
- Enemy stat scaling
- Status effects
- Death animations
- Damage numbers
- Enemy phases
- Elite/boss modifiers
- Event-driven combat feedback
========================================================
*/