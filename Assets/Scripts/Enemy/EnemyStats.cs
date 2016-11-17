﻿using UnityEngine;
using System.Collections;

public class EnemyStats : Health {


    [Range(1,5)]
    public int strength = 1;
    public float aggroRange;
    public float turnRate;
    public float damage;
    public float damagePerLevel;
    public float moveSpeed;
    public float moveSpeedPerLevel;
    public float attackDist;
    public float attackRangePerLevel;
    public float attackBredthInRadians;
    public float attackBredthPerLevel;
    public float attackSpeed;
    public float attackSpeedPerLevel;
<<<<<<< HEAD
    public bool onPause;
    public float pauseFor;


=======
    public GameObject Weapon;
    private RoomBuilder room;
>>>>>>> origin/Daniel

    public void Pause()
    {
        onPause = true;
    }

    public void UnPause()
    {
        onPause = false;
    }

    public void PauseFor(float secs)
    {
        onPause = true;
        pauseFor = secs;
    }

    void FixedUpdate()
    {
        if (pauseFor > 0)
        {
            pauseFor -= Time.fixedDeltaTime;
            if (pauseFor <= 0)
            {
                pauseFor = 0;
                onPause = false;
            }
        }
    }

    void Awake()
    {
<<<<<<< HEAD
       setHealthVars(strength.GetHashCode()+1);
        onPause = false;
        pauseFor = 0;
=======
        setHealthVars(strength.GetHashCode()+1);
    }

    void Start()
    {
        room = GetComponentInParent<RoomBuilder>();
        if (room)
            room.AddEnemy(gameObject);
>>>>>>> origin/Daniel
    }

    void Update()
    {
    }

    void OnDestroy()
    {
        room.RemoveEnemy(gameObject);
    }
}
