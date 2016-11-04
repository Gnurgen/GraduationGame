using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuButtonHover : MonoBehaviour {
    public GameObject normal, hover;
  
	void Start () {
	}
    public void hoverImageOn() {
        hover.SetActive(true);
    }
    public void hoverImageOff()
    {
        hover.SetActive(false);
    }
}
