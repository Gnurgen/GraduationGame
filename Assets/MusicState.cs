using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

public class MusicState : MonoBehaviour {

    public string Scene;
    void Start()
    {
        Application.targetFrameRate = 30;
        StartCoroutine(AtStartOfGame());
    }
	void OnLevelWasLoaded() // change music in menu/splash screens. Doesnt know when merging scenes
    {
        
        Scene =   SceneManager.GetActiveScene().name;
        if(Scene == "Menu")
        {
            AkSoundEngine.StopAll();
            AkSoundEngine.PostEvent("Environmental_Ambience_Play", gameObject);
            AkSoundEngine.PostEvent("Music_System_Play", gameObject);
            AkSoundEngine.SetState("Game_State", "In_Main_Menu");
            AkSoundEngine.RenderAudio();
        }
    
        if (Scene == "LoadingScreen")
        {
            AkSoundEngine.StopAll();
            AkSoundEngine.SetState("Game_State", "In_Loading_Screen");
            AkSoundEngine.PostEvent("Environmental_Ambience_Play", gameObject);
            AkSoundEngine.PostEvent("Music_System_Play", gameObject);
            if (GameManager.events != null)
            {
                GameManager.events.OnLoadComplete += SetGameState;
            }
        }
        

    }
    private void SetGameState()
    {
        StartCoroutine(WaitAFrameForSceneName());
    
    }

    private IEnumerator WaitAFrameForSceneName()
    {
        yield return new WaitForEndOfFrame();
        Scene = SceneManager.GetActiveScene().name;
        if (Scene == "BossLevel")
        {
            AkSoundEngine.SetState("Game_State", "In_Loading_Screen_After_Intro_Cutscene");
            AkSoundEngine.SetState("Environment", "Large");
        }
        else
        {
            AkSoundEngine.SetState("Game_State", "Out_Of_Battle");
            AkSoundEngine.SetState("Environment", "Small");
        }
    }

    IEnumerator AtStartOfGame()
    {
        yield return new WaitForEndOfFrame();
        Scene = SceneManager.GetActiveScene().name;
        if (Scene == "Splash")
        {
            AkSoundEngine.SetState("Environment", "Small");
            AkSoundEngine.SetState("Game_State", "In_Splash");
            AkSoundEngine.PostEvent("Environmental_Ambience_Play", gameObject);
            AkSoundEngine.PostEvent("Music_System_Play", gameObject);
            AkSoundEngine.RenderAudio();
        }
    }

    public void MenuButton()
    {
        AkSoundEngine.PostEvent("Button_Menu_Play", gameObject);
        AkSoundEngine.RenderAudio();
    }
    public void MenuStartGameButton()
    {
        AkSoundEngine.PostEvent("Button_Start_Game_Play", gameObject);
        AkSoundEngine.RenderAudio();
    }

}
