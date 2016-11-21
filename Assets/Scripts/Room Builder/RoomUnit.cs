using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;

public class RoomUnit : MonoBehaviour {
    
    public const int TILE_RATIO = 11;

    private RoomTile[] tiles = new RoomTile[TILE_RATIO * TILE_RATIO];
    private RoomWall[] walls;

#if UNITY_EDITOR
    void Reset() {
        walls = new RoomWall[4];

        int i;
        RoomTile tilePrefab = RoomBuilder.defaultTile;
        RoomWall wallPrefab = RoomBuilder.defaultWall;

        GameObject parent = new GameObject("Tiles");
        parent.transform.position = transform.position;
        parent.transform.parent = transform;

        for (i = 0; i < tiles.Length; i++)
        {
            tiles[i] = PrefabUtility.InstantiatePrefab(tilePrefab) as RoomTile;
            tiles[i].transform.position = new Vector3(transform.position.x + (i % TILE_RATIO)* RoomTile.TILE_SCALE, transform.position.y, transform.position.z + Mathf.Floor(i/ TILE_RATIO) * RoomTile.TILE_SCALE);
            tiles[i].transform.parent = parent.transform;
            tiles[i].index = i;
        }

        parent = new GameObject("Walls");
        parent.transform.position = transform.position;
        parent.transform.parent = transform;

        for (i = 0; i < walls.Length; i++)
        {
            walls[i] = PrefabUtility.InstantiatePrefab(wallPrefab) as RoomWall;
            walls[i].transform.parent = parent.transform;
            walls[i].index = i;
        }

        walls[0].transform.position = new Vector3(transform.position.x +  5.0f * RoomTile.TILE_SCALE, transform.position.y, transform.position.z -  0.5f * RoomTile.TILE_SCALE);
        walls[1].transform.position = new Vector3(transform.position.x -  0.5f * RoomTile.TILE_SCALE, transform.position.y, transform.position.z +  5.0f * RoomTile.TILE_SCALE);
        walls[2].transform.position = new Vector3(transform.position.x +  5.0f * RoomTile.TILE_SCALE, transform.position.y, transform.position.z + 10.5f * RoomTile.TILE_SCALE);
        walls[3].transform.position = new Vector3(transform.position.x + 10.5f * RoomTile.TILE_SCALE, transform.position.y, transform.position.z +  5.0f * RoomTile.TILE_SCALE);

        walls[1].transform.Rotate(Vector3.up * 90);
        walls[3].transform.Rotate(Vector3.up * 90);
    }
#endif

    void Start()
    {

    }

    void Update()
    {

    }

    public bool[] GetDoors()
    {
        if (walls == null)
        {
            walls = new RoomWall[4];
            RoomWall[] wallList = GetComponentsInChildren<RoomWall>();
            walls[0] = wallList[3];
            walls[1] = wallList[0];
            walls[2] = wallList[1];
            walls[3] = wallList[2];
        }
        return new bool[]
        {
            walls.Length > 0 && walls[0].hasDoor,
            walls.Length > 1 && walls[1].hasDoor,
            walls.Length > 2 && walls[2].hasDoor,
            walls.Length > 3 && walls[3].hasDoor
        };
    }

    public void ReplaceTile(RoomTile tile, int index)
    {
        tiles[index] = tile;
    }

    public void ReplaceWall(RoomWall wall, int index)
    {
        walls[index] = wall;
    }

    public void setWallDisplay(bool top, bool left, bool bottom, bool right)
    {
        walls[0].gameObject.SetActive(left);
        walls[1].gameObject.SetActive(top);
        walls[2].gameObject.SetActive(right);
        walls[3].gameObject.SetActive(bottom);
    }
}
