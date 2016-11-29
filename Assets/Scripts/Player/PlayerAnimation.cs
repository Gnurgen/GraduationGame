using UnityEngine;
using System.Collections;
using System;

public class PlayerAnimation : MonoBehaviour {

    Animator anim;
	// Use this for initialization
	void Start () {
        GameManager.events.OnPlayerDashBegin += DashBeginAni;
        GameManager.events.OnPlayerDashEnd += DashEndAni;
        GameManager.events.OnPlayerIdle += IdleAni;
        GameManager.events.OnPlayerMove += RunAni;
        GameManager.events.OnConeAbilityStart += ConeAbilityStart;
        GameManager.events.OnConeAbilityUsed += ConeAttack;
        GameManager.events.OnSpearDrawAbilityStart += ConeAbilityStart;
        GameManager.events.OnSpearDrawAbilityUsed += ConeAttack;
        GameManager.events.OnConeAbilityCancel += ConeCancel;
        anim = GetComponent<Animator>();
	}

 

    private void ConeAbilityStart(GameObject Id)
    {
        anim.SetBool("PowerAttack", true);
        anim.SetBool("Cancel", false);
    }

    private void ConeAttack(GameObject option)
    { 
        anim.SetBool("Dash", false);
        anim.SetBool("PowerAttack", false);
        anim.SetBool("Cancel", false);
    }

    private void ConeCancel(GameObject Id)
    {
        anim.SetBool("PowerAttack", false);
        anim.SetBool("Cancel", true);
    }


    private void RunAni(GameObject Id)
    {
        anim.SetBool("Run", true);   
    }

    private void IdleAni(GameObject Id)
    {
        anim.SetBool("Attack", false);
        anim.SetBool("Run", false);
    }

    private void DashEndAni(GameObject Id)
    {
        anim.SetBool("Dash", false);
    }

    private void DashBeginAni(GameObject Id)
    {
        anim.SetBool("Dash", true);
        anim.SetBool("Run", false);
    }

    private void SpearAttack(GameObject Id)
    {
        anim.SetBool("Dash", false);
        anim.SetTrigger("Attack");
    }

    // Update is called once per frame
    void Update () {
	
	}

}
