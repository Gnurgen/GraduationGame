using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TriggerBox : MonoBehaviour {
    public GameObject tutorialCanvas;
    TutorialClose tut;

    void OnTriggerEnter(Collider col)
    {      
        if (col.tag == "Player")
        {
            tutorialCanvas.SetActive(true);  
            gameObject.SetActive(false);
        }
    }


}
