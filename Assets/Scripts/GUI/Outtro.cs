using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class Outtro : MonoBehaviour {

    void Start()
    {
        StartCoroutine(next());
    }
    IEnumerator next()
    {
        Handheld.PlayFullScreenMovie("Outro_04.mp4", Color.black, FullScreenMovieControlMode.Hidden, FullScreenMovieScalingMode.AspectFit);
        Debug.Log("Now playing video file on android device (skipping video on unity play!)");
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(0.5f);
        clearData();
        SceneManager.LoadScene("Credits");
    }

    public void clearData()
    {
        GameManager.progress = 0;
        PlayerPrefs.SetInt("Progress", 0);
    }
}
