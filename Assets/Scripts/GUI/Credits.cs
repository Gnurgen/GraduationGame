using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Credits : MonoBehaviour {
    public GameObject sprite1, sprite2, theEnd, canvas;
    private float newPos;
    [SerializeField]
    private float speed = 5;
    private void Start()
    {
        Debug.Log("HI");
        sprite1.GetComponent<Image>().CrossFadeAlpha(0, 0, false);
        sprite2.GetComponent<Image>().CrossFadeAlpha(0, 0, false);
        theEnd.GetComponent<Image>().CrossFadeAlpha(0, 0, false);
    }
    public void startScrolling() {
        sprite1.GetComponent<Image>().CrossFadeAlpha(0, 0, false);
        sprite2.GetComponent<Image>().CrossFadeAlpha(0, 0, false);
        theEnd.GetComponent<Image>().CrossFadeAlpha(0, 0, false);

        StartCoroutine(scrolling());
    }
    IEnumerator scrolling() {
        gameObject.transform.localPosition = new Vector3(0, -500, 0);
        newPos = gameObject.transform.localPosition.y;
        while (newPos < 10901)
        {
            if (newPos > 7200)
            {
                fadeinSprite();
                
            }
       
            newPos += speed ;
            gameObject.transform.localPosition = new Vector3(0, newPos, 0);
            yield return null;
        }
        fadeoutSprite();
        yield return new WaitForSeconds(1.5f);
        theEnd.GetComponent<Image>().CrossFadeAlpha(1, 1, false);
        yield return new WaitForSeconds(1.1f);
        canvas.GetComponent<StartMenu>().selectMenu(0);
        theEnd.GetComponent<Image>().CrossFadeAlpha(0, 0, false);
        sprite1.GetComponent<Image>().CrossFadeAlpha(0, 0, false);
        sprite2.GetComponent<Image>().CrossFadeAlpha(0, 0, false);
        yield return null;
    }

    private void fadeinSprite() {
        sprite1.GetComponent<Image>().CrossFadeAlpha(1, 1, true);
        sprite2.GetComponent<Image>().CrossFadeAlpha(1, 1, true);
    }
    private void fadeoutSprite()
    {
        sprite1.GetComponent<Image>().CrossFadeAlpha(0, 1, true);
        sprite2.GetComponent<Image>().CrossFadeAlpha(0, 1, true);
    }
}
