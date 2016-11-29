using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpearController : MonoBehaviour {

    private int springForce;
    private float stunTime;
    private float damage;
	private float speed;
	private int index;
    //private float step;
    private int gameIDIndex = 0;
    private GameObject[] gameID = new GameObject[50];
    private   Vector3[] points;
    private float turnRate;
    private float pushForce;

    IEnumerator fly()
    {
        while (index < points.Length)
        {
            transform.LookAt(points[index]);
            while (Vector3.Distance(transform.position, points[index]) > 0 && NotPassedPoint(transform.position, points[index]))
            {
                transform.position += transform.forward * speed * Time.deltaTime;
                yield return null;
            }
            ++index;
            yield return null;
        }
        for (int i = 0; i < gameIDIndex; i++)
        {
            if (i == 3)
                break;
            else
            {
                try
                {
                    Destroy(gameID[i].GetComponent<SpringJoint>());
                    gameID[gameIDIndex - 1].GetComponent<EnemyStats>().PauseFor(0.2f);
                    gameID[i].GetComponent<Rigidbody>().velocity = Vector3.zero;
                }
                catch { };
            }
        }
        GameManager.events.SpearDrawAbilityEnd(gameObject);
        Destroy(gameObject);
        yield break;
    }

	/*void Update () {
        
		if (index < points.Length) {
            transform.LookAt(points[index]);
            transform.position += transform.forward * speed * Time.deltaTime;
            if (!NotPassedPoint(transform.position, points[index]))
                index++;

        } else {
            
	}*/

    private bool NotPassedPoint(Vector3 pos, Vector3 tar)
    {
        print(Vector3.Dot(transform.forward, (tar - pos).normalized));
       return Vector3.Dot(transform.forward, (tar - pos).normalized) > 0f;
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Boss")
        {
            bool hit = true;
            for (int i = 0; i <= gameIDIndex; i++)
            {
                if (gameID[i] == col.gameObject)
                {
                    hit = false;
                }
            }
            if (hit)
            {
                col.GetComponent<Health>().decreaseHealth(damage, Vector3.zero, pushForce);
                GameManager.events.SpearDrawAbilityHit(col.gameObject);
                gameID[gameIDIndex] = col.gameObject;
                gameIDIndex++;
            }
        }
        if(col.tag == "Enemy")
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
                GameManager.events.SpearDrawAbilityHit(col.gameObject);
                col.GetComponent<Health>().decreaseHealth(damage, (col.transform.position - transform.position), pushForce);
                gameID[gameIDIndex] = col.gameObject;
                gameIDIndex++;
            }
            if(gameIDIndex<4) // HOOKS 4-1 = 3 ENEMIES
            {
                gameID[gameIDIndex - 1].AddComponent<SpringJoint>();
                gameID[gameIDIndex - 1].GetComponent<SpringJoint>().spring = springForce;
                gameID[gameIDIndex - 1].GetComponent<SpringJoint>().connectedBody = GetComponent<Rigidbody>();
                gameID[gameIDIndex - 1].GetComponent<EnemyStats>().PauseFor(stunTime);
            }
        }
    }

	public void SetParameters(List<Vector3> ps, float speed, float damage, float force, int dragForce, float altitude, float turn, float st){
		points = ps.ToArray ();
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = new Vector3(points[i].x, altitude, points[i].z);
        }
        transform.position = points[0];
		index = 1;
		this.speed = speed;
        this.damage = damage;
        pushForce = force;
        springForce = dragForce*100;
        turnRate = turn;
        stunTime = st;
        StartCoroutine(fly());
	}
    
}
