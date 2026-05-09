using UnityEngine;
using System;
using System.Collections.Generic;
using Priority_Queue;

public class Utility : MonoBehaviour
{

    public static Utility instance;

    private void Awake()
    {
        instance = this;
    }

    public (int x, int y) ManhattanDistance((int x, int y) pos1, (int x, int y) pos2)
    {
        int x = Math.Abs(pos1.x - pos2.x);
        int y = Math.Abs(pos1.y - pos2.y);
        return (x, y);
    }

    public int AddManhattanDistance((int x, int y) pos1, (int x, int y) pos2)
    {
        (int x, int y) = ManhattanDistance(pos1, pos2);
        return x + y;
    }

    public List<(int x, int y)> AStarAlgorithm((int x, int y) start, (int x, int y) end, int size, List<List<int>> grid, List<int> obstacles)
    {
        List<(int cost, (int x, int y))> openSet = new List<(int cost, (int x, int y))>();

        Dictionary<(int x, int y), (int x, int y)> tunnel = new Dictionary<(int x, int y), (int x, int y)>();

        return new List<(int x, int y)>();
    }
}