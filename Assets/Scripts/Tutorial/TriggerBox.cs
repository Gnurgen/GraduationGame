using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TriggerBox : MonoBehaviour {
    public Sprite tutorialPic;
    public Image mainCanvas;
    TutorialClose tut;


	void Start () {

    }
    void OnTriggerEnter(Collider col)
    {      
        if (col.tag == "Player")
        {
            Debug.Log("Hit player");
            mainCanvas.gameObject.SetActive(true);  
            mainCanvas.overrideSprite = tutorialPic;
            gameObject.SetActive(false);
        }
    }


}
