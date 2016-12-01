using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pathfinding;

public class MapGenerator : MonoBehaviour {

    private static List<GameObject>[,,,,] roomsByDoors;
    private static int[,] neighbours;

    public MapSize size = MapSize.Default;
    public MapShape shape = MapShape.Default;
    public RoomLayout layout = RoomLayout.Default;
    public bool rotateAnyRoom = false;

    [HideInInspector]
    public int mapLevel;

    private List<GameObject>[] list = new List<GameObject>[4];
    private List<GameObject> rooms;
    private RoomGridEntry[,] mapGrid;
    private RoomTile[] tiles;
    private GameObject go;
    private GameObject elevatorShaft;
    private GameObject startElevator;
    private GameObject endElevator;
    private int[] mask;
    private int[] startRoom;
    private int[] endRoom;
    private int[] progressCoords;
    private int roomRollOdds;
    private int progress;
    private int totalProgress;
    private int i;
    private int j;
    private int l;
    private int x;
    private int y;
    private int rotateMod;
    private int offset;
    private int index;
    private int total;
    private bool[] doors;
    private bool completed;
    private bool containsElevator;

    public enum MapSize
    {
        Default,
        Large
    }

    public enum MapShape
    {
        Default,
        Frame,
    }

    public enum RoomLayout
    {
        Default,
        Maze
    }

