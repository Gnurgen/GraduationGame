using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour {
    public GameObject[] listOfMenues;
    public bool isDK = true;
    public Sprite dK, eN;
    public  GameObject language;
    public GameObject fade;

    private float showTime = 2;

    void Start()
    {
        StartCoroutine(FadeinStartScreen());
    }

    public void newGame() {
        SceneManager.LoadScene("CutScene"); // NEEDS TO BE CORRECT SCENE!!!!!!!!!!!!!
    }

    public void loadGame() {
        SceneManager.LoadScene("KrisTester"); // NEEDS TO BE CORRECT SCENE!!!!!!!!!!!!!
    }

    public void selectMenu(int menu) {
        for (int i = 0; i < listOfMenues.Length; i++) {
            if (i != menu)
                listOfMenues[i].SetActive(false);
            else
                listOfMenues[i].SetActive(true);
        }
    }
    public void changeLanguage() {
        if (isDK == true)
        {
            Debug.Log("dansk");
            language.GetComponent<Image>().overrideSprite = eN;
            isDK = false;
        }
        else if (isDK == false)
        {
            Debug.Log("engelsk");
            language.GetComponent<Image>().overrideSprite = dK;
            isDK = true;
        }
    }

    IEnumerator FadeinStartScreen()
    {
        fade.GetComponent<Image>().CrossFadeAlpha(0, 2, true);
        yield return new WaitForSeconds(showTime);
    }
}
