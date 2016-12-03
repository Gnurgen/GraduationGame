using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class FlipTiles : Editor
{
    [MenuItem("Tools/Flip Tiles in Scene", false, 8)]
    public static void FlipAll()
    {
        int i;
        int j;
        RoomTile[] tiles;
        RoomBuilder[] rooms = FindObjectsOfType<RoomBuilder>();

        for (i = 0; i < rooms.Length; i++)
        {
            tiles = rooms[i].GetComponentsInChildren<RoomTile>();

            for(j = 0; j < tiles.Length; j++)
            {
                tiles[j].transform.rotation = Quaternion.Inverse(rooms[i].transform.rotation);
            }
        }
    }
}

