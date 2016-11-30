using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Win : MonoBehaviour {

    void Start(){
        GameManager.events.OnBossDeath += cameraShake;
        GameManager.events.OnBossDeath += loadStartManu;
    }
    
    void cameraShake(GameObject ID) {
    }
    
    void loadStartManu(GameObject ID)
    {
        StartCoroutine(waitBeforeLoad());
    }

    IEnumerator waitBeforeLoad() {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("Outtro");
    }

}
