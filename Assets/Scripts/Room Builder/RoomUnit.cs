using UnityEngine;
using UnityEditor;
using System.Collections;

public class RoomUnit : MonoBehaviour {

    public const int TILE_RATIO = 11;

    private RoomTile[] tiles = new RoomTile[TILE_RATIO * TILE_RATIO];
    private RoomWall[] walls = new RoomWall[4];

	void Reset() {
        RoomTile tilePrefab = RoomBuilder.defaultTile;
        RoomWall wallPrefab = RoomBuilder.defaultWall;

        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i] = PrefabUtility.InstantiatePrefab(tilePrefab) as RoomTile;
            tiles[i].transform.position = new Vector3(transform.position.x + (i % TILE_RATIO), transform.position.y, transform.position.z + Mathf.Floor(i/ TILE_RATIO));
            tiles[i].transform.parent = transform;
        }
    }

    void Start()
    {

    }

    void Update()
    {

    }

    public void setWallDisplay(bool top, bool left, bool bottom, bool right)
    {

    }
}
