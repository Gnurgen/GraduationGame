using UnityEngine;
using System.Collections;

public class EnemyRangedAttack : MonoBehaviour {
    private Rigidbody rig;
    private Vector3 direction;
    private float step;
    private GameObject enemyID;
    private float dmg;
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
            Invoke("PoolItSelf", 1f);
            GameManager.events.EnemyRangedMiss(gameObject);
            GetComponent<BoxCollider>().enabled = false;
        }
        else if (col.tag == "Player")
        {
            targetHit = true;
            transform.SetParent(col.transform,true);
            GameManager.events.EnemyAttackHit(enemyID, dmg);
            col.GetComponent<Health>().decreaseHealth(dmg);
            GetComponent<BoxCollider>().enabled = false;
            Invoke("PoolItSelf", 1f);
        }
        
    }
    public void SetParameters(float speed, GameObject enemyID, float damage)
    {
        transform.position = enemyID.transform.position;
        this.enemyID = enemyID;
        this.speed = speed;
        dmg = damage;

    }
    void PoolItSelf()
    {
        transform.parent = null;
        GameManager.pool.PoolObj(gameObject);
    } 
}