using UnityEngine;

// Represents an item laying in the world that can be picked up.
public class ItemPickup : MonoBehaviour
{
    public ItemData itemData;

    public int itemAmount = 1;

    public bool PickupItem(Inventory targetInventory)
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
            Destroy(gameObject);
            return true;
        }

        return false;
    }
}