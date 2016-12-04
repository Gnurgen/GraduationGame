using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpiritLvlBar : MonoBehaviour
{
    private float scale;
    private void Start()
    {
        gameObject.GetComponent<Image>().fillAmount = 0;
    }

    public void updateBar(int max, int current, float percent) {
        if(percent != 0)
            scale = (1 - (float)current / max) / percent;
        if (scale > 1)
            scale = 1;
        gameObject.GetComponent<Image>().fillAmount = scale;
    }

    
}
