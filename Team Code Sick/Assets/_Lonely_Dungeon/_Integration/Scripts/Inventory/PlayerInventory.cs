using System.Collections.Generic;
using UnityEngine;

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

/*
========================================================
Project: Team Code Sick
Script: PlayerInventory.cs

Primary Developer:
- Heather

Integration Support:
- Avery Wilson

System Category:
- Inventory System
- Backend Data Management
- Item Storage Framework

Purpose:
- Stores and manages player inventory data during gameplay.
- Handles adding, removing, and stacking collected items.

Current Features:
- Inventory slot management
- Stackable item support
- Item quantity tracking
- Inventory capacity limits
- Debug inventory printing
- Null safety validation

Connected Team Systems:
- Heather: Item templates / loot systems
- Avery: Future stat/equipment integration
- Sean: Enemy loot/combat interaction support
- Nilo: Gameplay flow integration
- Future UI systems

Design Philosophy:
This script intentionally manages only backend
inventory data and storage logic.

Responsibilities intentionally excluded:
- Inventory UI rendering
- Equipment handling
- Item stat application
- Drag/drop interactions
- Item effect execution

These systems should remain modular and
handled separately to reduce system overlap.

Why This Separation Exists:
Separating backend inventory logic from UI
and gameplay systems:
- Simplifies debugging
- Improves scalability
- Reduces Git conflicts
- Supports future feature expansion

Development Notes:
- Uses InventorySlot objects for clean data separation.
- Stackable items merge into existing inventory stacks.
- Non-stackable items create unique slots.
- Temporary debug key (I) used for development testing.
- Built as the foundational backend inventory framework.

Current Limitations:
- No inventory UI
- No item sorting
- No drag/drop support
- No save/load support
- No equipment system
- No hotbar integration

Future Expansion Ideas:
- Inventory UI grid
- Equipment slots
- Drag/drop interactions
- Save/load persistence
- Currency systems
- Item rarity support
- Tooltips and descriptions
- Crafting integration
========================================================
*/