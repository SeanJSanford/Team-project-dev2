using UnityEngine;

public class DevMenu : MonoBehaviour
{
    [Header("References")]

    // Reference to the player stat system.
    // Used to test health and combat stat modifications in real time.
    [SerializeField] private PlayerStats playerStats;

    // Reference to the enemy spawning system.
    // Allows quick combat testing without needing full wave systems.
    [SerializeField] private AW_EnemySpawner enemySpawner;

    [Header("Debug Settings")]

    // Tracks whether the dev menu is currently open.
    [SerializeField] private bool devMenuOpen = false;

    // Toggles guaranteed loot drops for testing.
    [SerializeField] private bool forceLootDrop = false;

    private void Update()
    {
        // Toggle the debug menu on/off with F1.
        if (Input.GetKeyDown(KeyCode.F1))
        {
            devMenuOpen = !devMenuOpen;
        }

        // Prevents debug inputs from running while the menu is closed.
        if (!devMenuOpen)
            return;

        // Spawn a test enemy.
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            enemySpawner.SpawnEnemy();
        }

        // Heal the player.
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            playerStats.Heal(25);
        }

        // Damage the player.
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            playerStats.TakeDamage(25);
        }

        // Increase player base damage.
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            playerStats.AddModifier(new StatModifier(StatType.Damage, StatModifierType.Flat, 5f));
        }

        // Increase player fire rate modifier.
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            playerStats.AddModifier(new StatModifier(StatType.AttackRate, StatModifierType.PercentAdd, 0.25f));
        }

        // Toggle guaranteed loot drops for balancing/testing.
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            forceLootDrop = !forceLootDrop;

            // Static debug toggle used by the loot system.
            AW_LootDrop.ForceLootDrop = forceLootDrop;

            Debug.Log("Force Loot Drop: " + forceLootDrop);
        }
    }

    private void OnGUI()
    {
        // Prevent GUI drawing while menu is closed.
        if (!devMenuOpen)
            return;

        // Simple temporary IMGUI debug window.
        // This will likely be replaced with a proper UI system later.
        GUI.Box(new Rect(10, 10, 320, 220), "DEV MENU");

        GUI.Label(new Rect(25, 40, 280, 25), "F1 - Toggle Dev Menu");
        GUI.Label(new Rect(25, 65, 280, 25), "1 - Spawn Enemy");
        GUI.Label(new Rect(25, 90, 280, 25), "2 - Heal Player +25");
        GUI.Label(new Rect(25, 115, 280, 25), "3 - Damage Player -25");
        GUI.Label(new Rect(25, 140, 280, 25), "4 - Add Damage +5");
        GUI.Label(new Rect(25, 165, 280, 25), "5 - Add Fire Rate Modifier +0.25");
        GUI.Label(new Rect(25, 190, 280, 25), "6 - Toggle 100% Loot Drop: " + forceLootDrop);
    }
}

/*
========================================================
Project: Team Code Sick
Script: DevMenu.cs

Primary Developer:
- Avery Wilson

System Category:
- Development Utility
- Systems Integration Tool
- Runtime Debug Framework

Purpose:
- Centralized runtime development/debug menu used
  for rapid gameplay and systems testing.
- Allows gameplay systems to be tested independently
  before the complete gameplay loop is finalized.

Primary Responsibilities:
- Enemy spawn testing
- Player stat testing
- Health modification testing
- Fire rate modifier testing
- Loot system testing
- Runtime balancing support

Connected Team Systems:
- Avery: PlayerStats / StatModifier framework
- Heather: LootDrop systems and inventory testing
- Sean: Enemy spawning/combat integration support
- Dai: Movement/gameplay interaction testing
- Nilo: Overall gameplay integration oversight

Why This Exists:
Manually setting up gameplay scenarios during
development significantly slows iteration speed.

This menu allows developers to:
- Test systems independently
- Rapidly prototype balance changes
- Validate cross-system integration
- Simulate gameplay states quickly
- Stress test gameplay interactions

Design Philosophy:
This script intentionally acts as a lightweight
runtime debugging utility.

It is NOT intended to become:
- A permanent gameplay menu
- A production UI system
- A player-facing feature

Development Notes:
- Uses temporary IMGUI debug rendering.
- Intended for development/testing only.
- Built to accelerate gameplay iteration during prototyping.
- Should eventually be replaced with:
    - Dedicated debug UI
    - Developer console
    - Editor tooling
    - Debug command framework

Current Features:
- Spawn test enemies
- Modify player health
- Apply runtime stat modifiers
- Toggle forced loot drops
- Test combat scaling interactions

Future Expansion Ideas:
- Corruption scaling controls
- Weapon testing
- Perk/card injection
- Enemy wave simulation
- Currency/economy testing
- Save/load testing
- Boss spawn controls
- Runtime difficulty scaling

Portfolio Notes:
This script demonstrates:
- Runtime debugging workflows
- Modular stat integration
- Cross-system communication
- Gameplay prototyping practices
- Systems integration architecture
========================================================
*/