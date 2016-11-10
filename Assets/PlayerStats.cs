using UnityEngine;
using System.Collections;

public class PlayerStats : Health {
    [SerializeField]
    private int _healthPerResourcePickUp;
    [SerializeField]
    private bool _fullHealthOnLevelUp = false;

    void Awake()
    {
        gameObject.tag = "Player";
        setHealthVars(_healthPerResourcePickUp, _fullHealthOnLevelUp);
    }
}
