using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

public class Elevator : MonoBehaviour
{
    public GameObject invisibleWalls;
    private GameObject fade;
    CapsuleCollider CC;
    Material mat;
    private GameObject player;
    private float preLift = 2;
    private float underLift = 3;
    private InputManager IM;
    int ID;
    float speed = 1;
    bool isMoving = false;

    void Start()
    {
        CC = GetComponent<CapsuleCollider>();
        CC.enabled = false;
        mat = transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<MeshRenderer>().material;
        GameManager.events.OnElevatorActivated += ActivateME;
        fade = GameObject.Find("Fade");
        player = GameManager.player;
        IM = GameManager.input;
        ID = IM.GetID();
        Color col = new Color(0.3f, 0.3f, 0.3f);
        mat.color = col;
    }

    private void ActivateME()
    {
        CC.enabled = true;
        StartCoroutine(ShineGold());
    }
    IEnumerator ShineGold()
    {
        float step = .3f;
        while (step < 1)
        {
            step += 0.3f * Time.deltaTime;
            Color col = new Color(step, step, step);
            mat.color = col;
            yield return null;
        }
        yield return null;
    }
   

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            invisibleWalls.SetActive(true);
            player.transform.parent = gameObject.transform;
            StartCoroutine(elevatorLif());
            StartCoroutine(waitForAniStart());
        }
    }

    IEnumerator elevatorLif()
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
        SceneManager.LoadScene("BossLevel");
    }
}