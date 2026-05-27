using UnityEngine;

// Handles opening, closing, and refreshing the inventory UI.
public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel;

    private Inventory playerInventory;

    public InventorySlotUI[] inventorySlotUIs;

    private bool inventoryIsOpen;

    private void Start()
    {
        GetInventoryFromGameManager();

        inventoryPanel.SetActive(false);
        RefreshInventoryUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryIsOpen = !inventoryIsOpen;

            inventoryPanel.SetActive(inventoryIsOpen);

            RefreshInventoryUI();
        }

        if (inventoryIsOpen)
        {
            RefreshInventoryUI();
        }
    }

    private bool GetInventoryFromGameManager()
    {
        if (playerInventory != null)
            return true;

        if (gamemanager.instance == null)
        {
            Debug.LogError("No gamemanager instance found.");
            return false;
        }

        playerInventory = gamemanager.instance.GetComponent<Inventory>();

        if (playerInventory == null)
        {
            Debug.LogError("Inventory was not found on the GameManager.");
            return false;
        }

        return true;
    }

    public void RefreshInventoryUI()
    {
        if (!GetInventoryFromGameManager())
            return;

        for (int i = 0; i < inventorySlotUIs.Length; i++)
        {
            if (i < playerInventory.inventorySlots.Count)
            {
                inventorySlotUIs[i].SetSlot(playerInventory.inventorySlots[i]);
            }
            else
            {
                inventorySlotUIs[i].ClearSlot();
            }
        }
    }
}