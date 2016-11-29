using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TriggerBox : MonoBehaviour {
    public Sprite tutorialPic;
    private Image mainCanvas;

	void Start () {
        mainCanvas = GameObject.Find("Tutorial").GetComponent<Image>();
        mainCanvas.gameObject.SetActive(false);
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
