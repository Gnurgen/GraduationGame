﻿using UnityEngine;
using System.Collections;

public class StartElevator : MonoBehaviour {
    private GameObject fade;
    private GameObject player;
    private float speed = 0.4f;
    private float step= 0;
    private Vector3 EndPos, startPos;
	void Start () {
        player = GameManager.player;
        fade = GameObject.Find("Fade");
        fade.GetComponent<Fade>().fadeFromBlack(3);
        EndPos = new Vector3(transform.position.x, -3.45f, transform.position.z);
        startPos = new Vector3 (transform.position.x, -5.45f, transform.position.z);
        transform.position = startPos;
        player.transform.localPosition = Vector3.zero + Vector3.up * 0.453073f; //the float is 0.5 - kumos height in the local position
    }

    void Update () {
        if (step <= 1)
        {
            step += speed * Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, EndPos, step);
        }
        else
        {
            GameManager.events.ElevatorMoveStop();
            player.transform.position = new Vector3(player.transform.position.x, 0, player.transform.position.z);
            player.transform.parent = null;
            enabled = false;
        }
	}
    
    
}
