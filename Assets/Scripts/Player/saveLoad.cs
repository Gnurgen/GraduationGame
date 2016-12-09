using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class saveLoad : MonoBehaviour {
    //Variables to be saved
    public GameObject menu, tutorial;
    //Manager 
    private int ID;

    void Awake() {
        ID = GameManager.input.GetID();
    }

    public void save () {
        Debug.Log("Save");
        PlayerPrefs.SetInt("Progress", GameManager.progress);
        
	}

    public void load() {
        PlayerPrefs.GetInt("Progress", GameManager.progress);
        
    }

    public void closeMenu() {
        GameManager.events.MenuClose();
        //GameManager.time.SetTimeScale(1f);
        GameManager.events.DrawComplete(10);
        GameManager.input.ReleaseControl(ID);
        Time.timeScale = 1;
        menu.SetActive(false);
    }
    public void openTutorial() {
        tutorial.SetActive(true);
        tutorial.GetComponent<TutorialClose>().openTutorial();
        menu.SetActive(false);
    }
    public void openMenu()
    {      
        menu.SetActive(true);
        GameManager.events.MenuOpen();
        GameManager.time.SetTimeScale(0f);
        GameManager.input.TakeControl(ID);
    }
    public void loadStartMenu() {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }
}
