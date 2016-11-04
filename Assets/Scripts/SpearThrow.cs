using UnityEngine;
using System.Collections;

public class SpearThrow : MonoBehaviour {

	public float spearSpeed;
	public float drawLength;
	public float damage;
	public float cooldown;
	private CurveDraw drawTool;
	private bool active;
	private float currentCooldown;

	// Use this for initialization
	void Start () {
		drawTool = GetComponent<CurveDraw> ();
		if (drawTool == null) {
			Debug.Log ("The CurveDraw component is not attached.");
		}
		active = false; 
	}
	
	// Update is called once per frame
	void Update () {
		currentCooldown = currentCooldown - Time.deltaTime;
		
	}

	public void UseAbility(){
		if (currentCooldown < 0) {
			// Take over input
			active = true;
		}
	}

	void GetRelease(){
		active = false;
		currentCooldown = cooldown;
	}

	void GetNewPoints(Vector3 p){
		if (active) {
			drawTool.AddPoint (p);
		}
	}
}
