using UnityEngine;

public class TunnelTrigger : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gamemanager.instance.OutSafeRoom();
            gamemanager.instance.playerInSafeRoom = false;
            if (gamemanager.instance.playerInRoom)
            {
                gamemanager.instance.playerInRoom = false;
                gamemanager.instance.currentRoom = -1;
                gamemanager.instance.ExitRoom();
            } 
            gamemanager.instance.FinishedRoomOff();
        }
    }

}