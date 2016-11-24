using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpiritLvlBar : MonoBehaviour
{
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

        maxVal = 1;//Amoutn of spirits needed
    }

    void updateVal()
    {
        StartCoroutine(UpdateLvl());
    }



    IEnumerator UpdateLvl()
    {
        yield return new WaitForEndOfFrame();
        currentVal = 1;//Current Spirit level
        scale = 1 - ((maxVal - currentVal) / maxVal);
        if (currentVal <= maxVal)
        {
            gameObject.GetComponent<Image>().fillAmount = scale;
        }
    }
}
