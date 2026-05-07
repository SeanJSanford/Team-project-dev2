using UnityEngine;
using System.Collections.Generic;
using Unity.Mathematics;
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

        CreateLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateLevel()
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

        UnityEngine.Random.InitState(0);

        StartGrid();

        int value;

        for (int row = 0; row < size; row++)
        {
            for (int col = 0; col < size; col++)
            {
                value = grid[row][col];
                if (value == (int)Values.WALL)
                {
                    Instantiate(allPrefabs[value - 1], new Vector3(10 * col - 120, 5, 10 * row - 120), Quaternion.identity);
                }
                else if (value != (int)Values.EMPTY)
                {
                    Instantiate(allPrefabs[value - 1], new Vector3(10 * col - 120, 0, 10 * row - 120), Quaternion.identity);
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
            while (true)
            {
                possibleCenter = true;

                roomSize = allSizes[sizeIndex];

                startX = wallThickness + (int)(roomSize.x / 2);
                endX = size - (int)(roomSize.x / 2) - wallThickness - 1;
                startY = wallThickness + (int)(roomSize.y / 2);
                endY = size - (int)(roomSize.y / 2) - wallThickness - 1;

                xPos = UnityEngine.Random.Range(startX, endX); // The 2 is to get the half and getting the floor. The minus 1 in the second param is the index out of bound
                yPos = UnityEngine.Random.Range(startY, endY);

                (int x, int y) currentCenter = (xPos, yPos);
                for (int centerToCompare = 0; centerToCompare < allCenters.Count; centerToCompare++)
                {
                    (xDiff, yDiff) = ManhattanDistance(allSizes[centerToCompare], currentCenter);
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

        /*
        startX = wallThickness + (int)(SafeAreaSize.x / 2);
        endX = size - (int)(SafeAreaSize.x / 2) - wallThickness - 1;
        startY = wallThickness + (int)(SafeAreaSize.y / 2);
        endY = size - (int)(SafeAreaSize.y / 2) - wallThickness - 1;

        xPos = UnityEngine.Random.Range(startX, endX); // The 2 is to get the half and getting the floor. The minus 1 in the second param is the index out of bound
        yPos = UnityEngine.Random.Range(startY, endY);

        (int x, int y) safeAreaCenter = (xPos, yPos);

        for (int y = yPos - startY; y <= yPos + startY; y++)
        {
            for (int x = xPos - startX; x <= xPos + startX; x++)
            {
                if (y == yPos - startY || y == yPos + startY || x == xPos - startX || x == xPos + startX)
                    grid[y][x] = (int)Values.WALL;
                else
                    grid[y][x] = (int)Values.REST;
            }
        }
        allCenters.Add(safeAreaCenter);
        List<(int x, int y)> safeAreaExits = new List<(int x, int y)> { (safeAreaCenter.x - startX, safeAreaCenter.y), (safeAreaCenter.x, safeAreaCenter.y + startX), (safeAreaCenter.x + startX, safeAreaCenter.y), (safeAreaCenter.x, safeAreaCenter.y - startX) };
        allExits.Add(safeAreaExits);

        // For it to be a possible center it has to meet the following rules:
        // The center x should be greater than:
        //    The center x of the size of the room being compared + wall thickness * 2 (since it is 2 rooms) + room separation + the center x of the size of the room trying to fit
        //    or
        //    The center y of the size of the room being compared + wall thickness * 2 (since it is 2 rooms) + room separation + the center y of the size of the room trying to fit

        while (true)
        {
            possibleCenter = true;

            startX = wallThickness + (int)(FightRoomSize.x / 2);
            endX = size - (int)(FightRoomSize.x / 2) - wallThickness - 1;
            startY = wallThickness + (int)(FightRoomSize.y / 2);
            endY = size - (int)(FightRoomSize.y / 2) - wallThickness - 1;

            xPos = UnityEngine.Random.Range(startX, endX);
            yPos = UnityEngine.Random.Range(startY, endY);

            (int x, int y) fightRoomOneCenter = (xPos,  yPos);
            foreach ((int x, int y) center in allCenters)
            {
                (xDiff, yDiff) = ManhattanDistance(center, fightRoomOneCenter);
                possibleCenter = possibleCenter && (xDiff > (int)(center.x / 2) + (wallThickness * 2) + roomSeparation + (int)(FightRoomSize.x / 2) || yDiff > (int)(center.y / 2) + (wallThickness * 2) + roomSeparation + (int)(FightRoomSize.y / 2));
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
                            grid[y][x] = (int)Values.FIGHT;
                    }
                }
                allCenters.Add(fightRoomOneCenter);
                List<(int x, int y)> fightRoomOneExits = new List<(int x, int y)> { (fightRoomOneCenter.x - startX, fightRoomOneCenter.y), (fightRoomOneCenter.x, fightRoomOneCenter.y + startX), (fightRoomOneCenter.x + startX, fightRoomOneCenter.y), (fightRoomOneCenter.x, fightRoomOneCenter.y - startX) };
                allExits.Add(fightRoomOneExits);
                break;
            }
            break;
        }

        while (true)
        {
            possibleCenter = true;

            startX = wallThickness + (int)(ChestRoomSize.x / 2);
            endX = size - (int)(ChestRoomSize.x / 2) - wallThickness - 1;
            startY = wallThickness + (int)(ChestRoomSize.y / 2);
            endY = size - (int)(ChestRoomSize.y / 2) - wallThickness - 1;

            xPos = UnityEngine.Random.Range(startX, endX);
            yPos = UnityEngine.Random.Range(startY, endY);

            (int x, int y) chestRoomOneCenter = (xPos, yPos);
            foreach ((int x, int y) center in allCenters)
            {
                (xDiff, yDiff) = ManhattanDistance(center, chestRoomOneCenter);
                possibleCenter = possibleCenter && (xDiff > (int)(center.x / 2) + (wallThickness * 2) + roomSeparation + (int)(ChestRoomSize.x / 2) || yDiff > (int)(center.y / 2) + (wallThickness * 2) + roomSeparation + (int)(ChestRoomSize.y / 2));
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
                            grid[y][x] = (int)Values.CHEST;
                    }
                }
                allCenters.Add(chestRoomOneCenter);
                List<(int x, int y)> chestRoomOneExits = new List<(int x, int y)> { (chestRoomOneCenter.x - startX, chestRoomOneCenter.y), (chestRoomOneCenter.x, chestRoomOneCenter.y + startX), (chestRoomOneCenter.x + startX, chestRoomOneCenter.y), (chestRoomOneCenter.x, chestRoomOneCenter.y - startX) };
                allExits.Add(chestRoomOneExits);
                break;
            }
            break;
        }


        */
    }

    (int x, int y) ManhattanDistance((int x, int y) pos1, (int x, int y) pos2)
    {
        int x = Math.Abs(pos1.x - pos2.x);
        int y = Math.Abs(pos1.y - pos2.y);
        return (x, y);
    }
}
