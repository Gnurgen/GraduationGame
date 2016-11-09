using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {
    [SerializeField]
    private int _baseHealth, _healthPerResourcePickUp;
    [SerializeField]
    private int _healthIncreasePerLevelInPercentage;

    [SerializeField]
    private bool _fullHealthOnLevelUp = false;
    private int _health, _maxHealth;

    private const string _enemyTag = "Enemy";
    private const string _bossTag = "Boss";
    private const string _playerTag = "Player";
    private bool _isPlayer = false;

    public int health
    {
        get
        {
            return _health;
        }
        set
        {
            _health = value;
        }
    }
    public bool isPlayer
    {
        get
        {
            return _isPlayer;
        }
    }
    
    void Awake()
    {
        _health = _baseHealth;
        _maxHealth = _health;
        _isPlayer = gameObject.tag == _playerTag;
        Subscribe();
    }

    void Subscribe()
    {
        if (_isPlayer)
        {
            GameManager.events.OnEnemyAttackHit += decreaseHealth;
            GameManager.events.OnResourcePickup += increaseHealth;
            GameManager.events.OnLevelUp += levelUp;
        }
        else
            GameManager.events.OnPlayerAttackHit += decreaseHealth;
    }

    private void decreaseHealth(GameObject Id, int val)
    {
        _health -= val;
    }
    private void decreaseHealth(GameObject Id, GameObject me, int val)
    {
        if (me == gameObject)
        {
            _health -= val;
            if (_health >= 0)
            {
                GameManager.events.EnemyDeath(gameObject);
                enabled = false;
            }
        }
    }
    private void increaseHealth(GameObject Id, int val)
    {
        _health += _healthPerResourcePickUp;
        if (_health > _maxHealth)
            _health = _maxHealth;
        if (_health <= 0)
            GameManager.events.PlayerDeath(gameObject);
    }
    private void levelUp(int id)
    {
        _maxHealth *= _healthIncreasePerLevelInPercentage;
        if(_fullHealthOnLevelUp)
            _health = _maxHealth;
    }
}
