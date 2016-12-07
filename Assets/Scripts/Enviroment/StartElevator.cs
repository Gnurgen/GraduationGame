using UnityEngine;
using System.Collections;
using System;

public class StartElevator : MonoBehaviour {
    private GameObject fade;
    private GameObject player;
    private float speed = 0.4f;
    private float step= 0;
    private Vector3 EndPos, startPos;
	void Start () {
        player = GameManager.player;
        EndPos = new Vector3(transform.position.x, -3.45f, transform.position.z);
        startPos = new Vector3 (transform.position.x, -6.45f, transform.position.z);
        transform.position = startPos;
        GameManager.events.OnLoadComplete += MoveUp;
        player.transform.localPosition = Vector3.zero + Vector3.up * 0.453073f; //the float is 0.5 - kumos height in the local position
    }

    private void MoveUp()
    {
      
        StartCoroutine(MoveElevatorUp());
    }

    IEnumerator MoveElevatorUp() {
        /*while (step <= 1)
        {
            step += speed * Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, EndPos, step);
            yield return null;
        }*/
        transform.position = EndPos;
        GameManager.events.ElevatorMoveStop();
        player.transform.position = new Vector3(player.transform.position.x, 0, player.transform.position.z);
        player.transform.parent = null;
        enabled = false;
        yield break;
    }
    
    
}
