using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConeAbility : MonoBehaviour {



    private float tDist, speed, cDist, norm, damage, pushForce, stunTime, killTime;
    private int tris;
    private Vector3 myRot;
    private Vector3[] dir;
    private Mesh dmgMesh;
    private GameObject coneParticle;
    Ray dmgRay;
    RaycastHit[] hit;
    int cCounter = 0, cStart = 0;
    int enemy, enemyhit;


    void detect () {
        int i = 0;

        //Calculate all targets hit, ensure only one instance is recorded and apply ability-effects delayed based on distance and speed.
        for (int k = 1; k < dmgMesh.triangles.Length - 1; k += 3)
        {
            Vector3 partDir = Quaternion.Euler(myRot) * ((transform.position + dmgMesh.vertices[dmgMesh.triangles[k]]) - transform.position);
            dmgRay = new Ray(transform.position, partDir);
            hit = Physics.RaycastAll(dmgRay, partDir.magnitude);
            for (int q = 0; q < hit.Length; ++q)
            {
                if (hit[q].transform.gameObject.layer == enemy)
                {
                    hit[q].transform.gameObject.layer = enemyhit;
                    if(hit[q].transform.tag == "Boss")
                    {
                        StartCoroutine(ApplyConeEffect(hit[q].transform.gameObject, Vector3.Distance(transform.position, hit[q].transform.position) / speed));
                    }
                    else
                        StartCoroutine(ApplyConeEffect(hit[q].transform.gameObject, Vector3.Distance(transform.position, hit[q].transform.position+hit[q].transform.GetComponent<EnemyStats>().velo*speed) / speed));
                }
            }
            dir[i] = transform.position + partDir;
            ++i;
        }
        for (int l = 0; l < dir.Length; ++l)
        {
            coneParticle = GameManager.pool.GenerateObject("p_ConeParticle");
            coneParticle.transform.position = (transform.position - Vector3.up) + Vector3.up * Random.Range(0.1f, 1.5f);
            float coneDist = (dir[l] - transform.position).magnitude;
            tDist = tDist < coneDist ? coneDist : tDist;
            dir[l].y = coneParticle.transform.position.y;
            coneParticle.transform.LookAt(dir[l]);
            coneParticle.GetComponent<MovingConeParticle>().setVars(speed, coneDist);
        }
        StartCoroutine(EndConeAbility(tDist / speed));
    }


    public void setVars(float s, int t, Mesh m, float d, float p, float st)
    {
        enemy = LayerMask.NameToLayer("Enemy");
        enemyhit = LayerMask.NameToLayer("EnemyHit");
        speed = s;
        pushForce = p;
        damage = d;
        tDist = 0;
        stunTime = st;
        dir = new Vector3[t];
        dmgMesh = m;
        transform.position += Vector3.up;
        myRot = transform.rotation.eulerAngles;
        detect();
    }

    IEnumerator ApplyConeEffect(GameObject go, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        if (go != null)
        {
            go.layer = enemy;
            GameManager.events.ConeAbilityHit(go);
            if(go.tag == "Enemy")
            {
                go.GetComponent<Rigidbody>().AddForce((go.transform.position - transform.position).normalized*pushForce, ForceMode.Impulse);
                go.GetComponent<EnemyStats>().decreaseHealth(damage, (go.transform.position - transform.position), pushForce);
                go.GetComponent<EnemyStats>().PauseFor(stunTime);
            }
            else
            {
                go.GetComponent<Health>().decreaseHealth(damage, Vector3.zero, 0);
            }
        }
    }
    IEnumerator EndConeAbility(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameManager.events.ConeAbilityEnd(gameObject);
        Destroy(gameObject);
    }
}
