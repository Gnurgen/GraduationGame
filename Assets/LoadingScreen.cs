using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class LoadingScreen : MonoBehaviour {

    string LoadToScene;
    private int IMID = 0;
    public Image loadingProgress;
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
        StartCoroutine(OVERLOAD());
        loadingProgress = GameObject.Find("LoadProgress").GetComponent<Image>();
        loadingProgress.fillAmount = 0f;
        LoadingText.text = "L O A D I N G . . .";
        if (GameManager.progress == 0)
        {
            ImgAni.enabled = false;
            LoadToScene = "Tutorial"; 
            StartCoroutine(tutLevel());
        }
        else if (GameManager.progress <= GameManager.numberOfLevels) // Number of levels before Boss level 
        {
            saFrame = 0;
            ImgAni.enabled = true;
            ImgAni.sprite = ShuffleAnimation[saFrame];
            InfoText.text = "S H U F F L I N G    C H A M B E R S";
            LoadToScene = "Final";
            SceneManager.LoadSceneAsync("Final", LoadSceneMode.Additive);
           
            //SceneManager.MoveGameObjectToScene(GameManager.events.gameObject, SceneManager.GetSceneByName("Final"));
        }
        else  // Boss Level
        {
            ImgAni.enabled = false;
            LoadToScene = "BossLevel";
            StartCoroutine(tutLevel());
        }
    }

    private IEnumerator OVERLOAD()
    {
        LoadingText.text = "L O A D I N G . . .";
        while(LoadingText.text.Length < 100)
        {
            if(LoadingText.text.Length % 2 == 1)
                LoadingText.text.Insert(LoadingText.text.Length, ".");
            else
                LoadingText.text.Insert(LoadingText.text.Length, " ");
            yield return null;
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

    private void Loading(float loadingprogress) // FOR FINAL SCENE
    {
       
        if (loadingprogress <= 1f) // Daniels MAPPROGRESS SAVED
                mapProgress = loadingprogress;

        loadingProgress.fillAmount = mapProgress / totalprogress;

        if (mapProgress >= totalprogress && !loadComplete && MapGenerated)
        {
            loadComplete = true;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(LoadToScene));
        StartCoroutine(waitingForInput(LoadToScene == "Final"));
        }

        saFrame++;
        if (saFrame >= ShuffleAnimation.Length)
            saFrame = 0;
        ImgAni.sprite = ShuffleAnimation[saFrame];

    }
    
    private IEnumerator waitingForInput(bool animation)
    {
        print("ani: " + animation);

        LoadingText.text = "T O U C H   T O   C O N T I N U E";
        InfoText.text = "C H A M B E R S   S H U F F L E D";
        while (Input.touchCount < 1 && !Input.GetKeyDown(KeyCode.Space))
        {
            if (animation)
            {
                if (saFrame >= ShuffleAnimation.Length - 1)
                    saFrame = 0;
                else
                    saFrame++;
                ImgAni.sprite = ShuffleAnimation[saFrame];
                yield return new WaitForSeconds(0.0625f);
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

    private IEnumerator tutLevel() //Tutorial level loading
    {
        AsyncOperation AO = SceneManager.LoadSceneAsync(LoadToScene, LoadSceneMode.Additive);
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
        StartCoroutine(waitingForInput(LoadToScene == "Final"));
        yield return null;
    }
}
