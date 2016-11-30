using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class Outtro : MonoBehaviour {
  
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
        yield return new WaitForSeconds(showTime);
        frames[1].GetComponent<Image>().CrossFadeAlpha(0, 1, true);
        yield return new WaitForSeconds(showTime);
        frames[2].GetComponent<Image>().CrossFadeAlpha(0, 1, true);
        yield return new WaitForSeconds(showTime);
        frames[3].GetComponent<Image>().CrossFadeAlpha(0, 1, true);
        yield return new WaitForSeconds(showTime);
        frames[4].GetComponent<Image>().CrossFadeAlpha(0, 1, true);
        yield return new WaitForSeconds(showTime);
        frames[5].GetComponent<Image>().CrossFadeAlpha(0, 1, true);
        yield return new WaitForSeconds(showTime);
        frames[6].GetComponent<Image>().CrossFadeAlpha(0, 1, true);
        yield return new WaitForSeconds(showTime);

        SceneManager.LoadScene("Menu");
    }
    void Update()
    {
        if (Input.touchCount > 1)
        {
            SkipCutscene();
        }
    }
    public void SkipCutscene()
    {
        SceneManager.LoadScene("Menu");
    }
}
