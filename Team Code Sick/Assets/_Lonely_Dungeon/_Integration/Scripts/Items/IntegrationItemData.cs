using UnityEngine;

public enum ItemType // Not sure what kind of Items we want yet, feel free to add or remove if you want.
{
    Weapon,
    Armor,
    Consumable,
    Junk,
    Currency
}

// Lets us make assets in the unity create menu
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class IntegrationItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public ItemType itemType;

    [TextArea]
    public string description;

    public bool stackable;
    public int maxStack = 1;

    public int value;
}