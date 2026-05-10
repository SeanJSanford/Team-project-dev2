using UnityEngine;

// Represents an item laying in the world that can be picked up.
public class IntegrationItemPickup : MonoBehaviour
{
    public ItemData itemData;

    public int itemAmount = 1;

    // Attempts to add the item into an inventory.
    public void PickupItem(Inventory targetInventory)
    {
        if (targetInventory.AddItem(itemData, itemAmount))
        {
            Destroy(gameObject);
        }
    }
}