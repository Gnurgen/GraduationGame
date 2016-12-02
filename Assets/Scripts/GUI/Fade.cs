using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class Fade : MonoBehaviour {

    Image fadeImg;

    public Color black, alpha, white, whitealpha;
    public float duration = 2f;
    private Color startColor, endColor;
    private float currentTime, maxTime, scaling;
    private bool fading = false;

    void Start() {
        fadeImg = gameObject.GetComponent<Image>();
        GameManager.events.OnBossDeath += winFade;
        GameManager.events.OnFadeToBlack += fadeToBlack;
        GameManager.events.OnFadeToWhite += fadeToWhite;
        GameManager.events.OnFadeFromBlackToTransparent += fadeFromBlackToTransparent;
        GameManager.events.OnFadeFromBlackToTransparent += fadeFromWhiteToTransparent;
    }
    private IEnumerator IEFade(int color) // 0 = black, 1 = White, 2 = Transparent
    {
        float step = 0;
        while(step < duration)
        {
            step += Time.deltaTime / duration;
            fadeImg.color = Color.Lerp(startColor, endColor, step);
            yield return null;
        }
        switch(color)
        {
            case 0:
                GameManager.events.FadedBlackScreen();
                break;
            case 1:
                GameManager.events.FadedWhiteScreen();
                break;
            case 2:
                GameManager.events.FadedTransparentScreen();
                break;
            default:
                GameManager.events.FadedBlackScreen();
                break;
        }
    }
    public void fadeToBlack() {
        startColor = alpha;
        endColor = black;
        StartCoroutine(IEFade(0));
    }

    public void fadeToWhite() {
        startColor = alpha;
        endColor = white;
        StartCoroutine(IEFade(1));
    }

    private void fadeFromBlackToTransparent()
    {
        startColor = black;
        endColor = alpha;
        StartCoroutine(IEFade(2));
    }
    private void fadeFromWhiteToTransparent()
    {
        startColor = white;
        endColor = alpha;
        StartCoroutine(IEFade(2));
    }
 

    void winFade(GameObject ID) {
        gameObject.GetComponent<Image>().color = white;
        gameObject.GetComponent<Image>().CrossFadeAlpha(0, 0, true);
        gameObject.GetComponent<Image>().CrossFadeAlpha(1, 3, true);
    }
}
