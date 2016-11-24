using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {
    [Range(0,100)]
    public float healthIncreasePerLevelInPercentage;
    private float healthPerRes;
    private bool healthOnLevel = false;
    public float health, maxHealth;
    private const string playerTag = "Player";
    SpawnRagdoll rd;

    void Start()
    {
        health = maxHealth;
        Subscribe();
        rd = GetComponent<SpawnRagdoll>();

    }

    public bool isPlayer(string tag)
    {
        return tag == playerTag;
    }

    public void setHealthVars(int resHealth)
    {
        healthPerRes = resHealth;
    }

    private void Subscribe()
    {
        if (isPlayer(gameObject.tag))
        {
            GameManager.events.OnResourcePickup += increaseHealth;
        }
    }


    public void decreaseHealth(float val, Vector3 forceDir, float pushForce)
    {
        health -= val;
        if (health <= 0)
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
            forceDir.y = 0;
            forceDir = Vector3.Normalize(forceDir) * pushForce;
            forceDir.y = 2;
            rd.Execute(forceDir);
        }
    }


    public void increaseHealth(GameObject Id, int val)
    {
        health += healthPerRes;
        if (health > maxHealth)
            health = maxHealth;

    }
}
