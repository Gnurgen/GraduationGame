﻿using UnityEngine;
using System.Collections;
using System;

public class AudioManager : MonoBehaviour {

    [Header("Current State: ")]
    public string Game_State = "Out_Of_Battle";
    public string Battle_State = "Out_Of_Battle";
    public int AggroedEnemies = 0;
    
    // ######################################################################################################################################
    // ##########################################################                            ################################################
    // ##########################################################  Subscribe to EventManager ################################################
    // ##########################################################                            ################################################
    // ######################################################################################################################################

    void Subscribe()
    {
        
        GameManager.events.OnEnemyAggro += EnemyChatterPlay;
        GameManager.events.OnEnemyAggro += CheckState;

        GameManager.events.OnEnemyAggroLost += EnemyChatterStop;
        GameManager.events.OnEnemyAggroLost += CheckState;

        GameManager.events.OnEnemyDeath += EnemyDeathPlay;
        GameManager.events.OnEnemyDeath += CheckState;

        GameManager.events.OnEnemyAttackHit += EnemyAttackHitPlaySub;
        GameManager.events.OnEnemyRangedMiss += EnemyRangedAttackMissPlaySub;

        GameManager.events.OnPlayerAttack += PlayerSpearAttackPlay;
        GameManager.events.OnPlayerDashBegin += DashPlay;
        GameManager.events.OnPlayerAttackHit += PlayerAttackHitPlaySub;
        GameManager.events.OnPlayerDeath += PlayerDeathPlay;
        GameManager.events.OnPlayerDeath += EnemyChatterStop;
        //  GameManager.events.OnPlayerMove += PlayerMovePlay; // IS MISSING //MAYBE NOT
        //  GameManager.events.OnPlayerIdle += PlayerMoveStop; // IS MISSING
        GameManager.events.OnConeAbilityStart += ConeAbilityInteractPlay; 
        GameManager.events.OnConeAbilityUsed += ConeAbilityPlay;
        GameManager.events.OnConeAbilityHit += ConeAbilityHitPlay;
        GameManager.events.OnConeAbilityEnd += ConeAbilityStop;
        // GameManager.events.OnConeAbilityCancel += ConeAbilityInteractStop;

        GameManager.events.OnSpearDrawAbilityStart += SpearAbilityStart;
        GameManager.events.OnSpearDrawAbilityUsed += SpearAbilityUsed;
        GameManager.events.OnSpearDrawAbilityHit += SpearAbilityHit;
        GameManager.events.OnSpearDrawAbilityEnd += SpearAbilityEnd;
        //GameManager.events.OnSpearDrawAbilityCancel += SpearAbilityCancel;

        GameManager.events.OnMenuOpen += MenuOpenPlaySub; // IS MISSING (de kommer)+ I HAVE TO CHANGE STATE HERE
        GameManager.events.OnMenuClose += MenuClosePlaySub; // IS MISSING (de kommer) + I HAVE TO CHANGE STATE HERE
        GameManager.events.OnResourcePickup += PickupPlaySub;

        print("AudioManager Subscribed");
        
    }

    private void ConeAbilityStop(GameObject Id)
    {
        //throw new NotImplementedException();
    }

    private void ConeAbilityHitPlay(GameObject Id)
    {
        //throw new NotImplementedException();
    }

    private void CheckState(GameObject go)
    {
        if (AggroedEnemies > 0)
            Battle_State = "In_Battle";
        else
            Battle_State = "Out_Of_Battle";
        if(Game_State == "In_Battle" || Game_State == "Out_Of_Battle")
        {
            if(Battle_State != Game_State)
            {
               Game_State = Battle_State;
                AkSoundEngine.SetState("Game_State", Game_State);
            }
        }
    }
    private void EnemyRangedAttackMissPlaySub(GameObject enemyID)
    {
        EnemyRangedTargetPlay(enemyID, "Indestructable");
    }

   

