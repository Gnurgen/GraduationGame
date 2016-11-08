using UnityEngine;
using System.Collections;

public class RoomUnit : MonoBehaviour {

    public const int TILE_RATIO = 11;

    private RoomTile[] tiles = new RoomTile[TILE_RATIO * TILE_RATIO];
    private RoomWall[] walls = new RoomWall[4];

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
