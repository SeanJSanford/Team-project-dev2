using UnityEngine;

public class EnemyMeleeAI : MonoBehaviour
{
    [Header("Targeting")]
    [SerializeField] private float faceTargetSpeed = 8f;

    private Transform playerTarget;
    private bool playerInTrigger;

    private void Update()
    {
        if (!playerInTrigger || playerTarget == null)
            return;

        RotateToTarget();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerTarget = other.transform;
            playerInTrigger = true;
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
     * RotateToTarget
     * 
     * Smoothly rotates the enemy toward the player.
     */
    private void RotateToTarget()
    {
        Vector3 playerDirection =
            playerTarget.position - transform.position;

        playerDirection.y = 0f;

        if (playerDirection.sqrMagnitude <= 0.01f)
            return;

        Quaternion targetRotation =
            Quaternion.LookRotation(playerDirection.normalized);

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            targetRotation,
            Time.deltaTime * faceTargetSpeed
        );
    }
}

/*
========================================================
Project: Team Code Sick
Script: EnemyMeleeAI.cs

Primary Developer:
- Sean

Integration Support:
- Avery Wilson

System Category:
- Enemy AI
- Melee Enemy Behavior
- Target Tracking

Purpose:
- Handles basic melee enemy targeting behavior.
- Detects the player entering range and rotates
  toward the player while they remain nearby.

Current Features:
- Player detection through trigger ranges
- Player target tracking
- Smooth enemy rotation toward target
- Lightweight AI targeting behavior

Connected Team Systems:
- Sean: Enemy AI / melee combat behavior
- Avery: EnemyHealth / stat integration
- Dai: Player movement interactions
- Heather: Future loot/death interactions
- Nilo: Gameplay integration oversight

Design Philosophy:
This script is intentionally limited to AI behavior only.

Responsibilities intentionally excluded:
- Enemy HP management
- Damage handling
- Loot drops
- Hit feedback
- Stat calculations

These systems are handled separately by:
- EnemyHealth
- DamageDealer
- LootDropper

Why This Separation Exists:
Separating enemy AI from combat and health systems:
- Reduces Git conflicts
- Improves scalability
- Simplifies debugging
- Supports modular enemy architecture

Development Notes:
- Designed to support lightweight modular AI behavior.
- Built as an early foundation for future melee enemy systems.
- Intended to integrate with future combat and navigation systems.

Current Status:
- Prototype melee targeting behavior
- Active enemy AI integration component
- Supports future enemy expansion

Future Expansion Ideas:
- NavMesh movement
- Attack cooldowns
- Melee hitboxes
- State machine logic
- Aggro ranges
- Patrol behavior
- Stagger reactions
- Animation integration
========================================================
*/