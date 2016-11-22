using UnityEngine;
using System.Collections;

public class homingMissile : MonoBehaviour {

    public GameObject HomingMissilePrefab;
    public float Damage;
    public float Speed;
	void Update () {
        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            GameObject HMP = Instantiate(HomingMissilePrefab, transform.position, Quaternion.identity) as GameObject;
          //  HMP.GetComponent<HomingMissleAI>().SetParameters(Speed,Damage);
        }
	}
}
