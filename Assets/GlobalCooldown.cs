using UnityEngine;
using System.Collections;
using System;

public class GlobalCooldown : MonoBehaviour {

    public float GCooldown = 1;
    private float cooldown = 0;
    FlyingSpear FS;
    ConeDraw CD;
	// Use this for initialization
	void Start () {
        GameManager.events.OnConeAbilityUsed += Cooldown;
        GameManager.events.OnSpearDrawAbilityUsed += Cooldown;
        FS = GetComponent<FlyingSpear>();
        CD = GetComponent<ConeDraw>();
    }

    private void Cooldown(GameObject Id)
    {
        FS.enabled = false;
        CD.enabled = false;
        cooldown = GCooldown;
    }

    // Update is called once per frame
    void Update () {
        if(cooldown <= 0)
        {
            FS.enabled = true;
            CD.enabled = true;
        }
        else
        {
            cooldown -= 1 * Time.deltaTime;
        }
	}
}
