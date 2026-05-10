using UnityEngine;

// Handles random loot drops when an enemy dies.
public class IntegrationEnemyLoot : MonoBehaviour
{
    public LootEntry[] lootTable;

    public GameObject itemPickupPrefab;

    // Rolls through the loot table and spawns dropped items. 
    public void DropLoot()
    {
        foreach (LootEntry lootEntry in lootTable)
        {
            float randomRoll = Random.value;

            if (randomRoll <= lootEntry.dropChance)
            {
                int randomAmount = Random.Range(
                    lootEntry.minimumAmount,
                    lootEntry.maximumAmount + 1
                );

                GameObject spawnedPickup = Instantiate(
                    itemPickupPrefab,
                    transform.position,
                    Quaternion.identity
                );

                ItemPickup itemPickup = spawnedPickup.GetComponent<ItemPickup>();

                itemPickup.itemData = lootEntry.itemData;
                itemPickup.itemAmount = randomAmount;
            }
        }
    }
}