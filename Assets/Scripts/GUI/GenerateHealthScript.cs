using UnityEngine;
using System.Collections;

public class GenerateHealthScript : MonoBehaviour {
    private GameObject[] meele;
    private GameObject[] shield;
    private GameObject[] ranged;
    private GameObject[] healthBars;
    private GameObject healthBar;
    public GameObject HealthBarPrefab;

    void Start() {
        healthBars = new GameObject[4];
        for (int i = 0; i < healthBars.Length; i++) {
            healthBar = Instantiate(HealthBarPrefab);
            healthBar.transform.parent = GameObject.Find("Canvas").transform;
            healthBar.transform.localRotation = Quaternion.Euler(0,0,0);
            Debug.Log(healthBar.transform.rotation);
            healthBars[i] = healthBar;
        }
        moveAllHealthBars();
    }
    void moveAllHealthBars()
    {
        meele = null;
        meele = GameObject.FindGameObjectsWithTag("Meele");
        shield = null;
        shield = GameObject.FindGameObjectsWithTag("Shield");
        ranged = null;
        ranged = GameObject.FindGameObjectsWithTag("Ranged");
        Debug.Log(meele);
        if (GameObject.FindGameObjectsWithTag("Meele") != null)
        {
            for (int i = 0; i < meele.Length; i++)
            {
                healthBars[i].GetComponent<EnemyHealthBars>().getMyEnemy(meele[i], 0);
            }
        }
        if (GameObject.FindGameObjectsWithTag("Shield") != null)
        {
            for (int i = 0; i < shield.Length; i++)
            {
                healthBars[i + meele.Length].GetComponent<EnemyHealthBars>().getMyEnemy(shield[i], 0);
            }
        }
        if (GameObject.FindGameObjectsWithTag("Ranged") != null)
        {
            for (int i = 0; i < ranged.Length; i++)
            {
                healthBars[i + meele.Length + ranged.Length].GetComponent<EnemyHealthBars>().getMyEnemy(ranged[i], 0);
            }
        }
    }
}
