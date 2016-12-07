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
     [Header("Text")]
    public GameObject music;
    public GameObject sound, lang, credits, instructions;
    [Header("Start Menu")]
    public GameObject Shop;
    public GameObject Reset, Endless;

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
        Resources.UnloadUnusedAssets();
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
    public void IncreaseProgress()
    {
        GameManager.progress += 1;
        
        if (GameManager.progress < GameManager.numberOfLevels)
        {
            GameObject.Find("Endless").GetComponentInChildren<Text>().text = "Level "+ GameManager.progress +" Progress";
            GameObject.Find("LoadGame").GetComponent<Button>().interactable = true;
        }
        else if (GameManager.progress == GameManager.numberOfLevels)
        {
            GameObject.Find("Endless").GetComponentInChildren<Text>().text = "Boss Progress";
            GameObject.Find("LoadGame").GetComponent<Button>().interactable = true;
        }
        else if(GameManager.progress > GameManager.numberOfLevels)
        {
            GameObject.Find("Endless").GetComponentInChildren<Text>().text = "Tutorial Progress";
            GameObject.Find("LoadGame").GetComponent<Button>().interactable = false;
            GameManager.progress = 0;
        }
        PlayerPrefs.SetInt("Progress", GameManager.progress);
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
            changeButtonsDK();
            language.GetComponent<Image>().overrideSprite = dK;
            isDK = false;
            GameManager.game.language = GameManager.Language.Danish;
        }
        else if (isDK == false)
        {
            changeButtonsENG();
            language.GetComponent<Image>().overrideSprite = eN;
            isDK = true;
            GameManager.game.language = GameManager.Language.English;
        }
    }

    IEnumerator FadeinStartScreen()
    {
        fade.GetComponent<Image>().CrossFadeAlpha(0, 2, true);
        yield return new WaitForSeconds(showTime);
    }

    void changeButtonsDK() {
        music.GetComponent<Text>().text = "Musik";
        sound.GetComponent<Text>().text = "Lyd";
        lang.GetComponent<Text>().text = "Sprog";
        credits.GetComponent<Text>().text = "Rulletekster";
        instructions.GetComponent<Text>().text = "Instruktioner";
        Shop.GetComponent<Text>().text = "BUTIK";
        Reset.GetComponent<Text>().text = "NULSTIL";
        Endless.GetComponent<Text>().text = "ENDELØS UDFORDRING";
    }
    void changeButtonsENG()
    {
        music.GetComponent<Text>().text = "Music";
        sound.GetComponent<Text>().text = "Sound";
        lang.GetComponent<Text>().text = "Language";
        credits.GetComponent<Text>().text = "Credits";
        instructions.GetComponent<Text>().text = "Instructions";
        Shop.GetComponent<Text>().text = "SHOP";
        Reset.GetComponent<Text>().text = "RESET";
        Endless.GetComponent<Text>().text = "ENDLESS CHALLENGE";
    }
}
