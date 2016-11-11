using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {
    public float _baseHealth;
    public float _healthIncreasePerLevelInPercentage;
    private float _healthPerRes;
    private bool _healthOnLevel = false;
    private float _health, _maxHealth;
    private const string _playerTag = "Player";
    private bool _isPlayer = false;

    public float health
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


    public void decreaseHealth(float val)

    {
        _health -= val;
        if (_health <= 0)
        {
            if (_isPlayer)
            {
                GameManager.events.PlayerDeath(gameObject);
                print("øv :( (pik)spiller er død \n #  #\n#   #\n ###");
            }
            else
            {
                GameManager.events.EnemyDeath(gameObject);
                GameManager.events.ResourceDrop(gameObject, 3); // AMOUNT OF BLOBS DROPS
                Destroy(gameObject);
            }
        }
    }

    public void increaseHealth(GameObject Id, int val)
    {
        _health += _healthPerRes;
        if (_health > _maxHealth)
            _health = _maxHealth;

    }
    public void levelUp(int id)
    {
        _maxHealth *= _healthIncreasePerLevelInPercentage;
        if(_healthOnLevel)
            _health = _maxHealth;
    }
}
