using UnityEngine;

public class EnemyMeleeAI : MonoBehaviour
{
    [Header("Fallback AI Values")]
    [SerializeField] private float faceTargetSpeed = 8f;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float stopDistance = 1.5f;

    private Transform playerTarget;
    private EnemyStats enemyStats;
    private EnemyCombat enemyCombat;

    private bool playerInTrigger;

    private void Awake()
    {
        enemyStats = GetComponent<EnemyStats>();
        enemyCombat = GetComponent<EnemyCombat>();
    }

    private void Update()
    {
        if (!playerInTrigger || playerTarget == null)
            return;

        RotateToTarget();
        MoveToTarget();
        TryAttackTarget();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerTarget = other.transform;
        playerInTrigger = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerTarget = null;
        playerInTrigger = false;
    }

    /*
     * RotateToTarget
     * 
     * Smoothly rotates the melee enemy toward the player.
     */
    private void RotateToTarget()
    {
        Vector3 directionToPlayer = GetDirectionToTarget();

        if (directionToPlayer.sqrMagnitude <= 0.01f)
            return;

        Quaternion targetRotation =
            Quaternion.LookRotation(directionToPlayer.normalized);

        float rotationSpeed = enemyStats != null
            ? enemyStats.FaceTargetSpeed
            : faceTargetSpeed;

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            targetRotation,
            Time.deltaTime * rotationSpeed
        );
    }

    /*
     * MoveToTarget
     * 
     * Moves toward the player until the enemy reaches melee range.
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
     * TryAttackTarget
     * 
     * Tells EnemyCombat to attack when the player is within melee range.
     */
    private void TryAttackTarget()
    {
        if (enemyCombat == null)
            return;

        float distanceToPlayer = GetDistanceToTarget();

        float activeAttackRange = enemyStats != null
            ? enemyStats.AttackRange
            : stopDistance;

        if (distanceToPlayer <= activeAttackRange)
        {
            enemyCombat.TryAttack(playerTarget);
        }
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
}

/*
========================================================
Project: Team Code Sick / Lonely Dungeon
Script: EnemyMeleeAI.cs

Primary Developer:
- Sean

Integration Support:
- Avery Wilson

System Category:
- Enemy AI
- Melee Enemy Behavior
- Target Tracking
- Gameplay Integration

Purpose:
- Handles melee enemy targeting and movement behavior.
- Detects the player entering range.
- Rotates toward the player while they remain nearby.
- Moves toward the player until within melee attack range.
- Triggers EnemyCombat when the player is close enough.

Current Features:
- Player detection through trigger ranges
- Player target tracking
- Smooth enemy rotation toward target
- Basic chase movement toward the player
- Stop distance support for melee range
- EnemyCombat attack trigger support
- Lightweight modular AI behavior

Connected Team Systems:
- Sean: Enemy AI / melee combat behavior
- Avery Wilson: EnemyStats / stat integration
- Dai: Player movement interactions
- Heather: Future loot/death interactions
- Nilo: Gameplay integration oversight

Design Philosophy:
This script is intentionally limited to melee AI behavior only.

Responsibilities intentionally excluded:
- Enemy HP management
- Damage handling
- Loot drops
- Hit feedback
- Stat calculations
- Death handling

These systems are handled separately by:
- EnemyStats
- EnemyHealth
- EnemyCombat
- DamageDealer
- LootDropper

Why This Separation Exists:
Separating enemy AI from combat, health, and stat systems:
- Reduces Git conflicts
- Improves scalability
- Simplifies debugging
- Supports modular enemy architecture
- Allows combat/stat values to be balanced without editing AI logic

Development Notes:
- Designed to support lightweight modular AI behavior.
- Built as an early foundation for future melee enemy systems.
- Intended to integrate with future combat and navigation systems.
- Replaces older all-in-one EnemyMelee behavior with a cleaner modular version.

Current Status:
- Prototype melee targeting behavior
- Prototype melee chase behavior
- Active enemy AI integration component
- Supports future enemy expansion

Future Expansion Ideas:
- NavMesh movement
- Attack wind-up timing
- Melee hitboxes
- State machine logic
- Aggro ranges
- Patrol behavior
- Stagger reactions
- Animation integration
========================================================
*/