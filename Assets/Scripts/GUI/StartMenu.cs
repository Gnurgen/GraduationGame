﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour {
    public GameObject[] listOfMenues;
    public bool isDK = true;
    public Sprite dK, eN;
    public  GameObject language;

    public void newGame() {
        SceneManager.LoadScene("Elevator"); // NEEDS TO BE CORRECT SCENE!!!!!!!!!!!!!
    }

    public void loadGame() {
        SceneManager.LoadScene("Elevator"); // NEEDS TO BE CORRECT SCENE!!!!!!!!!!!!!
    }

    public void selectMenu(int menu) {
        for (int i = 0; i < listOfMenues.Length; i++) {
            if (i != menu)
                listOfMenues[i].SetActive(false);
            else
                listOfMenues[i].SetActive(true);
        }
    }
    public void changeLanguage() {
        if (isDK == true)
        {
            Debug.Log("dansk");
            language.GetComponent<Image>().overrideSprite = eN;
            isDK = false;
        }
        else if (isDK == false)
        {
            Debug.Log("engelsk");
            language.GetComponent<Image>().overrideSprite = dK;
            isDK = true;
        }
    }
}
