using UnityEngine;

// Handles opening, closing, and refreshing the inventory UI.
public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel;

    public Inventory playerInventory;

    public InventorySlotUI[] inventorySlotUIs;

    private bool inventoryIsOpen;

    private void Start()
    {
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
    }

    public void RefreshInventoryUI()
    {
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