using UnityEngine;
using System.Collections;

public class GenerateHealthScript : MonoBehaviour {
    private GameObject[] enemies;
    private GameObject[] healthBars;
    private GameObject healthBar;
    public GameObject HealthBarPrefab;

    void Start() {
      
      
        GameManager.events.OnLoadComplete += moveAllHealthBars;
    }
    public void moveAllHealthBars()
    {
       

        enemies = null;
        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        healthBars = new GameObject[enemies.Length];
        for (int i = 0; i < healthBars.Length; i++)
        {
            healthBar = Instantiate(HealthBarPrefab);
            healthBar.transform.SetParent(transform);
            healthBar.transform.SetAsFirstSibling();
            healthBar.transform.localRotation = Quaternion.Euler(0, 0, 0);
            healthBars[i] = healthBar;
            healthBars[i].GetComponent<EnemyHealthBars>().getMyEnemy(enemies[i]);
        }
    }
}
