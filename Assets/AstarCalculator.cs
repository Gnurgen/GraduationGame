using UnityEngine;
using System.Collections;
using Pathfinding;
using System;

public class AstarCalculator : MonoBehaviour {

    public float nodeSize;
    public GameObject A_Star;
    
    void Start()
    {
        GameManager.events.OnLoadComplete += ScanPick;
    }

    void ScanPick()
    {
        StartCoroutine(ScanPick2());
    }

    private IEnumerator ScanPick2()
    {
        yield return new WaitForSeconds(1f);

        if(GameManager.progress > 0 && GameManager.progress <= GameManager.numberOfLevels)
            RecalculateSize();
    }
    void RecalculateSize()
    {
        GameObject p = Instantiate(A_Star);
        AstarPath pA = p.GetComponent<AstarPath>();
        MapGenerator mg = FindObjectOfType<MapGenerator>();
        GridGraph g = (GridGraph)pA.graphs[0];
        g.center = new Vector3(mg.mapSize.xMin + ((mg.mapSize.xMax - mg.mapSize.xMin) * 0.5f), 0, mg.mapSize.yMin - ((mg.mapSize.yMax - mg.mapSize.yMin) * 0.5f));
        g.width = (int)(mg.mapSize.width / nodeSize);
        g.depth = (int)(mg.mapSize.height / nodeSize);
        g.UpdateSizeFromWidthDepth();
        pA.Scan();
    }
    //void Scan()
    //{
    //    GameObject[] triggers = GameObject.FindGameObjectsWithTag("TriggerBox");
    //    GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
    //    GameObject[] checkpoints = GameObject.FindGameObjectsWithTag("CheckPoint");
    //    foreach (GameObject go in triggers)
    //    {
    //        go.SetActive(false);
    //    }
    //    foreach(GameObject go in enemies)
    //    {
    //        go.SetActive(false);
    //    }
    //    foreach (GameObject go in checkpoints)
    //    {
    //        go.SetActive(false);
    //    }
    //    AstarPath p = FindObjectOfType<AstarPath>();
    //    if (p != null)
    //        p.Scan();
    //    foreach (GameObject go in triggers)
    //    {
    //        go.SetActive(true);
    //    }
    //    foreach (GameObject go in enemies)
    //    {
    //        go.SetActive(true);
    //    }
    //    foreach (GameObject go in checkpoints)
    //    {
    //        go.SetActive(true);
    //    }
    //}

    //void RecalculatingScan()
    //{
    //    GameObject[] triggers = GameObject.FindGameObjectsWithTag("TriggerBox");
    //    GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
    //    GameObject[] checkpoints = GameObject.FindGameObjectsWithTag("CheckPoint");
    //    foreach (GameObject go in triggers)
    //    {
    //        go.SetActive(false);
    //    }
    //    foreach (GameObject go in enemies)
    //    {
    //        go.SetActive(false);
    //    }
    //    foreach (GameObject go in checkpoints)
    //    {
    //        go.SetActive(false);
    //    }
    //    AstarPath p = FindObjectOfType<AstarPath>();
    //    if (p != null)
    //    {
    //        RecalculateSize(p);
    //        p.Scan();
    //    }
    //    foreach (GameObject go in triggers)
    //    {
    //        go.SetActive(true);
    //    }
    //    foreach (GameObject go in enemies)
    //    {
    //        go.SetActive(true);
    //    }
    //    foreach (GameObject go in checkpoints)
    //    {
    //        go.SetActive(true);
    //    }
    //}


}
