using UnityEngine;

/*
 * ItemType
 * 
 * Purpose:
 * Defines the general category an item belongs to.
 * 
 * Why This Exists:
 * Item types help other systems understand how an item
 * should behave without hardcoding checks for every item.
 * 
 * Example Uses:
 * - Weapons can be equipped
 * - Consumables can be used
 * - Currency can increase player gold
 * - Armor can modify player stats
 * 
 * Future Expansion Ideas:
 * - Artifact
 * - Perk
 * - QuestItem
 * - Material
 * - Ammo
 */

public enum ItemType
{
    Weapon,
    Armor,
    Consumable,
    Junk,
    Currency
}

/*
 * IntegrationItemData
 * 
 * Purpose:
 * ScriptableObject used to define item data outside of code.
 * 
 * Why Use ScriptableObjects:
 * This allows designers/developers to create and edit items
 * directly in the Unity Editor without creating separate scripts
 * for every item.
 * 
 * Example Assets:
 * - SO_HealthPotion
 * - SO_GoldPickup
 * - SO_Shotgun
 * - SO_FireArtifact
 * 
 * Connected Systems:
 * - Inventory
 * - Loot drops
 * - Item pickups
 * - Economy systems
 * - Equipment systems
 * - Future perk/artifact systems
 * 
 * Design Note:
 * This class only stores item DATA.
 * Actual gameplay behavior should happen in other systems.
 * 
 * Example:
 * A health potion item exists here,
 * but the healing logic should exist in a consumable/use system.
 */

// Creates an item asset option in Unity's Create menu.
[CreateAssetMenu(fileName = "SO_NewItem", menuName = "Integration/Inventory/Item")]

public class IntegrationItemData : ScriptableObject
{
    [Header("Basic Item Info")]

    // Name displayed in inventory/UI.
    public string itemName;

    // Inventory or world icon.
    public Sprite icon;

    // General gameplay category.
    public ItemType itemType;

    [Header("Item Description")]

    // Longer text description for UI/tooltips.
    [TextArea]
    public string description;

    [Header("Stacking")]

    // Determines if multiple copies can exist in one inventory slot.
    public bool stackable;

    // Maximum amount allowed in a single stack.
    public int maxStack = 1;

    [Header("Economy")]

    // General item value used for shops/currency systems.
    public int value;
}