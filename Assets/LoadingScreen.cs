using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class LoadingScreen : MonoBehaviour {

    public Image loadingProgress;
    bool loadComplete = false;
    float currentProgress;
    float mapProgress;
    float totalprogress = 1f; // REMEMBER TO CHANGE THIS FOR EVERY PROGRESS SEND
	// Use this for initialization
	void Start () {
        loadingProgress = GameObject.Find("LoadProgress").GetComponent<Image>();
        loadingProgress.fillAmount = 0f;
       
    }
	void Update()
    {
        if(GameManager.events != null)
        {
            GameManager.events.OnLoadingProgress += Loading;
            GameManager.events.OnLoadComplete += UnloadLoadingScene;
        }

    }


    private void Loading(float progress)
    {

        if (progress <= 1f) // Daniels MAPPROGRESS SAVED
        mapProgress = progress;
        
        if (progress > 2) // EVERYTHING ELSE
            currentProgress += .2f;

        loadingProgress.fillAmount = (currentProgress+mapProgress) / totalprogress;
        if(currentProgress+mapProgress >= totalprogress && !loadComplete)
        {
            loadComplete = true;
            print(currentProgress);
            GameManager.events.LoadComplete();
        }
    }
    private void UnloadLoadingScene()
    {
       SceneManager.UnloadScene("LoadingScreen");
    }
}
