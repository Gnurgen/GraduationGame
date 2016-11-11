using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MapGenerator : MonoBehaviour {

    private MapSize DEFAULT_SIZE = MapSize.Small;
    private MapShape DEFAULT_SHAPE = MapShape.Square;
    private RoomLayout DEFAULT_LAYOUT = RoomLayout.Open;

    public MapSize size;
    public MapShape shape;
    public RoomLayout layout;

    private RoomGridEntry[,] mapGrid;
    private int[,] neighbours;
    private int roomRollOdds;

    public enum MapSize
    {
        Default,
        Small,
        Medium,
        Large
    }

    public enum MapShape
    {
        Default,
        Square,
        Frame,
        Branching,
    }

    public enum RoomLayout
    {
        Default,
        Open,
        Maze
    }

    void Start() {
        int i;
        int j;
        int gridSize;
        List<GameObject> objectList = Resources.LoadAll("Room").Cast<GameObject>().ToList();
        List<GameObject>[,,,] roomsByDoors = new List<GameObject>[2,2,2,2];
        List<GameObject> list;
        GameObject go;
        bool[] doors;

        for (i = 0; i < objectList.Count; i++)
        {
            RoomBuilder room = objectList[i].GetComponent<RoomBuilder>();

            if (room != null)
            {
                int[] hasIndex = room.getHashIndex();
                if (roomsByDoors[hasIndex[0], hasIndex[1], hasIndex[2], hasIndex[3]] == null)
                    roomsByDoors[hasIndex[0], hasIndex[1], hasIndex[2], hasIndex[3]] = new List<GameObject>();
                roomsByDoors[hasIndex[0], hasIndex[1], hasIndex[2], hasIndex[3]].Add(room.gameObject);
            }
        }

        neighbours = new int[4, 2];

        neighbours[0, 0] = 1;
        neighbours[0, 1] = 0;

        neighbours[1, 0] = 0;
        neighbours[1, 1] = 1;

        neighbours[2, 0] = -1;
        neighbours[2, 1] = 0;

        neighbours[3, 0] = 0;
        neighbours[3, 1] = -1;

        switch (size)
        {
            case MapSize.Large:
                gridSize = 8;
                break;
            case MapSize.Medium:
                gridSize = 6;
                break;
            case MapSize.Small:
            default:
                gridSize = 4;
                break;
        }
        mapGrid = new RoomGridEntry[gridSize, gridSize];

        switch (shape)
        {
            case MapShape.Branching:
            case MapShape.Frame:
            case MapShape.Square:
            default:
                for (i = 0; i < mapGrid.GetLength(0); i++)
                    for (j = 0; j < mapGrid.GetLength(1); j++)
                        mapGrid[i, j] = new RoomGridEntry();
                break;
        }

        switch (layout)
        {
            case RoomLayout.Maze:
                roomRollOdds = 10;
                break;
            case RoomLayout.Open:
            default:
                roomRollOdds = 3;
                break;
        }

        for (j = 0; j < mapGrid.GetLength(1); j++)
        {
            for (i = 0; i < mapGrid.GetLength(0); i++)
            {
                updateRoom(i, j);
            }
        }

        for (j = 0; j < mapGrid.GetLength(1); j++)
        {
            for (i = 0; i < mapGrid.GetLength(0); i++)
            {
                if (mapGrid[i, j] != null)
                {
                    doors = mapGrid[i, j].doors;
                    list = roomsByDoors[
                        doors[0] ? 1 : 0,
                        doors[1] ? 1 : 0,
                        doors[2] ? 1 : 0,
                        doors[3] ? 1 : 0
                    ];

                    if (list != null)
                    {
                        go = Instantiate(list[Random.Range(0, list.Count-1)].gameObject);
                        go.transform.position = new Vector3(RoomUnit.TILE_RATIO * i, 0, - RoomUnit.TILE_RATIO * j);
                    }
                }
            }
        }
    }

    void Update() {

    }

    private void updateRoom(int i, int j){
        int l;
        RoomGridEntry entry = mapGrid[i, j];
        RoomGridEntry neighbour;

        if (entry != null && !entry.isSet)
        {
            if (entry.doors == null)
            {
                for (l = 0; l < neighbours.GetLength(0) && entry.doors == null; l++)
                {
                    neighbour = getNeighbour(i, j, l);

                    if (neighbour != null && neighbour.isSet)
                    {
                        entry.doors = new bool[4];
                        entry.doors[l] = true;
                    }
                }
            }
            if (entry.doors == null)
            {
                entry.doors = new bool[4];
                entry.doors[Random.Range(0, 1)] = true;
            }

            entry.isSet = true;

            for (l = 0; l < entry.doors.Length; l++)
            {
                neighbour = getNeighbour(i, j, l);
                if (neighbour != null && (entry.doors[l] || Random.Range(0, roomRollOdds) < 1))
                {
                    entry.doors[l] = true;

                    if (neighbour.doors == null)
                        neighbour.doors = new bool[4];
                    neighbour.doors[(l + 2) % 4] = true;


                    if (!neighbour.isSet)
                        updateRoom(i + neighbours[l, 0], j + neighbours[l, 1]);
                }
            }

        }
    }

    private RoomGridEntry getNeighbour(int i, int j, int l)
    {
        int indexX = i + neighbours[l, 0];
        int indexY = j + neighbours[l, 1];
        if (indexX >= 0 && indexX < mapGrid.GetLength(0) &&
            indexY >= 0 && indexY < mapGrid.GetLength(1)
        )
            return mapGrid[indexX, indexY];
        return null;
    }

    private void populate<T>(this T[] arr, T value)
    {
        for (int i = 0; i < arr.Length; i++)
            arr[i] = value;
    }

    private void populate<T>(this T[,] arr, T value)
    {
        int j;
        for (int i = 0; i < arr.GetLength(0); i++)
            for (j = 0; j < arr.GetLength(1); j++)
                arr[i, j] = value;
    }
}
