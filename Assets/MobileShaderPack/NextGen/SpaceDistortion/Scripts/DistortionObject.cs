using UnityEngine;
using System.Collections;

public class DistortionObject : MonoBehaviour 
{
	[SerializeField] private Material matForMainCam;
	[SerializeField] private Material matForEffectCam;

	public float tilingSpeed = 1.0f;

	void Awake ()
	{
		GetComponent<Renderer>().sharedMaterial = matForEffectCam;
	}

	void OnWillRenderObject ()
	{
		Camera currentCamera = Camera.current;
		if (currentCamera.name == "Main Camera") 
		{
			GetComponent<Renderer>().sharedMaterial = matForMainCam;
		}
		else if(currentCamera.name == "EffectCamera")
		{
			GetComponent<Renderer>().sharedMaterial = matForEffectCam;
			TilingUV();
		}
	}

	void TilingUV () 
	{
		Vector2 offset = GetComponent<Renderer>().sharedMaterial.GetTextureOffset("_MainTex");
		float delta = tilingSpeed * Time.deltaTime;
		delta = Mathf.FloorToInt(delta * 10000.0f) / 10000.0f;
		offset.y += delta;
		while(Mathf.Abs(offset.y) > 1.0f) offset.y -= 1.0f * Mathf.Sign(offset.y);
		
		GetComponent<Renderer>().sharedMaterial.SetTextureOffset("_MainTex", offset);
	}
}
