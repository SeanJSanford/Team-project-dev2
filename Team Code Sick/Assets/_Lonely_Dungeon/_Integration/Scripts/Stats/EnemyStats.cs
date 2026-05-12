using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 30;
    [SerializeField] private int currentHealth = 30;

    [Header("Combat")]
    [SerializeField] private int damage = 10;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackCooldown = 1.25f;
    [SerializeField] private float knockbackForce = 4f;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float faceTargetSpeed = 8f;
    [SerializeField] private float stopDistance = 1.25f;

    [Header("Targeting")]
    [SerializeField] private float detectionRange = 8f;

    public int MaxHealth => maxHealth;
    public int CurrentHealth => currentHealth;

    public int Damage => damage;
    public float AttackRange => attackRange;
    public float AttackCooldown => attackCooldown;
    public float KnockbackForce => knockbackForce;

    public float MoveSpeed => moveSpeed;
    public float FaceTargetSpeed => faceTargetSpeed;
    public float StopDistance => stopDistance;
    public float DetectionRange => detectionRange;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void ApplyDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }

    public bool IsDead()
    {
        return currentHealth <= 0;
    }
}

/*
========================================================
Project: Team Code Sick / Lonely Dungeon
Script: EnemyStats.cs

Primary Developers:
- Avery Wilson
- Sean

System Category:
- Runtime Stat Architecture
- Enemy Combat Data
- Gameplay Integration

Purpose:
- Stores runtime enemy combat and gameplay values.
- Centralizes enemy health, damage, movement,
  targeting, and combat-related stat data.
- Prevents combat values from being hardcoded
  inside AI or combat behavior systems.

Primary Responsibilities:
- Avery Wilson
    * Runtime stat architecture
    * Gameplay systems integration
    * Combat scaling framework
    * Shared gameplay stat structure

- Sean
    * Combat workflow integration
    * Damage system support
    * Enemy combat framework support

Connected Systems:
- EnemyHealth
    * Reads current/max health values

- EnemyCombat
    * Reads damage, cooldown, attack range,
      and knockback values

- EnemyMeleeAI / EnemyRangedAI
    * Read movement and targeting values

- DamageDealer
    * Uses combat stat values for damage handling

- EnemySpawner
    * Supports future enemy scaling and runtime balancing

Architecture Notes:
This script is intentionally data-focused.

Responsibilities intentionally excluded:
- Enemy movement logic
- Attack execution
- Damage application
- AI decision making
- Loot handling
- Death handling

These responsibilities are handled separately by:
- EnemyCombat
- EnemyMeleeAI
- EnemyRangedAI
- DamageDealer
- EnemyHealth
- LootDropper

Why This Separation Exists:
Separating stat architecture from gameplay behavior:
- Reduces Git conflicts
- Improves scalability
- Simplifies balancing
- Supports modular enemy design
- Keeps combat systems reusable
- Improves long-term maintainability

Development Status:
- Active gameplay integration framework
- Prototype enemy stat architecture
- Supports future combat scaling systems

Future Expansion Ideas:
- Difficulty scaling
- Elite enemy modifiers
- Status resistances
- Elemental damage types
- Corruption scaling
- Runtime buff/debuff systems
- Procedural stat generation
- Enemy archetype presets
========================================================
*/