using UnityEngine;
using System.Collections;

public class MovingConeParticle : MonoBehaviour {

    private float speed, tDist, cDist, norm;
    private bool move;
    Vector3 xz = new Vector3(1, 0, 1);
    PoolManager poolManager;

	// Use this for initialization
	void Start () {
        poolManager = FindObjectOfType<PoolManager>();
	}
	
	// Update is called once per frame
	void Update () {
        if (move)
        {
            norm = cDist / tDist;
            cDist += speed * Time.deltaTime;
            transform.localScale = xz * norm;
            transform.position += transform.forward * speed * Time.deltaTime;
            if (norm >= 1)
            {
                move = false;
                poolManager.PoolObj(gameObject);
            }
        }
	
	}
    public void setVars(float s, float d)
    {
        norm = 0;
        cDist = 0;
        speed = s;
        tDist = d;
        move = true;
    }
}
