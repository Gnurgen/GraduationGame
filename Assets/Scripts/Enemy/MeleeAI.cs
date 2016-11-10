using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[RequireComponent (typeof(Seeker))]
[RequireComponent (typeof(CharacterController))]
public class MeleeAI : MonoBehaviour {


	public GameObject aggroed;
	public float range;
	private Seeker seeker;

	// Use this for initialization
	void Start () {
		seeker = GetComponent<Seeker> ();
		stateHandler ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator stateHandler()
	{
		if (aggroed == null) {
			idle ();
		} else if (Vector3.Distance (transform.position, aggroed.transform.position) > range) {
			chasing ();
		} else {
			attacking ();
		}
		yield return null;
	}


	IEnumerator idle()
	{
		while(aggroed == null)
		{
			// Define idle behaviour

			yield return null;
		}
	}

	IEnumerator chasing()
	{
		while (aggroed != null && Vector3.Distance (transform.position, aggroed.transform.position) > range) 
		{
			
			yield return null;
		}
	}

	IEnumerator attacking()
	{
		while (aggroed != null && Vector3.Distance (transform.position, aggroed.transform.position) < range)
		{
			// Define attacking

			yield return null;
		}
	}
}
