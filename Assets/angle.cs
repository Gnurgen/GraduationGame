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
        print(Vector3.Angle(transform.position, lol.transform.position));
        angles = Vector3.Angle(transform.position, lol.transform.position);
        s1.transform.position =  Quaternion.AngleAxis(angles, Vector3.up) * -transform.forward + transform.position;
        s2.transform.position = Quaternion.AngleAxis(angles, Vector3.up) * transform.forward + transform.position;
    }
}
