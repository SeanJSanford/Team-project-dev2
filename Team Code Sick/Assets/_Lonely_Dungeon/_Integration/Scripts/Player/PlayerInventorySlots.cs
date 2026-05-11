using System;

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

/*
========================================================
Project: Team Code Sick
Script: IntegrationInventorySlot.cs

Primary Developer:
- Heather

Integration Support:
- Avery Wilson

System Category:
- Inventory System
- Inventory Data Structure
- Item Stack Management

Purpose:
- Represents a single item stack stored
  inside the player's inventory.

Current Features:
- Stores item references
- Stores stack quantities
- Supports stackable inventory behavior
- Serializable inventory slot data
- Constructor-based slot creation

Connected Team Systems:
- Heather: Inventory / loot architecture
- Avery: Future equipment/stat integration
- Nilo: Gameplay progression oversight
- Future inventory UI systems

How The System Works:
Each inventory slot stores:
- A reference to an ItemData object
- The quantity currently held

Example:
Slot 1:
- Health Potion
- Amount: 5

Slot 2:
- Gold
- Amount: 120

Why This Exists:
Separating inventory slots into their own class:
- Simplifies inventory management
- Improves scalability
- Supports stackable items cleanly
- Makes future UI integration easier

Instead of storing raw item lists directly,
PlayerInventory manages organized item stacks.

Why Use [Serializable]:
[Serializable] allows inventory slots to appear
inside Unity's Inspector while stored inside:
- Lists
- Arrays
- Serialized inventory systems

This improves:
- Debugging
- Workflow speed
- Inventory visualization
- Designer usability

Design Philosophy:
This class intentionally stores DATA ONLY.

Responsibilities intentionally excluded:
- Inventory UI rendering
- Item behavior
- Equipment logic
- Stat calculations
- Drag/drop handling

These systems should remain modular
and handled separately.

Why This Separation Exists:
Separating inventory stack data from gameplay
and UI systems:
- Simplifies debugging
- Improves scalability
- Reduces system overlap
- Supports future feature expansion

Development Notes:
- Built as lightweight backend inventory data.
- Designed to integrate cleanly with:
    - PlayerInventory
    - WorldItemPickup
    - Loot systems
    - Future inventory UI systems

Future Expansion Ideas:
- Slot locking
- Durability
- Stack splitting
- Favorite items
- Equipment states
- Item rarity visuals
- Hotbar support
- Inventory sorting
========================================================
*/