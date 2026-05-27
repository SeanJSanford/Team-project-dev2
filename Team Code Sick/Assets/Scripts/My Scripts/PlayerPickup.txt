using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    private Inventory inventory;

    private void Start()
    {
        inventory = GetComponent<Inventory>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        ItemPickup pickup = collision.GetComponent<ItemPickup>();

        if (pickup != null)
        {
            pickup.PickupItem(inventory);
        }
    }
}