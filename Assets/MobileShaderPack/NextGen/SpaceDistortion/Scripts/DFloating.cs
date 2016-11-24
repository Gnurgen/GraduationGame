using UnityEngine;
using System.Collections;

public class DFloating : MonoBehaviour 
{
	[SerializeField] private float Length = 3.0f;
	[SerializeField] private float speed = 1.0f;
	[SerializeField] private Vector3 direction = Vector3.forward;
	
	private float currentAngle = 0;
	private Vector3 startPos;
	
	void Awake () 
	{
		startPos = transform.position;
		direction = direction.normalized;
	}
	
	void Update () 
	{
		currentAngle += Time.deltaTime * speed;
		if(currentAngle >= 2.0f * Mathf.PI)
		{
			currentAngle -= 2.0f * Mathf.PI;
		}
		
		Vector3 newPos = startPos + Mathf.Sin(currentAngle) * Length * direction;
		transform.position = newPos;
	}
}
