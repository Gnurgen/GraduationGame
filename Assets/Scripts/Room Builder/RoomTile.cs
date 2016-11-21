using UnityEngine;
using System.Collections;

public class RoomTile : MonoBehaviour {

    public const int TILE_SCALE = 1;

    public string referenceName;
    public bool isDefault;
    public bool walkable;

    [HideInInspector]
    public int index;

    void Start () {
	
	}
	
	void Update () {
	
	}
}
