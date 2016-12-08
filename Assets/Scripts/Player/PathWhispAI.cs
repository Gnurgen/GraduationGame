using UnityEngine;
using System.Collections;

public class PathWhispAI : MonoBehaviour {

    public float speed;
    public float startScatter;
    public float endScatter;
    public float scatterRate;
    private Vector3[] path;
    private int index;
    private PKFxFX effectControl;

    public void Activate(Vector3[] path, Vector3 start)
    {
        this.path = path;
        index = 2;
        transform.position = start;
        effectControl = GetComponent<PKFxFX>();
        GameManager.events.WhispAntSpawn(gameObject);
        //effectControl.StartEffect();

        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        float scatter = startScatter;
        while(scatter > endScatter)
        {
            effectControl.SetAttribute(new PKFxManager.Attribute("Scatter", scatter));
            scatter -= scatterRate * Time.deltaTime;
            yield return null;
        }
        StartCoroutine(FollowPath());
    }

    IEnumerator FollowPath()
    {
        while(index < path.Length)
        {
            transform.position = Vector3.MoveTowards(transform.position, RealVector(path[index]), speed * Time.deltaTime);
            if(Vector3.Distance(transform.position, RealVector(path[index])) < 0.5f)
            {
                index += 1;
            }
            yield return null;
        }
        //effectControl.StopEffect();
        GameManager.events.WhispAntDespawn(gameObject);
        transform.position = new Vector3(0, -100000, 0);
        //Destroy(gameObject);
        GameManager.pool.PoolObj(gameObject);
    }

    Vector3 RealVector(Vector3 v)
    {
        return new Vector3(v.x, 1, v.z);
    }
}
