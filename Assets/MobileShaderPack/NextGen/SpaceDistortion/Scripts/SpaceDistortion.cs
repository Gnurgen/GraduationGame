using UnityEngine;
using System.Collections;

public class SpaceDistortion : MonoBehaviour 
{
	[SerializeField] private Camera effectCamera;
	[SerializeField] private Material screenMat;

	[SerializeField] private int renderTextureWidth = 512;
	[SerializeField] private int renderTextureHeight = 512;

	private RenderTexture heatTex;

	void Start ()
	{
		heatTex = new RenderTexture(renderTextureWidth, renderTextureHeight, 24, RenderTextureFormat.ARGB32);
		heatTex.wrapMode = TextureWrapMode.Repeat;
		heatTex.Create();

		effectCamera.targetTexture = heatTex;
		effectCamera.fieldOfView = Camera.main.fieldOfView;
		screenMat.SetTexture("_DistortionTex", heatTex);
	}

	void OnRenderImage (RenderTexture src, RenderTexture dst)
	{
		Graphics.Blit(src, dst, screenMat);
	}
}
