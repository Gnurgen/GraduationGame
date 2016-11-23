using UnityEngine;
using System.Collections;

public class HomingMissleAI : MonoBehaviour {

    Transform Player;
    float speed= 4;
    float dmg = 1;
    private float missileForce;
    void Start () {
        Player = GameManager.player.transform;
    }
    
	void Update () {
        transform.LookAt(Player.position+Vector3.up);
        // vector.up added for better visualization of missile
        transform.position = Vector3.MoveTowards(transform.position, Player.position + Vector3.up , speed * Time.deltaTime); 
	}
    public void SetParameters(float speed, float dmg, float force)
    {
        missileForce = force;
        this.speed = speed;
        this.dmg = dmg;
    }
    void OnTriggerEnter(Collider hit)
    {
        if (hit.tag == "Player")
        {
            GameManager.events.EnemyAttackHit(gameObject, dmg);
            hit.transform.GetComponent<Health>().decreaseHealth(dmg, (GameManager.player.transform.position-transform.position), missileForce);
            Destroy(gameObject);
        }
    }
}
