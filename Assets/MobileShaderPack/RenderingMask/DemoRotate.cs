using UnityEngine;
using System.Collections;

public class DemoRotate : MonoBehaviour 
{
	[SerializeField] private Vector3 rotateAxis = Vector3.up;
	[SerializeField] private float rotateSpeed = 10.0f;
	
	void Update () 
	{
		transform.Rotate(rotateAxis, Time.deltaTime * rotateSpeed, Space.World);
	}
}
