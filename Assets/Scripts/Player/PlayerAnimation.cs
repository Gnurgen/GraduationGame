using UnityEngine;
using System.Collections;
using System;

public class PlayerAnimation : MonoBehaviour {

    Animator anim;
	// Use this for initialization
	void Start () {
        GameManager.events.OnSpearDrawAbilityStart += SpearAbilityStart;
        GameManager.events.OnSpearDrawAbilityUsed += SpearAttack;
        GameManager.events.OnPlayerDashBegin += DashBeginAni;
        GameManager.events.OnPlayerDashEnd += DashEndAni;
        GameManager.events.OnPlayerIdle += IdleAni;
        GameManager.events.OnPlayerMove += RunAni;
        GameManager.events.OnConeAbilityStart += ConeAbilityStart;
        GameManager.events.OnConeAbilityUsed += ConeAttack;
        anim = GetComponent<Animator>();
	}

    private void SpearAbilityStart(GameObject Id)
    {
        throw new NotImplementedException();
    }

    private void ConeAbilityStart(GameObject Id)
    {
        anim.SetBool("PowerAttack", true);
    }

    private void ConeAttack(GameObject option)
    { 
        anim.SetBool("Dash", false);
        anim.SetBool("PowerAttack", false);
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
