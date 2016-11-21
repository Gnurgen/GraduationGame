using UnityEngine;
using System.Collections;

public class PlayerSpearAttack : MonoBehaviour {

    float dmg;
    int gameIDIndex = 0;
    GameObject[] gameID;
    // Use this for initialization
    void Start () {
        gameID = new GameObject[5];
        dmg = GameManager.player.GetComponent<PlayerControls>().Damage;
        gameObject.SetActive(false);
	}
    void OnEnable()
    {
        gameIDIndex = 0;
        gameID = null;
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Enemy") // Can bug if the trigger collider enters the same enemy twice
        {
            col.GetComponent<EnemyStats>().PauseFor(0.2f);
            col.GetComponent<Rigidbody>().AddForce(transform.forward * 10, ForceMode.Force);
            GameManager.events.PlayerAttackHit(GameManager.player, col.gameObject, dmg);
            col.GetComponent<Health>().decreaseHealth(dmg);
        }
    }
}
