using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpearController : MonoBehaviour {

	public float distance;
    private float damage;
	private float speed;
	public int index;
	public Vector3[] points;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (index < points.Length) {
			transform.LookAt (points [index]);
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
            col.GetComponent<Health>().decreaseHealth(damage);
            GameManager.events.PlayerAttackHit(gameObject, col.gameObject, damage);
        }
    }

	public void SetParameters(List<Vector3> ps, float speed, float damage){
		points = ps.ToArray ();
		transform.position = points[0];
		index = 1;
		this.speed = speed;
        this.damage = damage;
	}
}
