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

    void Update()
    {
        if(Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {
            SceneManager.LoadScene("Menu");
        }
    }

    private IEnumerator PlaySplashVideo()
    {
        titleScreen.GetComponent<Image>().CrossFadeAlpha(0, 0, true);
        Handheld.PlayFullScreenMovie("Splash.mp4", Color.black, FullScreenMovieControlMode.Hidden, FullScreenMovieScalingMode.AspectFit);
        Debug.Log("Now playing video file on android device (skipping video on unity play!)");
        yield return new WaitForSeconds(1);
        titleScreen.GetComponent<Image>().CrossFadeAlpha(1, fadingTime, true);
        AkSoundEngine.SetState("Game_State", "In_Main_Menu");
        yield return new WaitForSeconds(showTime);
        SceneManager.LoadScene("Menu");
    }
}
