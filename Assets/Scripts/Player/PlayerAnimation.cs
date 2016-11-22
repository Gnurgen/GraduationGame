using UnityEngine;
using System.Collections;
using System;

public class PlayerAnimation : MonoBehaviour {

    Animator anim;
	// Use this for initialization
	void Start () {
        GameManager.events.OnPlayerAttack += AttackAni;
        GameManager.events.OnPlayerDashBegin += DashBeginAni;
        GameManager.events.OnPlayerDashEnd += DashEndAni;
        GameManager.events.OnPlayerIdle += IdleAni;
        GameManager.events.OnPlayerMove += RunAni;
        GameManager.events.OnWheelSelect += PowerAttackAni;
        anim = GetComponent<Animator>();
	}

    private void PowerAttackAni(int option)
    {
        //print(option);
        if(option == 1) // FLYING SPEAR
        {
            anim.SetTrigger("PowerAbility");
            anim.SetBool("Run", false);
        }
    }

    private void RunAni(GameObject Id)
    {
        anim.SetBool("Run", true);   
    }

    private void IdleAni(GameObject Id)
    {
        anim.SetBool("Run", false);
    }

    private void DashEndAni(GameObject Id)
    {
        anim.SetBool("Run", false);
    }

    private void DashBeginAni(GameObject Id)
    {
        anim.SetTrigger("Dash");
        anim.SetBool("Run", false);

    }

    private void AttackAni(GameObject Id)
    {
        anim.SetTrigger("Attack");
        anim.SetBool("Run", false);
    }

    // Update is called once per frame
    void Update () {
	
	}

}
