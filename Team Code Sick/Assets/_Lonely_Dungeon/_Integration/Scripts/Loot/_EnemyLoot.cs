using UnityEngine;

public class _EnemyLoot : MonoBehaviour
{
    [Header("Loot Settings")]

    // List of possible loot entries this enemy can drop.
    public LootEntry[] lootTable;

    // World pickup prefab that gets spawned on successful loot rolls.
    public GameObject itemPickupPrefab;

    /*
     * DropLoot
     * 
     * Rolls through the loot table and spawns
     * item pickups based on drop chance.
     */
    public void DropLoot()
    {
        // Safety checks to prevent runtime errors.
        if (lootTable == null || lootTable.Length == 0)
        {
            Debug.LogWarning("No loot table assigned.");
            return;
        }

        if (itemPickupPrefab == null)
        {
            Debug.LogWarning("No item pickup prefab assigned.");
            return;
        }

        // Loop through every possible loot entry.
        foreach (LootEntry lootEntry in lootTable)
        {
            // Random value between 0 and 1.
            float randomRoll = Random.value;

            // Successful loot roll.
            if (randomRoll <= lootEntry.dropChance)
            {
                // Random amount within the allowed range.
                int randomAmount = Random.Range(
                    lootEntry.minimumAmount,
                    lootEntry.maximumAmount + 1
                );

                // Spawn the pickup object into the world.
                GameObject spawnedPickup = Instantiate(
                    itemPickupPrefab,
                    transform.position,
                    Quaternion.identity
                );

                // Get the pickup component from the spawned prefab.
                WorldItemPickup itemPickup =
                    spawnedPickup.GetComponent<WorldItemPickup>();

                // Safety check in case the prefab is missing the component.
                if (itemPickup == null)
                {
                    Debug.LogError("Spawned pickup missing IntegrationItemPickup component.");
                    continue;
                }

                // Assign the item information to the spawned pickup.
                itemPickup.itemData = lootEntry.itemData;
                itemPickup.itemAmount = randomAmount;
            }
        }
    }
}

/*
========================================================
Project: Team Code Sick
Script: EnemyLoot.cs
(Current Prototype Name: _EnemyLoot.cs)

Primary Developer:
- Heather

Integration Support:
- Avery Wilson

System Category:
- Loot System
- Enemy Reward Framework
- Item Drop Generation

Purpose:
- Handles loot generation when enemies die.
- Rolls randomized loot tables and spawns
  collectible world item pickups.

Current Features:
- Per-enemy loot tables
- Randomized drop chance rolls
- Variable item amount generation
- World pickup spawning
- Item data assignment
- Loot table validation checks

Connected Team Systems:
- Heather: Inventory/item architecture
- Sean: Enemy death/combat integration
- Avery: Future stat/luck modifier integration
- Nilo: Gameplay progression oversight
- Future economy systems

How The System Works:
Enemy Death ->
DropLoot() ->
Roll Loot Table ->
Spawn WorldItemPickup ->
Player Collects Item ->
PlayerInventory Updated

Design Philosophy:
This system intentionally only determines:
- WHAT items drop
- HOW MUCH drops
- DROP CHANCES

Responsibilities intentionally excluded:
- Enemy combat logic
- Item behavior
- Inventory storage
- Stat application
- Economy balancing

Those systems are handled separately to maintain
modular gameplay architecture.

Why This Separation Exists:
Separating loot generation from gameplay systems:
- Simplifies balancing
- Improves scalability
- Reduces hardcoded dependencies
- Supports future reward expansion

Development Notes:
- Uses randomized loot entry rolls.
- Supports independent loot tables per enemy.
- Built for rapid prototyping and future scalability.
- Intended as the foundation for more advanced:
    - Reward systems
    - Economy systems
    - Progression systems

Current Limitations:
- No rarity weighting
- No luck modifiers
- No drop tiers
- No boss loot pools
- No multiplayer ownership
- No adaptive scaling

Future Expansion Ideas:
- Weighted loot pools
- Corruption modifiers
- Luck stat scaling
- Rarity systems
- Boss-exclusive loot
- Procedural rewards
- Multiplayer loot ownership
- Difficulty-based drop scaling
========================================================
*/