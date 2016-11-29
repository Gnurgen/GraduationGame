using UnityEngine;
using System.Collections;

public class AutoPool : MonoBehaviour {
    private PoolManager pm;
	// Use this for initialization
	void Start ()
    { 
        Invoke("PoolMe",1f);
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void PoolMe()
    {
        pm = FindObjectOfType<PoolManager>();
        pm.PoolObj(gameObject);
        Destroy(this);
    }
}
