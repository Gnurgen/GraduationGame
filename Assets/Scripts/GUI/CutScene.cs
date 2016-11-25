using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class CutScene : MonoBehaviour {
    public GameObject[] frames;
    public GameObject fade;
    private float showTime = 2;
    private float fadingTime = 2;

    void Start()
    {
        StartCoroutine(next());
    }
    IEnumerator next()
    {
        frames[0].GetComponent<Image>().CrossFadeAlpha(0, 1, true);
        yield return new WaitForSeconds(1);
        frames[1].GetComponent<Image>().CrossFadeAlpha(0, 1, true);
        yield return new WaitForSeconds(1);
        frames[2].GetComponent<Image>().CrossFadeAlpha(0, 1, true);
        yield return new WaitForSeconds(1);
        frames[3].GetComponent<Image>().CrossFadeAlpha(0, 1, true);
        yield return new WaitForSeconds(1);
        frames[4].GetComponent<Image>().CrossFadeAlpha(0, 1, true);
        yield return new WaitForSeconds(1);
        frames[5].GetComponent<Image>().CrossFadeAlpha(0, 1, true);
        yield return new WaitForSeconds(1);
        frames[6].GetComponent<Image>().CrossFadeAlpha(0, 1, true);
        yield return new WaitForSeconds(1);
        frames[7].GetComponent<Image>().CrossFadeAlpha(0, 1, true);
        yield return new WaitForSeconds(3);
        //PAUSE
        fade.GetComponent<Image>().CrossFadeAlpha(0, 1, true);
        yield return new WaitForSeconds(1);
        //SCENE 2 START
        frames[8].GetComponent<Image>().CrossFadeAlpha(0, 1, true);
        yield return new WaitForSeconds(1);
        frames[9].GetComponent<Image>().CrossFadeAlpha(0, 1, true);
        yield return new WaitForSeconds(1);
        frames[10].GetComponent<Image>().CrossFadeAlpha(0, 1, true);
        yield return new WaitForSeconds(1);
        frames[11].GetComponent<Image>().CrossFadeAlpha(0, 1, true);
        yield return new WaitForSeconds(1);
        frames[12].GetComponent<Image>().CrossFadeAlpha(0, 1, true);
        yield return new WaitForSeconds(1);
        frames[13].GetComponent<Image>().CrossFadeAlpha(0, 1, true);
        yield return new WaitForSeconds(3);

        //SCENE 3 START
        frames[14].GetComponent<Image>().CrossFadeAlpha(0, 1, true);
        yield return new WaitForSeconds(1);
        frames[15].GetComponent<Image>().CrossFadeAlpha(0, 1, true);
        yield return new WaitForSeconds(1);
        frames[16].GetComponent<Image>().CrossFadeAlpha(0, 1, true);
        yield return new WaitForSeconds(1);
        frames[17].GetComponent<Image>().CrossFadeAlpha(0, 1, true);
        yield return new WaitForSeconds(1);
        frames[18].GetComponent<Image>().CrossFadeAlpha(0, 1, true);
        yield return new WaitForSeconds(1);
        frames[19].GetComponent<Image>().CrossFadeAlpha(0, 1, true);
        yield return new WaitForSeconds(3);
        frames[20].GetComponent<Image>().CrossFadeAlpha(0, 1, true);
        yield return new WaitForSeconds(1);
        frames[21].GetComponent<Image>().CrossFadeAlpha(0, 1, true);
        yield return new WaitForSeconds(3);

        //SCENE 4 START
        frames[22].GetComponent<Image>().CrossFadeAlpha(0, 1, true);
        yield return new WaitForSeconds(1);
        frames[23].GetComponent<Image>().CrossFadeAlpha(0, 1, true);
        yield return new WaitForSeconds(3);
        frames[24].GetComponent<Image>().CrossFadeAlpha(0, 1, true);
        yield return new WaitForSeconds(4);

        SceneManager.LoadScene("Final");
    }
    void Update()
    {
        if(Input.touchCount>1)
        {
            SkipCutscene();
        }
    }
    public void SkipCutscene()
    {
        SceneManager.LoadScene("Final");
    }
}
