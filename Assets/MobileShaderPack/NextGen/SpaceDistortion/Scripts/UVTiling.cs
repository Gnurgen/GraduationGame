using UnityEngine;
using System.Collections;

public class UVTiling : MonoBehaviour
{	
	public float tilingSpeed = 1.0f;

	void Start ()
	{
	}

	void Update () 
	{
		Vector2 offset = GetComponent<Renderer>().material.GetTextureOffset("_MainTex");
		offset.y += tilingSpeed * Time.deltaTime;
		while(offset.y > 1.0f) offset.y -= 1.0f;
	
		GetComponent<Renderer>().material.SetTextureOffset("_MainTex", offset);
	}
}
