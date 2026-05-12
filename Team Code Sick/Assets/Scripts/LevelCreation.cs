using UnityEngine;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;
using Unity.Mathematics;

public class LevelCreation : MonoBehaviour
{
    [SerializeField] int size;
    [SerializeField] int worldSize;
    [SerializeField] GameObject emptyFloor;
    [SerializeField] GameObject restRoomFloor;
    [SerializeField] GameObject tunnelFloor;
    [SerializeField] GameObject wall;
    [SerializeField] GameObject FightRoomFloor;
    [SerializeField] GameObject ChestRoomFloor;
    [SerializeField] GameObject StoreRoomFloor;

    public List<List<int>> grid = new List<List<int>>();
    public List<List<int>> worldGrid = new List<List<int>>();
    public List<(int x, int y)> allCenters = new List<(int x, int y)>();
    public List<(int x, int y)> directions = new List<(int x, int y)> { (0, -1), (0, 1), (-1, 0), (1, 0) };
    public List<(int x, int y)> roomConnections = new List<(int x, int y)>();

    List<GameObject> allPrefabs;

   public enum Values
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

    public enum Directions
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        allPrefabs = new List<GameObject> { emptyFloor, tunnelFloor, wall, restRoomFloor, FightRoomFloor, ChestRoomFloor, StoreRoomFloor };

        UnityEngine.Random.InitState(2);

        for (int y = 0; y < worldSize; y++)
        {
            List<int> row = new List<int>();
            for (int x = 0; x < worldSize; x++)
            {
                row.Add(0);
            }
            worldGrid.Add(row);
        }

        for (int i = 0; i < worldSize; i++)
        {
            for (int j = 0; j < worldSize; j++)
            {
                allCenters = new List<(int x, int y)>();
                grid = new List<List<int>>();
                CreateLevel(size * 10 * i + 10 * i, size * 10 * j + 10 * j, (i, j));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateLevel(int spacingX, int spacingY, (int x, int y) playerPos)
    {

        for (int y = 0; y < size; y++)
        {
            List<int> row = new List<int>();
            for (int x = 0; x < size; x++)
            {
                row.Add(0);
            }
            grid.Add(row);
        }

        StartGrid(playerPos);

        int value;

        for (int row = 0; row < size; row++)
        {
            for (int col = 0; col < size; col++)
            {
                value = grid[row][col];
                if (value == (int)Values.WALL)
                {
                    Instantiate(allPrefabs[value], new Vector3(10 * col - 120 + spacingX, 5, 10 * row - 120 + spacingY), Quaternion.identity);
                }
                else if (value != (int)Values.EMPTY)
                {
                    Instantiate(allPrefabs[value], new Vector3(10 * col - 120 + spacingX, 0, 10 * row - 120 + spacingY), Quaternion.identity);
                }
            }
        }
    }
    void StartGrid((int x, int y) playerPos)
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

        (int x, int y) currentCenter;

        List <(int x, int y)> rooms = new List<(int x, int y)> { SafeAreaSize, FightRoomSize, ChestRoomSize, StoreSize, SafeAreaSize, FightRoomSize }; // The order has to be the exact same as the first 4, it will break otherwise
        List<float> chances = new List<float> { 1f, 1f, 1f, 0.1f, 0f, 0.5f}; // This  should have 1 number per size above, sizes will be repeated
        List<int> notAdded = new List<int>();

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

        // Room Spawns

        for (int sizeIndex = 0; sizeIndex < rooms.Count; sizeIndex++)
        {
            if (UnityEngine.Random.Range(0.0f, 1.0f) <= chances[sizeIndex])
            {
                for (int _ = 0; _ < 1000; _++)
                {
                    possibleCenter = true;

                    roomSize = rooms[sizeIndex];

                    startX = wallThickness + (int)(roomSize.x / 2);
                    endX = size - (int)(roomSize.x / 2) - wallThickness;
                    startY = wallThickness + (int)(roomSize.y / 2);
                    endY = size - (int)(roomSize.y / 2) - wallThickness;

                    xPos = UnityEngine.Random.Range(startX, endX); // The 2 is to get the half and getting the floor. The minus 1 in the second param is the index out of bound
                    yPos = UnityEngine.Random.Range(startY, endY);

                    currentCenter = (xPos, yPos);
                    for (int centerToCompare = 0; centerToCompare < allCenters.Count; centerToCompare++)
                    {
                        if(!notAdded.Contains(centerToCompare))
                        {
                            (xDiff, yDiff) = Utility.instance.ManhattanDistance(allCenters[centerToCompare], currentCenter);
                            possibleCenter = possibleCenter && (xDiff > (int)(rooms[centerToCompare].x / 2) + (wallThickness * 2) + roomSeparation + (int)(roomSize.x / 2) || yDiff > (int)(rooms[centerToCompare].y / 2) + (wallThickness * 2) + roomSeparation + (int)(roomSize.y / 2));
                        }
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
                                    grid[y][x] = (int)Values.REST + (sizeIndex % 4); // sizeIndex will be the current room we are in, it be offset by REST
                            }
                        }
                        allCenters.Add(currentCenter);
                        List<(int x, int y)> exits = new List<(int x, int y)>();
                        foreach ((int x, int y) direction in directions)
                        {
                            exits.Add((currentCenter.x + (direction.x * startX), currentCenter.y + (direction.y * startY)));
                        }
                        allExits.Add(exits);
                        break;
                    }
                }
            }
            else
            {
                notAdded.Add(sizeIndex);
            }
        }

