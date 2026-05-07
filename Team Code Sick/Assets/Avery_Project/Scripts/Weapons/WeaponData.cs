using UnityEngine;

[CreateAssetMenu(fileName = "SO_NewWeapon", menuName = "Avery Project/Weapons/Weapon Data")]
public class WeaponData : ScriptableObject
{
    [Header("Weapon Info")]
    public string weaponName = "Basic Gun";

    [Header("Weapon Stats")]
    public int damage = 10;
    public float fireRate = 0.25f;
    public float range = 20f;
}