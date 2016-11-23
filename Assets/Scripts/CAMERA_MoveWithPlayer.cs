using UnityEngine;
using System.Collections;

public class CAMERA_MoveWithPlayer : MonoBehaviour {

    [SerializeField]
    private float distance;
    [SerializeField]
    private Vector3 viewingAngle;
    private Transform player;
	// Use this for initialization
	void Start () {
        transform.rotation = Quaternion.identity;
        player = GameManager.player.transform;
        transform.Rotate(viewingAngle);
    }
	
	// Update is called once per frame
	void Update () {
        if(player!=null)
            transform.position = player.position - transform.forward * distance;

    }
}
