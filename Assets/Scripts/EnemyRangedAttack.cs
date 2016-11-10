﻿using UnityEngine;
using System.Collections;

public class EnemyRangedAttack : MonoBehaviour {
    private Rigidbody rig;
    private Vector3 direction;
    private float step;
    private GameObject enemyID;
    private int dmg;
    private float speed;
    private bool targetHit = false;
    // Use this for initialization
    void Start() {
        rig = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void OnEnable()
    {
        GetComponent<BoxCollider>().enabled = true;
        targetHit = false;
        step = 0;
    }
    
    void FixedUpdate()
    {
        if (!targetHit)
        {
            step += Time.fixedDeltaTime;
            rig.MovePosition(transform.position + transform.forward * speed * step);
        }
        else
            rig.velocity = Vector3.zero;
    }
    
    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Indestructable")
        {
            targetHit = true;
            Invoke("PoolItSelf", 20f);
            GameManager.events.EnemyRangedMiss(gameObject);
            GetComponent<BoxCollider>().enabled = false;
        }
        if (col.tag == "Player")
        {
            targetHit = true;
            transform.SetParent(col.transform,true);
            GameManager.events.EnemyAttackHit(enemyID, dmg);
            GetComponent<BoxCollider>().enabled = false;
            Invoke("PoolItSelf", 20f);
        }
    }
    public void setSpecs(float speed, GameObject enemyID, int damage)
    {
        this.enemyID = enemyID;
        this.speed = speed;
        dmg = damage;

    }
    void PoolItSelf()
    {
        GameManager.events.PoolObject(gameObject);
        Destroy(gameObject);
    } 
}
