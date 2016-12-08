using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class FancyCredits : MonoBehaviour {
    public GameObject sprite1, sprite2, theEnd;
    private float newPos;
    [SerializeField]
    private float speed = 5;
    private void Start()
    {
        sprite1.GetComponent<Image>().CrossFadeAlpha(0, 0, false);
        sprite2.GetComponent<Image>().CrossFadeAlpha(0, 0, false);
        theEnd.GetComponent<Image>().CrossFadeAlpha(0, 0, false);
        startScrolling();
    }
    private void Update()
    {
        if (Input.GetMouseButton(0))
            speed = 50;
        else
            speed = 5;
    }
    
    public void startScrolling()
    {
        StartCoroutine(scrolling());
    }
    IEnumerator scrolling()
    {
        gameObject.transform.localPosition = new Vector3(0, -500, 0);
        newPos = gameObject.transform.localPosition.y;
        while (newPos < 10901)
        {
            if (newPos > 7200)
            {
                fadeinSprite();
            }
            newPos += speed;
            gameObject.transform.localPosition = new Vector3(0, newPos, 0);
            yield return null;
        }
        fadeoutSprite();
        yield return new WaitForSeconds(1.5f);
        theEnd.GetComponent<Image>().CrossFadeAlpha(1, 1, false);
        yield return new WaitForSeconds(1.1f);
        SceneManager.LoadScene(1);
        yield return null;
    }

    private void fadeinSprite()
    {
        sprite1.GetComponent<Image>().CrossFadeAlpha(1, 1, true);
        sprite2.GetComponent<Image>().CrossFadeAlpha(1, 1, true);
    }
    private void fadeoutSprite()
    {
        sprite1.GetComponent<Image>().CrossFadeAlpha(0, 1, true);
        sprite2.GetComponent<Image>().CrossFadeAlpha(0, 1, true);
    }
}
