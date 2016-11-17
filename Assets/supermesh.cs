using UnityEngine;
using System.Collections;

public class supermesh : MonoBehaviour {


    public int verts;
    Vector3[] vertices;
    int[] triangles;
    Vector3[] normals;
    Mesh mesh;
    const float _2pi = Mathf.PI * 2;
	// Use this for initialization
	void Start () {
        mesh = new Mesh();
        mesh.name = GetInstanceID().ToString();
        vertices = new Vector3[verts];
        normals = new Vector3[verts];
        triangles = new int[verts * 3];
        vertices[0] = Vector3.up; //Assign center vertix
        normals[0] = Vector3.up;
        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;
        for (int k = 1; k < verts; ++k) //Generate verticies for full circle
        {
            float phi = k * _2pi / (verts);
            vertices[k] = new Vector3(Mathf.Cos(phi), 1, -Mathf.Sin(phi));
            normals[k] = Vector3.up;
            if (k < verts - 1)
            {
            triangles[k * 3] = k;
            triangles[k * 3 + 1] = k + 1;
            triangles[k * 3 + 2] = 0;
            }
            else
            {

            }
        }
        vertices[vertices.Length - 1] = vertices[1];
        normals[vertices.Length - 1] = Vector3.up;
        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.triangles = triangles;
        GetComponent<MeshFilter>().mesh = mesh;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
