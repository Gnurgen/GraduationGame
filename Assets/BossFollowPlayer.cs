using UnityEngine;
using System.Collections;

public class BossFollowPlayer : MonoBehaviour {
    [SerializeField]
    private float turnSpeed;
    void Update () {
       // Vector3 player = new Vector3(GameManager.player.transform.position.x, transform.position.y, GameManager.player.transform.position.z);
        Vector3 dir = Quaternion.FromToRotation(transform.forward, GameManager.player.transform.position - transform.position).eulerAngles;
        dir.x = 0;
        dir.z = 0;
        if (dir.y <= 2 || dir.y >= 358)
        {
            transform.Rotate(dir);
        }
        else if (dir.y > 180)
            transform.Rotate(-dir.normalized * turnSpeed * Time.deltaTime);
        else
            transform.Rotate(dir.normalized * turnSpeed * Time.deltaTime);
	}
}