    private void PlayerAttackHitPlaySub(GameObject GO, GameObject Tar, float i)
    {
        PlayerSpearAttackTargetPlay(GO, Tar.tag);
    }

    private void PickupPlaySub(GameObject GO, int amount)
    {
        PickupMoveStop(GO);
        PickupPlay(GO);
    }

    private void MenuClosePlaySub()
    {
        Game_State = Battle_State;
        AkSoundEngine.SetState("Game_State", Game_State);
    }

    private void MenuOpenPlaySub()
    {
        Game_State = "In_Ability_Wheel";
        AkSoundEngine.SetState("Game_State",Game_State);
    }
    
    public void EnemyAttackHitPlaySub(GameObject enemyID, float dmg)
    {
        if (enemyID.tag == "Melee")
            EnemyMeleeHitPlayerPlay(GameManager.player);
        else if (enemyID.tag == "Ranged")
            EnemyRangedTargetPlay(GameManager.player, "Player");

    }

    // ######################################################################################################################################
    // ##########################################################              ##############################################################
    // ##########################################################  Boss sounds ##############################################################
    // ##########################################################              ##############################################################
    // ######################################################################################################################################

    public void BossBeamPlay(GameObject GO)
    {
        //When the Boss fires the beam, the sound keeps playing until he stops (continuous sound)
        
        AkSoundEngine.PostEvent("Boss_Beam_Play", GO);
        AkSoundEngine.RenderAudio();
    }
    public void BossBeamStop(GameObject GO)
    {
        //When the Boss stops firing the beam
        AkSoundEngine.PostEvent("Boss_Beam_Stop", GO);
        AkSoundEngine.RenderAudio();
    }
    public void BossBeamTargetPlay(GameObject GO, string tag)
    {
        //Beams noise where it hits (continuous sound)
        if(tag == "Player")
        {
            AkSoundEngine.SetSwitch("Target", tag, GO);
            AkSoundEngine.PostEvent("Boss_Beam_Target_Play", GO);
            AkSoundEngine.RenderAudio();
        }
        else if (tag== "Indestructable" || tag == "Walkable" || tag == "Occluder")
        {
            AkSoundEngine.SetSwitch("Target", "Indestructable", GO);
            AkSoundEngine.PostEvent("Boss_Beam_Target_Play", GO);
            AkSoundEngine.RenderAudio();
        }
        else
        {
            Debug.LogError(tag + "is wrong tag and event doesn't play audio");
        }
    }
    public void BossBeamTargetStop(GameObject GO)
    {
        //Beams noise stop
        AkSoundEngine.PostEvent("Boss_Beam_Target_Stop", GO);
        AkSoundEngine.RenderAudio();
    }
   
    public void BossRainPlay(GameObject GO)
    {
        //Boss's rain plays a sound when spawning
        AkSoundEngine.PostEvent("Boss_Rain_Play", GO);
        AkSoundEngine.RenderAudio();
    }


    // ######################################################################################################################################
    // ##########################################################                       #####################################################
    // ##########################################################  Menu Buttons Sounds  #####################################################
    // ##########################################################                       #####################################################
    // ######################################################################################################################################
    public void MenuButtonPlay(GameObject GO)
    {
        //Plays the menu button sound
        AkSoundEngine.PostEvent("Button_Menu_Play", GO);
        AkSoundEngine.RenderAudio();
    }
    public void MenuStartGamePlay(GameObject GO)
    {
        //Plays the Start Game Menu button sound
        AkSoundEngine.PostEvent("Button_Start_Menu_Play", GO);
        AkSoundEngine.RenderAudio();
    }
    // ######################################################################################################################################
    // ##########################################################                ############################################################
    // ########################################################## Ability Sounds ############################################################
    // ##########################################################                ############################################################
    // ######################################################################################################################################

        // SPEAR

