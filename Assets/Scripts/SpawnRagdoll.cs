using UnityEngine;
using System.Collections;

public class SpawnRagdoll : MonoBehaviour {
    public Transform[] Ragdoll;
    PoolManager poolManager;
    private string _tag;

    [SerializeField]
    bool debug = false;
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
        if(gameObject.tag=="Player")
            _tag = "KumoRagdoll";
    }

    public void Execute(Vector3 forceDir)
    {
        poolManager.GenerateRagdoll(Ragdoll,_tag, forceDir);
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (debug)
        {
            Execute(Vector3.zero);
        }
    }
}
