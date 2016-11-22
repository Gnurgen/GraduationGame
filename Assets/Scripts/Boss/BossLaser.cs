using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossLaser : MonoBehaviour {

    public PKFxFX[] lasers;
    private Ray[] rays = new Ray[4];
    private RaycastHit[] hits = new RaycastHit[4];

    Vector3[] LRPos = new Vector3[2];
    public float RotationSpeed = 5;
    public float secondsOfLaser = 10;
    public float laserDmgPerSecond;
    private float shooting;
    private bool rotating;
    private bool[] activeLasers = new bool[4] {false, false, false, false};
    [SerializeField]
    private float laserForce;

    // Use this for initialization
    void Start () {
        LRPos[0] = transform.position;
        LRPos[1] = transform.transform.GetChild(0).position;
        for(int i = 0; i < 4; i++)
        {
            rays[i] = new Ray();
            lasers[i].StopEffect();
        }
        rotating = false;
	}

    void Update()
    {
        shooting -= Time.deltaTime;
    }

    public void DeActivate()
    {
        shooting = -1;
    }

    public void Activate(bool[] activate, float newRotationSpeed, float newSecondsOfLaser)
    {
        if (newSecondsOfLaser > 0)
        {
            RotationSpeed = newRotationSpeed;
            secondsOfLaser = newSecondsOfLaser;
            shooting = secondsOfLaser;
            if (activate[0] && !activeLasers[0])
            {
                StartCoroutine(ShootLaser(0));
            }
            if (activate[1] && !activeLasers[1])
            {
                StartCoroutine(ShootLaser(1));
            }
            if (activate[2] && !activeLasers[2])
            {
                StartCoroutine(ShootLaser(2));
            }
            if (activate[3] && !activeLasers[3])
            {
                StartCoroutine(ShootLaser(3));
            }
            if (!rotating)
            {
                StartCoroutine(Rotating());
            }
        }
    }

    IEnumerator ShootLaser(int index)
    {
        GameManager.events.BossLaserActivation(gameObject);
        activeLasers[index] = true;
        Quaternion startPos = transform.rotation;
        rays[index].origin = transform.up; 
        lasers[index].StartEffect();
        while (shooting > 0)
        {
           
            switch (index)
            {
                case 0:
                    rays[index].direction = transform.forward * 100 ;
                    break;
                case 1:
                    rays[index].direction = -transform.forward * 100;
                    break;
                case 2:
                    rays[index].direction = -transform.right * 100;
                    break;
                case 3:
                    rays[index].direction = transform.right * 100;
                    break;
                default:
                    rays[index].direction = transform.forward * 100;
                    break;
            }
            Physics.Raycast(rays[index], out hits[index]);
            //transform.GetChild(0).position = hit.point + Vector3.up; // vector up to account for low raycast
            //LR.SetPosition(1, transform.GetChild(0).position);
            lasers[index].SetAttribute(new PKFxManager.Attribute("Target", hits[index].point));
            lasers[index].transform.position = hits[index].point;
            
            if (hits != null && hits[index].collider.tag == "Player")
            {
                GameManager.events.EnemyAttackHit(gameObject, laserDmgPerSecond);
                hits[index].transform.GetComponent<Health>().decreaseHealth(laserDmgPerSecond * Time.deltaTime, (GameManager.player.transform.position-transform.position).normalized*laserForce);
            }
            yield return null;
        }
        GameManager.events.BossLaserDeactivation(gameObject);
        lasers[index].StopEffect();
        activeLasers[index] = false;
    }

    IEnumerator Rotating()
    {
        rotating = true;
        while (shooting > 0)
        {
            transform.Rotate(Vector3.up, RotationSpeed * Time.deltaTime);
            yield return null;
        }
        rotating = false;
        yield break;
    }
}
