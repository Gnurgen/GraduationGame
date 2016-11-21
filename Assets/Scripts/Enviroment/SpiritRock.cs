using UnityEngine;
using System.Collections;

public class SpiritRock : MonoBehaviour {
    public GameObject blueFlame;

    void Start() {   
        
    }
    public void stopFlame() {
        blueFlame.GetComponent<PKFxFX>().StopEffect();
    }
}
