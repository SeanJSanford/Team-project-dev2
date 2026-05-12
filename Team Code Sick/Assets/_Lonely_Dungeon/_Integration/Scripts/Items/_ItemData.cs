using UnityEngine;

public enum ItemType
{
    Weapon,
    Armor,
    Consumable,
    Junk,
    Currency
}

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

/*
========================================================
Project: Team Code Sick
Script: IntegrationItemData.cs
Related Type: ItemType.cs

Primary Developer:
- Heather

Integration Support:
- Avery Wilson

System Category:
- Inventory Framework
- Item Data Architecture
- ScriptableObject Item System

Purpose:
- ScriptableObject used to define reusable item data
  outside of runtime gameplay code.
- Stores item information used across inventory,
  loot, economy, and future equipment systems.

Why Use ScriptableObjects:
Using ScriptableObjects allows item definitions
to exist independently from gameplay scripts.

This enables designers/developers to:
- Create items directly in the Unity Editor
- Reuse item data across systems
- Balance items without changing code
- Expand content more efficiently

Example Assets:
- SO_HealthPotion
- SO_GoldPickup
- SO_Shotgun
- SO_FireArtifact

Current Features:
- Item categories through ItemType enum
- Stackable item support
- Inventory icon support
- Economy value support
- Item descriptions/tooltips
- ScriptableObject asset creation workflow

Connected Team Systems:
- Heather: Inventory / loot systems
- Avery: Future stat/equipment integration
- Sean: Enemy loot/combat integration support
- Nilo: Gameplay progression integration
- Future UI systems

Design Philosophy:
This class intentionally stores DATA ONLY.

Responsibilities intentionally excluded:
- Healing logic
- Weapon firing behavior
- Stat modification behavior
- Equipment handling
- Consumable execution

Gameplay functionality should remain modular
and handled by separate systems.

Example:
A health potion item may exist here,
but the actual healing logic should exist
inside a dedicated consumable/use system.

Why This Separation Exists:
Separating item data from gameplay behavior:
- Simplifies balancing
- Improves scalability
- Reduces hardcoded dependencies
- Supports future content expansion

Development Notes:
- Built using Unity ScriptableObjects for scalability.
- Supports rapid item creation during development.
- Intended as the foundational item framework
  for future inventory and progression systems.
- Designed to integrate with:
    - Loot systems
    - Inventory systems
    - Economy systems
    - Future equipment systems

Current Limitations:
- No rarity system
- No procedural modifiers
- No equipment stats
- No crafting tags
- No perk/artifact integration yet

Future Expansion Ideas:
- Artifact items
- Perk cards
- Crafting materials
- Ammo systems
- Procedural modifiers
- Item rarity tiers
- Equipment stat bonuses
- Corruption modifiers
========================================================
*/