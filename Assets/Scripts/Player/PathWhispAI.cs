using UnityEngine;
using System.Collections;

public class PathWhispAI : MonoBehaviour {

    public float speed;
    public float startScatter;
    public float endScatter;
    public float scatterRate;
    private Vector3[] path;
    private Vector3 end;
    private Vector3 target;
    private int index;
    private PKFxFX effectControl;
    private bool subbed = false;

    public void Activate(Vector3[] path, Vector3 start, Vector3 end)
    {
        this.path = path;
        index = 1;
        this.end = end;
        transform.position = start;
        effectControl = GetComponent<PKFxFX>();
        GameManager.events.WhispAntSpawn(gameObject);
        if (!subbed)
        {
            GameManager.events.OnLoadNextLevel += NextLevel;
        }
        //effectControl.StartEffect();

        StartCoroutine(Spawn());
    }

    private void NextLevel()
    {
        GameManager.events.WhispAntDespawn(gameObject);
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
        while (Vector3.Distance(transform.position, RealVector(path[path.Length - 1])) > 6)
        {
            transform.position = Vector3.MoveTowards(transform.position, RealVector(path[index]), speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, RealVector(path[index])) < 0.5f)
            {
                index += 1;
            }
            yield return null;
        }
        Vector2 radPos = Random.insideUnitCircle * 3;
        target = path[path.Length-1] + new Vector3(radPos.x, 1, radPos.y);
        while (Vector3.Distance(transform.position, target) > 0.3f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            yield return null;
        }
        target = target - Vector3.up * 2;
        float scatter = endScatter;
        while (Vector3.Distance(transform.position, target) > 0.3f)
        {
            effectControl.SetAttribute(new PKFxManager.Attribute("Scatter", scatter));
            scatter += scatterRate * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            yield return null;
        }
        //effectControl.StopEffect();
        GameManager.events.WhispAntDespawn(gameObject);
        transform.position = end;
        yield return new WaitForSeconds(2f);
        //Destroy(gameObject);
        GameManager.pool.PoolObj(gameObject);
    }

    Vector3 RealVector(Vector3 v)
    {
        return v + Vector3.up;
    }
}
