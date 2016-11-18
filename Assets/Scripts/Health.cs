using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {
    [Range(0,100)]
    public float healthIncreasePerLevelInPercentage;
    private float healthPerRes;
    private bool healthOnLevel = false;
    public float health, maxHealth;
    private const string playerTag = "Player";

    void Start()
    {
        health = maxHealth;
        Subscribe();


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


    public void decreaseHealth(float val)

    {
        health -= val;
        if (health <= 0)
        {
            if (isPlayer(gameObject.tag))
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
        health += healthPerRes;
        if (health > maxHealth)
            health = maxHealth;

    }
}
