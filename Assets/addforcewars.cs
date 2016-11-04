using UnityEngine;
using System.Collections;

public class addforcewars : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Rigidbody rig = GetComponent<Rigidbody>();
        rig.AddForce(-Vector3.forward*100, ForceMode.Impulse);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
