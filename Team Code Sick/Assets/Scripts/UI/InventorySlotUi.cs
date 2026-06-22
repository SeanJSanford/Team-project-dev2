using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

// Controls one visible inventory slot in the UI.
public class InventorySlotUI : MonoBehaviour, IPointerClickHandler
{
    public Image itemIconImage;

    public TMP_Text itemAmountText;

    private Inventory sourceInventory;
    private Inventory targetInventory;
    private ItemData currentItemData;

    public void SetSlot(
        InventorySlot inventorySlot,
        Inventory source,
        Inventory target)
    {
        sourceInventory = source;
        targetInventory = target;
        currentItemData = inventorySlot.itemData;

        itemIconImage.sprite = inventorySlot.itemData.icon;
        itemIconImage.enabled = true;

        itemAmountText.text = inventorySlot.itemAmount.ToString();
    }

    public void ClearSlot()
    {
        sourceInventory = null;
        targetInventory = null;
        currentItemData = null;

        itemIconImage.sprite = null;
        itemIconImage.enabled = false;

        itemAmountText.text = "";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (gamemanager.instance == null)
        {
            if (sourceInventory == null ||
                targetInventory == null ||
                currentItemData == null)
            {
                return;
            }

            sourceInventory.TransferItem(
                targetInventory,
                currentItemData,
                1
            );
        }

        ItemData oldWeapon = gamemanager.instance.playerScript.defaultWeapon;
        gamemanager.instance.playerScript.SetWeapon(currentItemData);
        currentItemData = oldWeapon;
        itemIconImage.sprite = currentItemData.icon;
        InventoryUI.instance.RefreshInventoryUI();
    }
}