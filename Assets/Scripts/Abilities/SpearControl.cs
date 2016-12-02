using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpearControl : MonoBehaviour {

    public GameObject impact;
    public int maxUpgrades;
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
    private SphereCollider collider;
    private bool multipleHit;
    private float damageIncrease;
    private float scaleIncrease;
    private float colliderIncrease;
    private int currentUpgrades;

    private List<GameObject> enemiesHit;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    IEnumerator Fly()
    {
        print(points.Length);
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

                currentUpgrades += 1;
            }
            HitTarget(col.gameObject);
        }
        else if(col.tag == "Boss")
        {
            HitTarget(col.gameObject);
            Destroy(gameObject);
        }




        if (col.tag == "Boss")
        {
            col.GetComponent<Health>().decreaseHealth(damage, Vector3.zero, pushForce);
            GameManager.events.SpearDrawAbilityHit(col.gameObject);
            damage += damageIncrease;
            globalScale += scaleIncrease;
            collider.radius += colliderIncrease;
            GameObject imp = Instantiate(impact) as GameObject;
            imp.transform.position = transform.position + (col.gameObject.transform.position - transform.position).normalized * (Vector3.Distance(transform.position, col.gameObject.transform.position) * 0.25f);
            StartCoroutine(DelayedDelete(imp, 1));
            effectControl.SetAttribute(new PKFxManager.Attribute("GlobalScale", globalScale));
            foreach (GameObject go in effects)
            {
                go.GetComponent<PKFxFX>().StopEffect();
            }
            Destroy(gameObject);
        }
        if (col.tag == "Enemy")
        {
            if (!enemiesHit.Contains(col.gameObject))
            {
                enemiesHit.Add(col.gameObject);
            }
                


            if (!multipleHit)
            {
                if (!enemiesHit.Contains(col.gameObject))
                {
                    enemiesHit.Add(col.gameObject);
                    GameManager.events.SpearDrawAbilityHit(col.gameObject);
                    col.GetComponent<Health>().decreaseHealth(damage, (col.transform.position - transform.position), pushForce);
                    damage += damageIncrease;
                    globalScale += scaleIncrease;
                    collider.radius += colliderIncrease;

                    GameObject imp = Instantiate(impact) as GameObject;
                    imp.transform.position = transform.position + (col.gameObject.transform.position - transform.position).normalized * (Vector3.Distance(transform.position, col.gameObject.transform.position) * 0.5f);
                    StartCoroutine(DelayedDelete(imp, 1));
                    effectControl.SetAttribute(new PKFxManager.Attribute("GlobalScale", globalScale));
                }
            }
            else
            {
                GameManager.events.SpearDrawAbilityHit(col.gameObject);
                col.GetComponent<Health>().decreaseHealth(damage, (col.transform.position - transform.position), pushForce);
                damage += damageIncrease;
                globalScale += scaleIncrease;
                collider.radius += colliderIncrease;
                GameObject imp = Instantiate(impact) as GameObject;
                imp.transform.position = transform.position + (col.gameObject.transform.position - transform.position).normalized * (Vector3.Distance(transform.position, col.gameObject.transform.position) * 0.5f);
                StartCoroutine(DelayedDelete(imp, 1));
                effectControl.SetAttribute(new PKFxManager.Attribute("GlobalScale", globalScale));
            }
        }
    }

    private void HitTarget(GameObject target)
    {
        target.GetComponent<Health>().decreaseHealth(damage, Vector3.zero, pushForce);
        GameManager.events.SpearDrawAbilityHit(target);
        if(currentUpgrades >= maxUpgrades)
        {
            // Max damage and effect
        }
    }

    IEnumerator DelayedDelete(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(obj);
    }

    public void SetParameters(List<Vector3> ps, List<GameObject> es, float speed, float damage, float force, int dragForce, float altitude, float turn, float st, int dragAmount, bool multipleHit, float damageIncrease, float scaleIncrease, float colliderIncrease)
    {
        points = ps.ToArray();
        effects = es.ToArray();
        enemiesHit = new List<GameObject>();
        collider = GetComponent<SphereCollider>();
        collider.radius = 0.5f;
        this.multipleHit = multipleHit;
        this.damageIncrease = damageIncrease;
        this.scaleIncrease = scaleIncrease;
        this.colliderIncrease = colliderIncrease;
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
        }
        currentUpgrades = 0;
        StartCoroutine(Fly());
    }
}
