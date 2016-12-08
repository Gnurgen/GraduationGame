using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutorialRoomClear : MonoBehaviour {
    public int nr;
    public Sprite tutorialPic;
    public Image mainCanvas;

   
	void Update () {
        Health[] enemies = gameObject.transform.parent.gameObject.GetComponentsInChildren<Health>();
        if (enemies.Length == 0)
        {
            mainCanvas.gameObject.SetActive(true);
            mainCanvas.overrideSprite = tutorialPic;
            //GameObject.Find("TutorialText").GetComponent<Text>().text = GameObject.Find("Door").GetComponent<TutorialTexts>().getTextSnipped(nr);
            gameObject.SetActive(false);
        }
    }
}
