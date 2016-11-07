using UnityEngine;
using System.Collections;

public class CurveTest : MonoBehaviour {

	public float delay;
	private float currentDelay;
	public Vector3[] points;
	private int index;

	// Use this for initialization
	void Start () {
		currentDelay = delay;
		index = 0;
    }
	
	// Update is called once per frame
	void Update () {
		currentDelay = currentDelay - Time.deltaTime;
		if (currentDelay < 0 && index < points.Length) {
			GetComponent<CurveDraw> ().AddPoint (points [index]);
			index++;
			currentDelay = delay;
		}
	}
}
