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

    public List<(int x, int y)> ReconstructPath((int x, int y) current, (int x, int y) start, Dictionary<(int x, int y), (int x, int y)> tunnel)
    {
        List<(int x, int y)> path = new List<(int x, int y)>();

        while (tunnel.ContainsKey(current))
        {
            path.Add(current);
            current = tunnel[current];
        }
        path.Add(start);
        path.Reverse();

        return path;
    }

    public int AddManhattanDistance((int x, int y) pos1, (int x, int y) pos2)
    {
        (int x, int y) = ManhattanDistance(pos1, pos2);
        return x + y;
    }

    public List<(int x, int y)> AStarAlgorithm((int x, int y) start, (int x, int y) end, int size, List<List<int>> grid, List<int> obstacles, List<(int x, int y)> directions)
    {
        SimplePriorityQueue<(int x, int y)> openSet = new SimplePriorityQueue<(int x, int y)>();

        openSet.Enqueue(start, 0);

        Dictionary<(int x, int y), (int x, int y)> tunnel = new Dictionary<(int x, int y), (int x, int y)>();

        Dictionary<(int x, int y), int> gScore = new Dictionary<(int x, int y), int>();

        gScore[start] = 0;

        (int x, int y) current;
        (int x, int y) neighbor;

        int tentativeGScore;
        int fScore;

        while (openSet.Count > 0)
        {

            current = openSet.Dequeue();

            if (current == end)
                return ReconstructPath(current, start, tunnel);

            foreach ((int dx, int dy) in directions)
            {
                neighbor = (current.x + dx, current.y + dy);

                if (neighbor.x < 0 || neighbor.x >= size || neighbor.y < 0 || neighbor.y >= size) continue; // Boundary Check

                if (obstacles.Contains(grid[neighbor.y][neighbor.x])) continue; // Check if we should ignore that value

                tentativeGScore = gScore[current] + 1;

                if (!gScore.ContainsKey(neighbor) || tentativeGScore < gScore[neighbor])
                {
                    tunnel[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    fScore = tentativeGScore + AddManhattanDistance(neighbor, end);
                    openSet.Enqueue(neighbor, fScore);
                }
            }

        }

        return new List<(int x, int y)>(); // No path found
    }
}