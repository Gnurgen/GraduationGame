using UnityEngine;
using System.Collections;
using System;

public class PlayerAnimation : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameManager.events.OnPlayerAttack += AttackAni;
        GameManager.events.OnPlayerDashBegin += DashBeginAni;
        GameManager.events.OnPlayerDashEnd += DashEndAni;
        GameManager.events.OnPlayerIdle += IdleAni;
        GameManager.events.OnPlayerMove += RunAni;
	}

    private void RunAni(GameObject Id)
    {
        print("Ani: RUN");
    }

    private void IdleAni(GameObject Id)
    {
        print("Ani: IDLE");
    }

    private void DashEndAni(GameObject Id)
    {
        print("Ani: DASH END");
    }

    private void DashBeginAni(GameObject Id)
    {
        print("Ani: DASH BEGIN");

    }

    private void AttackAni(GameObject Id)
    {
        print("Ani: ATTACK");

    }

    // Update is called once per frame
    void Update () {
	
	}

}
