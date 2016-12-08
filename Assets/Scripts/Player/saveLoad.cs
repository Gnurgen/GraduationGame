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
        menu.SetActive(false);
        GameManager.time.SetTimeScale(1f);
        GameManager.events.DrawComplete(10);
        StartCoroutine(allowTouch(0.1f));
    }
    public void openTutorial() {
        tutorial.SetActive(true);
        menu.SetActive(false);
        StartCoroutine(allowTouch(0.1f));
        GameManager.time.SetTimeScale(1f);
    }
    public void openMenu()
    {      
        menu.SetActive(true);
        GameManager.time.SetTimeScale(0f);
        terminateTouch();
    }
    public void loadStartMenu() {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }
    void terminateTouch()
    {

        GameManager.input.TakeControl(ID);
    }

    IEnumerator allowTouch(float duration)
    {
        yield return new WaitForSeconds(duration);
        GameManager.input.ReleaseControl(ID);
    }
}
