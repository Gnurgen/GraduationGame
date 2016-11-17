using UnityEngine;
using System.Collections;

public class BossLaser : MonoBehaviour {

    LineRenderer LR;
    Vector3[] LRPos = new Vector3[2];
    public float RotationSpeed = 5;
    public float secondsOfLaser = 10;
    // Use this for initialization
    void Start () {
        LR = GetComponent<LineRenderer>();
        LRPos[0] = transform.position;
        LRPos[1] = transform.transform.GetChild(0).position;
        LR.SetPositions(LRPos);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
            CallEvent();
	}
    void CallEvent()
    {
        StartCoroutine("ShootLaser");

    }

    Ray ray;
    RaycastHit hit;
    public float laserDmgPerSecond;

    IEnumerator ShootLaser()
    {
        Quaternion startPos = transform.rotation;
        ray.origin = transform.position + Vector3.down; // vector down because the laser is too high
        float shooting = 0;
        while(shooting < secondsOfLaser)
        {
            shooting += 1 * Time.deltaTime;
            ray.direction = transform.forward * 100;
            Physics.Raycast(ray, out hit);
            transform.GetChild(0).position = hit.point + Vector3.up; // vector up to account for low raycast
            LR.SetPosition(1, transform.GetChild(0).position);
            transform.Rotate(Vector3.up,RotationSpeed * Time.deltaTime);
            if (hit.collider.tag == "Player")
            {
                GameManager.events.EnemyAttackHit(gameObject, laserDmgPerSecond);
                hit.transform.GetComponent<Health>().decreaseHealth(laserDmgPerSecond * Time.deltaTime);
            }

            yield return null;
        }
        transform.GetChild(0).position = transform.position;
        LR.SetPosition(1, transform.GetChild(0).position);
        //transform.rotation = Quaternion.Euler(0, 0, 0);
        
    }
}
