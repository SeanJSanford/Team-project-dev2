using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DrawMiniMap : MonoBehaviour
{
    public RawImage image;

    Texture2D tex;

    List<Color> colors;

    bool updated = false;

    void Update()
    {
        if (!updated && LevelCreation.instance.grid.Count > 0)
        {
            tex = new Texture2D(LevelCreation.instance.size, LevelCreation.instance.size);

            //colors = new List<Color> { Color.red, LevelCreation.instance.tunnelFloor.GetComponent<Renderer>().material.color, LevelCreation.instance.wall.GetComponent<Renderer>().material.color, LevelCreation.instance.safeRoomFloor.GetComponent<Renderer>().material.color, LevelCreation.instance.FightRoomFloor.GetComponent<Renderer>().material.color };
            colors = new List<Color> { Color.red, Color.black, Color.brown, Color.green, Color.pink };

            image.texture = tex;

            for (int row = 0; row < LevelCreation.instance.size; row++)
            {
                for (int col = 0; col < LevelCreation.instance.size; col++)
                {
                    int unit = LevelCreation.instance.grid[row][col];
                    if (unit != (int)LevelCreation.Values.EMPTY)
                        tex.SetPixel(col, row, colors[unit]);
                }
            }
            tex.SetPixel(gamemanager.instance.playerGridPosition.x, gamemanager.instance.playerGridPosition.y, Color.red);
            tex.Apply();
            //updated = true;
        }
    }
}
