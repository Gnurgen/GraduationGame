using UnityEngine;
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
            Invoke("PoolItself", 20f);
            GameManager.events.EnemyAttackHit(enemyID, -1);
        }
        if (col.tag == "Player")
        {
            targetHit = true;
            transform.SetParent(col.transform,true);
            GameManager.events.EnemyAttackHit(enemyID, dmg);
            Invoke("PoolItself", 20f);
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
        print("I Should pool myself, but I destroy myself");
        Destroy(gameObject);
    } 
}
