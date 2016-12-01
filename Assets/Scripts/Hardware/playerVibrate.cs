using UnityEngine;
using System.Collections;

public class playerVibrate : MonoBehaviour {
    public long vibrateForMiliSeconds = 100;
    private InputManager IM;
    private EventManager EM;
    int ID;

    void Start () {
        IM = GameManager.input;
        ID = IM.GetID();
        EM = GameManager.events;

        EM.OnEnemyAttackHit += AddStandardVibration;
        //EM.OnEnemyAttackHit += vibrateForSec;
	}


    void AddStandardVibration(GameObject ID, float dmg)
    {
        Handheld.Vibrate();
    }

    /*void vibrateForSec(GameObject ID, float dmg)
    {
        Debug.Log("Vibrate!");
#if UNITY_ANDROID
        Vibrator.Vibrate(vibrateForMiliSeconds);
#endif
    }
*/}
