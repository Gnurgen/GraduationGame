﻿using UnityEngine;
using System.Collections;

public class GenerateHealthScript : MonoBehaviour {
    private GameObject[] enemies;
    private GameObject[] healthBars;
    private GameObject healthBar;
    Transform canvas;
    public GameObject HealthBarPrefab;
    public int amountOfHPBars = 20;

    void Start() {
        canvas = GameObject.Find("Canvas").transform;
        healthBars = new GameObject[amountOfHPBars];
        for (int i = 0; i < healthBars.Length; i++) {
            healthBar = Instantiate(HealthBarPrefab);
            healthBar.transform.SetParent(canvas);
            healthBar.transform.SetAsFirstSibling();
            healthBar.transform.localRotation = Quaternion.Euler(0,0,0);
            healthBars[i] = healthBar;
        }
        GameManager.events.OnLoadComplete += moveAllHealthBars;
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
                healthBars[i].GetComponent<EnemyHealthBars>().getMyEnemy(enemies[i]);
            }
        }
        
        if (enemies.Length < healthBars.Length)
            for (int i = 0; i < healthBars.Length - enemies.Length; i++)
                healthBars[i + enemies.Length].SetActive(false);

    }
}