    void Start() {
        int i;
        int j;
        RoomBuilder room;
        int[] hashIndex;
        List<GameObject> objectList = Resources.LoadAll("Room").Cast<GameObject>().Where(g => g.GetComponent<RoomBuilder>().roomLevel <= mapLevel).ToList();

        endElevator = Instantiate(Resources.Load("Elevator/EndElevator") as GameObject);

        rooms = new List<GameObject>();
        roomsByDoors = new List<GameObject>[4, 2, 2, 2, 2];

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
            }
        }

        if (neighbours == null)
        {
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
        int gridSize;
        int center;

        clear();
        progress = 0;
        totalProgress = 0;
        progressCoords = new int[] {0, 0};
        completed = false;

        if (startElevator != null)
        {
            if (GameManager.player.transform.parent == startElevator)
                GameManager.player.transform.parent = startElevator.transform.parent;
            Destroy(startElevator);
        }

        startElevator = Instantiate(Resources.Load("Elevator/StartElevator") as GameObject);

        if (size == MapSize.Large){
            gridSize = 5;
            if (shape == MapShape.Frame)
                mask = new int[]
                {
                    1, 1, 1, 1, 1,
                    1, 1, 0, 1, 1,
                    1, 0, 0, 0, 1,
                    1, 1, 0, 1, 1,
                    1, 1, 1, 1, 1
                };
        }
        else
        {
            gridSize = 3;
            if (shape == MapShape.Frame)
                mask = new int[]
                    {
                        1, 1, 1,
                        1, 0, 1,
                        1, 1, 1
                    };
        }

        center = Mathf.FloorToInt(gridSize / 2);


        if (shape == MapShape.Frame)
        {
            switch (Random.Range(0, 3))
            {
                case 0:
                    startRoom = new int[] { 0, center };
                    endRoom = new int[] { gridSize - 1, center };
                    break;
                case 1:
                    startRoom = new int[] { center, 0 };
                    endRoom = new int[] { center, gridSize - 1 };
                    break;
                case 2:
                    startRoom = new int[] { gridSize - 1, center };
                    endRoom = new int[] { 0, center };
                    break;
                case 3:
                default:
                    startRoom = new int[] { center, gridSize - 1 };
                    endRoom = new int[] { center, 0 };
                    break;
            }
        }
        else
        {
            switch (Random.Range(0, 3))
            {
                case 0:
                    startRoom = new int[] { 0, 0 };
                    endRoom = new int[] { gridSize - 1, gridSize - 1 };
                    break;
                case 1:
                    startRoom = new int[] { gridSize - 1, 0 };
                    endRoom = new int[] { 0, gridSize - 1 };
                    break;
                case 2:
                    startRoom = new int[] { gridSize - 1, gridSize - 1 };
                    endRoom = new int[] { 0, 0 };
                    break;
                case 3:
                default:
                    startRoom = new int[] { 0, gridSize - 1 };
                    endRoom = new int[] { gridSize - 1, 0 };
                    break;
            }
            mask = new int[gridSize * gridSize];
            populate(mask, 1);
        }

        startElevator.transform.position = new Vector3((startRoom[0] + 0.5f) * RoomUnit.TILE_RATIO * RoomTile.TILE_SCALE - 1.5f, -2.0f, -(startRoom[1] - 0.5f) * RoomUnit.TILE_RATIO * RoomTile.TILE_SCALE - 1.5f);
        endElevator.transform.position = new Vector3((endRoom[0] + 0.5f) * RoomUnit.TILE_RATIO * RoomTile.TILE_SCALE - 1.5f, -3.45f, -(endRoom[1] - 0.5f) * RoomUnit.TILE_RATIO * RoomTile.TILE_SCALE - 1.5f);

        GameManager.player.transform.position = startElevator.transform.position;
        GameManager.player.transform.parent = startElevator.transform;

        Camera.main.transform.position = new Vector3(GameManager.player.transform.position.x - 7.5f, GameManager.player.transform.position.y + 11.1f, GameManager.player.transform.position.z - 7.5f);

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
        roomRollOdds = layout == RoomLayout.Maze ? 10 : 3;

        for (j = 0; j < mapGrid.GetLength(1); j++)
            for (i = 0; i < mapGrid.GetLength(0); i++)
                updateRoom(i, j);

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
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject obj in enemies)
        {
            obj.SetActive(false);
        }
        yield return new WaitForSeconds(2f);
        AstarPath p = FindObjectOfType<AstarPath>();
        AstarPath.active.Scan();
        yield return new WaitForSeconds(1f);
        foreach (GameObject obj in enemies)
        {
            if (obj != null)
                obj.SetActive(true);
        }
    }

    void Update() {
        if (completed)
            return;


        Debug.Log(i  + ", " + j);

        if (mapGrid[i, j] != null && mapGrid[i, j].segment < 0)
        {
            containsElevator = startRoom[0] == i && startRoom[1] == j || endRoom[0] == i && endRoom[1] == j;
            offset = containsElevator ? 1 : 0;
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
                    tiles = go.GetComponentsInChildren<RoomTile>();

                    if (containsElevator)
                    {
                        elevatorShaft = Instantiate(Resources.Load("Elevator/ElevatorShaft") as GameObject);
                        elevatorShaft.transform.parent = go.transform;

                        x = RoomUnit.TILE_RATIO / 2;
                        y = RoomUnit.TILE_RATIO * (x + 1);
                        elevatorShaft.transform.position = tiles[y - x - 1].transform.position;

                        for (l = 0; l < 9; l++)
                            tiles[y + RoomUnit.TILE_RATIO * (l/3 - 1) - x - (l % 3)].gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;
                    }

                    if (rotateMod > 0)
                    {
                        go.transform.Rotate(Vector3.up * -90 * rotateMod);
                        go.transform.position = new Vector3(go.transform.position.x + (rotateMod < 3 ? RoomTile.TILE_SCALE * (RoomUnit.TILE_RATIO - 1) : 0), 0, go.transform.position.z + (rotateMod > 1 ? RoomTile.TILE_SCALE * (RoomUnit.TILE_RATIO - 1) : 0));

                        for (l = 0; l < tiles.Length; l++)
                            tiles[l].transform.Rotate(Vector3.up * 90 * rotateMod);
                    }

                    go.GetComponent<RoomBuilder>().HideWalls(i + 1 < mapGrid.GetLength(0) && mapGrid[i + 1, j] != null, j + 1 < mapGrid.GetLength(1) && mapGrid[i, j + 1] != null);
                    rooms.Add(go);
                }
                else
                    Debug.LogError("Unable to find room:" +
                        "\n   Top: " + mapGrid[i, j].doors[3] +
                        "\n   Right: " + mapGrid[i, j].doors[0] +
                        "\n   Bottom: " + mapGrid[i, j].doors[1] +
                        "\n   Left: " + mapGrid[i, j].doors[2]
                    );
                progress++;
            } while (offset-- > 0 && total == 0);
        }

        if (++i >= mapGrid.GetLength(0))
        {
            i = 0;
            j++;
        }

        if (progress == totalProgress)
        {
            completed = true;

            GameManager.events.MapGenerated();
            StartCoroutine(DelayedScan());
            GameObject.Find("Canvas").GetComponent<GenerateHealthScript>().moveAllHealthBars();
        }

        GameManager.events.LoadingProgress(progress/totalProgress);
    }

    private void displayProgress()
    {

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
