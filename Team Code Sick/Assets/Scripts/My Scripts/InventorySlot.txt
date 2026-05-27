using System;

// Represents a single slot in an inventory.
[Serializable]
public class InventorySlot
{
    public ItemData itemData;

    public int itemAmount;

    public InventorySlot(ItemData newItemData, int newItemAmount)
    {
        itemData = newItemData;
        itemAmount = newItemAmount;
    }
}