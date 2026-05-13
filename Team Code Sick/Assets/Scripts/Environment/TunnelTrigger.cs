using UnityEngine;

public class TunnelTrigger : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if(gamemanager.instance.playerInRoom)
        {
            gamemanager.instance.playerInRoom = false;
            gamemanager.instance.currentRoom = -1;
            //gamemanager.instance.playerInRoom = false;
        }
    }

}