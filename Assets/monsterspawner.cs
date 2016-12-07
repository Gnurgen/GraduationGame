using UnityEngine;
using System.Collections;

public class monsterspawner : MonoBehaviour {

    public GameObject monster;
    public int monsters;

	// Use this for initialization
	void Start () {
        GameManager.events.OnBossMeteorImpact += SpawnMonster;
        GameManager.events.OnEnemyDeath += MonsterKilled;
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    void SpawnMonster(GameObject meteor)
    {
        if(monsters > 0)
        {
            GameObject m = Instantiate(monster) as GameObject;
            m.transform.position = meteor.transform.position;
            monsters -= 1;
        }
    }

    void MonsterKilled(GameObject enemy)
    {
        monsters += 1;
    }
}
