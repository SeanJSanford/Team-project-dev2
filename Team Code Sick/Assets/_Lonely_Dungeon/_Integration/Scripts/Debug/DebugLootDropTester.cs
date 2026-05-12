using UnityEngine;

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

/*
========================================================
Project: Team Code Sick
Script: DebugLootDropTester.cs

Primary Developer:
- Heather

System Category:
- Loot System Debug Utility
- Development Testing Tool

Purpose:
- Temporary debug tool for testing loot drops
  without requiring full enemy combat/death setup.
- Allows rapid iteration on loot tables,
  pickup spawning, and inventory integration.

How It Works:
- Searches for EnemyLoot on the same GameObject.
- Pressing K manually triggers DropLoot().
- Simulates enemy death for testing purposes.

Connected Team Systems:
- Heather: EnemyLoot / Item Templates / Loot Testing
- Avery: Stat integration and future item scaling
- Sean: Enemy death/combat integration support
- Inventory systems: Future item pickup integration
- Nilo: Gameplay direction and integration oversight

Development Notes:
- Intended for development/testing only.
- Should be removed or disabled in production builds.
- Useful for balancing drop rates during prototyping.
- Helps test loot behavior before final enemy death
  and combat systems are fully connected.

Future Improvements:
- Replace key input with debug menu tools
- Add configurable drop testing UI
- Add rarity simulation testing
- Add weighted drop analytics
========================================================
*/