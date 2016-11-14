using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpearController : MonoBehaviour {

	public float distance;
    public float heightOfSpear;
    private float damage;
	private float speed;
	private int index;
    //private float step;
    public int gameIDIndex = 0;
    GameObject[] gameID = new GameObject[50];
    public  Vector3[] points;
    public float turnRate;

    // Use this for initialization
    void Start () {


        for (int i = 0; i < points.Length; i++)
        {
            points[i] = new Vector3(points[i].x, heightOfSpear, points[i].z);
        }

    }
	
	// Update is called once per frame
	void Update () {
        
		if (index < points.Length) {

            /* KRistoffers forsøg på et bedre movement for spear
            step += speed / ( 1 + Vector3.Distance(points[index - 1],points[index + 0]) )* Time.deltaTime;
            transform.position = Vector3.Lerp(points[index - 1], points[index + 0], step);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(points[index - 1] - points[index + 0]) * Quaternion.Euler(270,0,0),step);

            if (step >= 1)
            {
                ++index;
                step = 0;
            }*/
      

            transform.LookAt(points[index]);
            transform.position += transform.forward * speed * Time.deltaTime;
            if (Vector3.Distance(transform.position, points[index]) < distance)
            {
                index++;
            }

        } else {
			Destroy (gameObject);
		}
	}

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Melee" || col.tag == "Ranged")
        {
            bool hit = true;
            for (int i = 0; i <= gameIDIndex; i++)
            {
                if(gameID[i] == col.gameObject)
                {
                    hit = false;
                }
            }
            if(hit)
            {
                col.GetComponent<Health>().decreaseHealth(damage);
                GameManager.events.PlayerAttackHit(gameObject, col.gameObject, damage);
                gameID[gameIDIndex] = col.gameObject;
                gameIDIndex++;
            }
            if(gameIDIndex<3)
            {
                gameID[gameIDIndex - 1].AddComponent<SpringJoint>();
                gameID[gameIDIndex - 1].GetComponent<SpringJoint>().connectedBody = GetComponent<Rigidbody>();
            }
        }
    }

	public void SetParameters(List<Vector3> ps, float speed, float damage){
		points = ps.ToArray ();
		transform.position = points[0];
		index = 1;
		this.speed = speed;
        this.damage = damage;
	}
    void AddGameID()
    {
        
    }
}
