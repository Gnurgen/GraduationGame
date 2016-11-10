using UnityEngine;
using System.Collections;
using UnityEditor;

public class Health : MonoBehaviour {
    [SerializeField]
    private int _baseHealth;
    [SerializeField]
    private int _healthIncreasePerLevelInPercentage;
    private int _healthPerRes;
    private bool _healthOnLevel = false;
    private int _health, _maxHealth;
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

    public void setHealthVars(int multiplier)
    {
        _baseHealth = 
        _health = _baseHealth;
        _maxHealth = _health;
        _isPlayer = gameObject.tag == _playerTag;
        Subscribe();
    }
    public void setHealthVars(int _resHealth, bool levelUpHealth)
    {
        _healthPerRes = _resHealth;
        _healthOnLevel = levelUpHealth;
        _health = _baseHealth;
        _maxHealth = _health;
        _isPlayer = gameObject.tag == _playerTag;
        Subscribe();
    }

    private void Subscribe()
    {
        if (_isPlayer)
        {
            GameManager.events.OnResourcePickup += increaseHealth;
            GameManager.events.OnLevelUp += levelUp;
        }
    }

    private void decreaseHealth(int val)
    {
        _health -= val;
        if (_health <= 0)
        {
            if (_isPlayer)
                GameManager.events.PlayerDeath(gameObject);
            else
                GameManager.events.EnemyDeath(gameObject);
        }
    }

    private void increaseHealth(GameObject Id, int val)
    {
        _health += _healthPerRes;
        if (_health > _maxHealth)
            _health = _maxHealth;

    }
    private void levelUp(int id)
    {
        _maxHealth *= _healthIncreasePerLevelInPercentage;
        if(_healthOnLevel)
            _health = _maxHealth;
    }
}
