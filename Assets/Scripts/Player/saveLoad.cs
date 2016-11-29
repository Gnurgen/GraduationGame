using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class saveLoad : MonoBehaviour {
    //Variables to be saved
    public GameObject menu;
    //Manager 
    InputManager IM;
    private int ID;

    void Awake() {
        IM = GameManager.input;
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
        Debug.Log("Terminate Touch");
        IM.TakeControl(ID);
    }

    IEnumerator allowTouch(float duration)
    {
        yield return new WaitForSeconds(duration);
        IM.ReleaseControl(ID);
    }
}
