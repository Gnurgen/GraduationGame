using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class PoolManager : MonoBehaviour {

    public GameObject EnemyRangedAttackPrefab, BlobPrefab;
    private Vector3 poolPosition = new Vector3(1000, 1000, 1000);
    private Dictionary<string, List<GameObject>> pool;

	// Use this for initialization
	void Start () {
        pool = new Dictionary<string, List<GameObject>>();
    }
	
    void Subscribe()
    {
        GameManager.events.OnPoolObject += PoolObj;
       // GameManager.events.OnEnemyDeath += GenerateRagdoll;
        //GameManager.events.OnEnemyDeath += PoolObj;
        GameManager.events.OnResourceDrop += GenerateBlob;
        
   

    }

    private void GenerateRagdoll(GameObject enemyID)
    {
        throw new NotImplementedException();
    }

    public void PoolObj(GameObject obj)
    {
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
        result.SetActive(true);
        return result;
    }



    // Update is called once per frame
    void Update () {
	
	}
}
