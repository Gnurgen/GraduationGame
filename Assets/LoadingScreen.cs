using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class LoadingScreen : MonoBehaviour {

    string LoadToScene;
    private int IMID = 0;
    public Image loadingProgress, loadingUnityProgress;
    bool loadComplete = false, subscribed = false, takeControl = false;
    public bool MapGenerated = false;
    float mapProgress;
    float totalprogress = 1f; // REMEMBER TO CHANGE THIS FOR EVERY PROGRESS SEND

    // animation variables
    public Text LoadingText;
    public Text InfoText;
    public Image ImgAni;
    public Sprite[] ShuffleAnimation = new Sprite[8];
    private int saFrame = 0; 
	// Use this for initialization
	void Start () {
      
        loadingProgress = GameObject.Find("LoadProgress").GetComponent<Image>();
        loadingProgress.fillAmount = 0f;
        LoadingText.text = "L O A D I N G . . .";
        if (GameManager.progress == 0)
        {
            
            LoadToScene = "Tutorial"; 
            StartCoroutine(LoadLevel(false));
        }
        else if (GameManager.progress <= GameManager.numberOfLevels) // Number of levels before Boss level 
        {
            saFrame = 0;
           
            ImgAni.sprite = ShuffleAnimation[saFrame];
            LoadToScene = "Final";
            StartCoroutine(LoadLevel(true));
            //SceneManager.LoadSceneAsync("Final", LoadSceneMode.Additive);
           
            //SceneManager.MoveGameObjectToScene(GameManager.events.gameObject, SceneManager.GetSceneByName("Final"));
        }
        else  // Boss Level
        {
           
            LoadToScene = "BossLevel";
            StartCoroutine(LoadLevel(false));
        }
    }



    void Update()
    {
        if(GameManager.events != null && !subscribed)
        {
            GameManager.events.OnLoadingProgress += Loading;
            GameManager.events.OnMapGenerated += ExtraSecurity;
            subscribed = true;
        }
        if(GameManager.input != null && !takeControl)
        {
            IMID = GameManager.input.GetID(); 
            GameManager.input.TakeControl(IMID);
            takeControl = true;
        }
    }

    private void ExtraSecurity()
    {
        MapGenerated = true;
    }

    private void Loading(float loadp) // FOR FINAL SCENE
    {
        ImgAni.fillAmount = 1f;
        // Daniels MAPPROGRESS SAVED
        mapProgress = loadp;

        loadingProgress.fillAmount = mapProgress / totalprogress;

        if (mapProgress >= totalprogress && !loadComplete && MapGenerated)
        {
            loadComplete = true;
        }

        saFrame++;
        if (saFrame >= ShuffleAnimation.Length)
            saFrame = 0;
        ImgAni.sprite = ShuffleAnimation[saFrame];

    }

    private IEnumerator LoadLevel(bool LoadMap) //Loading Level (LoadMap = true, for Final scene)
    {
        AsyncOperation AO = SceneManager.LoadSceneAsync(LoadToScene, LoadSceneMode.Additive);
        while (AO.isDone == false)
        {
            float loading = Mathf.Clamp01(AO.progress / 0.9f);
            loadingUnityProgress.fillAmount = loading;
            ImgAni.fillAmount = loading;
            if(loading == 1)
            {
                while(LoadMap && mapProgress < totalprogress)
                {
                    InfoText.text = "S H U F F L I N G    C H A M B E R S";
                    yield return null;
                }
                AO.allowSceneActivation = true;
            }
            yield return new WaitForFixedUpdate();
        }
        StartCoroutine(waitingForInput(LoadToScene == "Final"));
        yield return null;
    }

    private IEnumerator waitingForInput(bool finalScene)
    {
       
        LoadingText.text = "T O U C H   T O   C O N T I N U E";
        if(finalScene)
            InfoText.text = "C H A M B E R S   S H U F F L E D";
        while (Input.touchCount < 1 && !Input.GetKey(KeyCode.Mouse0))
        {
            if (Input.touchCount < 1 && !Input.GetKey(KeyCode.Mouse0))
            {
                if (saFrame >= ShuffleAnimation.Length - 1)
                    saFrame = 0;
                else
                    saFrame++;
                ImgAni.sprite = ShuffleAnimation[saFrame];
                yield return new WaitForSeconds(0.125f);
            }
            yield return null;
        }
      
        GameManager.events.LoadComplete();
        GameManager.input.ReleaseControl(IMID);
        GameManager.events.FadeFromBlackToTransparent();
        SceneManager.MergeScenes(SceneManager.GetSceneByName("LoadingScreen"), SceneManager.GetSceneByName(LoadToScene));
        yield return new WaitForEndOfFrame();
        Resources.UnloadUnusedAssets();
        Destroy(GameObject.Find("LoadingCamera"));
        Destroy(gameObject);
        yield break;
    }
}
