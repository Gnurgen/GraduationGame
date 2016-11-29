using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pathfinding;

public class MapGenerator : MonoBehaviour {

    private const MapSize DEFAULT_SIZE = MapSize.Small;
    private const MapShape DEFAULT_SHAPE = MapShape.Square;
    private const RoomLayout DEFAULT_LAYOUT = RoomLayout.Open;

    private static List<GameObject>[,,,,] roomsByDoors;
    private static List<RoomBuilder> largeRooms;
    private static int[,] neighbours;

    public MapSize size;
    public MapShape shape;
    public RoomLayout layout;
    public int maxLargeRooms;
    public int minLargeRooms;
    public bool rotateAnyRoom;

    [HideInInspector]
    public int mapLevel;

    private RoomGridEntry[,] mapGrid;
    private List<GameObject> rooms;
    private bool completed;
    private int[] mask;
    private int[] startRoom;
    private int[] goalRoom;
    private int[] progressCoords;
    private int roomRollOdds;
    private int progress;
    private int totalProgress;
    private int i;
    private int j;
    private int l;
    private int rotateMod;
    private int offset;
    private int index;
    private int total;
    private bool[] doors;
    private GameObject go;
    private List<GameObject>[] list = new List<GameObject>[4];
    private RoomTile[] tiles;

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
        RoomBuilder room;
        int[] hashIndex;
        List<GameObject> objectList = Resources.LoadAll("Room").Cast<GameObject>().Where(g => g.GetComponent<RoomBuilder>().roomLevel <= mapLevel).ToList();

        rooms = new List<GameObject>();

