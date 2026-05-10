using UnityEngine;

/*
 * IntegrationItemPickup
 * 
 * Purpose:
 * Represents a physical item object placed in the game world
 * that the player can collect.
 * 
 * How It Works:
 * - Stores a reference to ItemData
 * - Stores the amount being carried by this pickup
 * - Attempts to add itself into a target inventory
 * - Destroys itself if the inventory accepts the item
 * 
 * Connected Systems:
 * - EnemyLoot / LootDrop systems
 * - PlayerInventory
 * - Player pickup collision systems
 * - Future inventory UI
 * 
 * Example Flow:
 * Enemy dies ->
 * Loot system spawns IntegrationItemPickup ->
 * Player collides with pickup ->
 * PickupItem() attempts inventory insert ->
 * Pickup object destroys itself
 * 
 * Design Note:
 * This object only represents the WORLD VERSION of an item.
 * It should not contain item behavior logic.
 * 
 * Important:
 * If AddItem() returns false,
 * the pickup remains in the world.
 * 
 * This allows:
 * - Inventory full handling
 * - Multiplayer loot interactions
 * - Future vacuum/pickup radius systems
 */

public class IntegrationItemPickup : MonoBehaviour
{
    [Header("Pickup Data")]

    // Reference to the item definition this pickup represents.
    public ItemData itemData;

    // Amount of the item stored in this pickup.
    public int itemAmount = 1;

    /*
     * PickupItem
     * 
     * Attempts to insert this item into the target inventory.
     * 
     * If successful:
     * - The world pickup is destroyed
     * 
     * If unsuccessful:
     * - The pickup remains in the world
     *   (usually because the inventory is full)
     */
    public void PickupItem(Inventory targetInventory)
    {
        // Safety check to prevent null inventory errors.
        if (targetInventory == null)
        {
            Debug.LogWarning("Pickup attempted with no target inventory.");
            return;
        }

        // Attempt to add the item into the inventory system.
        if (targetInventory.AddItem(itemData, itemAmount))
        {
            // Remove the pickup object from the world after collection.
            Destroy(gameObject);
        }
    }
}