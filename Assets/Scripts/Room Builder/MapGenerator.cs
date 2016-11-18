using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pathfinding;

public class MapGenerator : MonoBehaviour {

    private const MapSize DEFAULT_SIZE = MapSize.Small;
    private const MapShape DEFAULT_SHAPE = MapShape.Square;
    private const RoomLayout DEFAULT_LAYOUT = RoomLayout.Open;

    public MapSize size;
    public MapShape shape;
    public RoomLayout layout;
    public int maxLargeRooms;
    public bool rotateAnyRoom;

    [HideInInspector]
    public int mapLevel;

    private RoomGridEntry[,] mapGrid;
    private int[,] neighbours;
    private int roomRollOdds;
    private List<GameObject> rooms = new List<GameObject>();
    private int[] mask;
    private int[] startRoom;
    private int[] goalRoom;

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
        Build();
    }

    public void Build()
    {
        int i;
        int j;
        int l;
        int index;
        int total;
        int branchPasses = 0;
        int gridSize;
        List<GameObject> objectList = Resources.LoadAll("Room").Cast<GameObject>().Where(g => g.GetComponent<RoomBuilder>().roomLevel <= mapLevel).ToList();
        List<GameObject>[,,,,] roomsByDoors = new List<GameObject>[2, 2, 2, 2, 2];
        List<GameObject> largeRooms = new List<GameObject>();
        List<GameObject>[] list = new List<GameObject>[4];
        GameObject go;
        bool[] doors;

        for (i = 0; i < objectList.Count; i++)
        {
            RoomBuilder room = objectList[i].GetComponent<RoomBuilder>();

            if (room != null)
            {
                int[] hasIndex = room.GetHashIndex();
                if (hasIndex[0] == 0)
                {
                    if (roomsByDoors[0, hasIndex[1], hasIndex[2], hasIndex[3], hasIndex[4]] == null)
                        roomsByDoors[0, hasIndex[1], hasIndex[2], hasIndex[3], hasIndex[4]] = new List<GameObject>();
                    roomsByDoors[0, hasIndex[1], hasIndex[2], hasIndex[3], hasIndex[4]].Add(room.gameObject);

                    if (room.isRotatable || rotateAnyRoom)
                    {
                        if (roomsByDoors[1, hasIndex[1], hasIndex[2], hasIndex[3], hasIndex[4]] == null)
                            roomsByDoors[1, hasIndex[1], hasIndex[2], hasIndex[3], hasIndex[4]] = new List<GameObject>();
                        roomsByDoors[1, hasIndex[1], hasIndex[2], hasIndex[3], hasIndex[4]].Add(room.gameObject);
                    }
                }
                else
                {
                    largeRooms.Add(room.gameObject);
                }
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
                gridSize = 7;
                switch (shape)
                {
                    case MapShape.Branching:
                        mask = new int[]
                        {
                            0, 0, 0, 0, 0, 0, 0,
                            0, 0, 0, 0, 0, 0, 0,
                            0, 0, 0, 2, 0, 0, 0,
                            0, 0, 2, 1, 2, 0, 0,
                            0, 0, 0, 2, 0, 0, 0,
                            0, 0, 0, 0, 0, 0, 0,
                            0, 0, 0, 0, 0, 0, 0
                        };
                        branchPasses = 3;
                        break;
                    case MapShape.Frame:
                        mask = new int[]
                        {
                            1, 1, 1, 1, 1, 1, 1,
                            1, 1, 1, 1, 1, 1, 1,
                            1, 1, 0, 0, 0, 1, 1,
                            1, 1, 0, 0, 0, 1, 1,
                            1, 1, 0, 0, 0, 1, 1,
                            1, 1, 1, 1, 1, 1, 1,
                            1, 1, 1, 1, 1, 1, 1
                        };
                        break;
                }
                break;
            case MapSize.Medium:
                gridSize = 5;
                switch (shape)
                {
                    case MapShape.Branching:
                        mask = new int[]
                        {
                            0, 0, 0, 0, 0,
                            0, 0, 2, 0, 0,
                            0, 2, 1, 2, 0,
                            0, 0, 2, 0, 0,
                            0, 0, 0, 0, 0
                        };
                        branchPasses = 2;
                        break;
                    case MapShape.Frame:
                        mask = new int[]
                        {
                            1, 1, 1, 1, 1,
                            1, 1, 0, 1, 1,
                            1, 0, 0, 0, 1,
                            1, 1, 0, 1, 1,
                            1, 1, 1, 1, 1
                        };
                        break;
                }
                break;
            case MapSize.Small:
            default:
                gridSize = 3;
                switch (shape)
                {
                    case MapShape.Branching:
                        mask = new int[]
                        {
                            0, 2, 0,
                            2, 1, 2,
                            0, 2, 0
                        };
                        branchPasses = 1;
                        break;
                    case MapShape.Frame:
                        mask = new int[]
                        {
                            1, 1, 1,
                            1, 0, 1,
                            1, 1, 1
                        };
                        break;
                }
                break;
        }

        int center = Mathf.FloorToInt(gridSize / 2);

        switch (shape)
        {
            case MapShape.Branching:
                int offset = 1;
                List<int> indexList = new List<int>();
                startRoom = new int[] { center, center };

                while (branchPasses > 0)
                {
                    for (j = center - offset; j <= center + offset; j++)
                    {
                        for (l = center - offset; l <= center + offset; l++)
                        {
                            if (mask[gridSize * j + l] == offset + 1)
                            {
                                indexList.Clear();

                                if (j + 1 < gridSize && mask[gridSize * (j + 1) + l] == 0)
                                    indexList.Add(gridSize * (j + 1) + l);
                                if (j - 1 >= 0 && mask[gridSize * (j - 1) + l] == 0)
                                    indexList.Add(gridSize * (j - 1) + l);
                                if (l + 1 < gridSize && mask[gridSize * j + (l + 1)] == 0)
                                    indexList.Add(gridSize * j + (l + 1));
                                if (l - 1 >= 0 && mask[gridSize * j + (l - 1)] == 0)
                                    indexList.Add(gridSize * j + (l - 1));

                                if (indexList.Count > 0)
                                {
                                    mask[indexList[Random.Range(0, indexList.Count - 1)]] = offset + 2;

                                    for (i = 0; i < indexList.Count; i++)
                                    {
                                        if (Random.Range(0, 4) < 1)
                                            mask[indexList[i]] = offset + 2;
                                    }
                                }
                            }
                        }
                    }

                    offset++;
                    branchPasses--;
                }
                indexList.Clear();
                for (j = 0; j < gridSize; j++)
                {
                    for (l = 0; l < gridSize; l++)
                    {
                        if (mask[gridSize * j + l] == offset)
                            indexList.Add(gridSize * j + l);
                    }
                }
                i = indexList[Random.Range(0, indexList.Count-1)];
                j = Mathf.FloorToInt(i / gridSize);
                goalRoom = new int[] {i - j, j};
                break;
            case MapShape.Frame:
                switch(Random.Range(0, 3))
                {
                    case 0:
                        startRoom = new int[] { 0, center };
                        goalRoom = new int[] { gridSize - 1, center };
                        break;
                    case 1:
                        startRoom = new int[] { center, 0 };
                        goalRoom = new int[] { center, gridSize - 1};
                        break;
                    case 2:
                        startRoom = new int[] { gridSize - 1, center };
                        goalRoom = new int[] { 0, center };
                        break;
                    case 3:
                    default:
                        startRoom = new int[] { center, gridSize - 1 };
                        goalRoom = new int[] { center, 0 };
                        break;
                }
                break;
            case MapShape.Square:
            default:
                switch (Random.Range(0, 3))
                {
                    case 0:
                        startRoom = new int[] { 0, 0 };
                        goalRoom = new int[] { gridSize - 1 , gridSize - 1 };
                        break;
                    case 1:
                        startRoom = new int[] { gridSize - 1, 0 };
                        goalRoom = new int[] { 0, gridSize - 1 };
                        break;
                    case 2:
                        startRoom = new int[] { gridSize - 1, gridSize - 1 };
                        goalRoom = new int[] { 0, 0 };
                        break;
                    case 3:
                    default:
                        startRoom = new int[] { 0, gridSize - 1 };
                        goalRoom = new int[] { gridSize - 1, 0 };
                        break;
                }
                mask = new int[gridSize * gridSize];
                populate(mask, 1);
                break;
        }

        mapGrid = new RoomGridEntry[gridSize, gridSize];

        for (i = 0; i < mapGrid.GetLength(0); i++)
        {
            for (j = 0; j < mapGrid.GetLength(1); j++)
            {
                if (mask[gridSize * i + j] > 0)
                {
                    mapGrid[i, j] = new RoomGridEntry();
                }
            }
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

        /* 
         * FIND LARGE ROOMS HERE
         */

        for (j = 0; j < mapGrid.GetLength(1); j++)
        {
            for (i = 0; i < mapGrid.GetLength(0); i++)
            {
                updateRoom(i, j);
            }
        }

        clear();

        for (j = 0; j < mapGrid.GetLength(1); j++)
        {
            for (i = 0; i < mapGrid.GetLength(0); i++)
            {
                if (mapGrid[i, j] != null)
                {
                    doors = mapGrid[i, j].doors;
                    list[0] = roomsByDoors[
                        0,
                        doors[0] ? 1 : 0,
                        doors[1] ? 1 : 0,
                        doors[2] ? 1 : 0,
                        doors[3] ? 1 : 0
                    ];
                    total = list[0].Count;

                    for (l = 1; l < list.Length; l++)
                    {
                        doors = rotateDoors(doors);
                        list[l] = roomsByDoors[
                            1,
                            doors[0] ? 1 : 0,
                            doors[1] ? 1 : 0,
                            doors[2] ? 1 : 0,
                            doors[3] ? 1 : 0
                        ];
                        if (list[l] == null)
                            list[l] = new List<GameObject>();
                        total += list[l].Count;
                    }

                    if (total > 0)
                    {
                        index = Random.Range(0, total - 1);

                        if (index < list[0].Count)
                        {
                            l = 0;
                        }
                        else if (index < list[0].Count + list[1].Count)
                        {
                            index -= list[0].Count;
                            l = 1;
                        }
                        else if (index < list[0].Count + list[1].Count + list[2].Count)
                        {
                            index -= list[0].Count + list[1].Count;
                            l = 2;
                        }
                        else
                        {
                            index -= list[0].Count + list[1].Count + list[2].Count;
                            l = 3;
                        }

                        go = Instantiate(list[l][index].gameObject);
                        go.transform.position = new Vector3(RoomUnit.TILE_RATIO * i * RoomTile.TILE_SCALE, 0, -RoomUnit.TILE_RATIO * j * RoomTile.TILE_SCALE);
                        if (l > 0)
                        {
                            go.transform.Rotate(Vector3.up * -90 * l);
                            go.transform.position = new Vector3(go.transform.position.x + (l < 3 ? RoomTile.TILE_SCALE * (RoomUnit.TILE_RATIO - 1)  : 0), 0, go.transform.position.z + (l > 1 ? RoomTile.TILE_SCALE * (RoomUnit.TILE_RATIO - 1) : 0));
                        }
                        rooms.Add(go);
                    }
                }
            }
        }

        StartCoroutine("DelayedScan");
    }

    private bool[] rotateDoors(bool[] doors)
    {
        bool temp = doors[0];
        doors[0] = doors[3];
        doors[3] = doors[2];
        doors[2] = doors[1];
        doors[1] = temp;
        return doors;
    }

    IEnumerator DelayedScan()
    {
        yield return new WaitForSeconds(2f);
        AstarPath p = FindObjectOfType<AstarPath>();
        AstarPath.active.Scan();
    }

    void Update() {

    }

    private void clear()
    {
        for(int i = 0; i < rooms.Count; i++)
            DestroyImmediate(rooms[i]);
        rooms.Clear();
    }

    private void updateRoom(int i, int j){
        int l;
        RoomGridEntry entry = mapGrid[i, j];
        RoomGridEntry neighbour;

        if (entry != null && !entry.isSet)
        {
            if (entry.doors == null)
            {
                List<int> doorList = new List<int>();
                for (l = 0; l < neighbours.GetLength(0); l++)
                {
                    neighbour = getNeighbour(i, j, l);

                    if (neighbour != null)
                    {
                        doorList.Add(l);
                        if (entry.doors == null && neighbour.isSet)
                        {
                            entry.doors = new bool[4];
                            entry.doors[l] = true;
                        }
                    }
                }
                if (entry.doors == null)
                {
                    entry.doors = new bool[4];
                    entry.doors[doorList[Random.Range(0, doorList.Count)]] = true;
                }
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

    private void populate<T>(T[] arr, T value)
    {
        for (int i = 0; i < arr.Length; i++)
            arr[i] = value;
    }

    private void populate<T>(T[,] arr, T value)
    {
        int j;
        for (int i = 0; i < arr.GetLength(0); i++)
            for (j = 0; j < arr.GetLength(1); j++)
                arr[i, j] = value;
    }
}
