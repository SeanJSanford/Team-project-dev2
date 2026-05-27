using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    private Inventory inventory;
    private InventoryUI inventoryUI;

    private bool GetReferencesFromGameManager()
    {
        if (gamemanager.instance == null)
        {
            Debug.LogError("No gamemanager instance found.");
            return false;
        }

        if (inventory == null)
            inventory = gamemanager.instance.GetComponent<Inventory>();

        if (inventoryUI == null)
            inventoryUI = FindFirstObjectByType<InventoryUI>(FindObjectsInactive.Include);

        if (inventory == null)
        {
            Debug.LogError("Inventory was not found on the GameManager.");
            return false;
        }

        return true;
    }

    private void OnTriggerEnter(Collider collision)
    {
        ItemPickup pickup = collision.GetComponent<ItemPickup>();

        if (pickup == null)
            return;

        if (!GetReferencesFromGameManager())
            return;

        bool pickedUpItem = pickup.PickupItem(inventory);

        if (pickedUpItem && inventoryUI != null)
        {
            inventoryUI.RefreshInventoryUI();
        }
    }
}