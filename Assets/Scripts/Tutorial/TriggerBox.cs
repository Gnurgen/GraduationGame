using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TriggerBox : MonoBehaviour {
    public int nr;
    public Sprite tutorialPic;
    public Image mainCanvas;
    TutorialClose tut;

    void OnTriggerEnter(Collider col)
    {      
        if (col.tag == "Player")
        {
            mainCanvas.gameObject.SetActive(true);  
            mainCanvas.overrideSprite = tutorialPic;
            GameObject.Find("TutorialText").GetComponent<Text>().text = GameObject.Find("Door").GetComponent<TutorialTexts>().getTextSnipped(nr);
            gameObject.SetActive(false);
        }
    }


}
