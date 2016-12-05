using UnityEngine;
using UnityEngine.UI;

using System.Collections;


public class Tutorial : MonoBehaviour {
    public Sprite[] tutorials;
    public GameObject prev, next;
    private int currentFrame = 0;

    void Start() {
        gameObject.GetComponent<Image>().overrideSprite = tutorials[0];
        prev.SetActive(false);

    }
    public void resetFrame(){
        currentFrame = 0;
        gameObject.GetComponent<Image>().overrideSprite = tutorials[0];
        prev.SetActive(false);
        next.SetActive(true);
    }


    public void nextFrame() {
        currentFrame++;
        Debug.Log(currentFrame);
        gameObject.GetComponent<Image>().overrideSprite = tutorials[currentFrame];        
        if (currentFrame != 0)
            prev.SetActive(true);
        if (currentFrame == tutorials.Length-1)
            next.SetActive(false);
    }

    public void prevFrame()
    {
        currentFrame--;
        Debug.Log(currentFrame);
        gameObject.GetComponent<Image>().overrideSprite = tutorials[currentFrame];
        if (currentFrame != tutorials.Length)
            next.SetActive(true);
        if(currentFrame == 0)
            prev.SetActive(false);
    }

    }
