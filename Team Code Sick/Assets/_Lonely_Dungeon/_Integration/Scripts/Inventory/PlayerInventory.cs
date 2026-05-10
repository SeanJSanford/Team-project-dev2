using System.Collections.Generic;
using UnityEngine;

/*
 * PlayerInventory
 * 
 * Purpose:
 * Stores the player's collected items during gameplay.
 * 
 * How It Works:
 * - Items are stored as InventorySlot entries
 * - Stackable items try to combine with an existing stack first
 * - Non-stackable items create a new inventory slot
 * - If the inventory is full, AddItem returns false
 * 
 * Connected Systems:
 * - ItemPickup calls AddItem() when the player collects an item
 * - Inventory UI can read inventorySlots to display item icons/counts
 * - Future economy/perk systems may read this for upgrades or resources
 * 
 * Design Note:
 * This script should only manage backend inventory data.
 * UI display, item effects, and equipment logic should stay in separate systems.
 * 
 * Development Note:
 * The I-key console print is temporary debug behavior.
 * Later this should move to a DebugInventoryTester or DevMenu script.
 */

public class PlayerInventory : MonoBehaviour
{
    [Header("Inventory Settings")]
    public int maximumSlots = 27;

    [Header("Runtime Inventory")]
    public List<InventorySlot> inventorySlots = new List<InventorySlot>();

    // Attempts to add an item to the inventory.
    // Returns true if the item was successfully added.
    public bool AddItem(ItemData itemData, int itemAmount = 1)
    {
        if (itemData == null)
        {
            Debug.LogWarning("Tried to add a null item to inventory.");
            return false;
        }

        // Stackable items should merge into an existing slot first.
        if (itemData.stackable)
        {
            foreach (InventorySlot inventorySlot in inventorySlots)
            {
                if (inventorySlot.itemData == itemData)
                {
                    inventorySlot.itemAmount += itemAmount;
                    return true;
                }
            }
        }

        // If no stack was found, create a new slot if space is available.
        if (inventorySlots.Count < maximumSlots)
        {
            inventorySlots.Add(new InventorySlot(itemData, itemAmount));
            return true;
        }

        Debug.Log("Inventory is full.");
        return false;
    }

    // Removes a specific amount of an item from the inventory.
    // If the stack reaches 0, the slot is removed.
    public void RemoveItem(ItemData itemData, int itemAmount = 1)
    {
        if (itemData == null)
        {
            Debug.LogWarning("Tried to remove a null item from inventory.");
            return;
        }

        for (int i = 0; i < inventorySlots.Count; i++)
        {
            if (inventorySlots[i].itemData == itemData)
            {
                inventorySlots[i].itemAmount -= itemAmount;

                if (inventorySlots[i].itemAmount <= 0)
                {
                    inventorySlots.RemoveAt(i);
                }

                return;
            }
        }
    }

    private void Update()
    {
        // Temporary debug print.
        // Press I to list the current inventory in the console.
        if (Input.GetKeyDown(KeyCode.I))
        {
            PrintInventoryDebug();
        }
    }

    private void PrintInventoryDebug()
    {
        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot == null)
            {
                Debug.LogError("NULL SLOT FOUND");
                continue;
            }

            if (slot.itemData == null)
            {
                Debug.LogError("SLOT WITH NULL ITEMDATA FOUND");
                continue;
            }

            Debug.Log(slot.itemData.itemName + " x" + slot.itemAmount);
        }
    }
}