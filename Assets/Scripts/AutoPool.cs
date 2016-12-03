using UnityEngine;
using System.Collections;

public class AutoPool : MonoBehaviour {
    private PoolManager pm;
	// Use this for initialization
	void Start ()
    { 
        StartCoroutine(PoolMe());
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    IEnumerator PoolMe()
    {
        yield return new WaitForSeconds(1f);
        pm = GameManager.pool;
        pm.PoolObj(gameObject);
        Destroy(this);
        yield return null;
    }
}
