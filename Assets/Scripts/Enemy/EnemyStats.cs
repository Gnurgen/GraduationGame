using UnityEngine;
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
    public bool onPause;
    public float pauseFor;



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
        onPause = false;
        pauseFor = 0;
    }

    void Update()
    {
    }
}
