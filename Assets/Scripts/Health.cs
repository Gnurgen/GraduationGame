using UnityEngine;
using System.Collections;
using System;

public class Health : MonoBehaviour, IHealth {
    private float healthPerRes;
    private bool healthOnLevel = false;
    private bool vulnerable = true;
    public float health, maxHealth;
    private const string playerTag = "Player";
    SpawnRagdoll rd;
    HealthController ht;
    Vector3 cForceDir;
    private RoomBuilder parentRoom = null;

    void OnEnable()
    {
        health = maxHealth;
        Subscribe();
        rd = GetComponent<SpawnRagdoll>();
        if(ht==null)
            ht = new HealthController();
        ht.SetHealth(this);
/*
        if (this != GameManager.player)
        {
            parentRoom = GetComponentInParent<RoomBuilder>();
            parentRoom.enemyCount--;
            Debug.Log("Enemy died");
        }*/
    }

    private void Subscribe()
    {
        if (isPlayer(gameObject.tag))
        {
            GameManager.events.OnResourcePickup += increaseHealth;
        }
    }


    #region Public methods
    public bool isPlayer(string tag)
    {
        return tag == playerTag;
    }

    public void setHealthVars(int resHealth)
    {
        healthPerRes = resHealth;
    }

    public void MakeVulnerable()
    {
        vulnerable = true;
    }

    public void MakeInvulnerable()
    {
        vulnerable = false;
    }

    public bool IsVulnerable()
    {
        return vulnerable;
    }

    public void decreaseHealth(float dmg, Vector3 forceDir, float pushForce)
    {
        if (ht == null)
        {
            ht = new HealthController();
            ht.SetHealth(this);
        }
        ht.DecreaseHealth(vulnerable, health, dmg, forceDir.x, forceDir.y, forceDir.z, pushForce);
    }

    public void EqualHealth()
    {
        health = maxHealth;
    }

    public void increaseHealth(GameObject Id, int val)
    {
        health += healthPerRes;
        if (health > maxHealth)
            health = maxHealth;

    }
    #endregion

    #region IHealth implementation
    public void SetHealth(float h)
    {
        health = h;
    }

    public void SpawnRagdoll(float forceX, float forceY, float forceZ, float pushForce)
    {
        Vector3 forceDir = new Vector3(forceX, forceY, forceZ);
        forceDir = Vector3.Normalize(forceDir) * pushForce;
        forceDir.y = 2;
        if(rd == null)
            rd = GetComponent<SpawnRagdoll>();
        rd.Execute(forceDir);
    }

    public void Dead()
    {
        if (isPlayer(gameObject.tag))
        {
            GameManager.events.PlayerDeath(gameObject);
        }
        else
        {
            GameManager.events.EnemyDeath(gameObject);
            GameManager.events.ResourceDrop(gameObject, 1); // AMOUNT OF BLOBS DROPS
        }
    }
    #endregion

}

public interface IHealth
{
    void SetHealth(float h);
    void SpawnRagdoll(float forceX, float forceY, float forceZ, float pushForce);
    void Dead();
}

[Serializable]
public class HealthController
{
    IHealth h;

    
    public void DecreaseHealth(bool vulnerable, float health, float dmg, float forceX, float forceY, float forceZ, float pushForce)
    {
        if (vulnerable)
        {
            if(health - dmg <= 0)
            {
                h.SetHealth(0);
                h.Dead();
                h.SpawnRagdoll(forceX, forceY, forceZ, pushForce);
            }
            else
            {
                h.SetHealth(health - dmg);
            }
        }
    }

    public void IncreaseHealth(float health, float inc, float maxHealth)
    {
        if(health + inc >= maxHealth)
        {
            h.SetHealth(maxHealth);
        }
        else
        {
            h.SetHealth(health + inc);
        }
    }


    public void SetHealth(IHealth h)
    {
        this.h = h;
    }
}
