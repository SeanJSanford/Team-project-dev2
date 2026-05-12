using UnityEngine;

public class WorldItemPickup : MonoBehaviour
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

/*
========================================================
Project: Team Code Sick
Script: WorldItemPickup.cs

Primary Developer:
- Heather

Integration Support:
- Avery Wilson

System Category:
- Inventory System
- World Pickup System
- Loot Collection Framework

Purpose:
- Represents a physical item object placed
  into the game world that players can collect.

Current Features:
- Stores item data references
- Stores pickup quantity
- Attempts inventory insertion
- Self-destroys after successful collection
- Supports inventory-full handling

Connected Team Systems:
- Heather: Inventory / loot architecture
- Sean: Enemy death and loot spawning integration
- Avery: Future stat/equipment integration
- Nilo: Gameplay flow oversight
- Future inventory UI systems

Example Gameplay Flow:
Enemy Dies ->
EnemyLoot Rolls Table ->
Spawn WorldItemPickup ->
Player Interacts With Pickup ->
Inventory Attempts AddItem() ->
Pickup Destroys On Success

Design Philosophy:
This object intentionally represents only the
WORLD VERSION of an item.

Responsibilities intentionally excluded:
- Item gameplay behavior
- Stat modification logic
- Equipment handling
- Consumable effects
- Inventory UI logic

These responsibilities remain separated into
other gameplay systems for modularity.

Why This Separation Exists:
Separating world pickups from inventory
and gameplay behavior:
- Simplifies debugging
- Improves scalability
- Supports modular item systems
- Reduces hardcoded dependencies

Development Notes:
- Pickup remains in the world if inventory insertion fails.
- Supports future systems such as:
    - Inventory full handling
    - Multiplayer loot ownership
    - Pickup radius systems
    - Vacuum collection systems
    - Delayed despawn logic
- Built as the bridge between:
    - Loot systems
    - World interactions
    - Inventory storage

Current Limitations:
- No pickup animation
- No audio feedback
- No UI tooltip prompts
- No rarity visuals
- No despawn timer
- No multiplayer ownership

Future Expansion Ideas:
- Magnetic pickup systems
- Loot rarity VFX
- Timed despawn
- Multiplayer loot permissions
- Pickup sounds/animations
- Interaction prompts
- Auto-pickup systems
========================================================
*/