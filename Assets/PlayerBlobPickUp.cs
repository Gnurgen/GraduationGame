using UnityEngine;
using System.Collections;

public class PlayerBlobPickUp : MonoBehaviour {

    [Header ("Pickup Radius")]
    public float radius;

    Collider[] objCols;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        objCols = Physics.OverlapSphere(transform.position, radius);

        for(int i = 0; i < objCols.Length;++i)
        {
            if(objCols[i].tag == "Blob")
            {
                if (Vector3.Distance(transform.position, objCols[i].transform.position) < 2f)
                {
                    GameManager.events.ResourcePickup(objCols[i].gameObject, 1);
                    
                    GameManager.events.PoolObject(objCols[i].gameObject);

                    objCols[i].gameObject.SetActive(false);
                }
                else
                    objCols[i].GetComponent<Rigidbody>().AddForce((transform.position - objCols[i].transform.position).normalized * Time.deltaTime * 1337);
            }
        }
            
	}
}
