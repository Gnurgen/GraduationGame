using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpearController : MonoBehaviour {

	public float distance;
	private float speed;
	private int index;
	private Vector3[] points;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (index < points.Length) {
			if (Vector3.Distance (transform.position, points [index]) < distance) {
				index++;
			}
			transform.LookAt (points [index]);
			transform.position = Vector3.MoveTowards (transform.position, points [index], speed * Time.deltaTime);
		} else {
			Destroy (gameObject);
		}
	}

	public void SetParameters(List<Vector3> ps, float speed){
		points = ps.ToArray ();
		transform.position = points[0];
		index = 1;
		this.speed = speed;
	}
}
