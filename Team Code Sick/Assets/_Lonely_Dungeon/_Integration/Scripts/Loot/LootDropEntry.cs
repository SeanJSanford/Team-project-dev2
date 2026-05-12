using UnityEngine;

[System.Serializable]

public class LootDropEntry
{
    [Header("Item Settings")]

    // Item definition that can be dropped.
    public ItemData itemData;

    [Header("Drop Chance")]

    // Percentage chance for this item to drop.
    // 0 = never drops
    // 1 = always drops
    [Range(0f, 1f)]
    public float dropChance = 0.5f;

    [Header("Drop Amount")]

    // Minimum amount that can spawn.
    public int minimumAmount = 1;

    // Maximum amount that can spawn.
    public int maximumAmount = 1;
}

/*
========================================================
Project: Team Code Sick
Script: LootDropEntry.cs

Primary Developer:
- Heather

Integration Support:
- Avery Wilson

System Category:
- Loot System
- Data Container
- Loot Table Architecture

Purpose:
- Defines a single possible item drop
  within an enemy loot table.

Current Features:
- Item reference storage
- Drop chance configuration
- Randomized quantity ranges
- Inspector-editable loot entries
- Serializable loot table support

Connected Team Systems:
- Heather: EnemyLoot / inventory systems
- Sean: Enemy death/combat integration
- Avery: Future luck/stat modifier integration
- Nilo: Gameplay progression oversight

How The System Works:
EnemyLoot contains arrays of LootDropEntry objects.

Each entry defines:
- What item can drop
- Chance of dropping
- Minimum quantity
- Maximum quantity

EnemyLoot rolls these entries during enemy death
to determine what rewards spawn.

Why Use [System.Serializable]:
Using [System.Serializable] allows loot entries
to appear directly inside Unity's Inspector
without requiring separate MonoBehaviours
or ScriptableObjects.

This improves:
- Workflow speed
- Loot table editing
- Rapid balancing
- Designer usability

Design Philosophy:
This class intentionally stores DATA ONLY.

Responsibilities intentionally excluded:
- Loot spawning
- Inventory handling
- Item behavior
- Economy balancing

Those responsibilities remain separated into
other gameplay systems.

Why This Separation Exists:
Separating loot data from gameplay behavior:
- Simplifies balancing
- Improves scalability
- Reduces hardcoded dependencies
- Supports modular reward systems

Development Notes:
- Built as lightweight modular loot data.
- Supports scalable loot table creation.
- Intended for future expansion into advanced
  reward generation systems.
- Designed to integrate cleanly with:
    - EnemyLoot
    - WorldItemPickup
    - PlayerInventory

Future Expansion Ideas:
- Item rarity weighting
- Luck modifiers
- Biome-specific loot
- Guaranteed drops
- Enemy level scaling
- Corruption modifiers
- Weighted drop pools
- Procedural loot generation
========================================================
*/