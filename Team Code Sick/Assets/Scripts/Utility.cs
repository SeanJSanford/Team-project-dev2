using UnityEngine;
using System;

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

}
