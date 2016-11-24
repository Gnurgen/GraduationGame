using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Water : MonoBehaviour 
{
	[SerializeField] private Camera refractionCamera;

	#region Reflection

	[SerializeField] private bool useDynamicReflection = true;

	/// <summary>
	/// please assign a fake reflection texture if you don't use dynamic reflection => "useDynamicReflection = false".
	/// </summary>
	[SerializeField] private Texture2D fakeReflectionTexture;

	[SerializeField] private int reflectionTextureSize = 512;
	[SerializeField] private float reflClipPlaneOffset = 0.0f;
	[SerializeField] private Camera reflectionCamera;

	private RenderTexture reflectionTexture = null;

	#endregion

	private RenderTexture refractionTexture = null;


	private static bool isReflectionRendering = false;

	void Awake ()
	{
		Renderer rd = GetComponent<Renderer>();
		if (!enabled || !rd || !rd.sharedMaterial || !rd.enabled)
		{
			Debug.LogError("Please make sure water renderer is available!");
			return;
		}
	
		// initialize refraction settings.
		InitRefraction(rd);

		// initialize reflection settings.
		InitReflection(rd);
	}

	private void InitRefraction (Renderer waterRenderer)
	{
		if(refractionCamera != null)
		{
			refractionTexture = new RenderTexture(Screen.width, Screen.height, 16, RenderTextureFormat.ARGB32);
			refractionTexture.isPowerOfTwo = false;
			refractionCamera.targetTexture = refractionTexture;
			refractionCamera.Render();
			
			waterRenderer.sharedMaterial.SetTexture("_RenderTex", refractionTexture);
		}
		else
		{
			Debug.LogError("Please assign refraction camera!");
			return;
		}
	}

	private void InitReflection (Renderer waterRenderer)
	{
		if(reflectionCamera != null)
		{
			reflectionCamera.enabled = false;
		}

		if(useDynamicReflection)
		{
			if(reflectionCamera != null)
			{
				reflectionTexture = new RenderTexture( reflectionTextureSize, reflectionTextureSize, 16, RenderTextureFormat.ARGB32);
				reflectionTexture.name = "ReflectionTexture";
				reflectionTexture.isPowerOfTwo = true;
				reflectionTexture.hideFlags = HideFlags.DontSave;
				
				reflectionCamera.targetTexture = reflectionTexture;
				reflectionCamera.enabled = false;
				
				if(waterRenderer.sharedMaterial.HasProperty("_ReflectionTex"))
					waterRenderer.sharedMaterial.SetTexture("_ReflectionTex", reflectionTexture);
			}
			else
			{
				Debug.LogError("Please assign reflectionCamera since you want to use dynamic reflection!");
				return;
			}
			
		}
		else
		{
			if(fakeReflectionTexture != null)
			{
				if(waterRenderer.sharedMaterial.HasProperty("_ReflectionTex"))
					waterRenderer.sharedMaterial.SetTexture("_ReflectionTex", fakeReflectionTexture);
			}
			else
			{
				Debug.LogError("Please assign fakeReflectionTexture since you don't use dynamic reflection!");
			}
		}
	}

	void OnDisable()
	{
		#if UNITY_EDITOR
		if(reflectionTexture) 
		{
			DestroyImmediate(reflectionTexture);
			reflectionTexture = null;
		}
		
		if(refractionTexture)
		{
			DestroyImmediate(refractionTexture);
			refractionTexture = null;
		}
		#endif
	}
	
	private void OnWillRenderObject()
	{	
		if(!useDynamicReflection)
			return;

		Camera cam = Camera.current;
		if(!cam)
			return;
		      
		if(isReflectionRendering)
			return;
		
		isReflectionRendering = true;

		Vector3 pos = transform.position;
		Vector3 normal = transform.up;
		
		SynchronousCamera(cam, reflectionCamera);

		float d = -Vector3.Dot (normal, pos) - reflClipPlaneOffset;
		Vector4 reflectionPlane = new Vector4 (normal.x, normal.y, normal.z, d);
		
		Matrix4x4 reflection = Matrix4x4.zero;
		CalculateReflectionMatrix (ref reflection, reflectionPlane);
		Vector3 oldpos = cam.transform.position;
		Vector3 newpos = reflection.MultiplyPoint( oldpos );
		reflectionCamera.worldToCameraMatrix = cam.worldToCameraMatrix * reflection;
		
		// cull out surfaces blow water plane.
		Vector4 clipPlane = CameraSpacePlane(reflectionCamera, pos, normal, 1.0f);
		Matrix4x4 projection = cam.CalculateObliqueMatrix(clipPlane);
		reflectionCamera.projectionMatrix = projection;
		

		GL.SetRevertBackfacing (true);
		reflectionCamera.transform.position = newpos;
		Vector3 euler = cam.transform.eulerAngles;
		reflectionCamera.transform.eulerAngles = new Vector3(0, euler.y, euler.z);
		reflectionCamera.Render();
		reflectionCamera.transform.position = oldpos;
		GL.SetRevertBackfacing (false);
		
		isReflectionRendering = false;
	}
		
	private void SynchronousCamera(Camera src, Camera dest)
	{
		if( dest == null )
			return;

		dest.farClipPlane = src.farClipPlane;
		dest.nearClipPlane = src.nearClipPlane;
		dest.orthographic = src.orthographic;
		dest.fieldOfView = src.fieldOfView;
		dest.aspect = src.aspect;
		dest.orthographicSize = src.orthographicSize;
	}
	
	private Vector4 CameraSpacePlane (Camera cam, Vector3 pos, Vector3 normal, float sideSign)
	{
		Vector3 offsetPos = pos + normal * reflClipPlaneOffset;
		Matrix4x4 m = cam.worldToCameraMatrix;
		Vector3 cpos = m.MultiplyPoint( offsetPos );
		Vector3 cnormal = m.MultiplyVector( normal ).normalized * sideSign;
		return new Vector4( cnormal.x, cnormal.y, cnormal.z, -Vector3.Dot(cpos,cnormal) );
	}

	#region Tools
	private static void CalculateReflectionMatrix (ref Matrix4x4 reflMatrix, Vector4 plane)
	{
		reflMatrix.m00 = (1.0F - 2.0F * plane[0] * plane[0]);
		reflMatrix.m01 = (-2.0F * plane[0] * plane[1]);
		reflMatrix.m02 = (-2.0F * plane[0] * plane[2]);
		reflMatrix.m03 = (-2.0F * plane[3] * plane[0]);
		
		reflMatrix.m10 = (-2.0F * plane[1] * plane[0]);
		reflMatrix.m11 = (1.0F - 2.0F * plane[1] * plane[1]);
		reflMatrix.m12 = (-2.0F * plane[1] * plane[2]);
		reflMatrix.m13 = (-2.0F * plane[3] * plane[1]);
		
		reflMatrix.m20 = (-2.0F * plane[2] * plane[0]);
		reflMatrix.m21 = (-2.0F * plane[2] * plane[1]);
		reflMatrix.m22 = (1.0F - 2.0F * plane[2] * plane[2]);
		reflMatrix.m23 = (-2.0F * plane[3] * plane[2]);
		
		reflMatrix.m30 = 0.0F;
		reflMatrix.m31 = 0.0F;
		reflMatrix.m32 = 0.0F;
		reflMatrix.m33 = 1.0F;
	}
	#endregion
}
