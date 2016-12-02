using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class LoadingScreen : MonoBehaviour {

    public Image loadingProgress;
    bool loadComplete = false, subscribed = false, takeControl = false;
    float currentProgress;
    float mapProgress;
    float totalprogress = 1f; // REMEMBER TO CHANGE THIS FOR EVERY PROGRESS SEND
	// Use this for initialization
	void Start () {
        loadingProgress = GameObject.Find("LoadProgress").GetComponent<Image>();
        loadingProgress.fillAmount = 0f;

        if (GameManager.progress == 0)
        {
            StartCoroutine(tutLevel());
        }
        else if (GameManager.progress <= GameManager.numberOfLevels) // Number of levels before Boss level 
        {
            SceneManager.LoadSceneAsync("Final", LoadSceneMode.Additive);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("Final"));
        }
        else if (GameManager.progress > GameManager.numberOfLevels) // Boss Level
        {
            StartCoroutine(bossLevel());
        }
    }

    

    void Update()
    {
        if(GameManager.events != null && !subscribed)
        {
            GameManager.events.OnLoadingProgress += Loading;
            GameManager.events.OnLoadComplete += UnloadLoadingScene;
            subscribed = true;
        }
        if(GameManager.input != null && !takeControl)
        {
            GameManager.input.TakeControl(gameObject.GetInstanceID());
            takeControl = true;
        }
    }


    private void Loading(float loadingprogress) // FOR FINAL SCENE
    {
        if (loadingprogress <= 1f) // Daniels MAPPROGRESS SAVED
                mapProgress = loadingprogress;

                loadingProgress.fillAmount = (currentProgress + mapProgress) / totalprogress;
            if (currentProgress + mapProgress >= totalprogress && !loadComplete)
            {
                loadComplete = true;
                GameManager.events.LoadComplete();
            }
    }
    private void UnloadLoadingScene()
    {
       GameManager.input.ReleaseControl(gameObject.GetInstanceID());
       SceneManager.UnloadScene("LoadingScreen");
    }

    private IEnumerator tutLevel()
    {
        AsyncOperation AO = SceneManager.LoadSceneAsync("Tutorial", LoadSceneMode.Additive);
       
        while(AO.isDone == false)
        {
            float loading = Mathf.Clamp01(AO.progress / 0.9f);
            loadingProgress.fillAmount = loading;
            if(loading == 1)
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneByName("Tutorial"));
            }
            yield return null;
        }
        GameManager.events.LoadComplete();
        yield return null;
    }
    private IEnumerator bossLevel()
    {
       AsyncOperation AO =  SceneManager.LoadSceneAsync("BossLevel", LoadSceneMode.Additive);
       SceneManager.SetActiveScene(SceneManager.GetSceneByName("BossLevel"));

        yield return null;
    }

}
