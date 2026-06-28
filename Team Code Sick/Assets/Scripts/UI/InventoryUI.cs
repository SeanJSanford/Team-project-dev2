using UnityEngine;

// Handles opening, closing, and refreshing the inventory UI.
public class InventoryUI : MonoBehaviour
{
    public static InventoryUI instance;

    [SerializeField] private GameObject inventoryPanel;

    [SerializeField] private Inventory inventory;

    [SerializeField] private Inventory targetInventory;

    [SerializeField] private bool toggleWithI = true;

    private InventorySlotUI[] inventorySlotUIs;

    private bool inventoryIsOpen;

    public bool IsInventoryOpen
    {
        get
        {
            return inventoryPanel != null && inventoryPanel.activeSelf;
        }
    }

    private void Awake()
    {
        if (inventoryPanel == null)
        {
            Debug.LogError(
                "Inventory Panel is not assigned on " +
                gameObject.name
            );

            return;
        }

        inventorySlotUIs =
            inventoryPanel.GetComponentsInChildren<InventorySlotUI>(true);
        instance = this;
    }

    private void Start()
    {
        if (inventory == null && gamemanager.instance != null)
        {
            inventory = gamemanager.instance.GetComponent<Inventory>();
        }

        if (inventory == null)
        {
            Debug.LogError(
                "No Inventory assigned to " +
                gameObject.name
            );

            return;
        }

        // Only reload if this inventory was previously saved.
        if (inventory.HasSavedInventory())
        {
            inventory.ReloadInventory();
        }

        if (toggleWithI)
        {
            inventoryPanel.SetActive(false);
            inventoryIsOpen = false;
        }
        else
        {
            inventoryPanel.SetActive(true);
            inventoryIsOpen = true;
        }

        RefreshInventoryUI();
    }

    private void Update()
    {
        if (toggleWithI && Input.GetKeyDown(KeyCode.I))
        {
            inventoryIsOpen = !inventoryIsOpen;

            inventoryPanel.SetActive(inventoryIsOpen);

            if (inventoryIsOpen)
            {
                Time.timeScale = 0f;
            }
            else
            {
               
                if (!gamemanager.instance.isPaused)
                {
                    Time.timeScale = 1f;
                }
            }

            RefreshInventoryUI();
        }

        if (inventoryPanel != null && inventoryPanel.activeSelf)
        {
            RefreshInventoryUI();
        }
    }

    public void RefreshInventoryUI()
    {
        if (inventory == null || inventorySlotUIs == null)
            return;

        for (int i = 0; i < inventorySlotUIs.Length; i++)
        {
            if (i < inventory.inventorySlots.Count)
            {
                inventorySlotUIs[i].SetSlot(
                    inventory.inventorySlots[i],
                    inventory,
                    targetInventory
                );
            }
            else
            {
                inventorySlotUIs[i].ClearSlot();
            }
        }
    }

    public void SortInventory()
    {
        inventory.SortInventory();
        RefreshInventoryUI();
    }
}