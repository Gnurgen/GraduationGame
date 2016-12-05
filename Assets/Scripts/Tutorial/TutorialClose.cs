using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutorialClose : MonoBehaviour {
    private Vector3 positionscale;
    private InputManager IM;
    private int ID;

    void Start () {
        positionscale = new Vector3(1f, 0.2f, 0);
        StartCoroutine(waitForID());
        GameManager.events.MapGenerated();
       // gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if(IM != null)
            terminateTouch();
    }

    public void movingAnimation()
    {
        StartCoroutine(waitForAniStart());
        
    }
    IEnumerator waitForAniStart()
    {
        gameObject.transform.localScale -= Vector3.one * Time.deltaTime*0.8f;
        gameObject.transform.position += positionscale * Time.deltaTime;
        gameObject.GetComponent<Image>().CrossFadeAlpha(0, 2, false);
        yield return new WaitForSeconds(2);
        allowTouch();
        gameObject.transform.localScale = Vector3.one;
        gameObject.transform.localPosition = Vector3.zero;
        gameObject.GetComponent<Image>().CrossFadeAlpha(1, 0, false);
        gameObject.SetActive(false);

    }
    IEnumerator waitForID() {
        while (GameManager.input.GetID() == null)
            yield return null;
        ID = GameManager.input.GetID();
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
