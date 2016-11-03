using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CurveDraw : MonoBehaviour {

	//public Vector3[] points;

	public GameObject line;
	private bool drawing;
	private List<Vector3> curvePoints;
	private int verticeIndex;
	private int indicieIndex;
	private Mesh mesh;
	private Vector3[] vertices;
	private int[] indicies;
	private float angle;

	// Use this for initialization
	void Start () {
		drawing = false;
		curvePoints = new List<Vector3> ();
		mesh = new Mesh ();
		vertices = new Vector3[500];
		indicies = new int[800];
		indicies = new int[1500];
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

	public void AddPoint(Vector3 p){
		if (curvePoints.Count == 0) {
			InitializeMesh (p);
		} else {
			Debug.Log ("Added point");
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
		mesh.vertices = vertices;

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
		mesh.triangles = indicies;
		line.GetComponent<MeshFilter> ().mesh = mesh;
	}

	void DrawNewPoint(Vector3 newPoint){
		CheckArraySizes ();
		Vector3 previousPoint = curvePoints [curvePoints.Count - 1];
		angle = Mathf.Atan2(newPoint.z - previousPoint.z, previousPoint.x - newPoint.x) * 180 / Mathf.PI;
		vertices[verticeIndex] = Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward + newPoint;
		verticeIndex++;
		vertices[verticeIndex] = Quaternion.AngleAxis(angle, Vector3.up) * -Vector3.forward + newPoint;
		verticeIndex++;

		indicies [indicieIndex] = verticeIndex - 1;
		indicieIndex++;
		indicies [indicieIndex] = verticeIndex - 2;
		indicieIndex++;
		indicies [indicieIndex] = verticeIndex - 3;
		indicieIndex++;
		indicies [indicieIndex] = verticeIndex - 1;
		indicieIndex++;
		indicies [indicieIndex] = verticeIndex - 0;
		indicieIndex++;
		indicies [indicieIndex] = verticeIndex - 2;
		indicieIndex++;
		mesh.vertices = vertices;
		mesh.triangles = indicies;
		line.GetComponent<MeshFilter> ().mesh = mesh;
	}

	void CheckArraySizes(){
		if (verticeIndex >= vertices.Length - 2) {
			Vector3[] temp = new Vector3[vertices.Length + 200];
			for (int i = 0; i < verticeIndex; i++) {
				temp [i] = vertices [i];
			}
			vertices = temp;
		}
		if (indicieIndex >= indicies.Length - 2) {
			int[] temp = new int[indicies.Length + 200];
			for (int i = 0; i < indicieIndex; i++) {
				temp [i] = indicies [i];
			}
			indicies = temp;
		}
	}
		
}
