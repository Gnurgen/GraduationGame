using UnityEngine;
using System.Collections;

public class GenerateHealthScript : MonoBehaviour {
    private GameObject[] enemies;
    private GameObject[] healthBars;
    private GameObject healthBar;
    public GameObject HealthBarPrefab;

    void Start() {
        healthBars = new GameObject[100];
        for (int i = 0; i < healthBars.Length; i++) {
            healthBar = Instantiate(HealthBarPrefab);
            healthBar.transform.parent = GameObject.Find("Canvas").transform;
            healthBar.transform.localRotation = Quaternion.Euler(0,0,0);
            healthBars[i] = healthBar;
        }
        moveAllHealthBars();
    }
    public void moveAllHealthBars()
    {
        for (int i = 0; i < healthBars.Length; i++)
            healthBars[i].SetActive(true);

        enemies = null;
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        
        if (GameObject.FindGameObjectsWithTag("Enemy") != null)
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                healthBars[i].GetComponent<EnemyHealthBars>().getMyEnemy(enemies[i], 0);
            }
        }
        
        if (enemies.Length < healthBars.Length)
            for (int i = 0; i < healthBars.Length - enemies.Length; i++)
                healthBars[i + enemies.Length].SetActive(false);

    }
}
