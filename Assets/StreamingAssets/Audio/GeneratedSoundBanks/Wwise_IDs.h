/////////////////////////////////////////////////////////////////////////////////////////////////////
//
// Audiokinetic Wwise generated include file. Do not edit.
//
/////////////////////////////////////////////////////////////////////////////////////////////////////

#ifndef __WWISE_IDS_H__
#define __WWISE_IDS_H__

#include <AK/SoundEngine/Common/AkTypes.h>

namespace AK
{
    namespace EVENTS
    {
        static const AkUniqueID ABILITY_APPEAR_PLAY = 1289281488U;
        static const AkUniqueID ABILITY_GRAB_PLAY = 1825762123U;
        static const AkUniqueID ABILITY_HOLD_PLAY = 855238596U;
        static const AkUniqueID ABILITY_HOLD_STOP = 3627685822U;
        static const AkUniqueID ABILITY_SLOT_PLAY = 2227190561U;
        static const AkUniqueID ABILITY_SWAP_PLAY = 3291280854U;
        static const AkUniqueID BOSS_BEAM_PLAY = 3074393677U;
        static const AkUniqueID BOSS_BEAM_STOP = 4191587739U;
        static const AkUniqueID BOSS_BEAM_TARGET_PLAY = 1433984317U;
        static const AkUniqueID BOSS_BEAM_TARGET_STOP = 3661299883U;
        static const AkUniqueID BOSS_MISSILE_MOVE_PLAY = 807044522U;
        static const AkUniqueID BOSS_MISSILE_MOVE_STOP = 2211246548U;
        static const AkUniqueID BOSS_MISSILE_PLAY = 3257950136U;
        static const AkUniqueID BOSS_MISSILE_TARGET_PLAY = 1758886478U;
        static const AkUniqueID BOSS_MISSILE_TARGET_STOP = 2802002224U;
        static const AkUniqueID BOSS_RAIN_MOVE_PLAY = 169826302U;
        static const AkUniqueID BOSS_RAIN_MOVE_STOP = 1207719104U;
        static const AkUniqueID BOSS_RAIN_PLAY = 2907375116U;
        static const AkUniqueID BOSS_RAIN_TARGET_PLAY = 4044112498U;
        static const AkUniqueID BOSS_RAIN_TARGET_STOP = 1460805868U;
        static const AkUniqueID BOSS_SHIELD_DOWN_PLAY = 650062122U;
        static const AkUniqueID BOSS_SHIELD_UP_PLAY = 2401642071U;
        static const AkUniqueID BUTTON_MENU_PLAY = 3906322206U;
        static const AkUniqueID BUTTON_START_GAME_PLAY = 789154922U;
        static const AkUniqueID BUTTON_WHEEL_PLAY = 3789878710U;
        static const AkUniqueID CHECKPOINT_PLAY = 3390114776U;
        static const AkUniqueID CIRCLE_ABILITY_CONTROL_PLAY = 2990023069U;
        static const AkUniqueID CIRCLE_ABILITY_CONTROL_STOP = 880897803U;
        static const AkUniqueID CIRCLE_ABILITY_PLAY = 1292313331U;
        static const AkUniqueID DASH_PLAY = 2174485018U;
        static const AkUniqueID DRAW_ABILITY_CONTROL_PLAY = 669353839U;
        static const AkUniqueID DRAW_ABILITY_CONTROL_STOP = 3386881389U;
        static const AkUniqueID DRAW_ABILITY_PLAY = 1855658509U;
        static const AkUniqueID DRAW_ABILITY_STOP = 2972852827U;
        static const AkUniqueID ENEMY_CHATTER_PLAY = 2311308570U;
        static const AkUniqueID ENEMY_CHATTER_STOP = 3751864356U;
        static const AkUniqueID ENEMY_MELEE_HIT_PLAYER_PLAY = 1233116581U;
        static const AkUniqueID ENEMY_MELEE_PLAY = 2339069971U;
        static const AkUniqueID ENEMY_RANGED_PLAY = 4072057392U;
        static const AkUniqueID ENEMY_RANGED_TARGET_PLAY = 914283814U;
        static const AkUniqueID ENEMY_SHIELD_PLAY = 3151436706U;
        static const AkUniqueID ENVIRONMENTAL_AMBIENCE_PLAY = 2813179063U;
        static const AkUniqueID ENVIRONMENTAL_AMBIENCE_STOP = 1543198181U;
        static const AkUniqueID MARK_ABILITY_CONTROL_PLAY = 220652244U;
        static const AkUniqueID MARK_ABILITY_PLAY = 2308852762U;
        static const AkUniqueID MUSIC_SYSTEM_PLAY = 350081739U;
        static const AkUniqueID MUSIC_SYSTEM_STOP = 3509289057U;
        static const AkUniqueID PICKUP_PLAY = 1715783110U;
        static const AkUniqueID PLAYER_DEATH_PLAY = 2348472398U;
        static const AkUniqueID PLAYER_RESPAWN_PLAY = 1389849668U;
        static const AkUniqueID SPEAR_PLAY = 2880001405U;
        static const AkUniqueID SPEAR_TARGET_PLAY = 2770920781U;
    } // namespace EVENTS

