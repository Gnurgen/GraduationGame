using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Fade : MonoBehaviour {

    Image blackImg;

    public Color black, alpha;
    private Color startColor, endColor;
    private float currentTime, maxTime, scaling;
    private bool fading = false;

    void Update() {
        if (fading == true && currentTime < maxTime)
        {
            currentTime += Time.deltaTime;
            scaling = (maxTime - (maxTime - currentTime)) / maxTime;
            blackImg.color = Color.Lerp(startColor, endColor, scaling);
        }
        else if (fading == true && currentTime >= maxTime)
            fading = false;

    }
    void Start() {
        blackImg = gameObject.GetComponent<Image>();
    }
    public void fadeToBlack(float duration) {
        fading = true;
        startColor = alpha;
        endColor = black;
        maxTime = duration;

    }
    public void fadeOut(float duration)
    {
        fading = true;
        startColor = black;
        endColor = alpha;
        maxTime = duration;
    }
}
