using UnityEngine;
using System.Collections;

public class RagdollBehaviour : MonoBehaviour {
    public float DespawnTime;
    PoolManager pm;
	// Use this for initialization
    void Start ()
    {
        pm = FindObjectOfType<PoolManager>();
        Invoke("Repool", DespawnTime);
    }
	void OnEnable () {
      
        Invoke("Repool", DespawnTime);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void Repool()
    {
        GameObject whisp = pm.GenerateObject("Whisp");
        pm.transform.position = new Vector3(transform.position.x, 1, transform.position.z);
        pm.PoolObj(gameObject);
        GameManager.events.EnemyRagdollDespawn(gameObject);
    }
}