    public void SpearAbilityStart(GameObject GO)
    {
        //When drawing the Line ability (continuous sound)
        AkSoundEngine.PostEvent("Draw_Ability_Control_Play", GO);
        AkSoundEngine.RenderAudio();
    }
    public void SpearAbilityEnd(GameObject GO)
    {
        //When Line ability is finished
        AkSoundEngine.PostEvent("Draw_Ability_Stop", GO);
        AkSoundEngine.RenderAudio();
    }
    public void SpearAbilityHit(GameObject GO)
    {
        if (GO.tag == "Boss" || GO.tag == "Enemy" || GO.tag == "Indestructable")
        {
            AkSoundEngine.SetSwitch("Target", GO.tag, GO);
            AkSoundEngine.PostEvent("Spear_Target_Play", GO);
            AkSoundEngine.RenderAudio();
        }
        else
        {
            Debug.LogError(GO.tag + "is wrong tag and event doesn't play audio");
        }
    }
    public void SpearAbilityUsed(GameObject GO)
    {
        //When finnished drawing the Line ability 
        AkSoundEngine.PostEvent("Draw_Ability_Control_Stop", GO);
        //When Line ability is activated and playing(Continuous sound)
        AkSoundEngine.PostEvent("Draw_Ability_Play", GO);
        AkSoundEngine.RenderAudio();
    }
    public void SpearAbilityCancel(GameObject GO)
    {
        //When finnished drawing the Line ability 
        AkSoundEngine.PostEvent("Draw_Ability_Control_Stop", GO);
        AkSoundEngine.RenderAudio();
        
    }

    // CONE

    public void ConeAbilityPlay(GameObject GO)
    {
        //When finnished drawing the cone Ability and activates 
        AkSoundEngine.PostEvent("Cone_Ability_Play", GO);
        AkSoundEngine.RenderAudio();
    }
    public void ConeAbilityInteractPlay(GameObject GO)
    {
        //When drawing the Cone Ability (Continuous sound)
        AkSoundEngine.PostEvent("Cone_Ability_Control_Play", GO);
        AkSoundEngine.RenderAudio();
    }
    public void ConeAbilityInteractStop(GameObject GO)
    {
        //When stops drawing the Cone Ability 
        AkSoundEngine.PostEvent("Cone_Ability_Control_Stop", GO);
        AkSoundEngine.RenderAudio();
    }
    // ######################################################################################################################################
    // ##########################################################                ############################################################
    // ##########################################################  Enemy Sounds  ############################################################
    // ##########################################################                ############################################################
    // ######################################################################################################################################

    public void EnemyChatterPlay(GameObject GO)
    {
        //Enemies random growls (continuous sound)
        AggroedEnemies++;
        AkSoundEngine.PostEvent("Enemy_Aggro_Play", GO);
        AkSoundEngine.RenderAudio();
    }

    public void EnemyChatterStop(GameObject GO)
    {
        //Enemies random growls stops 
        AggroedEnemies--;
        AkSoundEngine.PostEvent("Enemy_Aggro_Stop", GO);
        AkSoundEngine.RenderAudio();
    }
    public void EnemyMeleeHitPlayerPlay(GameObject GO)
    {
        //When Enemies hit the player
        AkSoundEngine.PostEvent("Enemy_Melee_Hit_Player_Play", GO);
        AkSoundEngine.RenderAudio();
    }
    public void EnemyMeleePlay(GameObject GO)
    {
        //When Enemies swing for an attack (WOOOSHH)
        AkSoundEngine.PostEvent("Enemy_Melee_Play", GO);
        AkSoundEngine.RenderAudio();
    }
    public void EnemyRangedPlay(GameObject GO)
    {
        //When Enemies fires an missile (WOOOSHH)
        AkSoundEngine.PostEvent("Enemy_Ranged_Play", GO);
        AkSoundEngine.RenderAudio();
    }
    public void EnemyRangedTargetPlay(GameObject GO, string tag)
    {
        //When Enemies' missile hits a target 
        if (tag == "Player" || tag == "Indestructable")
        {
            AkSoundEngine.SetSwitch("Target", tag, GO);
            AkSoundEngine.PostEvent("Enemy_Ranged_Target_Play", GO);
            AkSoundEngine.RenderAudio();
        }
        else
        {
            Debug.LogError(tag + "is wrong tag and event doesn't play audio");
        }

    }
    
