using System.Collections.Generic;
using UnityEngine;

public enum InventoryType
{
    Hub,
    Player
}

// Handles storing and managing items.
public class Inventory : MonoBehaviour
{
    public int maximumSlots = 27;

    public InventoryType inventoryType;

    public List<InventorySlot> inventorySlots = new List<InventorySlot>();

    private static List<InventorySlot> savedHubInventory =
        new List<InventorySlot>();

    private static List<InventorySlot> savedPlayerInventory =
        new List<InventorySlot>();

    private static bool hubInventoryHasBeenSaved;
    private static bool playerInventoryHasBeenSaved;

    // Attempts to add an item to the inventory.
    public bool AddItem(ItemData itemData, int itemAmount = 1)
    {
        if (itemData == null)
        {
            Debug.LogError("Cannot add item because ItemData is null.");
            return false;
        }

        if (itemAmount <= 0)
        {
            Debug.LogError("Cannot add item because item amount is 0 or less.");
            return false;
        }

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

    // Removes an amount of an item from the inventory
    public void RemoveItem(ItemData itemData, int itemAmount = 1)
    {
        if (itemData == null)
            return;

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

    public bool TransferItem(
        Inventory targetInventory,
        ItemData itemData,
        int itemAmount = 1)
    {
        if (targetInventory == null)
        {
            Debug.LogError("Target inventory is null.");
            return false;
        }

        if (itemData == null)
        {
            Debug.LogError("Cannot transfer an item because ItemData is null.");
            return false;
        }

        if (itemAmount <= 0)
        {
            Debug.LogError("Cannot transfer an item amount of 0 or less.");
            return false;
        }

        InventorySlot sourceSlot = null;

        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot != null && slot.itemData == itemData)
            {
                sourceSlot = slot;
                break;
            }
        }

        if (sourceSlot == null || sourceSlot.itemAmount < itemAmount)
        {
            Debug.Log("Not enough of that item to transfer.");
            return false;
        }

        if (!targetInventory.AddItem(itemData, itemAmount))
        {
            Debug.Log("Target inventory could not accept the item.");
            return false;
        }

        RemoveItem(itemData, itemAmount);

        return true;
    }

    public bool HasSavedInventory()
    {
        if (inventoryType == InventoryType.Hub)
        {
            return hubInventoryHasBeenSaved;
        }

        return playerInventoryHasBeenSaved;
    }

    // Use this function before you make a scene change
    public void SaveInventory()
    {
        List<InventorySlot> savedInventory;

        if (inventoryType == InventoryType.Hub)
        {
            savedInventory = savedHubInventory;
            hubInventoryHasBeenSaved = true;
        }
        else
        {
            savedInventory = savedPlayerInventory;
            playerInventoryHasBeenSaved = true;
        }

        savedInventory.Clear();

        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot == null || slot.itemData == null)
                continue;

            savedInventory.Add(
                new InventorySlot(slot.itemData, slot.itemAmount)
            );
        }

        Debug.Log(inventoryType + " inventory saved.");
    }

    // Use this one after to reload the inventory
    public void ReloadInventory()
    {
        List<InventorySlot> savedInventory;
        bool hasBeenSaved;

        if (inventoryType == InventoryType.Hub)
        {
            savedInventory = savedHubInventory;
            hasBeenSaved = hubInventoryHasBeenSaved;
        }
        else
        {
            savedInventory = savedPlayerInventory;
            hasBeenSaved = playerInventoryHasBeenSaved;
        }

        if (!hasBeenSaved)
        {
            Debug.Log(
                inventoryType +
                " inventory has not been saved yet."
            );

            return;
        }

        inventorySlots.Clear();

        foreach (InventorySlot slot in savedInventory)
        {
            if (slot == null || slot.itemData == null)
                continue;

            inventorySlots.Add(
                new InventorySlot(slot.itemData, slot.itemAmount)
            );
        }

        Debug.Log(inventoryType + " inventory reloaded.");
    }

    public void PrintInventory()
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            PrintInventory();
        }
    }
}