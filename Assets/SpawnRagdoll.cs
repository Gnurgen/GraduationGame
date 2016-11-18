using UnityEngine;
using System.Collections;

public class SpawnRagdoll : MonoBehaviour {
    public Transform[] Ragdoll;
    PoolManager poolManager;
    private string _tag;
    public string myTag
    {
        set
        {
            _tag = value;
        }
    }

	// Use this for initialization
	void Start () {
        poolManager = FindObjectOfType<PoolManager>();
    }

    public void Execute()
    {
        poolManager.GenerateRagdoll(Ragdoll,_tag);
        Destroy(gameObject);
    }
}
