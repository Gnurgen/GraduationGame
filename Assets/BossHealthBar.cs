using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BossHealthBar : MonoBehaviour {

    //Private stuff
    private float currentVal, maxVal;
    private GameObject actor;
    private float scale;

    //Input manager and Event manager

    void Start()
    {

        if (GameObject.Find("Boss") == null)
        {
            gameObject.SetActive(false);
            GameObject.Find("BossHealthBar_back").SetActive(false);
        }
        else
        {
            actor = GameObject.Find("Boss");
            GameManager.events.OnSpearDrawAbilityHit += updateVal;
            GameManager.events.OnConeAbilityHit += updateVal;
            maxVal = currentVal = actor.GetComponent<Health>().health;
        }
    }

    void updateVal(GameObject Id)
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