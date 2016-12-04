using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutorialClose : MonoBehaviour {
    private bool clicked = false;
    private Vector3 positionscale;
    private InputManager IM;
    private int ID;

    void Start () {
        positionscale = new Vector3(1f, 0.2f, 0);
        IM = GameManager.input;
        ID = IM.GetID();
        GameManager.events.MapGenerated();
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if(IM != null)
            terminateTouch();
    }

    void Update () {
        if (clicked == true)
        {
            gameObject.transform.localScale -= Vector3.one * Time.deltaTime*0.8f;
            gameObject.transform.position += positionscale * Time.deltaTime;
            gameObject.GetComponent<Image>().CrossFadeAlpha(0, 2, false);
            StartCoroutine(waitForAniStart());
        }

    }
    public void movingAnimation()
    {
        clicked = true;
    }
    IEnumerator waitForAniStart()
    {
        yield return new WaitForSeconds(1);
        allowTouch();
        yield return new WaitForSeconds(1);
        clicked = false;
        gameObject.transform.localScale = Vector3.one;
        gameObject.transform.localPosition = Vector3.zero;
        gameObject.GetComponent<Image>().CrossFadeAlpha(1, 0, false);
        gameObject.SetActive(false);

    }

    public void terminateTouch()
    {
        IM.TakeControl(ID);
    }

    public void allowTouch()
    {
        IM.ReleaseControl(ID);
    }


}
