using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SplashScreens : MonoBehaviour {
    public GameObject[] splashScreens;
    public GameObject fade;
    private float showTime = 2;
    private float fadingTime = 2;

    void Start() {
        StartCoroutine(splashing());
    }
    IEnumerator splashing()
    {
        fade.GetComponent<Image>().CrossFadeAlpha(0,2,true);
        yield return new WaitForSeconds(fadingTime+showTime);
        fade.GetComponent<Image>().CrossFadeAlpha(1, 2, true);
        yield return new WaitForSeconds(fadingTime);
        splashScreens[0].SetActive(false);

        fade.GetComponent<Image>().CrossFadeAlpha(0, 2, true);
        yield return new WaitForSeconds(fadingTime + showTime);
        fade.GetComponent<Image>().CrossFadeAlpha(1, 2, true);
        yield return new WaitForSeconds(fadingTime);
        splashScreens[1].SetActive(false);

        fade.GetComponent<Image>().CrossFadeAlpha(0, 2, true);
        yield return new WaitForSeconds(fadingTime + showTime);
        fade.GetComponent<Image>().CrossFadeAlpha(1, 2, true);
        yield return new WaitForSeconds(fadingTime);
        splashScreens[2].SetActive(false);
        Application.LoadLevel("Menu");
    }
}
