using System.Collections.Generic;
using UnityEngine;

// Handles storing and managing items.
public class Inventory : MonoBehaviour
{
    public int maximumSlots = 27;

    public List<InventorySlot> inventorySlots = new List<InventorySlot>();

    // Attempts to add an item to the inventory.
    public bool AddItem(ItemData itemData, int itemAmount = 1)
    {
        // Try stacking onto an existing stack first.
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

        // Create a new slot if there is room.
        if (inventorySlots.Count < maximumSlots)
        {
            inventorySlots.Add(new InventorySlot(itemData, itemAmount));
            return true;
        }

        Debug.Log("Inventory is full.");
        return false;
    }

    // Removes an amount of an item from the inventory.
    public void RemoveItem(ItemData itemData, int itemAmount = 1)
    {
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            if (inventorySlots[i].itemData == itemData)
            {
                inventorySlots[i].itemAmount -= itemAmount;

                // Remove the slot entirely if empty.
                if (inventorySlots[i].itemAmount <= 0)
                {
                    inventorySlots.RemoveAt(i);
                }

                return;
            }
        }

    }

    // Press I to print the inventory to the console.
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
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
}