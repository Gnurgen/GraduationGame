using UnityEngine;
using System.Collections;

public class angle : MonoBehaviour {

    public GameObject lol,s1,s2;
    float angles;
	// Use this for initialization
	void Start () {
	print(Vector3.Angle(transform.position,lol.transform.position));
	}
	
	// Update is called once per frame
	void Update () {
      
        angles = Mathf.Atan2(transform.position.z - lol.transform.position.z, lol.transform.position.x - transform.position.x) * 180 / Mathf.PI;
        print(angles);
        s1.transform.position =  Quaternion.AngleAxis(angles, Vector3.up) * -Vector3.forward + transform.position;
        s2.transform.position = Quaternion.AngleAxis(angles, Vector3.up) * Vector3.forward + transform.position;
    }
}
