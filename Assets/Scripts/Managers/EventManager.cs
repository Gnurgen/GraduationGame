using UnityEngine;
using System.Collections;

public class EventManager : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //################################################################    #### #### ####    ####  ###  #### ### ####################################################
    //################################################################ #######  ### #### ####### # # # ##### # #####################################################
    //################################################################    #### # ## ####    #### ## ## ###### ######################################################
    //################################################################ ####### ## # #### ####### ##### ###### ######################################################
    //################################################################    #### ###  ####    #### ##### ###### ######################################################

    public delegate void EnemyAction(GameObject enemyID);
    public event EnemyAction OnEnemyAttack;
    public event EnemyAction OnEnemyAggro;
    public event EnemyAction OnEnemyDeath;
    public void EnemyAttack(GameObject Id)
    {
        if (OnEnemyAttack != null)
            OnEnemyAttack(Id);
    }
    public void EnemyAggro(GameObject Id)
    {
        if (OnEnemyAggro != null)
            OnEnemyAggro(Id);
    }
    public void EnemyDeath(GameObject Id)
    {
        if (OnEnemyDeath != null)
            OnEnemyDeath(Id);
    }

<<<<<<< HEAD
    public delegate void EnemyHitAction(GameObject enemyID, int dmg);
    public event EnemyHitAction OnEnemyAttackHit;
    public void EnemyHit(GameObject Id, int dmg)
=======
    public delegate void EnemyAttackHitAction(GameObject enemyID, int dmg);
    public event EnemyAttackHitAction OnEnemyAttackHit;
    public void EnemyAttackHit(GameObject Id, int dmg)
>>>>>>> master
    {
        if (OnEnemyAttackHit != null)
            OnEnemyAttackHit(Id, dmg);
    }

    //##############################################################################################################################################################
    //################################################################################        ######################################################################
    //################################################################################ PLAYER ######################################################################
    //################################################################################        ######################################################################
    //##############################################################################################################################################################

    public delegate void PlayerAction(GameObject Id);
    public event PlayerAction OnPlayerAttack;
    public event PlayerAction OnPlayerDashBegin;
    public event PlayerAction OnPlayerDashEnd;
    public event PlayerAction OnPlayerDeath;
    public event PlayerAction OnPlayerMove;
    public event PlayerAction OnPlayerIdle;
<<<<<<< HEAD
    public delegate void PlayerHitAction(GameObject Id, GameObject tar, int val);
    public event PlayerHitAction OnPlayerAttackHit;
=======
    public delegate void PlayerAttackHitAction(GameObject Id, GameObject tar, int val);
    public event PlayerAttackHitAction OnPlayerAttackHit;
>>>>>>> master


    public void PlayerAttack(GameObject Id)
    {
        if (OnPlayerAttack != null)
            OnPlayerAttack(Id);
    }
    public void PlayerDashBegin(GameObject Id)
    {
        if (OnPlayerDashBegin != null)
            OnPlayerDashBegin(Id);
    }
    public void PlayerDashEnd(GameObject Id)
    {
        if (OnPlayerDashEnd != null)
            OnPlayerDashEnd(Id);
    }
    public void PlayerAttackHit(GameObject Id, GameObject tar, int val)
    {
        if (OnPlayerAttackHit != null)
            OnPlayerAttackHit(Id, tar, val);
    }
    public void PlayerDeath(GameObject Id)
    {
        if (OnPlayerDeath != null)
            OnPlayerDeath(Id);
    }
    public void PlayerMove(GameObject Id)
    {
        if (OnPlayerMove != null)
            OnPlayerMove(Id);
    }
    public void PlayerIdle(GameObject Id)
    {
        if (OnPlayerIdle != null)
            OnPlayerIdle(Id);
    }


    //##############################################################################################################################################################
    //#################################################################################     ########################################################################
    //################################################################################# GUI ########################################################################
    //#################################################################################     ########################################################################
    //##############################################################################################################################################################


    public delegate void GUIActionChoice(int option);
    public event GUIActionChoice OnWheelSelect;
    public event GUIActionChoice OnWheelHover;
    public event GUIActionChoice OnDrawComplete;
    public event GUIActionChoice OnLevelUp;

    public delegate void GUIAction();
    public event GUIAction OnWheelOpen;
    public event GUIAction OnMenuOpen;
    public event GUIAction OnMenuClose;
    public event GUIAction OnSave;

    public void WheelSelect(int i)
    {
        if (OnWheelSelect != null)
            OnWheelSelect(i);
    }
    public void WheelHover(int i)
    {
        if (OnWheelHover != null)
            OnWheelHover(i);
    }
    public void DrawComplete(int i)
    {
        if (OnDrawComplete!= null)
            OnDrawComplete(i);
    }
    public void LevelUp(int i)
    {
        if (OnLevelUp!= null)
            OnLevelUp(i);
    }
    public void WheelOpen()
    {
        if (OnWheelOpen != null)
            OnWheelOpen();
    }
    public void MenuOpen()
    {
        if (OnMenuOpen != null)
            OnMenuOpen();
    }
    public void MenuClose()
    {
        if (OnMenuClose != null)
            OnMenuClose();
    }
    public void Save()
    {
        if (OnSave != null)
            OnSave();
    }

    //##############################################################################################################################################################
    //#############################################################################             ####################################################################
    //############################################################################# ENVIRONMENT ####################################################################
    //#############################################################################             ####################################################################
    //##############################################################################################################################################################

    public delegate void RoomAction(int roomID);
    public event RoomAction OnRoomComplete;
    public event RoomAction OnRoomEnter;
    public event RoomAction OnRoomExit;

    public delegate void MapAction();
    public event MapAction OnMapComplete;

    public delegate void ObjectDestroyAction(GameObject GO);
    public event ObjectDestroyAction OnObjDestroyed;

    public delegate void ResourceAction(GameObject GO, int amount);
    public event ResourceAction OnResourceDrop;
    public event ResourceAction OnResourcePickup;

    public void RoomComplete(int i)
    {
        if (OnRoomComplete != null)
            OnRoomComplete(i);
    }
    public void RoomEnter(int i)
    {
        if (OnRoomEnter != null)
            OnRoomEnter(i);
    }
    public void RoomExit(int i)
    {
        if (OnRoomExit != null)
            OnRoomExit(i);
    }

    public void MapComplete()
    {
        if (OnMapComplete!= null)
            OnMapComplete();
    }
    public void ResourceDrop(GameObject go, int i)
    {
        if (OnResourceDrop!= null)
            OnResourceDrop(go,i);
    }
    public void ResourcePickup(GameObject go, int i)
    {
        if (OnResourcePickup != null)
            OnResourcePickup(go, i);
    }

    //##############################################################################################################################################################
    //#############################################################################             ####################################################################
    //#############################################################################    GAME     ####################################################################
    //#############################################################################             ####################################################################
    //##############################################################################################################################################################

    public delegate void CheckPointAction();
    public event CheckPointAction OnCheckPoint;

    public void CheckPoint()
    {
        if (OnCheckPoint != null)
            OnCheckPoint();
    }

}

