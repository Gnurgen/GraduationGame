using UnityEngine;
using System.Collections;

public class playerVibrate : MonoBehaviour {
#if UNITY_EDITOR
    void Start()
    {
        print("LOL");
    }

#elif UNITY_ANDROID
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
        Vibrator.Vibrate(enemyHitVibration);
    }

    void vibrateElevatorActivation()
    {
        Vibrator.Vibrate(elevatorVibration);
    }

    void vibrateCameraShake(float seconds)
    {
        Vibrator.Vibrate((long)(seconds * 1000f));
    }
#endif
}