        // Making tunnels connecting rooms

        List<(int x, int y)> allCentersCopy = new List<(int x, int y)>(allCenters);

        List<(int x, int y)> tunnel;

        List<int> obstacles = new List<int> { (int)Values.WALL, (int)Values.REST, (int)Values.FIGHT, (int)Values.CHEST, (int)Values.STORE, (int)Values.BOSS };

        int maxDistance = (int)(size * 1.25f); // Percentage of size

        (int x, int y) centerToCheck;

        int leastDistance;
        int closestExitCurrentCenter;
        int closestExitCheckingCenter;

        (int x, int y) currentCenterExit;
        (int x, int y) checkingCenterExit;

        (int x, int y) start;
        (int x, int y) end;


        for (int currentCenterIndex = allCentersCopy.Count - 1; currentCenterIndex >= 0; currentCenterIndex--)
        {
            currentCenter = allCentersCopy[currentCenterIndex];
            allCentersCopy.RemoveAt(currentCenterIndex);
            for (int centerToCheckIndex = 0; centerToCheckIndex < allCentersCopy.Count; centerToCheckIndex++)
            {
                centerToCheck = allCentersCopy[centerToCheckIndex];
                leastDistance = size * 2;
                closestExitCurrentCenter = 0;
                closestExitCheckingCenter = 0;
                if (Utility.instance.AddManhattanDistance(currentCenter, centerToCheck) <= maxDistance)
                {
                    for (int currentCenterExitIndex = 0; currentCenterExitIndex < allExits[currentCenterIndex].Count; currentCenterExitIndex++)
                    {
                        currentCenterExit = allExits[currentCenterIndex][currentCenterExitIndex];
                        for (int centerToCheckExitIndex = 0; centerToCheckExitIndex < allExits[centerToCheckIndex].Count; centerToCheckExitIndex++)
                        {
                            checkingCenterExit = allExits[centerToCheckIndex][centerToCheckExitIndex];
                            if (Utility.instance.AddManhattanDistance(currentCenterExit, checkingCenterExit) < leastDistance && Mathf.Abs(currentCenterExitIndex - centerToCheckIndex) % 2 == 0)
                            {
                                leastDistance = Utility.instance.AddManhattanDistance(currentCenterExit, checkingCenterExit);
                                closestExitCurrentCenter = currentCenterExitIndex;
                                closestExitCheckingCenter = centerToCheckExitIndex;
                            }
                        }
                    }
                    grid[allExits[currentCenterIndex][closestExitCurrentCenter].y][allExits[currentCenterIndex][closestExitCurrentCenter].x] = (int)Values.EMPTY;
                    grid[allExits[centerToCheckIndex][closestExitCheckingCenter].y][allExits[centerToCheckIndex][closestExitCheckingCenter].x] = (int)Values.EMPTY;

                    start = allExits[currentCenterIndex][closestExitCurrentCenter];
                    end = allExits[centerToCheckIndex][closestExitCheckingCenter];

                    tunnel = Utility.instance.AStarAlgorithm(start, end, size, grid, obstacles, directions);

                    if (tunnel != null)
                    {
                        foreach ((int x, int y) unit in tunnel)
                        {
                            grid[unit.y][unit.x] = (int)Values.TUNNEL;
                        }
                    }

                    grid[allExits[currentCenterIndex][closestExitCurrentCenter].y][allExits[currentCenterIndex][closestExitCurrentCenter].x] = (int)Values.TUNNEL;
                    grid[allExits[centerToCheckIndex][closestExitCheckingCenter].y][allExits[centerToCheckIndex][closestExitCheckingCenter].x] = (int)Values.TUNNEL;
                }
            }
        }

