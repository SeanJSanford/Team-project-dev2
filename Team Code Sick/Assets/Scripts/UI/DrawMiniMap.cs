using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DrawMiniMap : MonoBehaviour
{
    void Update()
    {
        transform.position = new Vector3(gamemanager.instance.player.transform.position.x, 50, gamemanager.instance.player.transform.position.z);
    }
}
