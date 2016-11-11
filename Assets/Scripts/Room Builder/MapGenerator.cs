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
    public MapShape layout;

    private int[,] mapGrid = new int[16,16];

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

    void Start () {
//        List<GameObject> objectList = Resources.LoadAll("Room").Cast<GameObject>().ToList();

    }

    void Update () {
	
	}
}
