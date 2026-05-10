using UnityEngine;

/*
 * DebugLootDropTester
 * 
 * Purpose:
 * Temporary debug utility used to test enemy loot drops
 * without needing full combat or enemy death logic.
 * 
 * How It Works:
 * - Looks for an EnemyLoot component on the same GameObject
 * - Pressing the K key manually triggers DropLoot()
 * - Useful for balancing drop chances and testing pickups
 * 
 * Used During Development For:
 * - Loot table testing
 * - Pickup spawning validation
 * - Inventory testing
 * - Rapid iteration without combat setup
 * 
 * Important:
 * This script is intended for development/testing only.
 * It should not be included in final gameplay builds.
 */

public class DebugLootDropTester : MonoBehaviour
{
    // Reference to the enemy loot system attached to this object
    private EnemyLoot enemyLoot;

    private void Start()
    {
        // Automatically grabs the EnemyLoot component
        // from the same GameObject at runtime.
        enemyLoot = GetComponent<EnemyLoot>();
    }

    private void Update()
    {
        // Press K to simulate an enemy death and trigger loot drops.
        if (Input.GetKeyDown(KeyCode.K))
        {
            enemyLoot.DropLoot();

            Debug.Log("Loot dropped.");
        }
    }
}