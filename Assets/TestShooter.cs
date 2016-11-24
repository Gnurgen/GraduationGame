using UnityEngine;
using System.Collections;

public class TestShooter : MonoBehaviour {

    public float speed;
    public int dmg = 0;
    public GameObject bullet;
    public Vector3 direction;



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	


        if(Input.GetKeyDown(KeyCode.Space))
        {
            GameObject bul;
            bul = Instantiate(bullet, transform.position, transform.rotation) as GameObject;
         //   bul.GetComponent<EnemyRangedAttack>().SetParameters(speed, gameObject, dmg);
        }
	}
}
