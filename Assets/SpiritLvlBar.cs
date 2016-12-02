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
        scale = 1-((float)current/((float)max*percent));
        Debug.Log("scale: "+scale + "scale: " + max+ "current: " + current);
        gameObject.GetComponent<Image>().fillAmount = scale;
    }

    
}
