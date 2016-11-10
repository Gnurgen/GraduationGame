using UnityEngine;
using System.Collections;

public class EnemyStats : Health {
    [SerializeField]
    private float _baseDamage;

    private enum Setting {_1x, _2x, _3x,_4x, _5x}
    [SerializeField]
    private Setting strength;
    void Awake()
    {
       setHealthVars(strength.GetHashCode()+1);
    }

    void Update()
    {
    }
}
