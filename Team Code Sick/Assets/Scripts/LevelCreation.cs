using UnityEngine;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;

public class LevelCreation : MonoBehaviour
{
    [SerializeField] int size;
    [SerializeField] GameObject restRoomFloor;
    [SerializeField] GameObject tunnelFloor;
    [SerializeField] GameObject wall;
    [SerializeField] GameObject FightRoomFloor;
    [SerializeField] GameObject ChestRoomFloor;
    [SerializeField] GameObject StoreRoomFloor;

    List<List<int>> grid = new List<List<int>>();
    List<(int x, int y)> allCenters = new List<(int x, int y)>();
    List<(int x, int y)> directions = new List<(int x, int y)> { (-1, 0), (0, 1), (1, 0), (0, -1) };

    List<GameObject> allPrefabs;

    enum Values
    {
        EMPTY,
        TUNNEL,
        WALL,
        REST,
        FIGHT,
        CHEST,
        STORE,
        BOSS
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        allPrefabs = new List<GameObject> { tunnelFloor, wall, restRoomFloor, FightRoomFloor, ChestRoomFloor, StoreRoomFloor };

        

        for (int i = 0; i < 10; i++)
        {
            allCenters = new List<(int x, int y)>();
            grid = new List<List<int>>();
            CreateLevel(400 * i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateLevel(int spacing)
    {

        for (int y = 0; y < size; y++)
        {
            List<int> row = new List<int>();
            for (int x = 0; x < size; x++)
            {
                row.Add(1);
            }
            grid.Add(row);
        }

        // UnityEngine.Random.InitState(0);

        StartGrid();

        int value;

        for (int row = 0; row < size; row++)
        {
            for (int col = 0; col < size; col++)
            {
                value = grid[row][col];
                if (value == (int)Values.WALL)
                {
                    Instantiate(allPrefabs[value - 1], new Vector3(10 * col - 120 + spacing, 5, 10 * row - 120), Quaternion.identity);
                }
                else if (value != (int)Values.EMPTY)
                {
                    Instantiate(allPrefabs[value - 1], new Vector3(10 * col - 120 + spacing, 0, 10 * row - 120), Quaternion.identity);
                }
            }
        }
    }
    void StartGrid()
    {
        /*
         * This will initialize the grid of this said level in a 2D Array.
         * 0 Empty
         * 1 is Safe Area/Rest Area
         * 2 Tunnel
         * 3 Wall
         * 4 Fight Room
         * 5 Chest Room
         * 6 Store
         * 7 Boss Room
         */

        List<List<(int x, int y)>> allExits = new List<List<(int x, int y)>>();

        (int x, int y) SafeAreaSize = (3, 5);
        (int x, int y) FightRoomSize = (5, 5);
        (int x, int y) ChestRoomSize = (3, 3);
        (int x, int y) StoreSize = (5, 3);

        List<(int x, int y)> allSizes = new List<(int x, int y)> { SafeAreaSize, FightRoomSize, ChestRoomSize, StoreSize };

        int wallThickness = 1; // This is the walls that go around the room
        int roomSeparation = 1; // This is so the walls are not right next to each other

        int startX; // For Random Numbers
        int endX;
        int startY;
        int endY;

        int xPos; // For center of rooms
        int yPos;

        int xDiff; // For Distance of Rooms
        int yDiff;

        bool possibleCenter; // For testing if the center is possible with no overlap

        (int x, int y) roomSize;

        for (int sizeIndex = 0; sizeIndex < allSizes.Count; sizeIndex++)
        {
            for (int _ = 0; _ < 1000; _++)
            {
                possibleCenter = true;

                roomSize = allSizes[sizeIndex];

                startX = wallThickness + (int)(roomSize.x / 2);
                endX = size - (int)(roomSize.x / 2) - wallThickness;
                startY = wallThickness + (int)(roomSize.y / 2);
                endY = size - (int)(roomSize.y / 2) - wallThickness;

                xPos = UnityEngine.Random.Range(startX, endX); // The 2 is to get the half and getting the floor. The minus 1 in the second param is the index out of bound
                yPos = UnityEngine.Random.Range(startY, endY);

                (int x, int y) currentCenter = (xPos, yPos);
                for (int centerToCompare = 0; centerToCompare < allCenters.Count; centerToCompare++)
                {
                    (xDiff, yDiff) = Utility.instance.ManhattanDistance(allCenters[centerToCompare], currentCenter);
                    possibleCenter = possibleCenter && (xDiff > (int)(allSizes[centerToCompare].x / 2) + (wallThickness * 2) + roomSeparation + (int)(roomSize.x / 2) || yDiff > (int)(allSizes[centerToCompare].y / 2) + (wallThickness * 2) + roomSeparation + (int)(roomSize.y / 2));
                }
                if (possibleCenter)
                {
                    for (int y = yPos - startY; y <= yPos + startY; y++)
                    {
                        for (int x = xPos - startX; x <= xPos + startX; x++)
                        {
                            if (y == yPos - startY || y == yPos + startY || x == xPos - startX || x == xPos + startX)
                                grid[y][x] = (int)Values.WALL;
                            else
                                grid[y][x] = (int)Values.REST + sizeIndex; // sizeIndex will be the current room we are in, it be offset by REST
                        }
                    }
                    allCenters.Add(currentCenter);
                    List<(int x, int y)> exits = new List<(int x, int y)> { (currentCenter.x - startX, currentCenter.y), (currentCenter.x, currentCenter.y + startX), (currentCenter.x + startX, currentCenter.y), (currentCenter.x, currentCenter.y - startX) };
                    allExits.Add(exits);
                    break;
                }
            }
        }
    }
}
