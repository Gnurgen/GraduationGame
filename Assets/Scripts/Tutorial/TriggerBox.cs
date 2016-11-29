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
            mainCanvas.gameObject.SetActive(true);  
            mainCanvas.overrideSprite = tutorialPic;       
        }
    }
    public void closeTutorial() {
        mainCanvas.GetComponent<Animator>().Play("TutorialClose");
    }
    IEnumerator waitForAniStart()
    {
        yield return new WaitForSeconds(2);
        
    }
}
