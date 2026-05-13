using Unity.VisualScripting;
using UnityEngine;

public class FightRoomTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!gamemanager.instance.playerInRoom)
        {
            gamemanager.instance.playerInRoom = true;
            (int x, int y) playerGridPosition = ((int)gamemanager.instance.player.transform.position.x / gamemanager.instance.unitSize, (int)gamemanager.instance.player.transform.position.z / gamemanager.instance.unitSize);
            int roomIndex = -1;
            for (int currentCenter = 0; currentCenter < LevelCreation.instance.allCenters.Count; currentCenter++)
            {
                (int x, int y) distanceToCenter = Utility.instance.ManhattanDistance(playerGridPosition, LevelCreation.instance.allCenters[currentCenter]);
                if(distanceToCenter.x < (int)(LevelCreation.instance.FightRoomSize.x / 2 + 0.5f) && distanceToCenter.y < (int)(LevelCreation.instance.FightRoomSize.y / 2 + 0.5f))
                {
                    roomIndex = currentCenter;
                    break;
                }
            }
            if (!gamemanager.instance.finishedRooms.Contains(roomIndex))
            {
                gamemanager.instance.currentRoom = roomIndex;
            }
        }
    }
}
