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
    private float pushForce;
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
        AkSoundEngine.PostEvent("Enemy_Ranged_Projectile_Play", gameObject);
        AkSoundEngine.RenderAudio();
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
            col.GetComponent<Health>().decreaseHealth(dmg, (GameManager.player.transform.position - transform.position), pushForce);
            GetComponent<BoxCollider>().enabled = false;
            Invoke("PoolItSelf", 1f);
        }
        
    }
    public void SetParameters(float speed, GameObject enemyID, float damage, float force)
    {
        transform.position = enemyID.transform.position;
        this.enemyID = enemyID;
        this.speed = speed;
        pushForce = force;
        dmg = damage;

    }
    void PoolItSelf()
    {
        AkSoundEngine.PostEvent("Enemy_Ranged_Projectile_Stop", gameObject);
        AkSoundEngine.RenderAudio();
        transform.parent = null;
        GameManager.pool.PoolObj(gameObject);
    } 
}