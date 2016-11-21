using UnityEngine;
using System.Collections;

public class RagdollBehaviour : MonoBehaviour {
    public float DespawnTime;
    PoolManager pm;
	// Use this for initialization
	void Awake () {
        pm = FindObjectOfType<PoolManager>();
        Invoke("Repool", DespawnTime);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void Repool()
    {
        pm.PoolObj(gameObject);
    }
}
