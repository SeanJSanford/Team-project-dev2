using UnityEngine;

public class EnemyRangedAI : MonoBehaviour
{
    [Header("Targeting")]
    [SerializeField] private float faceTargetSpeed = 8f;

    [Header("Shooting")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float shootRate = 1f;
    [SerializeField] private Transform gunPivot;
    [SerializeField] private Transform shootPosition;
    [SerializeField] private float gunRotateSpeed = 10f;

    private Transform playerTarget;
    private bool playerInTrigger;
    private float shootTimer;

    private void Update()
    {
        if (!playerInTrigger || playerTarget == null)
            return;

        RotateBodyToTarget();
        RotateGunToTarget();

        shootTimer += Time.deltaTime;

        if (shootTimer >= shootRate)
        {
            Shoot();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerTarget = other.transform;
            playerInTrigger = true;
            shootTimer = shootRate;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerTarget = null;
            playerInTrigger = false;
        }
    }

    /*
     * RotateBodyToTarget
     * 
     * Smoothly rotates the enemy body toward the player on the Y axis.
     */
    private void RotateBodyToTarget()
    {
        Vector3 direction =
            playerTarget.position - transform.position;

        direction.y = 0f;

        if (direction.sqrMagnitude <= 0.01f)
            return;

        Quaternion targetRotation =
            Quaternion.LookRotation(direction.normalized);

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            targetRotation,
            Time.deltaTime * faceTargetSpeed
        );
    }

    /*
     * RotateGunToTarget
     * 
     * Rotates the ranged weapon pivot toward the player.
     */
    private void RotateGunToTarget()
    {
        if (gunPivot == null)
            return;

        Vector3 direction =
            playerTarget.position - gunPivot.position;

        direction.y = 0f;

        if (direction.sqrMagnitude <= 0.01f)
            return;

        Quaternion targetRotation =
            Quaternion.LookRotation(direction.normalized);

        gunPivot.rotation = Quaternion.Lerp(
            gunPivot.rotation,
            targetRotation,
            Time.deltaTime * gunRotateSpeed
        );
    }

    /*
     * Shoot
     * 
     * Spawns the enemy projectile from the shoot position.
     */
    private void Shoot()
    {
        shootTimer = 0f;

        if (bulletPrefab == null || shootPosition == null)
        {
            Debug.LogWarning("EnemyRangedAI missing bullet prefab or shoot position.");
            return;
        }

        Instantiate(
            bulletPrefab,
            shootPosition.position,
            shootPosition.rotation
        );
    }
}

/*
========================================================
Project: Team Code Sick
Script: EnemyRangedAI.cs

Primary Developer:
- Sean

Integration Support:
- Avery Wilson

System Category:
- Enemy AI
- Ranged Enemy Behavior
- Projectile Combat AI

Purpose:
- Handles ranged enemy targeting and shooting behavior.
- Detects player targets, rotates toward them,
  aims weapon pivots, and fires projectiles.

Current Features:
- Player detection through trigger range
- Enemy body rotation toward player
- Independent gun pivot targeting
- Timed projectile firing
- Projectile spawn support

Connected Team Systems:
- Sean: Enemy AI / projectile combat integration
- Avery: EnemyHealth/stat balancing integration
- Dai: Player movement interactions
- Heather: Future loot/death interactions
- Nilo: Gameplay integration oversight

Design Philosophy:
This script is intentionally focused only on
ranged enemy AI behavior.

Responsibilities intentionally excluded:
- Enemy health management
- Damage handling
- Loot drops
- Hit feedback
- Stat calculations

Those systems are managed separately through:
- EnemyHealth
- DamageDealer
- LootDropper

Why This Separation Exists:
Separating ranged combat behavior from health
and loot systems:
- Reduces Git conflicts
- Improves scalability
- Simplifies debugging
- Supports modular enemy architecture

Development Notes:
- Designed to support lightweight modular AI behavior.
- Keeps ranged combat behavior independent from
  health and loot systems.
- Built as an early foundation for future enemy AI systems.

Current Limitations:
- No pathfinding/navigation
- No attack states
- No line-of-sight checks
- No cooldown variation
- No predictive aiming

Future Expansion Ideas:
- NavMesh movement
- Attack state machines
- Burst-fire patterns
- Strafing behavior
- Projectile spread/randomization
- Aggro systems
- Animation integration
- Difficulty scaling
- Cover systems
========================================================
*/