    namespace STATES
    {
        namespace ENVIRONMENT
        {
            static const AkUniqueID GROUP = 1229948536U;

            namespace STATE
            {
                static const AkUniqueID CORE = 3787826988U;
                static const AkUniqueID CORE_CORRIDOR = 525391897U;
                static const AkUniqueID CORRIDOR = 4063189299U;
                static const AkUniqueID INNER = 1778842195U;
                static const AkUniqueID INNER_CORRIDOR = 1592175108U;
                static const AkUniqueID OUTER = 175046638U;
                static const AkUniqueID OUTER_CORRIDOR = 2571765635U;
            } // namespace STATE
        } // namespace ENVIRONMENT

        namespace GAME_STATE
        {
            static const AkUniqueID GROUP = 766723505U;

            namespace STATE
            {
                static const AkUniqueID AFTER_BOSS = 1362486867U;
                static const AkUniqueID IN_ABILITY_CONTROL = 3853226033U;
                static const AkUniqueID IN_ABILITY_WHEEL = 878736223U;
                static const AkUniqueID IN_BATTLE = 210928257U;
                static const AkUniqueID IN_ENDING_CUTSCENE = 3858937137U;
                static const AkUniqueID IN_INTRO_CUTSCENE = 2874168282U;
                static const AkUniqueID IN_MAIN_MENU = 1415091186U;
                static const AkUniqueID IN_SKILL_TREE = 3788403633U;
                static const AkUniqueID OUT_OF_BATTLE = 304575330U;
            } // namespace STATE
        } // namespace GAME_STATE

    } // namespace STATES

    namespace SWITCHES
    {
        namespace TARGET
        {
            static const AkUniqueID GROUP = 3902528438U;

            namespace SWITCH
            {
                static const AkUniqueID BOSS = 1560169506U;
                static const AkUniqueID BOSS_SHIELD = 27428518U;
                static const AkUniqueID DESTRUCTIBLE = 1387800999U;
                static const AkUniqueID ENEMY = 2299321487U;
                static const AkUniqueID ENEMY_SHIELD = 3262939977U;
                static const AkUniqueID FLOOR = 1088209313U;
                static const AkUniqueID INDESTRUCTIBLE = 210809950U;
                static const AkUniqueID PLAYER = 1069431850U;
            } // namespace SWITCH
        } // namespace TARGET

    } // namespace SWITCHES

    namespace GAME_PARAMETERS
    {
        static const AkUniqueID SS_AIR_FEAR = 1351367891U;
        static const AkUniqueID SS_AIR_FREEFALL = 3002758120U;
        static const AkUniqueID SS_AIR_FURY = 1029930033U;
        static const AkUniqueID SS_AIR_MONTH = 2648548617U;
        static const AkUniqueID SS_AIR_PRESENCE = 3847924954U;
        static const AkUniqueID SS_AIR_RPM = 822163944U;
        static const AkUniqueID SS_AIR_SIZE = 3074696722U;
        static const AkUniqueID SS_AIR_STORM = 3715662592U;
        static const AkUniqueID SS_AIR_TIMEOFDAY = 3203397129U;
        static const AkUniqueID SS_AIR_TURBULENCE = 4160247818U;
    } // namespace GAME_PARAMETERS

    namespace TRIGGERS
    {
        static const AkUniqueID AWE_STINGER = 1429963265U;
        static const AkUniqueID THREAT_STINGER = 2246995438U;
    } // namespace TRIGGERS

    namespace BANKS
    {
        static const AkUniqueID INIT = 1355168291U;
        static const AkUniqueID MAIN = 3161908922U;
    } // namespace BANKS

    namespace BUSSES
    {
        static const AkUniqueID AMBIENCE = 85412153U;
        static const AkUniqueID COMBAT = 2764240573U;
        static const AkUniqueID ENVIRONMENTAL = 1973600711U;
        static const AkUniqueID MASTER_AUDIO_BUS = 3803692087U;
        static const AkUniqueID MASTER_SECONDARY_BUS = 805203703U;
        static const AkUniqueID MUSIC = 3991942870U;
        static const AkUniqueID REVERB = 348963605U;
        static const AkUniqueID SFX = 393239870U;
        static const AkUniqueID SYSTEM = 3859886410U;
    } // namespace BUSSES

    namespace AUX_BUSSES
    {
        static const AkUniqueID CORRIDOR = 4063189299U;
        static const AkUniqueID HALL = 3633416828U;
    } // namespace AUX_BUSSES

}// namespace AK

#endif // __WWISE_IDS_H__
