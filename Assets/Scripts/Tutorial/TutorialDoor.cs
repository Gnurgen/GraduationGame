﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutorialDoor : MonoBehaviour {
    float speed = 1;
    private float newPos;
    public Sprite tutorialPic;
    public Image mainCanvas;
    private InputManager IM;
    int ID;

    private void Start()
    {
        IM = GameManager.input;
        ID = IM.GetID();

    }

    void Update () {
        closeDoor();
    }


    public void closeDoor()
    {
        if (gameObject.transform.parent.transform.position.y < 0)
        {
            IM.TakeControl(ID);
            newPos = gameObject.transform.parent.transform.position.y + Time.deltaTime * speed;
            gameObject.transform.parent.transform.position = new Vector3(gameObject.transform.parent.transform.position.x, newPos, gameObject.transform.parent.transform.position.z);
        }
        else {
            IM.ReleaseControl(ID);
            mainCanvas.gameObject.SetActive(true);
            mainCanvas.overrideSprite = tutorialPic;            
            GameObject.Find("TutorialText").GetComponent<Text>().text = GameObject.Find("Door").GetComponent<TutorialTexts>().getTextSnipped(0);
            gameObject.SetActive(false);
        }
    }

}
