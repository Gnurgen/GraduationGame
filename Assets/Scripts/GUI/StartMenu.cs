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
        //SceneManager.LoadScene("CutScene"); // NEEDS TO BE CORRECT SCENE!!!!!!!!!!!!!
    }

    IEnumerator playVideo()
    {
        Handheld.PlayFullScreenMovie("Sequence 01.mp4", Color.black, FullScreenMovieControlMode.CancelOnInput, FullScreenMovieScalingMode.AspectFill);
        Debug.Log("Now playing video file on android device (skipping video on unity play!)");
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        SceneManager.LoadScene("Tutorial"); // NEEDS TO BE CORRECT SCENE!!!!!!!!!!!!!

    }

    public void loadGame()
    {
        GameManager.progress = PlayerPrefs.GetInt("Progress");
        Time.timeScale = 1;
        if (GameManager.progress <= 2)
        {
            SceneManager.LoadScene("Final");
        }
        else
        {
            SceneManager.LoadScene("BossLevel");
        }
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
            GameManager.game.language = GameManager.Language.Danish;
        }
        else if (isDK == false)
        {
            Debug.Log("engelsk");
            language.GetComponent<Image>().overrideSprite = dK;
            isDK = true;
            GameManager.game.language = GameManager.Language.English;
        }
    }

    IEnumerator FadeinStartScreen()
    {
        fade.GetComponent<Image>().CrossFadeAlpha(0, 2, true);
        yield return new WaitForSeconds(showTime);
    }
}
