using UnityEngine;
using System.Collections;
using System;

public class AudioManager : MonoBehaviour {


    [Header("Tags")]
    [SerializeField] 
    private string boss = "Boss";
    [SerializeField] 
    private string boss_Shield = "Boss_Shield", 
        destructible = "Destructible", 
        enemy = "Enemy",
        enemy_Shield = "Enemy_Shield",
        floor = "Floor",
        indestructible = "Indestructible",
        player = "Player";

    // ######################################################################################################################################
    // ##########################################################                            ################################################
    // ##########################################################  Subscribe to EventManager ################################################
    // ##########################################################                            ################################################
    // ######################################################################################################################################

    void Subscribe()
    {

        GameManager.events.OnEnemyAggro += EnemyChatterPlay;
        GameManager.events.OnEnemyDeath += EnemyDeathPlay; 
        GameManager.events.OnEnemyAttackHit += EnemyAttackHitPlaySub;
        GameManager.events.OnEnemyRangedMiss += EnemyRangedAttackMissPlaySub;
        GameManager.events.OnPlayerAttack += PlayerSpearAttackPlay;
        GameManager.events.OnPlayerDashBegin += DashPlay;
        GameManager.events.OnPlayerAttackHit += PlayerAttackHitPlaySub;
        GameManager.events.OnPlayerDeath += PlayerDeathPlay;
        GameManager.events.OnPlayerMove += PlayerMovePlay; // IS MISSING //MAYBE NOT
        GameManager.events.OnPlayerIdle += PlayerMoveStop; // IS MISSING
        GameManager.events.OnWheelOpen += AbilityWheelOpenSub; 
        GameManager.events.OnWheelSelect += AbilityWheelSelectPlaySub; 
        GameManager.events.OnWheelHover += AbilityWheelHoverPlaySub; 
        GameManager.events.OnLevelUp += LevelUpPlaySub;
        GameManager.events.OnMenuOpen += MenuOpenPlaySub; // IS MISSING (de kommer)+ I HAVE TO CHANGE STATE HERE
        GameManager.events.OnMenuClose += MenuClosePlaySub; // IS MISSING (de kommer) + I HAVE TO CHANGE STATE HERE
        GameManager.events.OnObjDestroyed += ObjectDestroyPlaySub; // IS MISSING -> speartargetplay
        GameManager.events.OnResourcePickup += PickupPlaySub;
        GameManager.events.OnCheckPoint += CheckPointPlaySub;

        print("AudioManager Subscribed");
    }

    private void EnemyRangedAttackMissPlaySub(GameObject enemyID)
    {
        EnemyRangedTargetPlay(enemyID, "Indestructable");
    }

    private void ObjectDestroyPlaySub(GameObject GO)
    {
        PlayerSpearAttackTargetPlay(GO, GO.tag);
    }

    private void PlayerAttackHitPlaySub(GameObject GO, GameObject Tar, int i)
    {
        PlayerSpearAttackTargetPlay(GO, Tar.tag);
    }

    private void CheckPointPlaySub()
    {
        CheckPointPlay(GameManager.player);
    }

    private void PickupPlaySub(GameObject GO, int amount)
    {
        PickupPlay(GO);
    }



    private void MenuClosePlaySub()
    {
        throw new NotImplementedException();
    }

    private void MenuOpenPlaySub()
    {
        throw new NotImplementedException();
    }

    private void LevelUpPlaySub(int i)
    {
        LevelUpPlay(GameManager.player);
    }
    private void AbilityWheelHoverPlaySub(int option)
    {
        AbilityWheelHoverPlay(gameObject);
    }

    private void AbilityWheelSelectPlaySub(int option)
    {
        if (option == 10)
            AbilityWheelClosePlay(gameObject);
        else
            AbilityWheelSelectPlay(gameObject);
    }

    private void AbilityWheelOpenSub()
    {
        AbilityWheelOpenPlay(gameObject);
    }

    private void PlayerMoveStop(GameObject Id)
    {
        throw new NotImplementedException();
    }

    private void PlayerMovePlay(GameObject Id)
    {
        throw new NotImplementedException();
    }

    public void EnemyAttackHitPlaySub(GameObject enemyID, float dmg)
    {
        if (enemyID.tag == "Melee")
            EnemyMeleeHitPlayerPlay(GameManager.player);
        else if (enemyID.tag == "Ranged")
            EnemyRangedTargetPlay(GameManager.player, "Player");

    }

    // ######################################################################################################################################
    // ##########################################################                       #####################################################
    // ##########################################################  Ability Cards Sounds #####################################################
    // ##########################################################                       #####################################################
    // ######################################################################################################################################

