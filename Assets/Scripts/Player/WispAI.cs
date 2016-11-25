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
    private float distancetoPlayer;
    [SerializeField]
    private float movementSpeed;


    private PKFxFX effectControl;
    private GameObject player;
    private float scatter;

	// Use this for initialization
	void Start ()
    {
        player = GameManager.player;
        scatter = startScatter;
        StartCoroutine(Spawn());
	}

    IEnumerator Spawn()
    {
        Vector3 pos = new Vector3(transform.position.x, 1, transform.position.z);
        transform.position = pos;
        effectControl = GetComponent<PKFxFX>();
        effectControl.SetAttribute(new PKFxManager.Attribute("Scatter", startScatter));
        yield return new WaitForFixedUpdate();
        while (scatter > endScatter)
        {
            transform.position = pos; // delete after testing
            scatter -= scatterDecay;
            effectControl.SetAttribute(new PKFxManager.Attribute("Scatter", scatter));
            yield return new WaitForFixedUpdate();
        }
        StartCoroutine(FindPlayer());
        yield break;
    }

    IEnumerator FindPlayer()
    {
        while(Vector3.Distance(transform.position, player.transform.position) > distancetoPlayer)
        {
            Vector3 dir = (player.transform.position - transform.position).normalized;
            dir.y = 0;
            transform.position += dir * movementSpeed;
            yield return new WaitForFixedUpdate();
        }
        StartCoroutine(FollowPlayer());
        yield break;
    }

    IEnumerator FollowPlayer()
    {
        for (;;)
        {
            if(Vector3.Distance(transform.position, player.transform.position) > distancetoPlayer)
            {
                Vector3 dir = (player.transform.position - transform.position).normalized;
                dir.y = 0;
                transform.position += dir * movementSpeed;
            }
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator OrbitPlayer()
    {
        yield break;
    }

    IEnumerator EnterSpear()
    {
        yield break;
    }

    IEnumerator GuidePlayer()
    {
        yield break;
    }
}