        if (roomsByDoors == null){
            roomsByDoors = new List<GameObject>[4, 2, 2, 2, 2];
            largeRooms = new List<RoomBuilder>();

            for (i = 0; i < objectList.Count; i++)
            {
                room = objectList[i].GetComponent<RoomBuilder>();

                if (room != null)
                {
                    hashIndex = room.GetHashIndex();
                    if (hashIndex[0] == 0)
                    {
                        for (j = 0; j < 4; j++)
                        {
                            if (!room.isBeaconRoom && (j == 0  || j == 1 && (room.isRotatable || rotateAnyRoom)) || room.isBeaconRoom && (j == 2 || j == 3 && (room.isRotatable || rotateAnyRoom)))
                            {
                                if (roomsByDoors[j, hashIndex[1], hashIndex[2], hashIndex[3], hashIndex[4]] == null)
                                    roomsByDoors[j, hashIndex[1], hashIndex[2], hashIndex[3], hashIndex[4]] = new List<GameObject>();
                                roomsByDoors[j, hashIndex[1], hashIndex[2], hashIndex[3], hashIndex[4]].Add(room.gameObject);
                            }
                        }
                    }
                    else
                        largeRooms.Add(room);
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
        }

        Build();
    }

    public void Build()
    {
        int y;
        int gridSize;
        int branchPasses = 0;
        RoomBuilder room;
        RoomGridEntry entry;
        int[] hashIndex;

        clear();
        progress = 0;
        totalProgress = 0;
        progressCoords = new int[] {0, 0};
        completed = false;

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
                offset = 1;
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
                                    if (Random.Range(0, 2) < 2)
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
                for (j = 0; j < mask.Length; j++)
                {
                    if (mask[j] == offset + 1)
                        indexList.Add(j);
                }
                i = indexList[Random.Range(0, indexList.Count - 1)];
                goalRoom = new int[] { Mathf.FloorToInt(i / gridSize), i % gridSize };
                break;
            case MapShape.Frame:
                switch (Random.Range(0, 3))
                {
                    case 0:
                        startRoom = new int[] { 0, center };
                        goalRoom = new int[] { gridSize - 1, center };
                        break;
                    case 1:
                        startRoom = new int[] { center, 0 };
                        goalRoom = new int[] { center, gridSize - 1 };
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
                        goalRoom = new int[] { gridSize - 1, gridSize - 1 };
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

        GameManager.player.transform.position = new Vector3((startRoom[0] + 0.5f) * RoomUnit.TILE_RATIO * RoomTile.TILE_SCALE, -2.0f, -(startRoom[1] - 0.5f) * RoomUnit.TILE_RATIO * RoomTile.TILE_SCALE);
        Camera.main.transform.position = new Vector3(GameManager.player.transform.position.x - 7.5f, GameManager.player.transform.position.y + 11.1f, GameManager.player.transform.position.z - 7.5f);
        GameObject.Find("Elevator").transform.position = new Vector3((goalRoom[0] + 0.5f) * RoomUnit.TILE_RATIO * RoomTile.TILE_SCALE, -0.75f, - (goalRoom[1] - 0.5f) * RoomUnit.TILE_RATIO * RoomTile.TILE_SCALE);

        mapGrid = new RoomGridEntry[gridSize, gridSize];

        for (i = 0; i < mapGrid.GetLength(0); i++)
        {
            for (j = 0; j < mapGrid.GetLength(1); j++)
            {
                if (mask[gridSize * i + j] > 0)
                {
                    mapGrid[i, j] = new RoomGridEntry();
                    totalProgress++;
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

        if (largeRooms.Count > 0)
        {
            bool b;
            float x = Random.value;
            int largeRoomNum = Mathf.RoundToInt(x * x * (3 - 2 * x) * (maxLargeRooms - minLargeRooms)) + minLargeRooms;

            List<int>[] largeRoomIndexList = new List<int>[2];
            largeRoomIndexList[0] = new List<int>();
            largeRoomIndexList[1] = new List<int>();

            int[,] largeRoomMask = new int[12, 2];
            largeRoomMask[0, 0] = 0;
            largeRoomMask[0, 1] = 1;
            largeRoomMask[1, 0] = 0;
            largeRoomMask[1, 1] = 2;
            largeRoomMask[2, 0] = 1;
            largeRoomMask[2, 1] = 0;
            largeRoomMask[3, 0] = 1;
            largeRoomMask[3, 1] = 1;
            largeRoomMask[4, 0] = 1;
            largeRoomMask[4, 1] = 2;
            largeRoomMask[5, 0] = 1;
            largeRoomMask[5, 1] = 3;
            largeRoomMask[6, 0] = 2;
            largeRoomMask[6, 1] = 0;
            largeRoomMask[7, 0] = 2;
            largeRoomMask[7, 1] = 1;
            largeRoomMask[8, 0] = 2;
            largeRoomMask[8, 1] = 2;
            largeRoomMask[9, 0] = 2;
            largeRoomMask[9, 1] = 3;
            largeRoomMask[10, 0] = 3;
            largeRoomMask[10, 1] = 1;
            largeRoomMask[11, 0] = 3;
            largeRoomMask[11, 1] = 2;

            while (largeRoomNum-- > 0)
            {
                largeRoomIndexList[0].Clear();
                largeRoomIndexList[1].Clear();
                for (j = 0; j < mapGrid.GetLength(1) - 3; j++)
                {
                    for (i = 0; i < mapGrid.GetLength(0) - 3; i++)
                    {
                        b = true;
                        for (l = 0; l < largeRoomMask.GetLength(0) && b; l++)
                        {
                            if (mapGrid[i + largeRoomMask[l, 0], j + largeRoomMask[l, 1]] == null ||
                                largeRoomMask[l, 0] > 0 && largeRoomMask[l, 0] < 3 && largeRoomMask[l, 1] > 0 && largeRoomMask[l, 1] < 3 && (
                                    i + largeRoomMask[l, 0] == startRoom[0] && j + largeRoomMask[l, 1] == startRoom[1] ||
                                    i + largeRoomMask[l, 0] == goalRoom[0] && j + largeRoomMask[l, 1] == goalRoom[1]
                                ) || 
                                mapGrid[i + largeRoomMask[l, 0], j + largeRoomMask[l, 1]].segment >= 0)
                            {
                                b = false;
                            }
                        }
                        if (b)
                        {
                            largeRoomIndexList[0].Add(i + 1);
                            largeRoomIndexList[1].Add(j + 1);
                        }
                    }
                }
                if (largeRoomIndexList[0].Count > 0)
                {
                    index = Random.Range(0, largeRoomIndexList[0].Count - 1);
                    room = largeRooms[Random.Range(0, largeRooms.Count - 1)];
                    i = largeRoomIndexList[0][index];
                    j = largeRoomIndexList[1][index];

                    for (l = 0; l < 4; l++)
                    {
                        y = Mathf.FloorToInt(l / 2);
                        entry = mapGrid[i + (l % 2), j + y];
                        entry.segment = l;

                        switch (l)
                        {
                            case 0:
                                hashIndex = room.GetHashIndex(1, 0);
                                break;
                            case 1:
                                hashIndex = room.GetHashIndex(1, 1);
                                break;
                            case 2:
                                hashIndex = room.GetHashIndex(0, 0);
                                break;
                            case 3:
                            default:
                                hashIndex = room.GetHashIndex(0, 1);
                                break;
                        }

                        entry.doors = new bool[] {
                            hashIndex[1] == 1,
                            hashIndex[2] == 1,
                            hashIndex[3] == 1,
                            hashIndex[4] == 1
                        };

                        Debug.Log(
                            hashIndex[1] + ", " + 
                            hashIndex[2] + ", " +
                            hashIndex[3] + ", " +
                            hashIndex[4]
                        );
                    }

                    go = Instantiate(room.gameObject);
                    go.transform.position = new Vector3(RoomUnit.TILE_RATIO * i * RoomTile.TILE_SCALE, 0, -RoomUnit.TILE_RATIO * (j + 1) * RoomTile.TILE_SCALE);
//                    go.GetComponent<RoomBuilder>().HideWalls(true, true);
                    progress += 4;
                    rooms.Add(go);
                }
                else
                    largeRoomNum = 0;
            }
        }

        for (j = 0; j < mapGrid.GetLength(1); j++)
        {
            for (i = 0; i < mapGrid.GetLength(0); i++)
            {
                updateRoom(i, j);
            }
        }

        j = 0;
        i = 0;
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
        if (completed)
            return;

        for (i = 0; i < mapGrid.GetLength(0); i++)
        {

            Debug.Log(i + " " + j);

            if (mapGrid[i, j] != null && mapGrid[i, j].segment < 0)
            {

                offset = startRoom[0] == i && startRoom[1] == j || goalRoom[0] == i && goalRoom[1] == j ? 1 : 0;
                do
                {
                    doors = mapGrid[i, j].doors;
                    list[0] = roomsByDoors[
                        offset * 2,
                        doors[0] ? 1 : 0,
                        doors[1] ? 1 : 0,
                        doors[2] ? 1 : 0,
                        doors[3] ? 1 : 0
                    ];
                    if (list[0] == null)
                        list[0] = new List<GameObject>();
                    total = list[0].Count;

                    for (l = 1; l < list.Length; l++)
                    {
                        doors = rotateDoors(doors);
                        list[l] = roomsByDoors[
                            offset * 2 + 1,
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
                            rotateMod = 0;
                        }
                        else if (index < list[0].Count + list[1].Count)
                        {
                            index -= list[0].Count;
                            rotateMod = 1;
                        }
                        else if (index < list[0].Count + list[1].Count + list[2].Count)
                        {
                            index -= list[0].Count + list[1].Count;
                            rotateMod = 2;
                        }
                        else
                        {
                            index -= list[0].Count + list[1].Count + list[2].Count;
                            rotateMod = 3;
                        }

                        go = Instantiate(list[rotateMod][index].gameObject);
                        go.transform.position = new Vector3(RoomUnit.TILE_RATIO * i * RoomTile.TILE_SCALE, 0, -RoomUnit.TILE_RATIO * j * RoomTile.TILE_SCALE);
                        progress++;

                        if (rotateMod > 0)
                        {
                            go.transform.Rotate(Vector3.up * -90 * rotateMod);
                            go.transform.position = new Vector3(go.transform.position.x + (rotateMod < 3 ? RoomTile.TILE_SCALE * (RoomUnit.TILE_RATIO - 1) : 0), 0, go.transform.position.z + (rotateMod > 1 ? RoomTile.TILE_SCALE * (RoomUnit.TILE_RATIO - 1) : 0));

                            tiles = go.GetComponentsInChildren<RoomTile>();
                            for (l = 0; l < tiles.Length; l++)
                                tiles[l].transform.Rotate(Vector3.up * 90 * rotateMod);
                        }

                        go.GetComponent<RoomBuilder>().HideWalls(i + 1 < mapGrid.GetLength(0) && mapGrid[i + 1, j] != null, j + 1 < mapGrid.GetLength(1) && mapGrid[i, j + 1] != null);
                        rooms.Add(go);
                    }
                } while (offset-- > 0 && total == 0);
            }
        }


        if (progress < totalProgress)
        {
            j++;
            displayProgress();
        }
        else
        {
            List<GameObject> elevators = Resources.LoadAll("Elevator").Cast<GameObject>().ToList();
            go = Instantiate(elevators[0]);
            go.transform.position = new Vector3((startRoom[0] + 0.5f) * RoomUnit.TILE_RATIO * RoomTile.TILE_SCALE, go.transform.position.y, -(startRoom[1] - 0.5f) * RoomUnit.TILE_RATIO * RoomTile.TILE_SCALE);
            GameManager.player.transform.parent = go.transform;
            completed = true;

            GameManager.events.MapGenerated();
            StartCoroutine(DelayedScan());
            GameObject.Find("Canvas").GetComponent<GenerateHealthScript>().moveAllHealthBars();
            hideProgress();
        }
    }

    private void displayProgress()
    {

    }

    private void hideProgress()
    {
        displayProgress();
    }

    private void clear()
    {
        for(int i = 0; i < rooms.Count; i++)
            if (rooms[i] != null)
                DestroyImmediate(rooms[i]);
        rooms.Clear();
    }

    private void updateRoom(int i, int j){
        int l;
        RoomGridEntry entry = mapGrid[i, j];
        RoomGridEntry neighbour;

        if (entry == null || entry.isSet)
            return;

        if (entry.segment < 0)
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
        else
        {
            for (l = 0; l < entry.doors.Length; l++)
            {
                neighbour = getNeighbour(i, j, l);
                if (neighbour != null && entry.doors[l])
                {
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
            indexY >= 0 && indexY < mapGrid.GetLength(1) &&
            mapGrid[indexX, indexY] != null &&
            mapGrid[indexX, indexY].segment < 0
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
