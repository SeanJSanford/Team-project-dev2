using UnityEngine;

public enum PickupType
{
    InventoryItem,
    Health
}

// Represents an item laying in the world that can be picked up.
public class ItemPickup : MonoBehaviour
{
    public PickupType pickupType;

    [Header("Inventory Item")]
    public ItemData itemData;
    public int itemAmount = 1;

    [Header("Health Pickup")]
    public float healthAmount = 25f;

    private void Start()
    {
        Destroy(gameObject, 30f);
    }

    public bool PickupItem(
        Inventory targetInventory,
        playerMovement player)
    {
        if (pickupType == PickupType.Health)
        {
            return PickupHealth(player);
        }

        return PickupInventoryItem(targetInventory);
    }

    private bool PickupInventoryItem(Inventory targetInventory)
    {
        if (targetInventory == null)
        {
            Debug.LogError("No inventory was given to ItemPickup.");
            return false;
        }

        if (itemData == null)
        {
            Debug.LogError("ItemPickup has no ItemData.");
            return false;
        }

        if (itemAmount <= 0)
        {
            Debug.LogError("ItemPickup amount is 0 or less.");
            return false;
        }

        if (targetInventory.AddItem(itemData, itemAmount))
        {
            
            PickupNotification.Instance.Show($"Picked up {itemAmount}x {itemData.itemName}");

            Destroy(gameObject);
            return true;
        }

        return false;
    }

    private bool PickupHealth(playerMovement player)
    {
        if (player == null)
        {
            Debug.LogError("No player was given to the health pickup.");
            return false;
        }

        if (healthAmount <= 0)
        {
            Debug.LogError("Health pickup amount is 0 or less.");
            return false;
        }

        if (player.Heal(healthAmount))
        {
            // Show floating text
            PickupNotification.Instance.Show(
                $"Healed {healthAmount} HP"
            );

            Destroy(gameObject);
            return true;
        }

        return false;
    }
}