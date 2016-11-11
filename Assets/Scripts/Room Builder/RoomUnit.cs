using UnityEngine;
using UnityEditor;
using System.Collections;

public class RoomUnit : MonoBehaviour {

    public const int TILE_RATIO = 11;

    private RoomTile[] tiles = new RoomTile[TILE_RATIO * TILE_RATIO];
    private RoomWall[] walls;


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
            tiles[i].transform.position = new Vector3(transform.position.x + (i % TILE_RATIO), transform.position.y, transform.position.z + Mathf.Floor(i/ TILE_RATIO));
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
        walls[0].transform.position = new Vector3(transform.position.x +  5.0f, transform.position.y, transform.position.z -  0.5f);
        walls[1].transform.position = new Vector3(transform.position.x -  0.5f, transform.position.y, transform.position.z +  5.0f);
        walls[2].transform.position = new Vector3(transform.position.x +  5.0f, transform.position.y, transform.position.z + 10.5f);
        walls[3].transform.position = new Vector3(transform.position.x + 10.5f, transform.position.y, transform.position.z +  5.0f);

        walls[1].transform.Rotate(Vector3.up * 90);
        walls[3].transform.Rotate(Vector3.up * 90);
    }

    void Start()
    {
    }

    void Update()
    {

    }

    public bool[] getDoors()
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

        return new bool[] {
            walls[0] != null && walls[0].hasDoor,
            walls[1] != null && walls[1].hasDoor,
            walls[2] != null && walls[2].hasDoor,
            walls[3] != null && walls[3].hasDoor
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
