using UnityEngine;
using System.Collections;

public class RagdollBehaviour : MonoBehaviour {
    public float DespawnTime;
    PoolManager pm;
	// Use this for initialization
    void Start ()
    {
        pm = FindObjectOfType<PoolManager>();
    }
	void OnEnable () {
        StartCoroutine(Repool());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator Repool()
    {
        yield return new WaitForSeconds(DespawnTime);
        pm.PoolObj(gameObject);
        GameManager.events.EnemyRagdollDespawn(gameObject);
    }
}
