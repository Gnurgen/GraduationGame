using UnityEngine;
using System.Collections;

public class ProjectileControl : MonoBehaviour {

    public Vector3 startSize = new Vector3(0.1f, 0.1f, 0.1f);
    public Vector3 endSize = new Vector3(0.5f, 0.5f, 0.5f);
    public float chargeTime = 0.5f;
    public float ignitionTime = 0.2f;
    public float speed = 3f;

    private Vector3 direction;
    private Vector3 deltaScale;
    private PKFxFX effectControl;
    private RangedAI owner;

    public IEnumerator Spawn()
    {
        while(transform.localScale.magnitude < endSize.magnitude)
        {
            transform.localScale += deltaScale * Time.deltaTime;
            yield return null;
        }
        effectControl.StartEffect();
        yield return new WaitForSeconds(ignitionTime);
        yield break;
    }

    public IEnumerator Activate(Vector3 start, Vector3 target, RangedAI owner)
    {
        this.owner = owner;
        effectControl = gameObject.GetComponentInChildren<PKFxFX>();
        direction = (target - start).normalized;
        direction = new Vector3(direction.x, 0, direction.z);
        transform.localScale = startSize;
        deltaScale = (endSize - startSize) / chargeTime;
        transform.position = start;
        yield return StartCoroutine(Spawn());
        StartCoroutine(Launch());
        yield break;
    }

    IEnumerator Launch()
    {
        transform.GetChild(0).transform.position += direction * 0.1f * speed;
        float t = 102f;
        while(t > 0)
        {
            transform.position += direction * speed * Time.deltaTime;
            t -= Time.deltaTime;
            yield return null;
        }
        destroy();
        yield break;
    }

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "Player")
        {
            GameManager.events.EnemyAttackHit(gameObject, owner.damage);
            col.gameObject.GetComponent<Health>().decreaseHealth(owner.damage, col.transform.position - transform.position, owner.force);
            GameObject eff = GameManager.pool.GenerateObject("p_EnemyRangedAttackHit");
            eff.transform.position = transform.position;
            eff.GetComponent<PKFxFX>().StartEffect();
            eff.GetComponent<PoolDelay>().DelayPool(0.5f);
            destroy();
        }
        else
        {
            GameManager.events.EnemyRangedMiss(gameObject);
            GameObject eff = GameManager.pool.GenerateObject("p_EnemyRangedAttackHit");
            eff.transform.position = transform.position;
            eff.GetComponent<PKFxFX>().StartEffect();
            eff.GetComponent<PoolDelay>().DelayPool(0.5f);
            destroy();
        }
    }

    void destroy()
    {
        effectControl.StopEffect();
        transform.position = new Vector3(0, -100000, 0);
        GameManager.pool.PoolObj(gameObject);
    }
}
