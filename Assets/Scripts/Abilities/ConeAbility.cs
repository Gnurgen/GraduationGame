using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConeAbility : MonoBehaviour {

    private float tDist, speed, cDist, norm, damage, pushForce, stunTime;
    private int tris;
    private bool move = false;
    private Vector3 xz = new Vector3(1, 0, 1), myRot;
    private Vector3[] dir;
    private Mesh dmgMesh;
    [SerializeField]
    private Sprite coneParticle;
    Ray dmgRay;
    RaycastHit[] hit;

    public GameObject test;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (move)
        {
            norm = cDist / tDist;
            transform.localScale = xz * norm + Vector3.up;
            cDist += speed * Time.fixedDeltaTime;
            if (cDist > tDist)
                move = false;
        }
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
        move = true;
        int i = 0;

        //Calculate all targets hit, ensure only one instance is recorded and apply ability-effects delayed based on distance and speed.
        for (int k = 1; k < dmgMesh.triangles.Length - 1; k += 3)
        {
            dmgRay = new Ray(transform.position, Quaternion.Euler(myRot) * ((transform.position + dmgMesh.vertices[dmgMesh.triangles[k]]) - transform.position));
            hit = Physics.RaycastAll(dmgRay,l, 10);
            for(int q = 0; q<hit.Length; ++q)
            {
                if(hit[q].transform.gameObject.layer == 9)
                {
                    hit[q].transform.gameObject.layer = 10;
                    StartCoroutine(ApplyConeEffect(hit[q].transform.gameObject, Vector3.Distance(transform.position, hit[q].transform.position) / speed));
                }
            }
            dir[i] = Quaternion.Euler(myRot) * ((transform.position + dmgMesh.vertices[dmgMesh.triangles[k]]) - transform.position);
            dir[i].y = 0;
            ++i;
        }
    }
    IEnumerator ApplyConeEffect(GameObject go, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        go.GetComponent<Rigidbody>().AddForce((go.transform.position - transform.position).normalized*pushForce);
        go.GetComponent<EnemyStats>().decreaseHealth(damage);
        //go.GetComponent<EnemyStats>().PauseFor(stunTime);
        go.layer = 9;
    }
}
