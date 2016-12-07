using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

public class SplashScreens : MonoBehaviour {
    public GameObject titleScreen;
    private float showTime = 5;
    private float fadingTime = 5;

    void Start()
    {
        StartCoroutine(PlaySplashVideo());
    }


    private IEnumerator PlaySplashVideo()
    {
        Handheld.PlayFullScreenMovie("Splash.mp4", Color.black, FullScreenMovieControlMode.Hidden, FullScreenMovieScalingMode.AspectFit);
        Debug.Log("Now playing video file on android device (skipping video on unity play!)");
        titleScreen.GetComponent<Image>().CrossFadeAlpha(0, 0, true);
        yield return new WaitForSeconds(1);
        titleScreen.GetComponent<Image>().CrossFadeAlpha(1, fadingTime, true);
        yield return new WaitForSeconds(showTime);
        SceneManager.LoadScene("Menu");
    }
}
