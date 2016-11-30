using UnityEngine;
using System.Collections;

public class playerVibrate : MonoBehaviour {
    public long vibrationDuration = 100;
    private InputManager IM;
    private EventManager EM;
    int ID;

    void Start () {
        IM = GameManager.input;
        ID = IM.GetID();
        EM = GameManager.events;

        EM.OnEnemyAttackHit += AttackVibrate;
        //EM.OnEnemyAttackHit += vibrateForSec;
	}


    void AttackVibrate(GameObject id, float dmg)
    {

        //Handheld.Vibrate();
        try
        {
            Vibrator.Vibrate(vibrationDuration);
        }
        catch
        {
            Debug.Log("LOL! NO VIBRATE!");
        }
    }

    /*void vibrateForSec(GameObject ID, float dmg)
    {
        Debug.Log("Vibrate!");
#if UNITY_ANDROID
        Vibrator.Vibrate(vibrateForMiliSeconds);
#endif
    }
*/
}
