using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Occluder : MonoBehaviour {

    float hideTime = 0.5f, hidTime;
    MeshFilter meshFilter;
    public bool hasDoor;
    public Mesh wallOcc, wallShow, wallDoorOcc, wallDoorShow;

    BoxCollider[] allBoxColls;
    BoxCollider triggerCol;

    void Awake()
    {
        allBoxColls = GetComponents<BoxCollider>();

        for (int i = 0; i < allBoxColls.Length; i++)
        {
            if (allBoxColls[i].isTrigger)
            {
                triggerCol = allBoxColls[i];
            }
        }

        GameObject objectHolder = new GameObject();
        objectHolder.transform.SetParent(transform);
        triggerCol.enabled = false;
    }

    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();

        GameManager.events.OnMapGenerated += EnableOnTrigger;
        GameManager.events.OnLoadComplete += DisableTriggerOnLoad;
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
            transform.GetChild(0).gameObject.SetActive(true);
            if (!hasDoor)
            {
                meshFilter.mesh = wallShow;
            }
            else if(hasDoor)
            {
                meshFilter.mesh = wallDoorShow;
            }
        }
        else if(!enable)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            if(!hasDoor)
            {
                meshFilter.mesh = wallOcc;
            }
            else if (hasDoor)
            {
                meshFilter.mesh = wallDoorOcc;
            }
        }

    }

    void OnTriggerStay(Collider col)
    {
        if (col.tag == "Indestructable" && col.transform.parent.tag != "Indestructable")
        {
            col.transform.SetParent(triggerCol.transform.GetChild(0));
        }
    }

    void DisableTriggerOnLoad()
    {
        triggerCol.enabled = false;
    }

    void EnableOnTrigger()
    {
        triggerCol.enabled = true;
    }

}
