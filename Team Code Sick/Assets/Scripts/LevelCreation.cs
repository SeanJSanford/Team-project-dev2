using UnityEngine;
using System.Collections.Generic;
using Unity.Mathematics;
using System;
using Unity.VisualScripting;

public class LevelCreation : MonoBehaviour
{
    [SerializeField] int size;

    List<List<int>> grid = new List<List<int>>();
    List<(int x, int y)> allCenters = new List<(int x, int y)>();
    List<(int x, int y)> directions = new List<(int x, int y)> { (-1, 0), (0, 1), (1, 0), (0, -1) };

    enum Values
    {
        EMPTY,
        REST,
        TUNNEL,
        WALL,
        FIGHT,
        CHEST,
        BOSS
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
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

        StartGrid();
    }

    // Update is called once per frame
    void Update()
    {
        
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
        int wallThickness = 1;

        int startX;
        int endX;
        int startY;
        int endY;

        int xPos;
        int yPos;

        startX = wallThickness + (int)(SafeAreaSize.x / 2);
        endX = size - (int)(SafeAreaSize.x / 2) - wallThickness - 1;
        startY = wallThickness + (int)(SafeAreaSize.y / 2);
        endY = size - (int)(SafeAreaSize.y / 2) - wallThickness - 1);

        xPos = UnityEngine.Random.Range(startX, endX); // The 2 is to get the half and getting the floor. The minus 1 in the second param is the index out of bound
        yPos = UnityEngine.Random.Range(startY, endY);

        (int x, int y) safeAreaCenter = (xPos, yPos);
        allCenters.Add(safeAreaCenter);

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
        List<(int x, int y)> safeAreaExits = new List<(int x, int y)> { (safeAreaCenter.x - startX, safeAreaCenter.y), (safeAreaCenter.x, safeAreaCenter.y + startX), (safeAreaCenter.x + startX, safeAreaCenter.y), (safeAreaCenter.x, safeAreaCenter.y - startX) };
        allExits.Add(safeAreaExits);

        while (true)
        { 
            
            

        }
    }
}
