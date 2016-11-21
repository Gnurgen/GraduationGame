using UnityEngine;
using System.Collections;

public class StartElevator : MonoBehaviour {
    private GameObject fade;
    private GameObject player;
    private bool isMoving = false;
    private float speed = 1;

	void Start () {
        player = GameObject.Find("Kumo");
        fade = GameObject.Find("Fade");
        fade.GetComponent<Fade>().fadeFromBlack(3);

    }
    void elevatorLif()
    {
        float newPos = gameObject.transform.position.y + Time.deltaTime * speed;
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, newPos, gameObject.transform.position.z);
    }
    void Update () {
        if (gameObject.transform.position.y <= -0.8)
            elevatorLif();
        else
        {
            player.transform.position = new Vector3(player.transform.position.x, 0, player.transform.position.z);
            player.transform.parent = null;
        }
	}
    
    
}
