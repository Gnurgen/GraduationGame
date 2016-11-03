using UnityEngine;
using System.Collections;

public class EventManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //################################################################    #### #### ####    ####  ###  #### ### ####################################################
    //################################################################ #######  ### #### ####### # # # ##### # #####################################################
    //################################################################    #### # ## ####    #### ## ## ###### ######################################################
    //################################################################ ####### ## # #### ####### ##### ###### ######################################################
    //################################################################    #### ###  ####    #### ##### ###### ######################################################

    public delegate void EnemyAttackAction(int Type, GameObject enemyID);  
    public event EnemyAttackAction OnEnemy1Attack;
    public void Enemy1Attack(int i,  GameObject Id)
    {
        if (OnEnemy1Attack != null)
            OnEnemy1Attack(i,Id);
    }

    //##############################################################################################################################################################
    //##############################################################################################################################################################
    //#################################################################################player#######################################################################
    //##############################################################################################################################################################
    //##############################################################################################################################################################

    public delegate void PlayerHealth(int value);
    public event PlayerHealth OnPlayerTakeDamage;
    public event PlayerHealth OnPlayerRecover;
    public void PlayerTakeDamage(int i)
    {
        if (OnPlayerTakeDamage != null)
            OnPlayerTakeDamage(i);
    }
    public void PlayerRecover(int i)
    {
        if (OnPlayerRecover != null)
            OnPlayerRecover(i);
    }
    public delegate void PlayerExperience(int value);
    public event PlayerExperience OnPlayerExperienceGet;
    public event PlayerExperience OnPlayerLevelUp;
    public void PlayerExperienceGet(int i)
    {
        if (OnPlayerExperienceGet != null)
            OnPlayerExperienceGet(i);
    }
    public void PlayerLevelUp(int i)
    {
        if (OnPlayerLevelUp != null)
            OnPlayerLevelUp(i);
    }
    public delegate void PlayerMovement(Vector2 position);
    public event PlayerMovement OnPlayerMove;
    public event PlayerMovement OnPlayerDash;
    public event PlayerMovement OnPlayerAttack;
    public void PlayerMove(Vector2 pos)
    {
        if (OnPlayerMove != null)
            OnPlayerMove(pos);
    }
    public void PlayerDash(Vector2 pos)
    {
        if (OnPlayerDash != null)
            OnPlayerDash(pos);
    }
    public void PlayerAttack(Vector2 pos)
    {
        if (OnPlayerAttack != null)
            OnPlayerAttack(pos);
    }
    public delegate void PlayerMenu();
    public event PlayerMenu OnPlayerMenuOpen;
    public event PlayerMenu OnPlayerMenuClose;
    public void PlayerMenuOpen()
    {
        if (OnPlayerMenuOpen != null)
            OnPlayerMenuOpen();
    }
    public void PlayerMenuClose()
    {
        if (OnPlayerMenuClose != null)
            OnPlayerMenuClose();
    }

}
