﻿using UnityEngine;
using System.Collections;

public class snekmesh : MonoBehaviour
{

    Vector2[] allUV;
    Vector3[] allNormals;
    private int[] newTriangles;
    int vertsInShape;
    int segments = 6;
    float step;
    Vector3[] allVertices;
    int[] allTriangles;
    Mesh mesh;
    Material meshColor;
    // Use this for initialization
    Vector3[] vertices = new Vector3[]{
            new Vector3( -0.25f, -0.5f, 0),
            new Vector3( 0.25f,-0.5f,0),
            new Vector3( 0.5f, 0.0f, 0),
            new Vector3( 0.25f, 0.25f, 0),
            new Vector3( 0f, 0.35f, 0),
            new Vector3( -0.25f, 0.25f, 0),
            new Vector3( -0.5f, 0.0f, 0),
            new Vector3( -0.25f,-0.5f,0)

        };

    int[] trisFront = new int[]
    {
        0,6,5,0,5,4,0,4,1,1,4,3,1,3,2

    };
    int[] trisBack = new int[]
    {
    0,5,6,0,4,5,0,1,4,1,3,4,1,2,3
    };
    void Start()
    {

        mesh = new Mesh();
        mesh.name = gameObject.GetInstanceID().ToString();
        if (GetComponent<MeshFilter>() == null)
            gameObject.AddComponent<MeshFilter>();
        if (GetComponent<MeshRenderer>() == null)
            gameObject.AddComponent<MeshRenderer>();
        GetComponent<MeshFilter>().mesh = mesh;
        vertsInShape = vertices.Length;
        meshColor = GetComponent<MeshRenderer>().material;
        //allUV = new Vector2[offsetVertices];
        allVertices = new Vector3[vertsInShape * segments * 2];
        allTriangles = new int[vertsInShape * segments * 12];
        allUV = new Vector2[vertsInShape * segments * 2];
        allNormals = new Vector3[vertsInShape * segments * 2];


        for (int y = 0; y < segments + 1; y++)
        {
            for (int x = 0; x < vertsInShape; x++)
            {
                if (y == 0 || y == segments)
                {
                    if (y == 0)
                        allVertices[x + (y * vertsInShape)] = vertices[x] / 2;
                    else
                        allVertices[x + (y * vertsInShape)] = vertices[x] / 2 + Vector3.up / 2;
                    allVertices[x + (y * vertsInShape)].z = y / 2f - segments / 4f;
                }
                else
                {
                    allVertices[x + (y * vertsInShape)] = vertices[x];
                    allVertices[x + (y * vertsInShape)].z = y / 2f - segments / 4f;
                }
                allUV[x + (y * vertsInShape)] = new Vector2(y / (0.25f * segments), x);
                allNormals[x + (y * vertsInShape)] = (allVertices[x + (y * vertsInShape)] - new Vector3(0.0f, 0.0f, y / 2f - segments / 4f)).normalized;
            }
        }
        int ti = 0;
        // front
        for (int t = 0; t < trisFront.Length; ++t)
        {
            allTriangles[ti] = trisFront[t]; ++ti;
        }
        for (int i = 0; i < segments; i++) //Generate triangles for drawn mesh
        {
            int offset = i * vertsInShape;
            for (int l = 0; l < vertsInShape; l += 1)
            {
                int a = offset + l + vertsInShape;
                int b = offset + l;
                int c = offset + l + 1;
                int d = offset + l + 1 + vertsInShape;
                allTriangles[ti] = a; ti++;
                allTriangles[ti] = b; ti++;
                allTriangles[ti] = c; ti++;
                allTriangles[ti] = c; ti++;
                allTriangles[ti] = d; ti++;
                allTriangles[ti] = a; ti++;
            }
        }
        for (int t = 0; t < trisFront.Length; ++t)
        {
            allTriangles[ti] = trisBack[t] + vertsInShape * segments; ++ti;
        }
        mesh.Clear();

        mesh.vertices = allVertices;
        mesh.triangles = allTriangles;
        mesh.normals = allNormals;
        mesh.uv = allUV;

    }
    // Update is called once per frame
    void Update()
    {
        /*
        step += 7 * Time.deltaTime;
        for (int y = 0; y < segments + 1; ++y)
        {
            for (int x = 0; x < vertsInShape; ++x)
            {
                if (x == 7 || x == 0)
                    allVertices[x + y * vertsInShape].y += Mathf.Sin(step + y * 50 * Time.deltaTime) / 120;
                if (x == 1)
                    allVertices[x + y * vertsInShape].y += Mathf.Sin(step + y * 50 * Time.deltaTime) / 120;
                if (x <= 6 && x >= 2)
                {
                    allVertices[x + y * vertsInShape].z += Mathf.Sin(step + y * 10 * Time.deltaTime) / 60;
                    allVertices[x + y * vertsInShape].y += Mathf.Sin(step + x * y * 10 * Time.deltaTime) / 60;
                }
            }
        }
        //meshColor.color = Color.Lerp(Color.red, Color.white, Life);
        mesh.vertices = allVertices;
        */
    }
    public void SetColor(float life)
    {
        meshColor.color = Color.Lerp(Color.red, Color.white, life);
    }
}