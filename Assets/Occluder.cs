using UnityEngine;
using System.Collections.Generic;

public class Occluder : MonoBehaviour {

    float hideTime = 0.5f, hidTime;
    MeshFilter meshFilter;
    public bool hasDoor;
    public Mesh wallOcc, wallShow, wallDoorOcc, wallDoorShow;

    public bool hasHid = false;

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
            if (!hasDoor)
            {
                meshFilter.mesh = wallShow;
            }
            else if(hasDoor)
            {
                meshFilter.mesh = wallDoorShow;
            }
            hasHid = false;
        }
        else if(!enable)
        {
            if(!hasDoor)
            {
                meshFilter.mesh = wallOcc;
            }
            else if (hasDoor)
            {
                meshFilter.mesh = wallDoorOcc;
            }
            hasHid = true;
        }

    }

    void OnTriggerStay(Collider col)
    {
        if (col.transform.parent.parent.tag == "Indestructable" && hasHid == true)
        {
            col.GetComponentInChildren<MeshRenderer>().enabled = false;
        }

        if(col.transform.parent.parent.tag == "Indestructable" && hasHid == false)
        {
            col.GetComponentInChildren<MeshRenderer>().enabled = true;
        }
    }

}
