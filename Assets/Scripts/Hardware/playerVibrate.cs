using UnityEngine;
using System.Collections;

public class playerVibrate : MonoBehaviour {
    public long enemyHitVibration = 10000;
    public long elevatorVibration = 100;
    private InputManager IM;
    private EventManager EM;
    int ID;

    void Start () {
        IM = GameManager.input;
        ID = IM.GetID();
        EM = GameManager.events;

        EM.OnEnemyAttackHit += vibrateHit;
        EM.OnLoadComplete += vibrateElevatorActivation;
        EM.OnCameraShake += vibrateCameraShake;
	}

    void vibrateHit(GameObject ID, float dmg)
    {
#if UNITY_ANDROID
        Vibrator.Vibrate(enemyHitVibration);
#endif
    }

    void vibrateElevatorActivation()
    {
#if UNITY_ANDROID
        Vibrator.Vibrate(elevatorVibration);
#endif
    }

    void vibrateCameraShake(float seconds)
    {
#if UNITY_ANDROID
        Vibrator.Vibrate((long)(seconds * 1000f));
#endif
    }
}
