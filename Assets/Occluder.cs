using UnityEngine;
using System.Collections;

public class Occluder : MonoBehaviour {

    float hideTime = 0.5f, hidTime;
    MeshFilter meshFilter;
    public Mesh wallOcc, wallShow, wallDoorOcc, wallDoorShow;
    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
           
    }
	// Update is called once per frame
	void Update () {
        if (hidTime < hideTime)
            hidTime += Time.unscaledDeltaTime;
        else
            changeMesh(true);
	}

    public void Stop()
    {
        hidTime = 0;
        changeMesh(false);
    }

    public void changeMesh(bool enable)
    {
        if(enable)
        {
            if (meshFilter.mesh == wallOcc)
            {
                meshFilter.mesh = wallShow;
            }
            else if(meshFilter.mesh == wallDoorOcc)
            {
                meshFilter.mesh = wallDoorShow;
            }
        }
        else if(!enable)
        {
            if(meshFilter.mesh = wallShow)
            {
                meshFilter.mesh = wallOcc;
            }
            else if (meshFilter.mesh == wallDoorShow)
            {
                meshFilter.mesh = wallDoorOcc;
            }
        }

    }
}
