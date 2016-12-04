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
    public event EnemyAction OnEnemyRangedAttack;
    public event EnemyAction OnEnemyAggro;
    public event EnemyAction OnEnemyAggroLost;
    public event EnemyAction OnEnemyDeath;
    public event EnemyAction OnEnemyRagdollDespawn;
    public event EnemyAction OnEnemyRangedMiss;
    public void EnemyAttack(GameObject Id)
    {
        if (OnEnemyAttack != null)
            OnEnemyAttack(Id);
    }
    public void EnemyRangedAttack(GameObject Id)
    {
        if (OnEnemyRangedAttack != null)
            OnEnemyRangedAttack(Id);
    }
    public void EnemyAggro(GameObject Id)
    {
        if (OnEnemyAggro != null)
            OnEnemyAggro(Id);
    }
    public void EnemyAggroLost(GameObject Id)
    {
        if (OnEnemyAggroLost != null)
            OnEnemyAggroLost(Id);
    }
    public void EnemyDeath(GameObject Id)
    {
        if (OnEnemyDeath != null)
            OnEnemyDeath(Id);
    }
    public void EnemyRagdollDespawn(GameObject Id)
    {
        if (OnEnemyRagdollDespawn != null)
            OnEnemyRagdollDespawn(Id);
    }
    public void EnemyRangedMiss(GameObject Id)
    {
        if (OnEnemyRangedMiss != null)
            OnEnemyRangedMiss(Id);
    }
    public delegate void EnemyAttackHitAction(GameObject enemyID, float dmg);
    public event EnemyAttackHitAction OnEnemyAttackHit;
    public void EnemyAttackHit(GameObject Id, float dmg)
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
    public event PlayerAction OnConeAbilityStart;
    public event PlayerAction OnConeAbilityUsed;
    public event PlayerAction OnConeAbilityCharged;
    public event PlayerAction OnConeAbilityHit;
    public event PlayerAction OnConeAbilityEnd;
    public event PlayerAction OnConeAbilityCancel;
    public event PlayerAction OnSpearDrawAbilityStart;
    public event PlayerAction OnSpearDrawAbilityUsed;
    public event PlayerAction OnSpearDrawAbilityHit;
    public event PlayerAction OnSpearDrawAbilityEnd;
    public delegate void SpearHitAction(GameObject spear, GameObject enemyHit);
    public event SpearHitAction OnSpearDrawAbilityDragStart;
    public event SpearHitAction OnSpearDrawAbilityDragEnd;
    public delegate void PlayerAttackHitAction(GameObject Id, GameObject tar, float val);
    public event PlayerAttackHitAction OnPlayerAttackHit;


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
    public void PlayerAttackHit(GameObject Id, GameObject tar, float val)
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
    public void ConeAbilityStart(GameObject Id)
    {
        print("ConeAbility START");
        if (OnConeAbilityStart != null)
            OnConeAbilityStart(Id);
    }
    public void ConeAbilityUsed(GameObject Id)
    {
        print("ConeAbility USED");
        if (OnConeAbilityUsed != null)
            OnConeAbilityUsed(Id);
    }
    public void ConeAbilityCharged(GameObject Id)
    {
        print("ConeAbility CHARGED");
        if (OnConeAbilityCharged != null)
            OnConeAbilityCharged(Id);
    }
    public void ConeAbilityHit(GameObject Id)
    {
        print("ConeAbility HIT");
        if (OnConeAbilityHit != null)
            OnConeAbilityHit(Id);
    }
    public void ConeAbilityEnd(GameObject Id)
    {
        print("ConeAbility END");
        if (OnConeAbilityEnd != null)
            OnConeAbilityEnd(Id);
    }
    public void ConeAbilityCancel(GameObject Id)
    {
        print("ConeAbility CANCEL");
        if (OnConeAbilityCancel != null)
            OnConeAbilityCancel(Id);
    }
    public void SpearDrawAbilityStart(GameObject Id)
    {
        if (OnSpearDrawAbilityStart != null)
            OnSpearDrawAbilityStart(Id);
    }
    public void SpearDrawAbilityUsed(GameObject Id)
    {
        if (OnSpearDrawAbilityUsed != null)
            OnSpearDrawAbilityUsed(Id);
    }
    public void SpearDrawAbilityHit(GameObject Id)
    {
        if (OnSpearDrawAbilityHit != null)
            OnSpearDrawAbilityHit(Id);
    }
    public void SpearDrawAbilityDragStart(GameObject spear, GameObject enemyHit)
    {
        if (OnSpearDrawAbilityDragStart != null)
            OnSpearDrawAbilityDragStart(spear, enemyHit);
    }
    public void SpearDrawAbilityDragEnd(GameObject spear, GameObject enemyHit)
    {
        if (OnSpearDrawAbilityDragEnd != null)
            OnSpearDrawAbilityDragEnd(spear, enemyHit);
    }
    public void SpearDrawAbilityEnd(GameObject Id)
    {
        if (OnSpearDrawAbilityEnd != null)
            OnSpearDrawAbilityEnd(Id);
    }

    //##############################################################################################################################################################
    //################################################################################        ######################################################################
    //################################################################################  BOSS  ######################################################################
    //################################################################################        ######################################################################
    //##############################################################################################################################################################

    public delegate void BossAction(GameObject Id);
    public event BossAction OnBossActivated;
    public event BossAction OnBossPhaseChange;
    public event BossAction OnBossLaserActivation;
    public event BossAction OnBossLaserDeactivation;
    public event BossAction OnBossLaserHitPlayer;
    public event BossAction OnBossMeteorActivation;
    public event BossAction OnBossMeteorImpact;
    public event BossAction OnBossDeath;

    public void BossActivated(GameObject Id)
    {
        if (OnBossActivated != null)
            OnBossActivated(Id);
    }
    public void BossPhaseChange(GameObject Id)
    {
        if (OnBossPhaseChange != null)
            OnBossPhaseChange(Id);
    }
    public void BossLaserActivation(GameObject Id)
    {
        if (OnBossLaserActivation != null)
            OnBossLaserActivation(Id);
    }
    public void BossLaserDeactivation(GameObject Id)
    {
        if (OnBossLaserDeactivation != null)
            OnBossLaserDeactivation(Id);
    }
    public void BossLaserHitPlayer (GameObject Id)
    {
        if (OnBossLaserHitPlayer != null)
            OnBossLaserHitPlayer(Id);
    }

    public void BossMeteorActivation(GameObject Id)
    {
        if (OnBossMeteorActivation != null)
            OnBossMeteorActivation(Id);
    }
    public void BossMeteorImpact(GameObject Id)
    {
        if (OnBossMeteorImpact != null)
            OnBossMeteorImpact(Id);
    }
    public void BossDeath(GameObject Id)
    {
        if (OnBossDeath != null)
            OnBossDeath(Id);
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
    public event MapAction OnElevatorActivated;
    public event MapAction OnElevatorMoveStart;
    public event MapAction OnElevatorMoveStop;

    public delegate void ObjectDestroyAction(GameObject GO);
    public event ObjectDestroyAction OnObjDestroyed;

    public delegate void ResourceAction(GameObject GO, int amount);
    public event ResourceAction OnResourceDrop;
    public event ResourceAction OnResourcePickup;

    public delegate void WhispAction(GameObject GO);
    public event WhispAction OnGuideWhispScatter;
    public event WhispAction OnGuideWhispScatterStop;
    public event WhispAction OnGuideWhispFollowPath;
    public event WhispAction OnGuideWhispFollowPathStop;
    public event WhispAction OnWhispEnterElevator;

    public void ElevatorMoveStart()
    {
        if (OnElevatorMoveStart != null)
            OnElevatorMoveStart();
    }
    public void ElevatorMoveStop()
    {
        if (OnElevatorMoveStop != null)
            OnElevatorMoveStop();
    }
    public void GuideWhispScatter(GameObject GO)
    {
        if (OnGuideWhispScatter != null)
            OnGuideWhispScatter(GO);
    }
    public void GuideWhispScatterStop(GameObject GO)
    {
        if (OnGuideWhispScatterStop != null)
            OnGuideWhispScatterStop(GO);
    }
    public void GuideWhispFollowPath(GameObject GO)
    {
        if (OnGuideWhispFollowPath != null)
            OnGuideWhispFollowPath(GO);
    }
    public void GuideWhispFollowPathStop(GameObject GO)
    {
        if (OnGuideWhispFollowPathStop != null)
            OnGuideWhispFollowPathStop(GO);
    }
    public void WhispEnterElevator(GameObject GO)
    {
        if (OnWhispEnterElevator != null)
            OnWhispEnterElevator(GO);
    }

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
    public void ElevatorActivated()
    {
        if (OnElevatorActivated != null)
            OnElevatorActivated();
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
    public void ObjDestroyed(GameObject go)
    {
        if (OnResourcePickup != null)
            OnObjDestroyed(go);
    }

    //##############################################################################################################################################################
    //#############################################################################             ####################################################################
    //#############################################################################    GAME     ####################################################################
    //#############################################################################             ####################################################################
    //##############################################################################################################################################################


    public delegate void LoadingAction(float progress);
    public event LoadingAction OnLoadingProgress;

    public delegate void LoadingCompleteAction();
    public event LoadingCompleteAction OnLoadComplete;
    public event LoadingCompleteAction OnLoadNextLevel;


    public void LoadNextlevel()
    {
        if (OnLoadNextLevel!= null)
        {
            print("LOADING NEXT LEVEL");
            OnLoadNextLevel();

        }
    }

    public void LoadComplete()
    {
        if (OnLoadComplete != null)
        {
            print("LOAD COMPLETE");
            OnLoadComplete();
        }
    }

    public void LoadingProgress(float progress)
    {
        if (OnLoadingProgress != null)
            OnLoadingProgress(progress);
    }


    public delegate void FadeAction();
    public event FadeAction OnFadeToBlack;
    public event FadeAction OnFadeToWhite;
    public event FadeAction OnFadeFromBlackToTransparent;
    public event FadeAction OnFadeFromWhiteToTransparent;
    public event FadeAction OnFadedBlackScreen;
    public event FadeAction OnFadedWhiteScreen;
    public event FadeAction OnFadedTransparentScreen;

    public void FadeToBlack()
    {
        if (OnFadeToBlack != null)
        {
            OnFadeToBlack();
        }
    }
    public void FadeToWhite()
    {
        if (OnFadeToWhite != null)
        {
            OnFadeToWhite();
        }
    }
    public void FadeFromBlackToTransparent()
    {
        if (OnFadeFromBlackToTransparent != null)
        {
            OnFadeFromBlackToTransparent();
        }
    }
    public void FadeFromWhiteToTransparent()
    {
        if (OnFadeFromWhiteToTransparent != null)
        {
            OnFadeFromWhiteToTransparent();
        }
    }
    public void FadedBlackScreen()
    {
        if (OnFadedBlackScreen != null)
        {
            OnFadedBlackScreen();
        }
    }
    public void FadedWhiteScreen()
    {
        if (OnFadedWhiteScreen != null)
        {
            OnFadedWhiteScreen();
        }
    }
    public void FadedTransparentScreen()
    {
        if (OnFadedTransparentScreen != null)
        {
            OnFadedTransparentScreen();
        }
    }

    public delegate void CheckPointAction();
    public event CheckPointAction OnCheckPoint;

    public void CheckPoint()
    {
        if (OnCheckPoint != null)
            OnCheckPoint();
    }
    public delegate void PoolAction(GameObject go);
    public event PoolAction OnPoolObject;
    public void PoolObject(GameObject go)
    {
        if (OnPoolObject != null)
            OnPoolObject(go);
    }

    public delegate void MapGeneration();
    public event MapGeneration OnMapGenerated;
    public void MapGenerated()
    {
        if (OnMapGenerated != null)
            OnMapGenerated();
    }

    public delegate void ReSpawn();
    public event ReSpawn OnRespawn;
    public void Respawned()
    {
        if (OnRespawn != null)
            OnRespawn();
    }
}

