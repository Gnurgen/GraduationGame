﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomBuilder : MonoBehaviour {

    public const int MAX_UNIT_SIZE = 3;

    private static RoomTile _defaultTile;
    private static RoomWall _defaultWall;

    public string roomName = "New Room";
    public int mapIndex = 0;
    public RoomUnit[,] roomUnits = new RoomUnit[MAX_UNIT_SIZE, MAX_UNIT_SIZE];

    private List<GameObject> objectList = new List<GameObject>();
    private Vector2 _roomSize = new Vector2(1, 1);

    public Vector2 roomSize
    {
        get
        {
            return _roomSize;
        }
        set
        {
            if (_roomSize != value)
            {
                _roomSize = value;
                for(int i = 0; i < roomUnits.GetLength(0); i++)
                {
                    for (int j = 0; j < roomUnits.GetLength(1); j++)
                    {
                        if (!roomUnits[i, j])
                        {
                            if (i < _roomSize.x && j < _roomSize.y)
                            {
                                roomUnits[i, j] = createRoomUnit();
                                roomUnits[i, j].transform.position = new Vector3(transform.position.x + i * RoomUnit.TILE_RATIO, transform.position.y, transform.position.z + j * RoomUnit.TILE_RATIO);

                            }
                        }
                        else
                        {
                            if (i < _roomSize.x && j < _roomSize.y)
                            {
                                roomUnits[i, j].gameObject.SetActive(true);
                            }
                            else
                                roomUnits[i, j].gameObject.SetActive(false);
                        }
                        if (roomUnits[i, j] && roomUnits[i, j].gameObject.activeInHierarchy)
                            roomUnits[i, j].setWallDisplay(i == 0, j == 0, _roomSize.x - i == 1, _roomSize.y - j == 1);
                    }
                }
            }
        }
    }

    public static RoomTile defaultTile
    {
        get
        {
            if (!_defaultTile)
            {
                Object[] objects = Resources.LoadAll("Tile");
                for (int i = 0; i < objects.Length && !_defaultTile; i++)
                {
                    RoomTile tile = (objects[i] as GameObject).GetComponent<RoomTile>();
                    if (tile.isDefault)
                        _defaultTile = tile;
                }

                if (!_defaultTile)
                    Debug.LogError("Unable to find default tile prefab in resources.");
            }
            return _defaultTile;
        }
    }

    public static RoomWall defaultWall
    {
        get
        {
            if (!_defaultWall)
            {
                Object[] objects = Resources.LoadAll("Wall");
                for (int i = 0; i < objects.Length && !_defaultWall; i++)
                {
                    RoomWall wall = (objects[i] as GameObject).GetComponent<RoomWall>();
                    if (wall.isDefault)
                        _defaultWall = wall;
                }

                if (!_defaultWall)
                    Debug.LogError("Unable to find default wall prefab in resources.");
            }
            return _defaultWall;
        }
    }

    void Reset() {
        roomUnits[0, 0] = createRoomUnit();
    }

    void Start()
    {

    }

    void Update () {
	}

    public void AddRoomObject(GameObject go)
    {
        go.transform.parent = transform;
    }

    /*
    public void AddRoomObject(GameObject go)
    {
        refreshObjectList();
        if (objectList.IndexOf(go) < 0)
        {
            objectList.Add(go);
            Debug.Log("Added " + go.name + " to room");
        }
    }
 
    public Object[] GetObjects()
    {
        refreshObjectList();
        Object[] objects = new Object[objectList.Count];
        for (int i = 0; i < objectList.Count ; i++)
            objects[i] = objectList[i];
        return objects;
    }

    public int GetObjectsNum()
    {
        refreshObjectList();
        return objectList.Count;
    }

    private void refreshObjectList()
    {
        objectList = objectList.Where(item => item != null).ToList();
    }
    */

    private RoomUnit createRoomUnit()
    {
        RoomUnit unit = new GameObject("RoomUnit").AddComponent<RoomUnit>();
        unit.transform.parent = transform;
        return unit;
    }
}
