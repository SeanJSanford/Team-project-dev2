using UnityEngine;
using System.Collections;

public class DamageDealer : MonoBehaviour
{
    private enum DamageType
    {
        Bullet,
        Stationary,
        DOT
    }

    [Header("Damage Settings")]
    [SerializeField] private DamageType type;
    [SerializeField] private int damageAmount = 10;
    [SerializeField] private float damageRate = 1f;

    [Header("Bullet Settings")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float bulletSpeed = 20f;
    [SerializeField] private float bulletDestroyTime = 3f;

    [Header("Effects")]
    [SerializeField] private ParticleSystem hitEffect;

    private bool isDamaging;

    private void Start()
    {
        if (type == DamageType.Bullet)
        {
            if (rb == null)
            {
                rb = GetComponent<Rigidbody>();
            }

            if (rb != null)
            {
                rb.linearVelocity = transform.forward * bulletSpeed;
                // rb.velocity = transform.forward * bulletSpeed; (In case we get in errors with Unity)
            }

            Destroy(gameObject, bulletDestroyTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
            return;

        IDamageable damageable = other.GetComponent<IDamageable>();

        if (damageable != null && type != DamageType.DOT)
        {
            damageable.TakeDamage(damageAmount);
        }

        if (type == DamageType.Bullet)
        {
            SpawnHitEffect();
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.isTrigger)
            return;

        IDamageable damageable = other.GetComponent<IDamageable>();

        if (damageable != null && type == DamageType.DOT && !isDamaging)
        {
            StartCoroutine(DamageOverTime(damageable));
        }
    }

    private IEnumerator DamageOverTime(IDamageable damageable)
    {
        isDamaging = true;

        damageable.TakeDamage(damageAmount);

        yield return new WaitForSeconds(damageRate);

        isDamaging = false;
    }

    private void SpawnHitEffect()
    {
        if (hitEffect != null)
        {
            Instantiate(hitEffect, transform.position, Quaternion.identity);
        }
    }
}

/*
========================================================
Project: Team Code Sick
Script: DamageDealer.cs

Primary System:
- Combat Damage Utility

Primary Developer:
- Sean

Integration Support:
- Avery Wilson

Connected Team Systems:
- Sean: Shooting Logic / Melee Combat / Enemy Combat Interactions
- Avery: Player Stats / Weapon Stats / Combat Integration
- Heather: Future item effects, loot-triggered damage, traps, hazards
- Dai: Movement/combat gameplay integration
- Nilo: Gameplay direction and system oversight

Purpose:
- Applies damage through trigger collisions.
- Supports:
    - Bullet damage
    - Stationary damage zones
    - Damage-over-time zones
- Uses IDamageable so multiple gameplay objects
  can receive damage through a shared combat pipeline.

Combat Workflow:
Damage Source ->
DamageDealer ->
IDamageable ->
Target Handles Damage Internally

This allows:
- Enemies
- Players
- Future bosses
- Breakable objects
- Environmental hazards

to all use the same combat interaction system.

Design Notes:
- This script intentionally handles ONLY damage application.
- Health logic remains separated into:
    - EnemyHealth
    - PlayerStats
    - Future destructible systems

Responsibilities intentionally excluded:
- Enemy AI behavior
- Loot drops
- Combat visuals
- UI feedback
- Projectile spawning

Development Notes:
- Refactored from the original Full Sail lecture damage system.
- Updated into a cleaner modular combat utility
  using the shared IDamageable architecture.
- Built to support scalable combat expansion
  while reducing hardcoded dependencies.

Future Expansion Ideas:
- Elemental damage
- Armor penetration
- Critical hit support
- Damage falloff
- Team/faction filtering
- Status effect application
- Multiplayer ownership validation
========================================================
*/