using UnityEngine;
using System.Collections;

public class WhispActivationAI : MonoBehaviour {

    [SerializeField]
    private float orbitTimeMin;
    [SerializeField]
    private float orbitTimeMax;
    [SerializeField]
    private float orbitSpeed;
    [SerializeField]
    private float enterSpeed;

    private Vector3 center;
    private PKFxFX effectControl;

    public void Activate(Vector3 center, Vector3 start)
    {
        transform.position = start;
        effectControl = GetComponent<PKFxFX>();
        effectControl.StartEffect();
        this.center = center;
        StartCoroutine(MoveToCenter());
    }

    IEnumerator MoveToCenter()
    {
        while (Vector3.Distance(transform.position, center) > 3f)
        {
            transform.position = Vector3.MoveTowards(transform.position, center, enterSpeed * Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }
        if (Random.Range(0f, 1f) > 0.5f)
        {
            StartCoroutine(OrbitRight());
        }
        else
        {
            StartCoroutine(OrbitLeft());
        }
        yield break;
    }

    IEnumerator OrbitRight()
    {
        float currentOrbitTime = Random.Range(orbitTimeMin, orbitTimeMax);
        while (currentOrbitTime > 0)
        {
            transform.RotateAround(center, Vector3.up, orbitSpeed * Time.deltaTime);
            currentOrbitTime -= Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        StartCoroutine(EnterCenter());
        yield break;
    }

    IEnumerator OrbitLeft()
    {
        float currentOrbitTime = Random.Range(orbitTimeMin, orbitTimeMax);
        while (currentOrbitTime > 0)
        {
            transform.RotateAround(center, -Vector3.up, orbitSpeed * Time.deltaTime);
            currentOrbitTime -= Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        StartCoroutine(EnterCenter());
        yield break;
    }

    IEnumerator EnterCenter()
    {
        center = new Vector3(center.x, 0, center.z);
        while (Vector3.Distance(transform.position, center) > 0.1)
        {
            transform.position = Vector3.MoveTowards(transform.position, center, enterSpeed * Time.deltaTime);
            yield return null;
        }
        effectControl.StopEffect();
        Destroy(gameObject);
        yield break;
    }
}
