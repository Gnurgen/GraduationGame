using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Elevator : MonoBehaviour
{
    public GameObject invisibleWalls;
    private GameObject fade;
    private GameObject player;
    private float preLift = 2;
    private float underLift = 2;
    private InputManager IM;
    int ID;
    float speed = 1;
    bool isMoving = false;

    void Start()
    {
        fade = GameObject.Find("Fade");
        player = GameObject.Find("Kumo");
        IM = GameManager.input;
        ID = IM.GetID();
    }

    void Update()
    {
        if (isMoving == true)
            elevatorLif();
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            invisibleWalls.SetActive(true);
            Debug.Log("Kumo has entered the elevator");
            player.transform.parent = gameObject.transform;
            StartCoroutine(waitForAniStart());
        }
    }

    void elevatorLif()
    {
        float newPos = gameObject.transform.position.y + Time.deltaTime * speed;
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, newPos, gameObject.transform.position.z);
    }
    IEnumerator waitForAniStart()
    {
        yield return new WaitForSeconds(preLift);
        fade.GetComponent<Fade>().fadeToBlack(2);
        isMoving = true;
        yield return new WaitForSeconds(underLift);
    }
}