using UnityEngine;
using System.Collections;

public class Occluder : MonoBehaviour {

    float hideTime = 0.5f, hidTime;
	// Update is called once per frame
	void Update () {
        if (hidTime < hideTime)
            hidTime += Time.unscaledDeltaTime;
        else
            GetComponent<MeshRenderer>().enabled = true;
	}

    public void Stop()
    {
        hidTime = 0;
        GetComponent<MeshRenderer>().enabled = false;
    }
}
