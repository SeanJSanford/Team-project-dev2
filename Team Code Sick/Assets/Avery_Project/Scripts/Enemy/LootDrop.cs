using UnityEngine;

public class LootDrop : MonoBehaviour
{
    public static bool ForceLootDrop = false;

    [SerializeField] private GameObject[] lootPrefabs;
    [SerializeField] private float dropChance = 0.25f;

    public void DropLoot()
    {
        if (lootPrefabs == null || lootPrefabs.Length == 0)
            return;

        bool shouldDrop = ForceLootDrop || Random.value <= dropChance;

        if (!shouldDrop)
            return;

        int index = Random.Range(0, lootPrefabs.Length);
        Instantiate(lootPrefabs[index], transform.position, Quaternion.identity);

        Debug.Log("Loot dropped.");
    }
}