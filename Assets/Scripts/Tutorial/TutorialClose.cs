using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutorialClose : MonoBehaviour {
    private Vector3 positionscale;
    int ID;
    float speed = 20f;
    float newPosX, newPosY;

    void Start () {
        positionscale = new Vector3(1f, 0.2f, 0);
        GameManager.events.MapGenerated();
        ID = GameManager.input.GetID();
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if (GameManager.input != null)
        {
            terminateTouch();
            GameManager.time.SetTimeScale(0f);
        }
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
            gameObject.transform.localScale -= Vector3.one * 0.04f;
            gameObject.transform.localPosition = new Vector3(newPosX, newPosY, gameObject.transform.localPosition.z);
            yield return null;
        }
        allowTouch();
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
        GameManager.input.TakeControl(ID);
    }

    public void allowTouch()
    {
        GameManager.input.ReleaseControl(ID);
        GameManager.time.SetTimeScale(1f);
    }


}
