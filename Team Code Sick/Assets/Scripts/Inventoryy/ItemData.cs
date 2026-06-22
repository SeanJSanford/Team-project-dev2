using UnityEngine;

public enum ItemTypeX
{
    Weapon,
    Armor,
    Consumable,
    Junk,
    Currency
}

public enum WeaponType
{
    None,
    Sword,
    Rifle,
    Pistol,
    Shotgun,
}

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

    [Header("Weapon Data")]
    public Weapon weaponData;
}
