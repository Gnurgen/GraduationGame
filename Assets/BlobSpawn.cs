using UnityEngine;
using System.Collections;

public class BlobSpawn : MonoBehaviour {

	void OnEnable()
    {
        GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(0, 5), Random.Range(0, 5), Random.Range(0, 5)));
    }
}
