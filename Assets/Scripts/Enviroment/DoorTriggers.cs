using UnityEngine;
using System.Collections;

public class DoorTriggers : MonoBehaviour {
    public bool isOne = false;
	void Start () {
        if (gameObject.name == "trigger1")
            isOne = true;

	}
    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {

        }
        Debug.Log("Close Doors");
    }
}
