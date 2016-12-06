﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutorialClose : MonoBehaviour {
    private Vector3 positionscale;
    public InputManager IM;
    public int ID;
    float speed = 10f;
    float newPosX, newPosY;

    void Start () {
        positionscale = new Vector3(1f, 0.2f, 0);
        GameManager.events.MapGenerated();
        IM = GameManager.input;
        ID = IM.GetID();
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if(IM != null)
            terminateTouch();
    }

    public void movingAnimation()
    {
        StartCoroutine(waitForAniStart());
    }
    IEnumerator waitForAniStart()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        gameObject.GetComponent<Image>().CrossFadeAlpha(0, 2, true);
        
        while (gameObject.transform.localPosition.x < 650) {
            newPosX = gameObject.transform.localPosition.x + speed*1.5f;
            newPosY = gameObject.transform.localPosition.y + speed;
            gameObject.transform.localScale -= Vector3.one * 0.02f;
            gameObject.transform.localPosition = new Vector3(newPosX, newPosY, gameObject.transform.localPosition.z);
            yield return null;
        }
       // allowTouch();
        Debug.Log("Hi");
        gameObject.transform.localScale = Vector3.one;
        gameObject.transform.localPosition = Vector3.zero;
        gameObject.GetComponent<Image>().CrossFadeAlpha(1, 0, false);
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
        gameObject.SetActive(false);
        yield return null;

    }

    public void terminateTouch() {
        IM.TakeControl(ID);
    }

    public void allowTouch()
    {
        IM.ReleaseControl(ID);
    }


}
