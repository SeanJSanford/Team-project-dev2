using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    private Inventory inventory;
    private InventoryUI inventoryUI;
    private playerMovement playerScript;

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
        {
            inventoryUI = FindFirstObjectByType<InventoryUI>(
                FindObjectsInactive.Include
            );
        }

        if (playerScript == null)
            playerScript = GetComponent<playerMovement>();

        return true;
    }

    private void OnTriggerEnter(Collider collision)
    {
        ItemPickup pickup = collision.GetComponent<ItemPickup>();

        if (pickup == null)
            return;

        if (!GetReferencesFromGameManager())
            return;

        bool pickedUpItem = pickup.PickupItem(
            inventory,
            playerScript
        );

        if (pickedUpItem &&
            pickup.pickupType == PickupType.InventoryItem &&
            inventoryUI != null)
        {
            inventoryUI.RefreshInventoryUI();
        }
    }
}