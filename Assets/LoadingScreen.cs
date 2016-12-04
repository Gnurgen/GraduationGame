using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class LoadingScreen : MonoBehaviour {

    string LoadToScene;
    public Image loadingProgress;
    bool loadComplete = false, subscribed = false, takeControl = false;
    float currentProgress;
    public bool MapGenerated = false;
    float mapProgress;
    float totalprogress = 1f; // REMEMBER TO CHANGE THIS FOR EVERY PROGRESS SEND
	// Use this for initialization
	void Start () {
        loadingProgress = GameObject.Find("LoadProgress").GetComponent<Image>();
        loadingProgress.fillAmount = 0f;

        if (GameManager.progress == 0)
        {
            LoadToScene = "Tutorial"; 
            StartCoroutine(tutLevel());
        }
        else if (GameManager.progress <= GameManager.numberOfLevels) // Number of levels before Boss level 
        {
            LoadToScene = "Final";
            SceneManager.LoadSceneAsync("Final", LoadSceneMode.Additive);
           
            //SceneManager.MoveGameObjectToScene(GameManager.events.gameObject, SceneManager.GetSceneByName("Final"));
        }
        else  // Boss Level
        {
            LoadToScene = "BossLevel";
            StartCoroutine(tutLevel());
        }
    }

    

    void Update()
    {
        if(GameManager.events != null && !subscribed)
        {
            GameManager.events.OnLoadingProgress += Loading;
            GameManager.events.OnLoadComplete += UnloadLoadingScene;
            GameManager.events.OnMapGenerated += ExtraSecurity;
            subscribed = true;
        }
        if(GameManager.input != null && !takeControl)
        {
            GameManager.input.TakeControl(gameObject.GetInstanceID());
            takeControl = true;
        }
    }

    private void ExtraSecurity()
    {
        MapGenerated = true;
    }

    private void Loading(float loadingprogress) // FOR FINAL SCENE
    {
        if (loadingprogress <= 1f) // Daniels MAPPROGRESS SAVED
                mapProgress = loadingprogress;

                loadingProgress.fillAmount = (currentProgress + mapProgress) / totalprogress;
            if (currentProgress + mapProgress >= totalprogress && !loadComplete && MapGenerated)
            {
                loadComplete = true;
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(LoadToScene));
            GameManager.events.LoadComplete();
            }
    }
    private void UnloadLoadingScene()
    {
        GameManager.input.ReleaseControl(gameObject.GetInstanceID());
        GameManager.events.FadeFromBlackToTransparent();
        SceneManager.MergeScenes(SceneManager.GetSceneByName("LoadingScreen"), SceneManager.GetSceneByName(LoadToScene));
        Destroy(GameObject.Find("LoadingCamera"));
        Destroy(gameObject);
    }

    private IEnumerator tutLevel() //Tutorial level loading
    {
        AsyncOperation AO = SceneManager.LoadSceneAsync(LoadToScene, LoadSceneMode.Additive);
        //SceneManager.MoveGameObjectToScene(GameManager.events.gameObject, SceneManager.GetSceneByName("Tutorial"));
        while (AO.isDone == false)
        {
            float loading = Mathf.Clamp01(AO.progress / 0.9f);
            loadingProgress.fillAmount = loading;
            if(loading == 1)
            {
                AO.allowSceneActivation = true;
            }
            yield return null;
        }
        GameManager.events.LoadComplete();
        yield return null;
    }
}
