using UnityEngine;
using System.Collections;

public class TestMoveWithPlayer : MonoBehaviour {

    public Transform player;
    Vector3 distance;
	// Use this for initialization
	void Start () {
        distance = transform.position - player.position;
    }
	
	// Update is called once per frame
	void Update () {
       
        transform.position = player.position + distance;
	}
}
