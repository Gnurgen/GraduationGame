using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BossHealthBar : MonoBehaviour {

    //Private stuff
    private float currentVal, maxVal;
    private GameObject actor;
    private float scale;

    //Input manager and Event manager
    private InputManager IM;
    private EventManager EM;
    int ID;

    void Start()
    {
        IM = GameManager.input;
        ID = IM.GetID();
        EM = GameManager.events;

        if (GameObject.Find("Boss") == null) 
        gameObject.SetActive(false);

        actor = GameObject.Find("Boss");
        EM.OnPlayerAttackHit += updateVal;
        maxVal = currentVal = actor.GetComponent<Health>().health;
    }

    void updateVal(GameObject Id, GameObject tar, float val)
    {
        StartCoroutine(UpdateHP());
    }

    IEnumerator UpdateHP()
    {
        yield return new WaitForEndOfFrame();
        currentVal = actor.GetComponent<Health>().health;
        scale = 1 - ((maxVal - currentVal) / maxVal);
        if (currentVal >= 0)
        {
            gameObject.GetComponent<Image>().fillAmount = scale;
        }
    }
}