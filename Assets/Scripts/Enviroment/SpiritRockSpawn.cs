using UnityEngine;
using System.Collections;

public class SpiritRockSpawn : MonoBehaviour {

    SpiritRock rock;

	void Start () {
        rock = GetComponentInParent<SpiritRock>();
	}
	
	void Update () {
	
	}


    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject == GameManager.player && rock.isEnabled && GameManager.game.waypoint != rock)
            rock.EnableFlame();
    }
}
