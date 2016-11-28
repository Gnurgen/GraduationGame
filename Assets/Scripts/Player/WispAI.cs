using UnityEngine;
using System.Collections;

public class WispAI : MonoBehaviour {

    [SerializeField]
    private float startScatter;
    [SerializeField]
    private float endScatter;
    [SerializeField]
    private float scatterDecay;
    [SerializeField]
    private float distanceToPlayer;
    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private float orbitSpeed;
    [SerializeField]
    private float orbitTime;



    private PKFxFX effectControl;
    private GameObject player;
    private float scatter;

	// Use this for initialization
	void OnEnable ()
    {
        player = GameManager.player;
        scatter = startScatter;
        StartCoroutine(Spawn());
	}

    IEnumerator Spawn()
    {
        //Vector3 pos = new Vector3(transform.position.x, 1, transform.position.z);
        //transform.position = pos;
        effectControl = GetComponent<PKFxFX>();
        effectControl.StartEffect();
        effectControl.SetAttribute(new PKFxManager.Attribute("Scatter", startScatter));
        yield return new WaitForFixedUpdate();
        while (scatter > endScatter)
        {
            //transform.position = pos; // delete after testing
            scatter -= scatterDecay;
            effectControl.SetAttribute(new PKFxManager.Attribute("Scatter", scatter));
            yield return new WaitForFixedUpdate();
        }
        StartCoroutine(FindPlayer());
        yield break;
    }

    IEnumerator FindPlayer()
    {
        while(Vector3.Distance(transform.position, new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z)) > distanceToPlayer)
        {
            Vector3 dir = (player.transform.position - transform.position).normalized;
            dir.y = 0;
            transform.position += dir * movementSpeed * Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        if (Random.Range(0f,1f) > 0.5f)
        {
            StartCoroutine(OrbitPlayerRight());
        }
        else
        {
            StartCoroutine(OrbitPlayerLeft());
        }
        yield break;
    }

    IEnumerator OrbitPlayerRight()
    {
        float currentOrbitTime = orbitTime;
        while (currentOrbitTime > 0)
        {
            transform.RotateAround(player.transform.position, Vector3.up, orbitSpeed * Time.deltaTime);
            currentOrbitTime -= Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        StartCoroutine(EnterSpear());
        yield break;
    }

    IEnumerator OrbitPlayerLeft()
    {
        float currentOrbitTime = orbitTime;
        while (currentOrbitTime > 0)
        {
            transform.RotateAround(player.transform.position, -Vector3.up, orbitSpeed * Time.deltaTime);
            currentOrbitTime -= Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        StartCoroutine(EnterSpear());
        yield break;
    }

    IEnumerator EnterSpear()
    {
        while(Vector3.Distance(transform.position, GameManager.spear.transform.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, GameManager.spear.transform.position, movementSpeed * Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }
        GameManager.events.ResourcePickup(gameObject, 1);
        effectControl.StopEffect();
        GameManager.pool.PoolObj(gameObject);
        yield break;
    }
}