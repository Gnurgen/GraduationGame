using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpearControl : MonoBehaviour {
    
    public int maxUpgrades;
    [SerializeField]
    private float damageIncrease = 1;
    [SerializeField]
    private float scaleIncrease = 1;
    [SerializeField]
    private float colliderIncrease = 0.5f;
    [SerializeField]
    private Vector3 startColor = new Vector3(1, 0.5f, 0.1f);
    [SerializeField]
    private Vector3 endColor = new Vector3(1, 0.1f, 0.1f);
    private int springForce;
    private float damage;
    private float speed;
    private int index;
    //private float step;
    private Vector3[] points;
    private GameObject[] effects;
    private float pushForce;
    private PKFxFX effectControl;
    private float globalScale;
    private new SphereCollider collider;
    private int currentUpgrades;
    private float xColorDelta;
    private float yColorDelta;
    private float zColorDelta;
    private List<GameObject> enemiesHit;

    IEnumerator Fly()
    {
        while(index < points.Length)
        {
            transform.position = Vector3.MoveTowards(transform.position, points[index], speed * Time.deltaTime);
            if(Vector3.Distance(transform.position, points[index]) < 1)
            {
                effects[index-1].GetComponent<PKFxFX>().SetAttribute(new PKFxManager.Attribute("FireColor", new Vector3(1f, 0.1f, 0.1f)));
                //effects[index - 1].GetComponent<PKFxFX>().StopEffect();
                index += 1;
                if(index > 4)
                {
                    effects[index - 5].GetComponent<PKFxFX>().StopEffect();
                }
            }
            yield return null;
        }
        foreach(GameObject go in effects)
        {
            go.GetComponent<PKFxFX>().StopEffect();
        }
        GameManager.events.SpearDrawAbilityEnd(gameObject);
        effectControl.StopEffect();
        Destroy(gameObject);

    }

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Enemy")
        {
            if (!enemiesHit.Contains(col.gameObject))
            {
                enemiesHit.Add(col.gameObject);
                damage += damageIncrease;
                globalScale += scaleIncrease;
                collider.radius += colliderIncrease;
                currentUpgrades += 1;
                effectControl.SetAttribute(new PKFxManager.Attribute("GlobalScale", globalScale));
                if(currentUpgrades < maxUpgrades)
                {
                    effectControl.SetAttribute(new PKFxManager.Attribute("CustomColor", new Vector3(startColor.x + xColorDelta * currentUpgrades, startColor.y + yColorDelta * currentUpgrades, startColor.z + zColorDelta * currentUpgrades)));
                }
                else
                {
                    effectControl.SetAttribute(new PKFxManager.Attribute("CustomColor",endColor));
                }
            }
            HitTarget(col.gameObject);
        }
        else if(col.tag == "Boss")
        {
            GameManager.events.SpearDrawAbilityEnd(gameObject);
            foreach (GameObject go in effects)
            {
                go.GetComponent<PKFxFX>().StopEffect();
            }
            HitTarget(col.gameObject);
            Destroy(gameObject);
        }
    }

    private void HitTarget(GameObject target)
    {
        target.GetComponent<Health>().decreaseHealth(damage, target.transform.position-transform.position, pushForce);
        GameManager.events.SpearDrawAbilityHit(target);
        if(currentUpgrades >= maxUpgrades)
        {
            // Max damage and effect
            GameObject imp = GameManager.pool.GenerateObject("p_FlyingSpearBigImpact");
            imp.transform.position = transform.position + (target.transform.position - transform.position).normalized * (Vector3.Distance(transform.position, target.transform.position) * 0.5f);
            StartCoroutine(DelayedPool(imp, 1));
        }
        else
        {
            GameObject imp = GameManager.pool.GenerateObject("p_FlyingSpearImpact");
            if(target.tag == "Boss")
            {
                imp.transform.position = transform.position + (target.transform.position - transform.position).normalized * (Vector3.Distance(transform.position, target.transform.position) * 0.7f);
            }
            else
            {
                imp.transform.position = transform.position + (target.transform.position - transform.position).normalized * (Vector3.Distance(transform.position, target.transform.position) * 0.5f);
            }
            StartCoroutine(DelayedPool(imp, 1));
        }
    }

    IEnumerator DelayedPool(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        GameManager.pool.PoolObj(obj);
    }

    public void SetParameters(List<Vector3> ps, List<GameObject> es, float speed, float damage, float force, int dragForce, float altitude, float turn, float st, int dragAmount)
    {
        points = ps.ToArray();
        effects = es.ToArray();
        enemiesHit = new List<GameObject>();
        collider = GetComponent<SphereCollider>();
        collider.radius = 0.5f;
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = new Vector3(points[i].x, altitude, points[i].z);
        }
        transform.position = points[0];
        index = 1;
        this.speed = speed;
        this.damage = damage;
        pushForce = force;
        springForce = dragForce * 100;
        if (GetComponent<PKFxFX>() != null)
        {
            effectControl = GetComponent<PKFxFX>();
            effectControl.StartEffect();
            globalScale = 1;
            effectControl.SetAttribute(new PKFxManager.Attribute("GlobalScale", globalScale));
            effectControl.SetAttribute(new PKFxManager.Attribute("CustomColor", startColor));
        }
        currentUpgrades = 0;
        xColorDelta = (endColor.x - startColor.x) / (float)maxUpgrades;
        yColorDelta = (endColor.y - startColor.y) / (float)maxUpgrades;
        zColorDelta = (endColor.z - startColor.z) / (float)maxUpgrades;
        StartCoroutine(Fly());
    }
}
