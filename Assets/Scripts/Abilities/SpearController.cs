﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpearController : MonoBehaviour {

    private int springForce;
    private float stunTime;
    private float damage;
	private float speed;
	private int index, dragTars;
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
            float dist = Vector3.Distance(transform.position, points[index]);
            while (Vector3.Distance(transform.position, points[index]) > 0 && NotPassedPoint(transform.position, points[index]))
            {
                //transform.position += transform.forward * speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, points[index], speed * Time.deltaTime);
                yield return null;
            }
            ++index;
            yield return null;
        }
        for (int i = 0; i < gameIDIndex; i++)
        {
            if (i == dragTars)
                break;
            else
            {
                try
                {
                    GameManager.events.SpearDrawAbilityDragEnd(gameObject, gameID[i]);
                    Destroy(gameID[i].GetComponent<SpringJoint>());
                    gameID[gameIDIndex - 1].GetComponent<EnemyStats>().PauseFor(0.2f);
                    gameID[i].GetComponent<Rigidbody>().velocity = Vector3.zero;
                }
                catch { };
            }
        }
        GameManager.events.SpearDrawAbilityEnd(gameObject);
        if (GetComponent<PKFxFX>() != null)
        {
            GetComponent<PKFxFX>().StopEffect();
        }
        Destroy(gameObject);
        yield break;
    }

    private bool NotPassedPoint(Vector3 pos, Vector3 tar)
    {
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
            if(gameIDIndex<dragTars+1) // HOOKS 4-1 = 3 ENEMIES
            {
                gameID[gameIDIndex - 1].AddComponent<SpringJoint>();
                gameID[gameIDIndex - 1].GetComponent<SpringJoint>().spring = springForce;
                gameID[gameIDIndex - 1].GetComponent<SpringJoint>().connectedBody = GetComponent<Rigidbody>();
                GameManager.events.SpearDrawAbilityDragStart(gameObject, col.gameObject);
                gameID[gameIDIndex - 1].GetComponent<EnemyStats>().PauseFor(stunTime);
            }
        }
    }

	public void SetParameters(List<Vector3> ps, float speed, float damage, float force, int dragForce, float altitude, float turn, float st, int dragAmount){
		points = ps.ToArray ();
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = new Vector3(points[i].x, altitude, points[i].z);
        }
        transform.position = points[0];
        dragTars = dragAmount;
		index = 1;
		this.speed = speed;
        this.damage = damage;
        pushForce = force;
        springForce = dragForce*100;
        turnRate = turn;
        stunTime = st;
        if(GetComponent<PKFxFX>() != null)
        {
            GetComponent<PKFxFX>().StartEffect();
        }
        StartCoroutine(fly());
	}
    
}
