using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class PoolManager : MonoBehaviour {

    public GameObject EnemyRangedAttackPrefab, BlobPrefab, MeeleeRagdoll, RangedRagdoll;
    private Dictionary<string, List<GameObject>> pool;

	// Use this for initialization
	void Awake () {
        pool = new Dictionary<string, List<GameObject>>();
    }
	
    void Subscribe()
    {
        GameManager.events.OnPoolObject += PoolObj;
       // GameManager.events.OnEnemyDeath += GenerateRagdoll;
        //GameManager.events.OnEnemyDeath += PoolObj;
        GameManager.events.OnResourceDrop += GenerateBlob;
    }

    public void PoolObj(GameObject obj)
    {
        if (obj.GetComponent<AutoPool>() != null)
        {
            obj.GetComponent<AutoPool>().enabled = false;
            Destroy(obj.GetComponent<AutoPool>());
        }
        List<GameObject> ListResult;
        if(pool.TryGetValue(obj.tag, out ListResult))
        {
            ListResult.Add(obj);
        }
        else
        {
            ListResult = new List<GameObject>();
            ListResult.Add(obj);
            pool.Add(obj.tag, ListResult);
        }
        obj.SetActive(false);
    }

    private void GenerateBlob(GameObject GO, int amount)
    {
        for (int i= 0; i < amount; ++i)
        {
            GameObject result;
            List<GameObject> ListResult;
            if (pool.TryGetValue(BlobPrefab.tag, out ListResult))
            {
                if (ListResult.Count > 0)
                {
                    result = ListResult[ListResult.Count - 1];
                    ListResult.RemoveAt(ListResult.Count - 1);
                }
                else
                {
                    result = Instantiate(BlobPrefab);
                }
            }
            else
            {
                result = Instantiate(BlobPrefab);
            }
            result.SetActive(true);
            result.transform.position = GO.transform.position + Vector3.up;
        }
    }

    public GameObject GenerateBullet()
    {
        List<GameObject> ListResult;
        if(pool.TryGetValue(EnemyRangedAttackPrefab.tag, out ListResult))
        {
            if(ListResult.Count > 0)
            {
                GameObject result = ListResult[ListResult.Count - 1];
                ListResult.RemoveAt(ListResult.Count - 1);
                return result;
            }
            else
            {
                GameObject result = Instantiate(EnemyRangedAttackPrefab);
                return result;
            }
       
        }
        else
        {
            GameObject result = Instantiate(EnemyRangedAttackPrefab);
            return result;
        }
    }
    public GameObject GenerateObject(string tag)
    {
        GameObject result;
        List<GameObject> ListResult;
        if(pool.TryGetValue(tag, out ListResult))
        {
            if (ListResult.Count > 0)
            {
                result = ListResult[ListResult.Count - 1];
                ListResult.RemoveAt(ListResult.Count - 1);
            }
            else
                result = Instantiate(Resources.Load<GameObject>("Pool/" + tag));
        }
        else
            result = Instantiate(Resources.Load<GameObject>("Pool/" + tag));
        if (result.GetComponent<AutoPool>() != null)
        {
            result.GetComponent<AutoPool>().enabled = false;
            Destroy(result.GetComponent<AutoPool>());
        }
        result.SetActive(true);
        return result;
    }

    public void GenerateRagdoll(Transform[] p, string tag, Vector3 force)
    {
        GameObject result;
        List<GameObject> ListResult;
        if (pool.TryGetValue(tag, out ListResult))
        {
            if (ListResult.Count > 0)
            {
                result = ListResult[ListResult.Count - 1];
                ListResult.RemoveAt(ListResult.Count - 1);
            }
            else
                result = Instantiate(Resources.Load<GameObject>("Pool/" + tag));
        }
        else
            result = Instantiate(Resources.Load<GameObject>("Pool/" + tag));
        if (result.GetComponent<AutoPool>() != null)
        {
            result.GetComponent<AutoPool>().enabled = false;
            Destroy(result.GetComponent<AutoPool>());
        }
        result.SetActive(true);
        for(int k = 0; k<p.Length; ++k)
        {
            result.transform.GetChild(0).GetChild(k).transform.position = p[k].position;
            result.transform.GetChild(0).GetChild(k).transform.rotation = p[k].rotation;
            result.transform.GetChild(0).GetChild(k).transform.localScale = p[k].transform.lossyScale;
            result.transform.GetChild(0).GetChild(k).GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
        }
    }

    // Update is called once per frame
    void Update () {
	
	}
}
