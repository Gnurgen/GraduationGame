using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class LoadingScreen : MonoBehaviour {

    public Image loadingProgress;

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
        
        
        loadingProgress.fillAmount = progress;
        if(progress <= 1)
        {
            GameManager.events.LoadComplete();
        }
    }
    private void UnloadLoadingScene()
    {
        SceneManager.UnloadScene("LoadingScreen");
    }
}
