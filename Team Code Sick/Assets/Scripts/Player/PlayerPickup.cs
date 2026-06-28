using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    private Inventory inventory;
    private InventoryUI inventoryUI;
    private playerMovement playerScript;

    [Header("Pickup Audio")]
    [SerializeField] private AudioSource pickupAudio;
    [SerializeField] private AudioClip itemPickupSound;
    [SerializeField] private AudioClip healthPickupSound;

    [Range(0f, 1f)]
    [SerializeField] private float pickupVolume = 0.5f;

    private void Awake()
    {
        if (pickupAudio == null)
        {
            pickupAudio = GetComponent<AudioSource>();
        }
    }

    private bool GetReferencesFromGameManager()
    {
        if (gamemanager.instance == null)
        {
            Debug.LogError("No gamemanager instance found.");
            return false;
        }

        if (inventory == null)
            inventory = gamemanager.instance.GetComponent<Inventory>();

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

        PickupType pickupType = pickup.pickupType;

        bool pickedUpItem = pickup.PickupItem(
            inventory,
            playerScript
        );

        if (pickedUpItem)
        {
            PlayPickupSound(pickupType);
        }

        if (pickedUpItem &&
            pickupType == PickupType.InventoryItem &&
            inventoryUI != null)
        {
            inventoryUI.RefreshInventoryUI();
        }
    }

    private void PlayPickupSound(PickupType pickupType)
    {
        if (pickupAudio == null)
            return;

        AudioClip clipToPlay = null;

        if (pickupType == PickupType.InventoryItem)
        {
            clipToPlay = itemPickupSound;
        }
        else if (pickupType == PickupType.Health)
        {
            clipToPlay = healthPickupSound;
        }

        if (clipToPlay != null)
        {
            pickupAudio.PlayOneShot(clipToPlay, pickupVolume);
        }
    }
}