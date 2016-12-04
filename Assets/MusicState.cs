using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class MusicState : MonoBehaviour {

    public string Scene;
    void Start()
    {
        StartCoroutine(VENTFISSE());
    }
	void OnLevelWasLoaded() // change music in menu/splash screens. Doesnt know when merging scenes
    {
        Scene =   SceneManager.GetActiveScene().name;
        print(Scene);
       
        if(Scene == "Menu")
        {
            AkSoundEngine.SetState("Game_State", "In_Main_Menu");
        }
        if(Scene == "LoadingScreen")
        {
            AkSoundEngine.SetState("Game_State", "In_Loading_Screen");
            if (GameManager.events != null)
            {
                GameManager.events.OnLoadComplete += SetGameState;
            }
        }

    }
    private void SetGameState()
    {
        Scene = SceneManager.GetActiveScene().name;
        AkSoundEngine.SetState("Game_State", "Out_Of_Battle");
    }
    IEnumerator VENTFISSE()
    {
        yield return new WaitForEndOfFrame();
        Scene = SceneManager.GetActiveScene().name;
        if (Scene == "Splash")
        {
            AkSoundEngine.SetState("Environment", "Small");
            AkSoundEngine.SetState("Game_State", "None");
            AkSoundEngine.PostEvent("Environmental_Ambience_Play", gameObject);
            AkSoundEngine.PostEvent("Music_System_Play", gameObject);
        }
    }
}
