using UnityEngine;
using System.Collections;

public class PlayerSpearAttack : MonoBehaviour {

    float dmg;
    int gameIDIndex = 0;
    GameObject[] gameID;
    private float spearForce;
    // Use this for initialization
    void Start () {
        gameID = new GameObject[5];
        dmg = GameManager.player.GetComponent<PlayerControls>().Damage;
        spearForce = GameManager.player.GetComponent<PlayerControls>().meeleeForce;
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
<<<<<<< HEAD:Assets/Scripts/Player/PlayerSpearAttack.cs
            col.GetComponent<EnemyStats>().PauseFor(2 * Time.fixedDeltaTime);
            col.GetComponent<Rigidbody>().AddForce(transform.forward * 1000, ForceMode.Force);
=======
            col.GetComponent<Health>().decreaseHealth(dmg, (col.transform.position - transform.position).normalized*spearForce);
>>>>>>> origin/master:Assets/PlayerSpearAttack.cs
            GameManager.events.PlayerAttackHit(GameManager.player, col.gameObject, dmg);
            col.GetComponent<Health>().decreaseHealth(dmg);
        }
    }
}
