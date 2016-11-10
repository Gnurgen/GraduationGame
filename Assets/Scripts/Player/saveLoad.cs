﻿using UnityEngine;
using System.Collections;

public class saveLoad : MonoBehaviour {
    //Variables to be saved
    Vector3 playerPos;
    float playerHealth;
    int playerLVL;
    float playerXP;

    //Manager 


    public void save () {
        Debug.Log("Save");
        //Get all variables 
        //~~~~~~~~~~~~~~~~~~~~Need to get the varibales from other scripts~~~~~~~~~~~~

        //Save stats to PlayerPrefs
        PlayerPrefs.SetFloat("Player health", playerHealth);
        PlayerPrefs.SetFloat("Player XP", playerXP);
        PlayerPrefs.SetFloat("Player Position X", playerPos.x);
        PlayerPrefs.SetFloat("Player Position Y", playerPos.y);
        PlayerPrefs.SetFloat("Player Position Z", playerPos.z);
        PlayerPrefs.SetInt("Player Level", playerLVL);

        closeMenu();
	}

    public void load() {
        Debug.Log("Load");
        print(PlayerPrefs.GetFloat("Player health"));
        print(PlayerPrefs.GetFloat("Player XP"));
        print(new Vector3(PlayerPrefs.GetFloat("Player Position X"), PlayerPrefs.GetFloat("Player Position Y"), PlayerPrefs.GetFloat("Player Position Z")));
        print(PlayerPrefs.GetInt("Player Level"));

        closeMenu();
    }

    void closeMenu() {
        gameObject.SetActive(false);
    }
	
}
