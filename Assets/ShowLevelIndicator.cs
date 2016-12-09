using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class ShowLevelIndicator : MonoBehaviour {

    [SerializeField]
    private Sprite[] imglvl = new Sprite[4];
    Image img;
    [SerializeField]
    float fadeTime = 1;
    int id;
    // Use this for initialization
    void Start () {
        img = GetComponent<Image>();
        GameManager.events.OnLoadComplete += ShowLvlIndicator;
        GameManager.events.OnElevatorMoveStop += HideLvlIndicator;
        transform.parent.Find("Options").GetComponent<Button>().interactable = false;
    }
    void Update()
    {
        
    }

    private void HideLvlIndicator()
    {
        StartCoroutine(FadeImg(fadeTime));
    }

    private IEnumerator FadeImg(float fadeTime)
    {
        if(PlayerPrefs.GetInt("Progress") > 0)
        {
            StartCoroutine(DelayedRelease(0));
        }
        else
        {
            StartCoroutine(DelayedRelease(2));
        }
        float step = 1;
        float c = 200f / 255f;
        Color col; 
        while (step > 0)
        {
            step -= (1/fadeTime) * Time.deltaTime;
            col = new Color(c, c, c, step);
            img.color = col;
            yield return null;
        }
        img.enabled = false;
    }

    IEnumerator DelayedRelease(float time)
    {
        if(time > 0)
            yield return new WaitForSeconds(time);
        while(!GameManager.input.ReleaseControl(id))
        {
            yield return null;
        }
        transform.parent.Find("Options").GetComponent<Button>().interactable = true;
    }

    IEnumerator TakeControl()
    {
        id = GameManager.input.GetID();
        while(!GameManager.input.TakeControl(id))
        {
            yield return null;
        }
    }

    private void ShowLvlIndicator()
    {
        StartCoroutine(TakeControl());
        img.sprite = imglvl[PlayerPrefs.GetInt("Progress")];
        img.enabled = true;
        if (PlayerPrefs.GetInt("Progress") == 0)
        {
            StartCoroutine(FadeImg(fadeTime+3));
        }
    }
}
