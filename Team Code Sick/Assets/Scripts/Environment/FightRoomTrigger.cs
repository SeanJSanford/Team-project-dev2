using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;

public class FightRoomTrigger : MonoBehaviour
{
    [SerializeField] GameObject door;

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
                if(distanceToCenter.x < (LevelCreation.instance.FightRoomSize.x - 1) && distanceToCenter.y < (LevelCreation.instance.FightRoomSize.y - 1))
                {
                    roomIndex = currentCenter;
                    break;
                }
            }
            if (!gamemanager.instance.finishedRooms.Contains(roomIndex))
            {
                gamemanager.instance.roomCleared = false;
                gamemanager.instance.currentRoom = roomIndex;
                for (int currentExitIndex = 0; currentExitIndex < LevelCreation.instance.allExits[roomIndex].Count; currentExitIndex++)
                {
                    (int x, int y) currentExit = LevelCreation.instance.allExits[roomIndex][currentExitIndex];
                    if (LevelCreation.instance.grid[currentExit.y][currentExit.x] == (int)LevelCreation.Values.TUNNEL)
                    {
                        gamemanager.instance.allDoors.Add(Instantiate(door, new Vector3(gamemanager.instance.unitSize * currentExit.x, 5, gamemanager.instance.unitSize * currentExit.y), Quaternion.identity));
                    }
                }
            }
            else if (gamemanager.instance.finishedRooms.Contains(roomIndex))
            {
                gamemanager.instance.roomCleared = true;
            }
        }
    }
}
