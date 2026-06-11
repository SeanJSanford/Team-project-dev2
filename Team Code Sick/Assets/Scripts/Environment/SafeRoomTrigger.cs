using UnityEngine;

public class SafeRoomTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            gamemanager.instance.InSafeRoom();
            gamemanager.instance.playerInSafeRoom = true;
        }
    }
}
