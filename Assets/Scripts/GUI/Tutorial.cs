using UnityEngine;
using UnityEngine.UI;

using System.Collections;


public class Tutorial : MonoBehaviour {
    public Sprite[] tutorials;
    public GameObject next, exit;
    private int currentFrame = 0;
    private GameObject tutText;

    void Start() {
        tutText = GameObject.Find("TutorialText");
        gameObject.GetComponent<Image>().overrideSprite = tutorials[0];
        tutText.GetComponent<Text>().text = gameObject.GetComponent<TutorialTexts>().getTextSnipped(0);
        exit.SetActive(false);

    }

    public void resetFrame(){
        currentFrame = 0;
        gameObject.GetComponent<Image>().overrideSprite = tutorials[0];
        tutText.GetComponent<Text>().text = gameObject.GetComponent<TutorialTexts>().getTextSnipped(0);
        next.SetActive(true);
        exit.SetActive(false);
    }


    public void nextFrame() {
            currentFrame++;
        if (currentFrame >= tutorials.Length)
        {
            currentFrame = 0;
        }
        Debug.Log(currentFrame);
        tutText.GetComponent<Text>().text = gameObject.GetComponent<TutorialTexts>().getTextSnipped(currentFrame);
        gameObject.GetComponent<Image>().overrideSprite = tutorials[currentFrame];        
        
        if (currentFrame == tutorials.Length - 1)
        {
            exit.SetActive(true);
        }
    }
    /*
    public void prevFrame()
    {
        currentFrame--;
        tutText.GetComponent<Text>().text = gameObject.GetComponent<TutorialTexts>().getTextSnipped(currentFrame);
        gameObject.GetComponent<Image>().overrideSprite = tutorials[currentFrame];
        if (currentFrame != tutorials.Length)
            next.SetActive(true);
        if(currentFrame == 0)
            prev.SetActive(false);
    }*/

    }