        // Making tunnels to sides

        int startingDistance = size * 2;
        int maxSize = size + 1;

        List<(int x, int y)> closestTunnels = new List<(int x, int y)> { (0, 0), (0, 0), (0, 0), (0, 0) };
        List<int> distances = new List<int> { startingDistance, startingDistance, startingDistance, startingDistance };

        (int x, int y) currentExit;

        for (int roomIndex = 0; roomIndex < allCenters.Count; roomIndex++)
        {
            for (int exitIndex = 0; exitIndex < allExits[roomIndex].Count; exitIndex++)
            {
                currentExit = allExits[roomIndex][exitIndex];
                (int x, int y) distance = Utility.instance.ManhattanDistance(currentExit, (-1, -1));
                if (distance.x < distances[(int)Directions.LEFT] && exitIndex == (int)Directions.LEFT)
                {
                    closestTunnels[(int)Directions.LEFT] = currentExit;
                    distances[(int)Directions.LEFT] = distance.x;
                }
                if (maxSize - distance.x <  distances[(int)Directions.RIGHT] && exitIndex == (int)Directions.RIGHT)
                {
                    closestTunnels[(int)Directions.RIGHT] = currentExit;
                    distances[(int)Directions.RIGHT] = maxSize - distance.x;
                }
                if (distance.y < distances[(int)Directions.UP] && exitIndex == (int)Directions.UP)
                {
                    closestTunnels[(int)Directions.UP] = currentExit;
                    distances[(int)Directions.UP] = distance.y;
                }
                if (maxSize - distance.y < distances[(int)Directions.DOWN] && exitIndex == (int)Directions.DOWN)
                {
                    closestTunnels[(int)Directions.DOWN] = currentExit;
                    distances[(int)Directions.DOWN] = maxSize - distance.y;
                }
            }
        }

        List<bool> possibleDirections = new List<bool> { true, true, true, true };

        for (int directionIndex = 0; directionIndex < directions.Count; directionIndex++)
        {
            (int x, int y) dir = directions[directionIndex];
            if (!(0 <= playerPos.x + dir.x && playerPos.x < worldGrid.Count && 0 <= playerPos.y + dir.y && playerPos.y < worldGrid[0].Count))
            {
                possibleDirections[directionIndex] = false;
            }
        }
        (int x, int y) lastChanged = (-1, -1);
        for (int shortestExitIndex = 0; shortestExitIndex < closestTunnels.Count; shortestExitIndex++)
        { 
            (int x, int y) shortestExit = closestTunnels[shortestExitIndex];
            lastChanged = (-1, -1);
            if (possibleDirections[shortestExitIndex])
            {
                for (int distance = 0; distance < distances[shortestExitIndex]; distance++)
                {
                    lastChanged = (shortestExit.x + (distance * directions[shortestExitIndex].x), shortestExit.y + (distance * directions[shortestExitIndex].y));
                    grid[lastChanged.y][lastChanged.x] = (int)Values.TUNNEL;
                }
            }
            roomConnections.Add(lastChanged);
        }

        // Surrounding Tunnels with Walls

        List<(int x, int y)> eightDirections = new List<(int x, int y)> { (0, -1), (0, 1), (-1, 0), (1, 0), (-1, -1), (1, 1), (1, -1), (-1, 1)};

        for (int col = 0; col < grid.Count; col++)
        { 
            for (int row = 0; row < grid[col].Count; row++)
            {
                if (grid[col][row] == (int)Values.TUNNEL)
                {
                    foreach ((int x, int y) dir in eightDirections)
                    {
                        (int x, int y) directionCheck = (dir.x + row, dir.y + col);
                        if (0 <= directionCheck.x && directionCheck.x < size && 0 <= directionCheck.y && directionCheck.y < size)
                        {
                            if (grid[directionCheck.y][directionCheck.x] == (int)Values.EMPTY)
                            {
                                grid[directionCheck.y][directionCheck.x] = (int)Values.WALL;
                            }
                        }
                    }
                }
            }
        }
    }
}
