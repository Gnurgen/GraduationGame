using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Fade : MonoBehaviour {

    Image fadeImg;

    public Color black, alpha, white, whitealpha;
    private Color startColor, endColor;
    private float currentTime, maxTime, scaling;
    private bool fading = false;

    void Update() {
        if (fading == true && currentTime < maxTime)
        {
            currentTime += Time.deltaTime;
            scaling = (maxTime - (maxTime - currentTime)) / maxTime;
            fadeImg.color = Color.Lerp(startColor, endColor, scaling);
        }
        else if (fading == true && currentTime >= maxTime)
            fading = false;

    }
    void Start() {
        fadeImg = gameObject.GetComponent<Image>();
        GameManager.events.OnBossDeath += winFade;

    }
    public void fadeToBlack(float duration) {
        fading = true;
        startColor = alpha;
        endColor = black;
        maxTime = duration;

    }
    public void fadeFromBlack(float duration)
    {
        fading = true;
        startColor = black;
        endColor = alpha;
        maxTime = duration;
    }

    public void fadeToWhite(float duration) {
        fading = true;
        startColor = alpha;
        endColor = white;
        maxTime = duration;
    }
    public void fadeFromWhite(float duration) {
        fading = true;
        startColor = white;
        endColor = alpha;
        maxTime = duration;
    }

    void winFade(GameObject ID) {
        gameObject.GetComponent<Image>().color = white;
        gameObject.GetComponent<Image>().CrossFadeAlpha(0, 0, true);
        gameObject.GetComponent<Image>().CrossFadeAlpha(1, 3, true);
    }
}
