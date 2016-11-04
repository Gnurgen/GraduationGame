using UnityEngine;
using System.Collections;

public class killme : MonoBehaviour {
    public GameObject RD;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnTriggerEnter (Collider col)
    {
        if(col.gameObject.tag == "Player")
        {
            Instantiate(RD,transform.position,transform.rotation);
            Destroy(transform.parent.parent.gameObject);
        }
    }
}
