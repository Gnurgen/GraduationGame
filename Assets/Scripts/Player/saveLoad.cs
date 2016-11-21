using UnityEngine;
using System.Collections;

public class saveLoad : MonoBehaviour {
    //Variables to be saved
    Vector3 playerPos;
    float playerHealth;
    int playerLVL;
    float playerXP;
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
        //Get all variables 
        //~~~~~~~~~~~~~~~~~~~~Need to get the varibales from other scripts~~~~~~~~~~~~

        //Save stats to PlayerPrefs
        PlayerPrefs.SetFloat("Player health", playerHealth);
        PlayerPrefs.SetFloat("Player XP", playerXP);
        PlayerPrefs.SetFloat("Player Position X", playerPos.x);
        PlayerPrefs.SetFloat("Player Position Y", playerPos.y);
        PlayerPrefs.SetFloat("Player Position Z", playerPos.z);
        PlayerPrefs.SetInt("Player Level", playerLVL);
        
	}

    public void load() {
        Debug.Log("Load");
        print(PlayerPrefs.GetFloat("Player health"));
        print(PlayerPrefs.GetFloat("Player XP"));
        print(new Vector3(PlayerPrefs.GetFloat("Player Position X"), PlayerPrefs.GetFloat("Player Position Y"), PlayerPrefs.GetFloat("Player Position Z")));
        print(PlayerPrefs.GetInt("Player Level"));
        
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
        Application.LoadLevel("Menu");
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
