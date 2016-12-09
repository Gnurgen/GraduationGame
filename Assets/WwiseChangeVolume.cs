using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WwiseChangeVolume : MonoBehaviour {

    Slider Music, SFX;
    MusicState MS;
    bool PlaySound = false;
	// Use this for initialization
	void Start () {
        Music = GameObject.Find("MusicSlider").GetComponent<Slider>();
        SFX = GameObject.Find("SFXSlider").GetComponent<Slider>();
        Music.value = PlayerPrefs.GetFloat("MusicSlider", 50);
        SFX.value = PlayerPrefs.GetFloat("SFXSlider", 75);
        MS = FindObjectOfType<MusicState>();
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.touchCount == 0 && PlaySound)
        {
            MS.MenuButton();
            PlaySound = false;
        }
    }
    public void SFXSlider(float set)
    {
      
        AkSoundEngine.SetRTPCValue("Sound_Effects_Volume_Slider", set);
        PlaySound = true;


    }
    public void MusicSlider(float set)
    {
        AkSoundEngine.SetRTPCValue("Music_Volume_Slider", set);

        PlaySound = true;

    }
}
