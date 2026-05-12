using System.Collections;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    [Header("Fallback Combat Values")]
    [SerializeField] private int damage = 10;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private float knockbackForce = 4f;
    [SerializeField] private float attackPauseDuration = 0.15f;

    private EnemyStats enemyStats;

    private bool canAttack = true;

    private void Awake()
    {
        enemyStats = GetComponent<EnemyStats>();
    }

    /*
     * TryAttack
     * 
     * Attempts to attack the supplied target.
     * Prevents attacks during cooldown.
     */
    public void TryAttack(Transform target)
    {
        if (!canAttack || target == null)
            return;

        StartCoroutine(AttackRoutine(target));
    }

    /*
     * AttackRoutine
     * 
     * Handles attack timing, damage application,
     * and cooldown management.
     */
    private IEnumerator AttackRoutine(Transform target)
    {
        canAttack = false;

        yield return new WaitForSeconds(attackPauseDuration);

        ApplyDamage(target);
        ApplyKnockback(target);

        yield return new WaitForSeconds(GetAttackCooldown());

        canAttack = true;
    }

    /*
     * ApplyDamage
     * 
     * Applies damage to the target if they implement IDamageable.
     */
    private void ApplyDamage(Transform target)
    {
        IDamageable damageable =
            target.GetComponent<IDamageable>();

        if (damageable == null)
            return;

        damageable.TakeDamage(GetDamage());
    }

    /*
     * ApplyKnockback
     * 
     * Pushes the player away from the enemy.
     */
    private void ApplyKnockback(Transform target)
    {
        Rigidbody targetRb =
            target.GetComponent<Rigidbody>();

        if (targetRb == null)
            return;

        Vector3 knockDirection =
            (target.position - transform.position).normalized;

        knockDirection.y = 0f;

        targetRb.AddForce(
            knockDirection * GetKnockbackForce(),
            ForceMode.Impulse
        );
    }

    /*
     * GetDamage
     * 
     * Returns active enemy damage value.
     */
    private int GetDamage()
    {
        if (enemyStats != null)
            return enemyStats.Damage;

        return damage;
    }

    /*
     * GetAttackCooldown
     * 
     * Returns active enemy attack cooldown.
     */
    private float GetAttackCooldown()
    {
        if (enemyStats != null)
            return enemyStats.AttackCooldown;

        return attackCooldown;
    }

    /*
     * GetKnockbackForce
     * 
     * Returns active enemy knockback force.
     */
    private float GetKnockbackForce()
    {
        if (enemyStats != null)
            return enemyStats.KnockbackForce;

        return knockbackForce;
    }
}

/*
========================================================
Project: Team Code Sick / Lonely Dungeon
Script: EnemyCombat.cs

Primary Developer:
- Sean

Integration Support:
- Avery Wilson

System Category:
- Combat Framework
- Enemy Combat
- Gameplay Integration

Purpose:
- Handles enemy combat execution behavior.
- Applies damage and knockback to player targets.
- Manages attack cooldown timing.
- Centralizes enemy attack logic outside of AI systems.

Current Features:
- Melee damage application
- Knockback support
- Attack cooldown handling
- Runtime stat integration
- Modular combat execution support

Connected Team Systems:
- Sean: Combat framework integration
- Avery Wilson: EnemyStats/stat architecture
- Dai: Player movement/physics interaction
- Heather: Future death/loot interaction support
- Nilo: Gameplay integration oversight

Connected Gameplay Systems:
- EnemyStats
- EnemyMeleeAI
- EnemyRangedAI
- DamageDealer
- PlayerHealth
- Rigidbody movement systems

Design Philosophy:
This script is intentionally focused only on
combat execution behavior.

Responsibilities intentionally excluded:
- Enemy targeting logic
- Enemy movement
- Enemy HP management
- Loot drops
- Animation control
- AI decision making

Those systems are handled separately by:
- EnemyMeleeAI
- EnemyRangedAI
- EnemyHealth
- EnemyStats
- LootDropper

Why This Separation Exists:
Separating combat execution from AI and health systems:
- Reduces Git conflicts
- Improves scalability
- Simplifies debugging
- Supports modular combat architecture
- Allows AI and combat systems to evolve independently

Development Notes:
- Built as a shared combat execution layer
  for future enemy types.
- Supports future melee and ranged enemy expansion.
- Intended to support future animation events
  and advanced combat systems.

Current Status:
- Prototype combat execution framework
- Active gameplay integration component
- Supports future combat expansion

Future Expansion Ideas:
- Animation event attacks
- Area-of-effect attacks
- Status effects
- Elemental damage
- Attack wind-up timing
- Combo attacks
- Critical hits
- Combat events
- Multiplayer synchronization
========================================================
*/