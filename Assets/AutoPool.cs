using UnityEngine;
using System.Collections;

public class AutoPool : MonoBehaviour {
    private PoolManager pm;
	// Use this for initialization
	void Start () {
        pm = FindObjectOfType<PoolManager>();
        pm.PoolObj(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
