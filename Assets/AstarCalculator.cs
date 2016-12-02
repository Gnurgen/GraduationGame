using UnityEngine;
using System.Collections;
using Pathfinding;

public class AstarCalculator : MonoBehaviour {

    public float nodeSize;

    
    void Start()
    {
        GameManager.events.OnLoadComplete += ScanPick;
    }

    void ScanPick()
    {
        if(GameManager.progress == 0 || GameManager.progress > GameManager.numberOfLevels)
        {
            Scan();
        }
        else
        {
            RecalculatingScan();
        }
    }

    void Scan()
    {
        print("normal scan");
        GameObject[] triggers = GameObject.FindGameObjectsWithTag("TriggerBox");
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] checkpoints = GameObject.FindGameObjectsWithTag("CheckPoint");
        foreach (GameObject go in triggers)
        {
            go.SetActive(false);
        }
        foreach(GameObject go in enemies)
        {
            go.SetActive(false);
        }
        foreach (GameObject go in checkpoints)
        {
            go.SetActive(false);
        }
        AstarPath p = FindObjectOfType<AstarPath>();
        if (p != null)
            p.Scan();
        foreach (GameObject go in triggers)
        {
            go.SetActive(true);
        }
        foreach (GameObject go in enemies)
        {
            go.SetActive(true);
        }
        foreach (GameObject go in checkpoints)
        {
            go.SetActive(true);
        }
    }

    void RecalculatingScan()
    {
        print("Recalculating scan");
        GameObject[] triggers = GameObject.FindGameObjectsWithTag("TriggerBox");
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] checkpoints = GameObject.FindGameObjectsWithTag("CheckPoint");
        foreach (GameObject go in triggers)
        {
            go.SetActive(false);
        }
        foreach (GameObject go in enemies)
        {
            go.SetActive(false);
        }
        foreach (GameObject go in checkpoints)
        {
            go.SetActive(false);
        }
        AstarPath p = FindObjectOfType<AstarPath>();
        if (p != null)
            RecalculateSize(p);
            p.Scan();
        foreach (GameObject go in triggers)
        {
            go.SetActive(true);
        }
        foreach (GameObject go in enemies)
        {
            go.SetActive(true);
        }
        foreach (GameObject go in checkpoints)
        {
            go.SetActive(true);
        }
    }

    void RecalculateSize(AstarPath p)
    {
        MapGenerator mg = FindObjectOfType<MapGenerator>();
        GridGraph g = (GridGraph)p.graphs[0];
        g.center = new Vector3(mg.mapSize.xMin + ((mg.mapSize.xMax - mg.mapSize.xMin) * 0.5f), -10, mg.mapSize.yMin - ((mg.mapSize.yMax - mg.mapSize.yMin) * 0.5f));
        g.width = (int)(mg.mapSize.width / nodeSize);
        g.depth = (int)(mg.mapSize.height / nodeSize);
        g.UpdateSizeFromWidthDepth();
    }
}
