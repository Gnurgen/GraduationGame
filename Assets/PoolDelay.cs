using UnityEngine;
using System.Collections;

public class PoolDelay : MonoBehaviour {

	public void DelayPool(float time)
    {
        StartCoroutine(DelayedPool(time));
    }

    IEnumerator DelayedPool(float time)
    {
        yield return new WaitForSeconds(time);
        transform.position = new Vector3(0, -100000, 0);
        GameManager.pool.PoolObj(gameObject);
    }
}
