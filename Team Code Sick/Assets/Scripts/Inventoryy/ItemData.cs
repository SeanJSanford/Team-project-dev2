using UnityEngine;

public enum ItemTypeX
{
    Weapon,
    Armor,
    Consumable,
    Junk,
    Currency
}

// Lets us make assets in the unity create menu.
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public ItemTypeX itemType;

    [TextArea]
    public string description;

    public bool stackable;
    public int maxStack = 1;

    public int value;
}