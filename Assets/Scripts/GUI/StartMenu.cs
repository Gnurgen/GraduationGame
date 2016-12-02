using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour {
    public GameObject[] listOfMenues;
    public bool isDK = true;
    public Sprite dK, eN;
    public  GameObject language;
    public GameObject fade;

    private float showTime = 2;

    void Start()
    {
        StartCoroutine(FadeinStartScreen());
        if (PlayerPrefs.GetInt("Progress") == 0)
            GameObject.Find("LoadGame").GetComponent<Button>().interactable = false;
    }

    public void ResetProgress()
    {
        GameManager.progress = 0;
        PlayerPrefs.SetInt("Progress", 0);
        GameObject.Find("LoadGame").GetComponent<Button>().interactable = false;
    }

    public void newGame() {
        ResetProgress();
        Time.timeScale = 1;
        StartCoroutine(playVideo());
     
    }

    IEnumerator playVideo()
    {
        Handheld.PlayFullScreenMovie("Sequence 01.mp4", Color.black, FullScreenMovieControlMode.CancelOnInput, FullScreenMovieScalingMode.AspectFill);
        Debug.Log("Now playing video file on android device (skipping video on unity play!)");
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        GameManager.LoadNextLevel();

    }

    public void loadGame()
    {
        Time.timeScale = 1;
        GameManager.LoadNextLevel();
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

    IEnumerator FadeinStartScreen()
    {
        fade.GetComponent<Image>().CrossFadeAlpha(0, 2, true);
        yield return new WaitForSeconds(showTime);
    }
}
