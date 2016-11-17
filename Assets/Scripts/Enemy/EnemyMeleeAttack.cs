using UnityEngine;
using System.Collections;

public class EnemyMeleeAttack : MonoBehaviour {

    private Transform body;
    private float dmg;
    private Collider myCol;
    void Start()
    {
        myCol = GetComponent<Collider>();
        myCol.isTrigger = true;
        myCol.enabled = false;
        body = transform.parent;
        setMyDmg();
    }

    void setMyDmg()
    {
        if (body.tag == "Melee")
        {
            dmg = body.GetComponent<EnemyStats>().damage;
        }
        else
        {
            body = body.parent;
            setMyDmg();
        }
    }

    public void Swing(bool swing)
    {
        myCol.enabled = swing;
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player" || col.tag == "Destructable")
        {
            col.GetComponent<Health>().decreaseHealth(dmg);
            myCol.enabled = false;
            GameManager.events.EnemyAttackHit(body.gameObject, dmg);
        }
    }
}
