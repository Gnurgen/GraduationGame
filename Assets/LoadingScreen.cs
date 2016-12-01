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
        }

    }

    private void Loading(float progress)
    {
        loadingProgress.fillAmount = progress;
    }
}
