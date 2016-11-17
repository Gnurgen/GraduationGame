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
    public GameObject Weapon;
    private RoomBuilder room;

    void Awake()
    {
        setHealthVars(strength.GetHashCode()+1);
    }

    void Start()
    {
        room = GetComponentInParent<RoomBuilder>();
        if (room)
            room.AddEnemy(gameObject);
    }

    void Update()
    {
    }

    void OnDestroy()
    {
        room.RemoveEnemy(gameObject);
    }
}
