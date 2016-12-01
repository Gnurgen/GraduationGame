using UnityEngine;
using System.Collections;

public class CAMERA_MoveWithPlayer : MonoBehaviour {

    [SerializeField]
    private float distance;
    [SerializeField]
    private Vector3 viewingAngle;
    private Transform player;

    //Camera Shakes
    float duration = 3;
    float magnitude = 0.2f;
    bool shaking = false;
    Vector3 shakingVec;
    void Start() {
        GameManager.events.OnBossDeath += shakeCamera;
    }
    
    void Awake () {
        transform.rotation = Quaternion.identity;
        player = GameManager.player.transform;
        transform.Rotate(viewingAngle);
        if (player != null)
            transform.position = player.position - transform.forward * distance;
    }
	
	void LateUpdate () {
        if (player != null && shaking == false)
        {
            transform.position = player.position - transform.forward * distance;
            Debug.Log("Not shaking");
        }
        else if (player != null && shaking == true)
        {
            Debug.Log("Shaking");
            shakingVec = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.4f, 0.4f), 0);
            transform.position = (player.position - transform.forward * distance) + shakingVec;
        }
    }
    public void shakeCamera(GameObject ID)
    {
        StartCoroutine(Shake());
    }

    IEnumerator Shake()
    {
        shaking = true;
        yield return new WaitForSeconds(3);
        shaking = false;
    }
}