    public void EnemyDeathPlay(GameObject GO)
    {
        //Enemie Dies and stops his random growls
        AggroedEnemies--;
        AkSoundEngine.PostEvent("Enemy_Aggro_Stop", GO);
        AkSoundEngine.PostEvent("Enemy_Death_Play", GO);
        AkSoundEngine.RenderAudio();
    }

    // ######################################################################################################################################
    // ##########################################################                            ################################################
    // ##########################################################  Environment+Music Sounds  ################################################            
    // ##########################################################                            ################################################
    // ######################################################################################################################################

    public void EnvironmentalAmbiencePlay(GameObject GO)
    {
        // Ambience sound playing (Continuous sound)
        AkSoundEngine.PostEvent("Environmental_Ambience_Play", GO);
        AkSoundEngine.RenderAudio();
    }
    public void EnvironmentalAmbienceStop(GameObject GO)
    {
        // Ambience sound stopping 
        AkSoundEngine.PostEvent("Environmental_Ambience_Stop", GO);
        AkSoundEngine.RenderAudio();
    }
    public void MusicSystemPlay(GameObject GO)
    {
        // Music playing (Continuous sound)
        AkSoundEngine.PostEvent("Music_System_Play", GO);
        AkSoundEngine.RenderAudio();
    }
    public void MusicSystemStop(GameObject GO)
    {
        // Music stops 
        AkSoundEngine.PostEvent("Music_System_Stop", GO);
        AkSoundEngine.RenderAudio();
    }
    public void PickupMovePlay(GameObject GO)
    {
        AkSoundEngine.PostEvent("Pickup_Move_Play", GO);
        AkSoundEngine.RenderAudio();
    }
    public void PickupMoveStop(GameObject GO)
    {
        AkSoundEngine.PostEvent("Pickup_Move_Stop", GO);
        AkSoundEngine.RenderAudio();
    }
    public void PickupPlay(GameObject GO)
    {
        AkSoundEngine.PostEvent("Pickup_Play", GO);
        AkSoundEngine.RenderAudio();
    }
   
    // ######################################################################################################################################
    // ##########################################################                 ###########################################################
    // ##########################################################  Player Sounds  ###########################################################
    // ##########################################################                 ###########################################################
    // ######################################################################################################################################

    public void DashPlay(GameObject GO)
    {
        //When Dashing
        AkSoundEngine.PostEvent("Dash_Play", GO);
        AkSoundEngine.RenderAudio();
    }
    public void PlayerDeathPlay(GameObject GO)
    {
        //When Player dies
        AkSoundEngine.PostEvent("Player_Death_Play", GO);
        AkSoundEngine.RenderAudio();
    }
    public void PlayerRespawnPlay(GameObject GO)
    {
        //When Player respawns
        AkSoundEngine.PostEvent("Player_Respawn_Play", GO);
        AkSoundEngine.RenderAudio();
    }
    public void PlayerSpearAttackPlay(GameObject GO)
    {
        //When Player attacks with spear
        AkSoundEngine.PostEvent("Spear_Play", GO);
        AkSoundEngine.RenderAudio();
    }
    public void PlayerSpearAttackTargetPlay(GameObject GO, string tag)
    {
        //When Player attacks with spear and hits a target
        if(tag == "Boss" || tag == "Enemy" || tag == "Indestructable")
        {
        AkSoundEngine.SetSwitch("Target", tag, GO);
        AkSoundEngine.PostEvent("Spear_Target_Play", GO);
        AkSoundEngine.RenderAudio();
        }
        else
        {
            Debug.LogError(tag + "is wrong tag and event doesn't play audio");
        }
    }
    
}
