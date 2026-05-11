using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] spawnPoints;

    public void SpawnEnemy()
    {
        if (enemyPrefab == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning("EnemySpawner missing prefab or spawn points.");
            return;
        }

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

        Debug.Log("Spawned test enemy.");
    }
}

/*
========================================================
Project: Team Code Sick
Script: EnemySpawner.cs

Primary Developers:
- Sean
- Heather

Integration Support:
- Avery Wilson

System Category:
- Enemy Spawning
- Combat Testing Utility
- Gameplay Integration

Purpose:
- Spawns enemy prefabs at randomized spawn points.
- Used during development for rapid enemy testing
  without requiring full wave/gameplay systems.

Current Features:
- Randomized spawn point selection
- Runtime enemy spawning
- Simple spawn validation
- Debug logging support

Connected Team Systems:
- Sean: Enemy combat and gameplay testing
- Heather: Loot drop validation/testing
- Avery: DevMenu integration and stat testing
- Dai: Movement/combat interaction testing
- Nilo: Gameplay flow oversight

Design Philosophy:
This script intentionally acts as a lightweight
prototype spawning utility.

Responsibilities intentionally excluded:
- Wave management
- Difficulty balancing
- Enemy progression scaling
- Room generation logic
- AI behavior management

These systems should remain separated into
dedicated gameplay systems.

Development Notes:
- Primarily used for gameplay/system testing.
- Supports rapid iteration during combat development.
- Lightweight prototype spawner before implementation
  of larger enemy management systems.
- Integrated with DevMenu for rapid gameplay testing.

Current Limitations:
- No enemy caps
- No wave logic
- No spawn cooldowns
- No difficulty scaling
- No room validation
- No NavMesh checks

Future Expansion Ideas:
- Wave spawning
- Spawn weighting
- Difficulty scaling
- Enemy pool randomization
- Room-based spawning
- Spawn limits
- Corruption/event spawning
- Dynamic encounter generation
========================================================
*/