using System;

// Represents a single slot in an inventory.
[Serializable]
public class IntegrationInventorySlot
{
    public ItemData itemData;

    public int itemAmount;

    public IntegrationInventorySlot(ItemData newItemData, int newItemAmount)
    {
        itemData = newItemData;
        itemAmount = newItemAmount;
    }
}