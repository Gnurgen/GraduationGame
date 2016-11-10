using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class PoolManager : MonoBehaviour {

    public GameObject EnemyRangedAttackPrefab, BlobPrefab;
    
    private Dictionary<string, List<GameObject>> pool;

	// Use this for initialization
	void Start () {
        
    }
	
    void Subscribe()
    {
        GameManager.events.OnPoolObject += PoolObj;
        GameManager.events.OnEnemyDeath += GenerateRagdoll;
        //GameManager.events.OnEnemyDeath += PoolObj;
        GameManager.events.OnResourceDrop += GenerateBlob;
        
   

    }

    private void GenerateRagdoll(GameObject enemyID)
    {
        throw new NotImplementedException();
    }

    private void PoolObj(GameObject obj)
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
            result.transform.position = GO.transform.position;
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



    // Update is called once per frame
    void Update () {
	
	}
}
