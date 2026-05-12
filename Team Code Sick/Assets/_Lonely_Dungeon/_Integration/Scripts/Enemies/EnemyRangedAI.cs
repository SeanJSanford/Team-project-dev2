using UnityEngine;

public class EnemyRangedAI : MonoBehaviour
{
    [Header("Fallback AI Values")]
    [SerializeField] private float faceTargetSpeed = 8f;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float stopDistance = 6f;

    [Header("Shooting")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float shootRate = 1f;
    [SerializeField] private Transform gunPivot;
    [SerializeField] private Transform shootPosition;
    [SerializeField] private float gunRotateSpeed = 10f;

    private Transform playerTarget;
    private EnemyStats enemyStats;

    private bool playerInTrigger;
    private float shootTimer;

    private void Awake()
    {
        enemyStats = GetComponent<EnemyStats>();
    }

    private void Update()
    {
        if (!playerInTrigger || playerTarget == null)
            return;

        RotateBodyToTarget();
        RotateGunToTarget();
        MoveToTarget();
        UpdateShooting();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerTarget = other.transform;
        playerInTrigger = true;

        shootTimer = GetShootRate();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerTarget = null;
        playerInTrigger = false;
    }

    /*
     * RotateBodyToTarget
     * 
     * Smoothly rotates the enemy body toward the player on the Y axis.
     */
    private void RotateBodyToTarget()
    {
        Vector3 directionToPlayer = GetDirectionToTarget();

        if (directionToPlayer.sqrMagnitude <= 0.01f)
            return;

        Quaternion targetRotation =
            Quaternion.LookRotation(directionToPlayer.normalized);

        float activeFaceTargetSpeed = enemyStats != null
            ? enemyStats.FaceTargetSpeed
            : faceTargetSpeed;

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            targetRotation,
            Time.deltaTime * activeFaceTargetSpeed
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

        Vector3 directionToPlayer =
            playerTarget.position - gunPivot.position;

        directionToPlayer.y = 0f;

        if (directionToPlayer.sqrMagnitude <= 0.01f)
            return;

        Quaternion targetRotation =
            Quaternion.LookRotation(directionToPlayer.normalized);

        gunPivot.rotation = Quaternion.Lerp(
            gunPivot.rotation,
            targetRotation,
            Time.deltaTime * gunRotateSpeed
        );
    }

    /*
     * MoveToTarget
     * 
     * Moves toward the player until the enemy reaches ranged stop distance.
     */
    private void MoveToTarget()
    {
        float distanceToPlayer = GetDistanceToTarget();

        float activeStopDistance = enemyStats != null
            ? enemyStats.StopDistance
            : stopDistance;

        if (distanceToPlayer <= activeStopDistance)
            return;

        Vector3 directionToPlayer = GetDirectionToTarget();

        float activeMoveSpeed = enemyStats != null
            ? enemyStats.MoveSpeed
            : moveSpeed;

        transform.position +=
            directionToPlayer.normalized * activeMoveSpeed * Time.deltaTime;
    }

    /*
     * UpdateShooting
     * 
     * Handles ranged fire timing.
     */
    private void UpdateShooting()
    {
        shootTimer += Time.deltaTime;

        if (shootTimer < GetShootRate())
            return;

        Shoot();
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

        Quaternion spawnRotation = gunPivot != null
            ? gunPivot.rotation
            : shootPosition.rotation;

        Instantiate(
            bulletPrefab,
            shootPosition.position,
            spawnRotation
        );
    }

    /*
     * GetDirectionToTarget
     * 
     * Returns a flattened direction toward the player.
     */
    private Vector3 GetDirectionToTarget()
    {
        Vector3 directionToPlayer =
            playerTarget.position - transform.position;

        directionToPlayer.y = 0f;

        return directionToPlayer;
    }

    /*
     * GetDistanceToTarget
     * 
     * Returns flat horizontal distance to the player.
     */
    private float GetDistanceToTarget()
    {
        Vector3 directionToPlayer = GetDirectionToTarget();

        return directionToPlayer.magnitude;
    }

    /*
     * GetShootRate
     * 
     * Returns the active ranged attack cooldown.
     */
    private float GetShootRate()
    {
        if (enemyStats != null)
            return enemyStats.AttackCooldown;

        return shootRate;
    }
}

/*
========================================================
Project: Team Code Sick / Lonely Dungeon
Script: EnemyRangedAI.cs

Primary Developer:
- Sean

Integration Support:
- Avery Wilson

System Category:
- Enemy AI
- Ranged Enemy Behavior
- Projectile Combat AI
- Gameplay Integration

Purpose:
- Handles ranged enemy targeting, movement, aiming, and shooting behavior.
- Detects player targets through trigger ranges.
- Rotates the enemy body toward the player.
- Rotates weapon pivot toward the player.
- Moves toward the player until within ranged stop distance.
- Fires enemy projectiles on a timed cooldown.

Current Features:
- Player detection through trigger range
- Enemy body rotation toward player
- Independent gun pivot targeting
- Basic chase movement toward player
- Stop distance support for ranged enemies
- Timed projectile firing
- Projectile spawn support
- Lightweight modular AI behavior

Connected Team Systems:
- Sean: Enemy AI / projectile combat integration
- Avery Wilson: EnemyStats / stat balancing integration
- Dai: Player movement interactions
- Heather: Future loot/death interactions
- Nilo: Gameplay integration oversight

Design Philosophy:
This script is intentionally focused only on ranged enemy AI behavior.

Responsibilities intentionally excluded:
- Enemy health management
- Damage handling
- Loot drops
- Hit feedback
- Stat calculations
- Death handling

Those systems are managed separately through:
- EnemyStats
- EnemyHealth
- EnemyCombat
- DamageDealer
- LootDropper

Why This Separation Exists:
Separating ranged combat behavior from health, stats,
and loot systems:
- Reduces Git conflicts
- Improves scalability
- Simplifies debugging
- Supports modular enemy architecture
- Allows combat/stat values to be balanced without editing AI logic

Development Notes:
- Designed to support lightweight modular AI behavior.
- Keeps ranged combat behavior independent from health and loot systems.
- Built as an early foundation for future enemy AI systems.
- Replaces older all-in-one EnemyRanged behavior with a cleaner modular version.

Current Limitations:
- No NavMesh/pathfinding
- No attack state machine
- No line-of-sight checks
- No predictive aiming
- No projectile spread/randomization

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