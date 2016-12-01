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
    int tilesInvis = 0;
    void Start()
    {
        mat = transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<MeshRenderer>().material;
        GameManager.events.OnElevatorActivated += ActivateME;
        fade = GameObject.Find("Fade");
        player = GameManager.player;
        IM = GameManager.input;
        ID = IM.GetID();
        Color col = new Color(0.3f, 0.3f, 0.3f);
        mat.color = col;
        CC = GetComponent<CapsuleCollider>();
        CC.enabled = false;
    }
   

    
    private void ActivateME()
    {
        StartCoroutine(ShineGold());
    }
    IEnumerator ShineGold()
    {
        float step = .3f;
        while (step < 1)
        {
            step += 0.5f * Time.deltaTime;
            Color col = new Color(step, step, step);
            mat.color = col;
            yield return null;
        }
        CC.enabled = true;
        yield return null;
    }
   

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            invisibleWalls.SetActive(true);
            player.transform.parent = gameObject.transform;
            StartCoroutine(elevatorLift());
            CC.enabled = false; 
        }
    }

    IEnumerator elevatorLift()
    {
        yield return new WaitForSeconds(preLift);
        GameManager.events.ElevatorMoveStart();
        float newPos = gameObject.transform.position.y + Time.deltaTime * speed;
        fade.GetComponent<Fade>().fadeToBlack(2);
        float newPosStart = newPos;
        while (newPos < newPosStart + speed * underLift)
        {
            newPos = gameObject.transform.position.y + Time.deltaTime * speed;
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, newPos, gameObject.transform.position.z);
            yield return null;
        }
        GameManager.progress++;
        PlayerPrefs.SetInt("Progress", GameManager.progress);
        GameManager.events.ElevatorMoveStop();
        GameManager.events.LoadNextlevel();
        yield return null;
    }
}