    public void AbilityCardAppearPlay(GameObject GO)
    {
        AkSoundEngine.PostEvent("Ability_Appear_Play", GO);
        AkSoundEngine.RenderAudio();
    }
    public void AbilityCardGrabPlay(GameObject GO)
    {
        //When the player grabs the card and holds it (continuous sound)
        AkSoundEngine.PostEvent("Ability_Grab_Play", GO);
        AkSoundEngine.PostEvent("Ability_Hold_Play", GO);
        AkSoundEngine.RenderAudio();
    }
    public void AbilityCardGrabStop(GameObject GO)
    {
        //When the player drops the card (either nowhere or on top of another card) 
        AkSoundEngine.PostEvent("Ability_Hold_Stop", GO);
        AkSoundEngine.RenderAudio();
    }
    public void AbilityCardSlotPlay(GameObject GO)
    {
        //When the player drops the card on a slot 
        AkSoundEngine.PostEvent("Ability_Slot_Play", GO);
        AkSoundEngine.RenderAudio();
    }
    public void AbilityCardSwapPlay(GameObject GO)
    {
        //When the player drops the card on a slot with another card
        AkSoundEngine.PostEvent("Ability_Swap_Play", GO);
        AkSoundEngine.RenderAudio();
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
        AkSoundEngine.SetSwitch("Target", tag, GO);
        AkSoundEngine.PostEvent("Boss_Beam_Target_Play", GO);
        AkSoundEngine.RenderAudio();
    }
    public void BossBeamTargetStop(GameObject GO)
    {
        //Beams noise stop
        AkSoundEngine.PostEvent("Boss_Beam_Target_Stop", GO);
        AkSoundEngine.RenderAudio();
    }
    public void BossMissileMovePlay(GameObject GO)
    {
        //Boss's blob moving around (continuous sound)
        AkSoundEngine.PostEvent("Boss_Missile_Move_Play", GO);
        AkSoundEngine.RenderAudio();
    }
    public void BossMissileMoveStop(GameObject GO)
    {
        //Boss's blob moving stops
        AkSoundEngine.PostEvent("Boss_Missile_Move_Stop", GO);
        AkSoundEngine.RenderAudio();
    }
    public void BossMissilePlay(GameObject GO)
    {
        //Boss's blob plays its spawn sound
        AkSoundEngine.PostEvent("Boss_Missile_Play", GO);
        AkSoundEngine.RenderAudio();
    }
    public void BossMissileTargetPlay(GameObject GO, string tag)
    {
        //Boss's blob sound when it hits a target (Continuous sound ????)
        AkSoundEngine.SetSwitch("Target", tag, GO);
        AkSoundEngine.PostEvent("Boss_Missile_Target_Play", GO);
        AkSoundEngine.RenderAudio();
    }
    public void BossRainMovePlay(GameObject GO)
    {
        //Boss's rain plays a sound of it moving (continuous sound)
        AkSoundEngine.PostEvent("Boss_Rain_Move_Play", GO);
        AkSoundEngine.RenderAudio();
    }
    public void BossRainMoveStop(GameObject GO)
    {
        //Boss's rain stops its sound of it moving (continuous sound)
        AkSoundEngine.PostEvent("Boss_Rain_Move_Stop", GO);
        AkSoundEngine.RenderAudio();
    }
    public void BossRainPlay(GameObject GO)
    {
        //Boss's rain plays a sound when spawning
        AkSoundEngine.PostEvent("Boss_Rain_Play", GO);
        AkSoundEngine.RenderAudio();
    }
    public void BossRainTargetPlay(GameObject GO, string tag)
    {
        //Boss's rain plays a sound when hitting a target (Continuous sound)
        AkSoundEngine.SetSwitch("Target", tag, GO);
        AkSoundEngine.PostEvent("Boss_Rain_Target_Play", GO);
        AkSoundEngine.RenderAudio();
    }

