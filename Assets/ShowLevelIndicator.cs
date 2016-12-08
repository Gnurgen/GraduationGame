﻿using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class ShowLevelIndicator : MonoBehaviour {

    [SerializeField]
    private Sprite[] imglvl = new Sprite[4];
    Image img;
    [SerializeField]
    float fadeTime = 1;
    // Use this for initialization
    void Start () {
        img = GetComponent<Image>();
        GameManager.events.OnLoadComplete += ShowLvlIndicator;
        GameManager.events.OnElevatorMoveStop += HideLvlIndicator;

    }

    private void HideLvlIndicator()
    {
        StartCoroutine(FadeImg(fadeTime));
    }

    private IEnumerator FadeImg(float fadeTime)
    {
        float step = 1;
        float c = 200f / 255f;
        Color col; 
        while (step > 0)
        {
            step -= (1/fadeTime) * Time.deltaTime;
            col = new Color(c, c, c, step);
            img.color = col;
            yield return null;
        }
        img.enabled = false;
    }

    private void ShowLvlIndicator()
    {
        img.sprite = imglvl[PlayerPrefs.GetInt("Progress")];
        if(PlayerPrefs.GetInt("Progress") == 0)
        {
            StartCoroutine(FadeImg(fadeTime+3));
        }
    }
}
