using UnityEngine;
using System.Collections;

public class WwiseChangeVolume : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void SFXSlider(float set)
    {
        AkSoundEngine.SetRTPCValue("Sound_Effects_Volume_Slider", set);
    }
    public void MusicSlider(float set)
    {
        AkSoundEngine.SetRTPCValue("Music_Volume_Slider", set);
    }
}
