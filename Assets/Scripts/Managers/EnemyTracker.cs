using UnityEngine;
using System.Collections;

public class EnemyTracker : MonoBehaviour {

    [SerializeField]
    private float percentageToKill;
    [SerializeField]
    private GameObject guidingWhisp;

    [SerializeField]
    private int allEnemies;
    [SerializeField]
    private int currentEnemiesAlive;
    private bool guideSpawned;


    SpiritLvlBar spiritLevelBar;
	// Use this for initialization
	void Start () {
        GameManager.events.OnResourcePickup += OnPickUp;
        GameManager.events.OnMapGenerated += GetTotalEnemies;
        guideSpawned = false;
	}
	

    void OnPickUp(GameObject go, int blob)
    {
        currentEnemiesAlive -= 1;
        if( spiritLevelBar == null)
            spiritLevelBar = GameObject.Find("SpiritBar").GetComponent<SpiritLvlBar>();
        spiritLevelBar.updateBar(allEnemies, currentEnemiesAlive, percentageToKill);
        if(!guideSpawned && ((float)currentEnemiesAlive / (float)allEnemies) <= (1 - percentageToKill))
        {
            Instantiate(guidingWhisp, GameManager.spear.transform.position, Quaternion.identity);
            guideSpawned = true;
        }
    }


    private void GetTotalEnemies()
    {
        allEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
        currentEnemiesAlive = allEnemies;
    }
}
