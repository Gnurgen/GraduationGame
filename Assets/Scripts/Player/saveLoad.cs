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
        Time.timeScale = 1;
        StartCoroutine(delayedRelease());
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
        Time.timeScale = 0;
        GameManager.input.TakeControl(ID);
    }
    public void loadStartMenu() {
        closeMenu();
        StartCoroutine(delayedLoad());
        
    }
    IEnumerator delayedRelease()
    {
        yield return new WaitForSeconds(Time.deltaTime);
        GameManager.input.ReleaseControl(ID);
    }
    IEnumerator delayedLoad()
    {
        yield return new WaitForSeconds(Time.deltaTime * 3f);
        SceneManager.LoadScene("Menu");
    }
}
