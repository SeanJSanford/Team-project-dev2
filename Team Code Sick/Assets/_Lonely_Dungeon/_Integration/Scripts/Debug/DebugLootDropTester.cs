using UnityEngine;

// Temporary script for testing enemy loot drops.
public class DebugLootDropTester : MonoBehaviour
{
    private EnemyLoot enemyLoot;

    private void Start()
    {
        enemyLoot = GetComponent<EnemyLoot>();
    }

    private void Update()
    {
        // Press K to simulate the enemy dying.
        if (Input.GetKeyDown(KeyCode.K))
        {
            enemyLoot.DropLoot();

            Debug.Log("Loot dropped.");
        }
    }
}