using UnityEngine;

/*
 * IntegrationEnemyLoot
 * 
 * Purpose:
 * Handles loot generation when an enemy dies.
 * 
 * How It Works:
 * - Each enemy can contain its own loot table
 * - Every LootEntry has:
 *      - item reference
 *      - drop chance
 *      - amount range
 * 
 * When DropLoot() is called:
 * - The script rolls through every loot entry
 * - Successful rolls spawn a world pickup object
 * - Spawned pickups receive item data + amount values
 * 
 * Connected Systems:
 * - EnemyAI death handling
 * - LootEntry
 * - IntegrationItemPickup
 * - PlayerInventory
 * - Future rarity/economy systems
 * 
 * Example Flow:
 * Enemy dies ->
 * DropLoot() ->
 * Roll loot table ->
 * Spawn IntegrationItemPickup ->
 * Player collects item
 * 
 * Design Notes:
 * This system currently uses simple random drop logic.
 * 
 * Future Expansion Ideas:
 * - Rarity scaling
 * - Corruption modifiers
 * - Luck stat modifiers
 * - Weighted loot pools
 * - Boss-exclusive drops
 * - Multiplayer loot ownership
 * 
 * Important:
 * The enemy itself does NOT store item behavior.
 * It only decides WHAT can drop.
 */

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