    public void BossShieldUpPlay(GameObject GO)
    {
        //Boss plays a sound of it shielding up
        AkSoundEngine.PostEvent("Boss_Shield_Up_Play", GO);
        AkSoundEngine.RenderAudio();
    }
    public void BossShieldDownPlay(GameObject GO)
    {
        //Boss plays a sound of it shielding going down
        AkSoundEngine.PostEvent("Boss_Shield_Down_Play", GO);
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
    public void AbilityWheelOpenPlay(GameObject GO)
    {
        //When opening the Ability Wheel
        AkSoundEngine.PostEvent("Wheel_Open_Play", GO);
        AkSoundEngine.RenderAudio();
    }
    public void AbilityWheelClosePlay(GameObject GO)
    {
        //When opening the Ability Wheel
        AkSoundEngine.PostEvent("Wheel_Close_Play", GO);
        AkSoundEngine.RenderAudio();
    }
    public void AbilityWheelHoverPlay(GameObject GO)
    {
        //When opening the Ability Wheel
        AkSoundEngine.PostEvent("Button_Wheel_Hover_Play", GO);
        AkSoundEngine.RenderAudio();
    }
    public void AbilityWheelSelectPlay(GameObject GO)
    {
        AkSoundEngine.PostEvent("Button_Wheel_Select_play", GO);
        AkSoundEngine.RenderAudio();
    }
    public void CheckPointPlay(GameObject GO)
    {
        //When placing a Checkpoint
        AkSoundEngine.PostEvent("Checkpoint_Play", GO);
        AkSoundEngine.RenderAudio();
    }
    
    // ######################################################################################################################################
    // ##########################################################                ############################################################
    // ########################################################## Ability Sounds ############################################################
    // ##########################################################                ############################################################
    // ######################################################################################################################################

    public void LineAbilityPlay(GameObject GO)
    {
        //When Line ability is activated and playing(Continuous sound)
        //Whenever the spear hits a target it plays the SpearTargetPlay()
        AkSoundEngine.PostEvent("Draw_Ability_Play", GO);
        AkSoundEngine.RenderAudio();
    }
    public void LineAbilityStop(GameObject GO)
    {
        //When Line ability is finished
        AkSoundEngine.PostEvent("Draw_Ability_Stop", GO);
        AkSoundEngine.RenderAudio();
    }
    public void LineAbilityInteractPlay(GameObject GO)
    {
        //When drawing the Line ability (continuous sound)
        AkSoundEngine.PostEvent("Draw_Ability_Control_Play", GO);
        AkSoundEngine.RenderAudio();
    }
    public void LineAbilityInteractStop(GameObject GO)
    {
        //When finnished drawing the Line ability 
        AkSoundEngine.PostEvent("Draw_Ability_Control_Stop", GO);
        AkSoundEngine.RenderAudio();
    }
    public void MultiTapAbilityPlay(GameObject GO)
    {
        //When the MultiTap ability is activated (probably being called every time it hits an enemy)
        AkSoundEngine.PostEvent("Mark_Ability_Play", GO);
        AkSoundEngine.RenderAudio();
    }
    public void MultiTapAbilityInteractPlay(GameObject GO)
    {
        //When the player taps the enemies when interacting the MultiTap Ability
        AkSoundEngine.PostEvent("Mark_Ability_Control_Play", GO);
        AkSoundEngine.RenderAudio();
    }
    public void CircleAbilityPlay(GameObject GO)
    {
        //When finnished drawing the Circle Ability and activates 
        AkSoundEngine.PostEvent("Circle_Ability_Play", GO);
        AkSoundEngine.RenderAudio();
    }
    public void CircleAbilityInteractPlay(GameObject GO)
    {
        //When drawing the Circle Ability (Continuous sound)
        AkSoundEngine.PostEvent("Circle_Ability_Control_Play", GO);
        AkSoundEngine.RenderAudio();
    }
    public void CircleAbilityInteractStop(GameObject GO)
    {
        //When stops drawing the Circle Ability 
        AkSoundEngine.PostEvent("Circle_Ability_Control_Stop", GO);
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
        AkSoundEngine.PostEvent("Enemy_Chatter_Play", GO);
        AkSoundEngine.RenderAudio();
    }

    public void EnemyChatterStop(GameObject GO)
    {
        //Enemies random growls stops 
        AkSoundEngine.PostEvent("Enemy_Chatter_Stop", GO);
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
        AkSoundEngine.SetSwitch("Target", tag, GO);
        AkSoundEngine.PostEvent("Enemy_Ranged_Target_Play", GO);
        AkSoundEngine.RenderAudio();
    }
    public void EnemyShieldPlay(GameObject GO)
    {
        //Shield of enemy (IDK)
        AkSoundEngine.PostEvent("Enemy_Shield_Play", GO);
        AkSoundEngine.RenderAudio();
    }
    public void EnemyDeathPlay(GameObject GO)
    {
        //Enemie Dies and stops his chatter
        AkSoundEngine.PostEvent("Enemy_Chatter_Stop", GO);
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
    public void PickupPlay(GameObject GO)
    {
        AkSoundEngine.PostEvent("Pickup_Play", GO);
        AkSoundEngine.RenderAudio();
    }
    public void LevelUpPlay(GameObject GO)
    {
        AkSoundEngine.PostEvent("Level_Up_Play", GO);
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
        AkSoundEngine.SetSwitch("Target", tag, GO);
        AkSoundEngine.PostEvent("Spear_Target_Play", GO);
        AkSoundEngine.RenderAudio();
    }
    
}
