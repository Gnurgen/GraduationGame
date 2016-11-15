using UnityEngine;
using System.Collections;

public class BossLaser : MonoBehaviour {

    LineRenderer LR;
    Vector3[] LRPos = new Vector3[2];
    public float RotationSpeed = 5;
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
    
    IEnumerator ShootLaser()
    {
        Quaternion startPos = transform.rotation;
        ray.origin = transform.position + Vector3.down;
        ray.direction= transform.forward * 100;
        bool shooting = true;
        while(shooting)
        {
            ray.direction = transform.forward * 100;
            Physics.Raycast(ray, out hit);
            transform.GetChild(0).position = hit.point + Vector3.up;
            LR.SetPosition(1, transform.GetChild(0).position);
            transform.Rotate(Vector3.up,RotationSpeed * Time.deltaTime);
            
            if(transform.rotation.y <= 1  || transform.rotation.y >= -1)
            {
                shooting = false;
                transform.GetChild(0).position = transform.position;
                LR.SetPosition(1, transform.GetChild(0).position);
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            yield return null;
        }
    }
}
