using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpearThrow : MonoBehaviour {

	public GameObject spearPrefab;
	public float spearSpeed;
	public float drawLength;
	public float damage;
	public float cooldown;
	private CurveDraw drawTool;
	private InputManager im;
	private bool active;
	private float currentCooldown;
	private float currentDrawLength;
	private bool initialized;
	private Vector3 lastPoint;

	// Use this for initialization
	void Start () {
		drawTool = GetComponent<CurveDraw> ();
        im = FindObjectOfType<InputManager>();
		if (im == null) {
			Debug.Log ("The InputManager component is not attached");
		}
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
        Debug.Log("ABILITY USED!");
		if (currentCooldown < 0) {
			// Take over input
			active = true;
			currentDrawLength = 0;
			initialized = false;
			im.curState = InputManager.InputState.draw;
			im.OnDrag += GetNewPoints;
            Invoke("Sub2Release", 0.5f);
		}
	}

	void GetRelease(Vector3 p){
		active = false;
		currentCooldown = cooldown;
		im.curState = InputManager.InputState.move;
		im.OnTouchEnd -= GetRelease;
		im.OnDrag -= GetNewPoints;
		ThrowSpear (drawTool.GetPoints());
        drawTool.CleanUp();
    }

	void GetNewPoints(Vector3 p){
		if (active && currentDrawLength < drawLength) {
			if (!initialized) {
                lastPoint = p;
                initialized = true;

            }
			currentDrawLength += Vector3.Distance (lastPoint, p);
			drawTool.AddPoint (p);
            lastPoint = p;
		}
	}

	void ThrowSpear(List<Vector3> ps){
		GameObject spear = Instantiate (spearPrefab) as GameObject;
		spear.GetComponent<SpearController> ().SetParameters(ps,spearSpeed);
	}

    private void Sub2Release()
    {
        im.OnTouchEnd += GetRelease;
    }
}
