using UnityEngine;
using System.Collections;

public class playerVibrate : MonoBehaviour {
    private EventManager EM;

    void Start () {
        EM = GameManager.events;
        EM.OnEnemyAttackHit += vibrateForSec;
	}

    void vibrateForSec(GameObject ID, float dmg)
    {
        Debug.Log("Vibrate!");
        //Vibrator.Vibrate(100);
    }
}
