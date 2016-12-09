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
    public GameObject NewGame;
    public GameObject Load;
    public GameObject Shop;
    public GameObject Reset, Endless;
    [Header("Shop")]
    public GameObject skulls;
    public GameObject items;
    public GameObject content;
    public GameObject popup, popup2;
    public GameObject[] kranier;
    public GameObject popular, best;

    private float showTime = 2;

    void Start()
    {
        StartCoroutine(FadeinStartScreen());

        if (PlayerPrefs.GetInt("Progress") == 0)
            GameObject.Find("LoadGame").GetComponent<Button>().interactable = false;
        setLanguage();
        //setVibPref();
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

    public void PlayBoss()
    {
        GameManager.progress = 3;
        PlayerPrefs.SetInt("Progress", 3);
        GameManager.LoadNextLevel();
    }

    public void UseVibration()
    {
        GameManager.useVibrations = !GameManager.useVibrations;
        GameObject.FindObjectOfType<Toggle>().isOn = GameManager.useVibrations;
    }
    private void setVibPref()
    {
        GameObject.FindObjectOfType<Toggle>().isOn = true;
        GameManager.useVibrations = true;
    }
    IEnumerator playVideo()
    {
        Handheld.PlayFullScreenMovie("Intro_01.mp4", Color.black, FullScreenMovieControlMode.CancelOnInput, FullScreenMovieScalingMode.AspectFit);
        Debug.Log("Now playing video file on android device (skipping video on unity play!)");
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        AkSoundEngine.SetState("Game_State", "In_Loading_Screen_After_Intro_Cutscene");
        GameManager.LoadNextLevel();

    }
    public void IncreaseProgress()
    {
        GameManager.progress += 1;
        
        if (GameManager.progress < GameManager.numberOfLevels + 1)
        {
            GameObject.Find("Endless").GetComponentInChildren<Text>().text = "Level "+ GameManager.progress +" Progress";
            GameObject.Find("LoadGame").GetComponent<Button>().interactable = true;
        }
        else if (GameManager.progress == GameManager.numberOfLevels + 1)
        {
            GameObject.Find("Endless").GetComponentInChildren<Text>().text = "Boss Progress";
            GameObject.Find("LoadGame").GetComponent<Button>().interactable = true;
        }
        else if(GameManager.progress  > GameManager.numberOfLevels + 1)
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
    private void setLanguage()
    {
        if (GameManager.game.language == GameManager.Language.Danish)
            changeButtonsDK();
        else
            changeButtonsENG();
    }
    IEnumerator FadeinStartScreen()
    {
        fade.GetComponent<Image>().CrossFadeAlpha(0, 2, true);
        yield return new WaitForSeconds(showTime);
    }

    void changeButtonsDK() {
        NewGame.GetComponent<Text>().text = "NYT\nSPIL";
        Load.GetComponent<Text>().text = "HENT\nSPIL";
        music.GetComponent<Text>().text = "Musik";
        sound.GetComponent<Text>().text = "Lyd";
        lang.GetComponent<Text>().text = "Sprog";
        credits.GetComponent<Text>().text = "Rulletekster";
        instructions.GetComponent<Text>().text = "Instruktioner";
        Shop.GetComponent<Text>().text = "BUTIK";
        Reset.GetComponent<Text>().text = "NULSTIL";
        Endless.GetComponent<Text>().text = "EVIGT\nSPIL";
        skulls.GetComponent<Text>().text = "KRANIER";
        items.GetComponent<Text>().text = "GENSTANDE";
        content.GetComponent<Text>().text = "INDHOLD";
        popup.GetComponent<Text>().text = "DETTE ER IKKE EN RIGTIG BUTIK...";
        popup2.GetComponent<Text>().text = "DETTE INDHOLD ER IKKE BLEVET TILFØJET ENDNU...";
        foreach (GameObject i in kranier) {
        i.GetComponent<Text>().text = "Krystal Kranier";
        }
        best.GetComponent<Text>().text = "Best til Pris";
        popular.GetComponent<Text>().text = "Mest Solgte";
    }
    void changeButtonsENG()
    {
        NewGame.GetComponent<Text>().text = "NEW GAME";
        Load.GetComponent<Text>().text = "LOAD";
        music.GetComponent<Text>().text = "Music";
        sound.GetComponent<Text>().text = "Sound";
        lang.GetComponent<Text>().text = "Language";
        credits.GetComponent<Text>().text = "Credits";
        instructions.GetComponent<Text>().text = "Instructions";
        Shop.GetComponent<Text>().text = "SHOP";
        Reset.GetComponent<Text>().text = "RESET";
        Endless.GetComponent<Text>().text = "ENDLESS GAME";
        skulls.GetComponent<Text>().text = "SKULLS";
        items.GetComponent<Text>().text = "ITEMS";
        content.GetComponent<Text>().text = "CONTENT";
        popup.GetComponent<Text>().text = "THIS IS NOT A REAL SHOP...";
        popup.GetComponent<Text>().text = "THIS CONTENT HAS NOT YET BEEN ADDED...";
        foreach (GameObject i in kranier)
        {
            i.GetComponent<Text>().text = "Crystal Skulls";
        }
        best.GetComponent<Text>().text = "Best Value";
        popular.GetComponent<Text>().text = "Most Popular";

    }
}
