using UnityEngine;

// Represents a possible loot drop for an enemy.
[System.Serializable]
public class LootEntry
{
    public ItemData itemData;

    [Range(0f, 1f)]
    public float dropChance = 0.5f;

    public int minimumAmount = 1;

    public int maximumAmount = 1;
}