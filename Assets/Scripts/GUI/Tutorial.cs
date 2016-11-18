using UnityEngine;
using UnityEngine.UI;

using System.Collections;


public class Tutorial : MonoBehaviour {
    public GameObject[] tutorials;
    public GameObject prev, next;
    private int currentFrame = 0;

    void Start() {
        for (int i = 0; i < tutorials.Length; i++)
            if (i != 0)
                tutorials[i].SetActive(false);
        prev.SetActive(false);

    }
    public void resetFrame(){
        currentFrame = 0;
        tutorials[0].SetActive(true);
        for (int i = 0; i < tutorials.Length; i++)
            if (i != 0)
                tutorials[i].SetActive(false);
        prev.SetActive(false);
        next.SetActive(true);
    }


    public void nextFrame() {
        currentFrame++;
        Debug.Log(currentFrame);
        tutorials[currentFrame-1].SetActive(false);
        tutorials[currentFrame].SetActive(true);
        if (currentFrame != 0)
            prev.SetActive(true);
        if (currentFrame == tutorials.Length-1)
            next.SetActive(false);
    }

    public void prevFrame()
    {
        currentFrame--;
        Debug.Log(currentFrame);
        tutorials[currentFrame+1].SetActive(false);
        tutorials[currentFrame].SetActive(true);
        if (currentFrame != tutorials.Length)
            next.SetActive(true);
        if(currentFrame == 0)
            prev.SetActive(false);
    }

    }
