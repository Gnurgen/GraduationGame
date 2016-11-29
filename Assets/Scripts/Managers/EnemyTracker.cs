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

	// Use this for initialization
	void Start () {
        GameManager.events.OnResourcePickup += OnPickUp;
        guideSpawned = false;
        StartCoroutine(GetTotalEnemies());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnPickUp(GameObject go, int blob)
    {
        currentEnemiesAlive -= 1;
        if(!guideSpawned && ((float)currentEnemiesAlive / (float)allEnemies) <= (1 - percentageToKill))
        {
            Instantiate(guidingWhisp, GameManager.spear.transform.position, Quaternion.identity);
            guideSpawned = true;
        }
    }


    IEnumerator GetTotalEnemies()
    {
        yield return new WaitForSeconds(5f);
        allEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
        currentEnemiesAlive = allEnemies;
        yield break;
    }
}
