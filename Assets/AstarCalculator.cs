using UnityEngine;
using System.Collections;
using Pathfinding;

public class AstarCalculator : MonoBehaviour {

    public bool delayedScan;
    public float scanDelay;


    // Use this for initialization
    void Start()
    {

        if(delayedScan)
            StartCoroutine(DelayedScan(scanDelay));
    }



    IEnumerator DelayedScan(float delay)
    {
        if(scanDelay > 0)
        {
            yield return new WaitForSeconds(delay);
        }
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
}
