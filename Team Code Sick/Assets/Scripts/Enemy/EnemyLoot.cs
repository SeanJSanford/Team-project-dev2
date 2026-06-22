using UnityEngine;

// Handles random loot drops when an enemy dies.
public class EnemyLoot : MonoBehaviour
{
    public LootEntry[] lootTable;

    public GameObject itemPickupPrefab;

    [Header("Health Drop")]
    public GameObject healthPickupPrefab;

    [Range(0f, 1f)]
    public float healthDropChance = 0.25f;

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

                if (itemPickup == null)
                {
                    Debug.LogError("The item pickup prefab does not have an ItemPickup script.");
                    continue;
                }

                itemPickup.pickupType = PickupType.InventoryItem;
                itemPickup.itemData = lootEntry.itemData;
                itemPickup.itemAmount = randomAmount;
            }
        }

        TryDropHealth();
    }

    private void TryDropHealth()
    {
        if (healthPickupPrefab == null)
            return;

        float randomRoll = Random.value;

        if (randomRoll <= healthDropChance)
        {
            Instantiate(
                healthPickupPrefab,
                transform.position,
                Quaternion.identity
            );
        }
    }
}