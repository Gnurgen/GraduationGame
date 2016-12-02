using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Win : MonoBehaviour {

    void Start(){
        GameManager.events.OnBossDeath += loadStartManu;
        GameManager.events.OnBossDeath += removeGUI;
    }

 
    void loadStartManu(GameObject ID)
    {
        StartCoroutine(waitBeforeLoad());
    }
    void removeGUI(GameObject ID) {
        GameObject.Find("PlayerHealthBar").SetActive(false);
        GameObject.Find("BossHealthBar").SetActive(false);
        GameObject.Find("Options").SetActive(false);

    }
    IEnumerator waitBeforeLoad() {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("Outtro");
    }

}
