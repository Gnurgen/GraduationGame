using UnityEngine;
using System.Collections;

public class SpiritRock : MonoBehaviour {

    public GameObject blueFlame;

    [HideInInspector]
    public SpiritRockSpawn spawn;

    [HideInInspector]
    public bool isEnabled;

    void Start() {
        spawn = GetComponentInChildren<SpiritRockSpawn>();
    }

    public void EnableFlame()
    {
        if (GameManager.game.waypoint)
            GameManager.game.waypoint.DisableFlame();
        GameManager.game.waypoint = this;
        isEnabled = true;
    }

    public void DisableFlame()
    {
        blueFlame.GetComponent<PKFxFX>().StopEffect();
    }
}
