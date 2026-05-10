using System;

/*
 * IntegrationInventorySlot
 * 
 * Purpose:
 * Represents a single item stack inside the player's inventory.
 * 
 * How It Works:
 * Each slot stores:
 * - A reference to an item definition
 * - The amount of that item currently held
 * 
 * Example:
 * Slot 1:
 * - Health Potion
 * - Amount: 5
 * 
 * Slot 2:
 * - Gold
 * - Amount: 120
 * 
 * Why This Exists:
 * Separating inventory slots into their own class
 * makes inventory management cleaner and easier to expand.
 * 
 * Instead of storing raw item lists directly,
 * the inventory can manage organized item stacks.
 * 
 * Connected Systems:
 * - PlayerInventory
 * - Inventory UI
 * - Item pickups
 * - Loot systems
 * - Future equipment systems
 * 
 * Future Expansion Ideas:
 * - Slot locking
 * - Durability
 * - Item rarity visuals
 * - Favorite/protected items
 * - Equipment state
 * - Stack splitting
 * 
 * Why Serializable:
 * [Serializable] allows this class to appear
 * inside the Unity Inspector when stored
 * inside Lists or Arrays.
 */

[Serializable]

public class IntegrationInventorySlot
{
    // Item definition stored in this inventory slot.
    public ItemData itemData;

    // Current amount of the item in this stack.
    public int itemAmount;

    /*
     * IntegrationInventorySlot Constructor
     * 
     * Creates a new inventory slot using:
     * - An item reference
     * - An item quantity
     * 
     * Used when:
     * - Creating a brand new inventory stack
     * - Adding a new item to the inventory
     */
    public IntegrationInventorySlot(ItemData newItemData, int newItemAmount)
    {
        itemData = newItemData;
        itemAmount = newItemAmount;
    }
}