using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Controls one visible inventory slot in the UI.
public class InventorySlotUI : MonoBehaviour
{
    public Image itemIconImage;

    public TMP_Text itemAmountText;

    public void SetSlot(InventorySlot inventorySlot)
    {
        itemIconImage.sprite = inventorySlot.itemData.icon;
        itemIconImage.enabled = true;

        itemAmountText.text = inventorySlot.itemAmount.ToString();
    }

    public void ClearSlot()
    {
        itemIconImage.sprite = null;
        itemIconImage.enabled = false;

        itemAmountText.text = "";
    }
}