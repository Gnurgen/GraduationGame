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

    public void Activate(Vector3[] path)
    {
        this.path = path;
        index = 0;
        transform.position = RealVector(path[index]);
        effectControl = GetComponent<PKFxFX>();
        effectControl.StartEffect();

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
        effectControl.StopEffect();
        Destroy(gameObject);
    }

    Vector3 RealVector(Vector3 v)
    {
        return new Vector3(v.x, 1, v.z);
    }
}
