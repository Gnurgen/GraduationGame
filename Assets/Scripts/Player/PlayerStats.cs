using UnityEngine;
using System.Collections;

public class PlayerStats : Health {
    [SerializeField]
    private int healthPerResourcePickUp;

    void Awake()
    {
        gameObject.tag = "Player";
        setHealthVars(healthPerResourcePickUp);
    }
}
