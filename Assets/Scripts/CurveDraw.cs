using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CurveDraw : MonoBehaviour {

	public float points;

	public GameObject line;
	private bool drawing;
	private List<Vector3> curvePoints;
	private int verticeIndex;
	private int indicieIndex;
	private Mesh mesh;
	private Vector3[] vertices;
	private int[] indicies;

	// Use this for initialization
	void Start () {
		drawing = false;
		curvePoints = new List<Vector3> ();
		mesh = new Mesh ();
		points = new Vector3[500];
		indicies = new int[800];
		verticeIndex = 0;
		indicieIndex = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void StartDraw(){
		drawing = true;
		curvePoints = new List<Vector3> (); 
	}

	void AddPoint(Vector3 p){
		if (curvePoints.Count == 0) {
			InitializeMesh (p);
		} else {
			DrawNewPoint (p);
		}
		curvePoints.Add (p);
	}

	Vector3 CastPoint(Vector2 p){
		return Camera.main.ScreenToWorldPoint (new Vector3 (p.x, p.y, 0));
	}

	Vector3 CastPoint(Vector3 p){
		return Camera.main.ScreenToWorldPoint (p);
	}

	void InitializeMesh(Vector3 p){
		vertices[verticeIndex] = p;
		verticeIndex++;
		vertices[verticeIndex] = p;
		verticeIndex++;
		vertices[verticeIndex] = p;
		verticeIndex++;
		vertices[verticeIndex] = p;
		verticeIndex++;
		mesh.vertices = points;

		indicies[indicieIndex] = 0;
		indicieIndex++;
		indicies[indicieIndex] = 2;
		indicieIndex++;
		indicies[indicieIndex] = 1;
		indicieIndex++;
		indicies[indicieIndex] = 2;
		indicieIndex++;
		indicies[indicieIndex] = 3;
		indicieIndex++;
		indicies[indicieIndex] = 1;
		indicieIndex++;
		mesh.SetIndices (indicies);
		line.GetComponent<MeshFilter> ().mesh = mesh;
	}

	void DrawNewPoint(Vector3 newPoint){
		Vector3 previousPoint = curvePoints [curvePoints.Count - 1];
	}
		
}
