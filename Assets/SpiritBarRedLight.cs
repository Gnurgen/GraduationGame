using UnityEngine;
using System.Collections;

public class SpiritBarRedLight : MonoBehaviour {

    PlayerHealthBar PHB;

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player")
        {
            if (PHB == null)
                PHB = FindObjectOfType<PlayerHealthBar>();
            if(PHB.GetProgress() < 1)
            {
                PHB.AnimateRed();
            }
        }
    }
    void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player")
        {
            if (PHB == null)
                PHB = FindObjectOfType<PlayerHealthBar>();
            if (PHB.GetProgress() < 1)
            {
                PHB.StopAnimateRed();
            }
        }
    }
}
