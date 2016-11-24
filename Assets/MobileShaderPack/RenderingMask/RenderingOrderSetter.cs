using UnityEngine;
using System.Collections;

/// <summary>
/// Rendering order setter.
/// If you have problem with rendering a geometry in front of Mask Object, use this script to set geometry rendering order bigger like "3004".
/// </summary>
public class RenderingOrderSetter : MonoBehaviour 
{
	[SerializeField] private int order = 3004;

	void Awake () 
	{
		Renderer rd = gameObject.GetComponent<Renderer>();
		if(rd != null)
		{
			rd.material.renderQueue = order;
		}
	}
}
