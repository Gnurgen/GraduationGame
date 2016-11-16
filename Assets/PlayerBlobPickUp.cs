﻿using UnityEngine;
using System.Collections;

public class PlayerBlobPickUp : MonoBehaviour {

    public float HealthGain = 1;
    [Header ("Pickup Radius")]
    public float RadiusOfAttraction;
    private float RadiusOfPickup = 1.1f;
    

    Collider[] objCols;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        objCols = Physics.OverlapSphere(transform.position, RadiusOfAttraction);

        for(int i = 0; i < objCols.Length;++i)
        {
            if(objCols[i].tag == "Blob")
            {
                if (Vector3.Distance(transform.position, objCols[i].transform.position) < RadiusOfPickup) 
                {
                    GameManager.events.ResourcePickup(objCols[i].gameObject, 1);
                    GetComponent<Health>().decreaseHealth(-HealthGain);
                    GameManager.events.PoolObject(objCols[i].gameObject);

                    objCols[i].gameObject.SetActive(false);
                }
                else
                    objCols[i].GetComponent<Rigidbody>().AddForce((transform.position - objCols[i].transform.position).normalized * Time.deltaTime * 1337);
            }
        }
            
	}
}
