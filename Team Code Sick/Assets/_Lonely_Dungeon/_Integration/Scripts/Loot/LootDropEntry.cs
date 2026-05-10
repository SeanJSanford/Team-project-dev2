using UnityEngine;

/*
 * LootDropEntry
 * 
 * Purpose:
 * Represents a single possible item drop inside
 * an enemy loot table.
 * 
 * How It Works:
 * Each LootDropEntry defines:
 * - What item can drop
 * - The chance of it dropping
 * - The possible quantity range
 * 
 * IntegrationEnemyLoot uses these entries
 * to determine what loot should spawn when
 * an enemy dies.
 * 
 * Example:
 * Gold Pickup
 * - 75% drop chance
 * - Amount between 5 and 15
 * 
 * Health Potion
 * - 20% drop chance
 * - Amount between 1 and 2
 * 
 * Why Use Serializable:
 * [System.Serializable] allows this class
 * to appear inside the Unity Inspector
 * without needing a separate MonoBehaviour
 * or ScriptableObject.
 * 
 * Connected Systems:
 * - IntegrationEnemyLoot
 * - IntegrationItemPickup
 * - PlayerInventory
 * - Future rarity/luck systems
 * 
 * Future Expansion Ideas:
 * - Item rarity weighting
 * - Corruption scaling modifiers
 * - Minimum enemy level requirements
 * - Biome-specific drops
 * - Guaranteed drops
 * - Weighted loot pools
 */

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