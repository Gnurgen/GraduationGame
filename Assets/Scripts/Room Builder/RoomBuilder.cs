using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomBuilder : MonoBehaviour {

    private static List<RoomBuilder> _list = new List<RoomBuilder>();

    public string roomName = "New Room";
    public int roomIndex = 0;
    public bool roomModified = true;
    public Vector2 roomSize = new Vector2(1,1);
    public RoomUnit[,] roomUnits = new RoomUnit[4,4];

    private List<GameObject> objectList = new List<GameObject>();

    public static List<RoomBuilder> list
    {
        get
        {
            _list.Add(null);
            _list.Add(null);
            return new List<RoomBuilder>(_list);
        }
    }

    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    void Update () {
	
	}

}
