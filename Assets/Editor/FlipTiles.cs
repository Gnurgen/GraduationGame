using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class FlipTiles : Editor
{
    [MenuItem("Tools/Flip Tiles/Flip in Selection", false, 8)]
    public static void FlipInSelection()
    {
        int i;
        int j;
        Object[] objects = Selection.objects;
        RoomTile[] tiles;

        for (i = 0; i < objects.Length; i++)
        {
            tiles = (objects[i] as GameObject).GetComponentsInChildren<RoomTile>();
            for (j = 0; j < tiles.Length; j++)
                tiles[j].transform.Rotate(Vector3.up * 90);
        }
    }

    [MenuItem("Tools/Flip Tiles/Reset in Selection", false, 8)]
    public static void ResetInSelection()
    {
        int i;
        int j;
        Object[] objects = Selection.objects;
        RoomTile[] tiles;

        for (i = 0; i < objects.Length; i++)
        {
            tiles = (objects[i] as GameObject).GetComponentsInChildren<RoomTile>();
            for (j = 0; j < tiles.Length; j++)
                tiles[j].transform.rotation = Quaternion.Euler(tiles[j].transform.localRotation.eulerAngles.x, 90, tiles[j].transform.localRotation.eulerAngles.z);
        }
    }

    [MenuItem("Tools/Flip Tiles/Flip Selection %e", false, 8)]
    public static void ResetSelection()
    {
        int i;
        Object[] objects = Selection.objects;
        RoomTile tile;

        for (i = 0; i < objects.Length; i++)
        {
            tile = (objects[i] as GameObject).GetComponent<RoomTile>();

            if (tile == null)
                tile = (objects[i] as GameObject).GetComponentInParent<RoomTile>();

            if (tile != null)
                tile.transform.Rotate(Vector3.up * 90);
        }
    }
}

