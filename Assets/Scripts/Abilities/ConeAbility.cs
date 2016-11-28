using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConeAbility : MonoBehaviour {

    /*TODO:
     * STOP CONE ANIMATION AT SOLID OBJECTS 
    */


    private float tDist, speed, cDist, norm, damage, pushForce, stunTime;
    private int tris;
    private bool detect = false;
    private Vector3 myRot;
    private Vector3[] dir;
    private Mesh dmgMesh;
    private GameObject coneParticle;
    Ray dmgRay;
    RaycastHit[] hit;
    PoolManager poolManager;
    int cCounter = 0, cStart = 0;
	// Use this for initialization
	void Awake () {
        poolManager = FindObjectOfType<PoolManager>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (detect)
        {
            int i = 0;

            //Calculate all targets hit, ensure only one instance is recorded and apply ability-effects delayed based on distance and speed.
            for (int k = 1; k < dmgMesh.triangles.Length - 1; k += 3)
            {
                Vector3 partDir = Quaternion.Euler(myRot) * ((transform.position + dmgMesh.vertices[dmgMesh.triangles[k]]) - transform.position);
                dmgRay = new Ray(transform.position, partDir);
                hit = Physics.RaycastAll(dmgRay, partDir.magnitude);
                for (int q = 0; q < hit.Length; ++q)
                {
                    if(hit[q].transform.gameObject.layer == 10)
                    {
                        hit[q].transform.gameObject.layer = 11;
                        StartCoroutine(ApplyConeEffect(hit[q].transform.gameObject, Vector3.Distance(transform.position, hit[q].transform.position) / speed));
                        ++cStart;
                    }
                }
                dir[i] = transform.position + partDir;
                ++i;
            }
            Spawncone();
            detect = false;
        }
    }
    private void Spawncone()
    {
        for(int i = 0; i<dir.Length; ++i)
        {
            coneParticle = poolManager.GenerateObject("p_ConeParticle");
            coneParticle.transform.position = (transform.position-Vector3.up) + Vector3.up*Random.Range(0.1f, 1.5f);
            float coneDist = (dir[i]-transform.position).magnitude;
            dir[i].y = coneParticle.transform.position.y;
            coneParticle.transform.LookAt(dir[i]);
            coneParticle.GetComponent<MovingConeParticle>().setVars(speed, coneDist);
        }
        if (cStart == 0)
            Destroy(gameObject);
    }

    public void setVars(float l, float s, int t, Mesh m, float d, float p, float st)
    {
        tDist = l;
        speed = s;
        pushForce = p;
        damage = d;
        stunTime = st;
        dir = new Vector3[t];
        dmgMesh = m;
        transform.position += Vector3.up;
        myRot = transform.rotation.eulerAngles;
        detect = true;
    }

    IEnumerator ApplyConeEffect(GameObject go, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        if (go == null)
        {
            ++cCounter;
        }
        else
        {
            print("applycone");
            GameManager.events.ConeAbilityHit(go);
            go.GetComponent<Rigidbody>().AddForce((go.transform.position - transform.position).normalized*pushForce, ForceMode.Impulse);
            go.GetComponent<EnemyStats>().decreaseHealth(damage, (go.transform.position - transform.position), pushForce);
            go.GetComponent<EnemyStats>().PauseFor(stunTime);
            go.layer = 10;
            ++cCounter;
        }
        if (cCounter == cStart)
            Destroy(gameObject);
    }
}
