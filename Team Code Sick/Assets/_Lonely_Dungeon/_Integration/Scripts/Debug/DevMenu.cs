using UnityEngine;

/*
 * IntegrationDevMenu
 * 
 * Purpose:
 * Central development/debug menu used during gameplay testing.
 * 
 * This tool allows the team to quickly test:
 * - Enemy spawning
 * - Player stat scaling
 * - Health systems
 * - Fire rate modifiers
 * - Loot drop systems
 * 
 * Why This Exists:
 * During development, constantly setting up gameplay scenarios
 * manually slows testing down significantly.
 * 
 * This menu provides quick runtime controls so systems can
 * be tested independently while the full gameplay loop
 * is still under development.
 * 
 * Connected Systems:
 * - PlayerStats
 * - EnemySpawner
 * - LootDrop
 * 
 * Future Expansion Ideas:
 * - Add corruption scaling controls
 * - Add perk/card testing
 * - Add weapon switching
 * - Add economy/currency testing
 * - Add enemy wave spawning
 * 
 * Important:
 * This is a development-only utility.
 * It should eventually be disabled or removed
 * from production builds.
 */

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