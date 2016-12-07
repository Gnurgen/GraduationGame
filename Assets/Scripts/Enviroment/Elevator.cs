using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

public class Elevator : MonoBehaviour
{
    public GameObject invisibleWalls, invisibleWalls2;
    private GameObject fade;
    
    CapsuleCollider CC;
    Material mat;
    private bool InvisWall;
    private GameObject player;
    private float preLift = 2;
    private float underLift = 3;
    private InputManager IM;
    int ID;
    float speed = 1;
    int tilesInvis = 0;
    private SpiritLvlBar spiritLevelBar;

    void Start()
    {
        spiritLevelBar = GameObject.Find("SpiritBar").GetComponent<SpiritLvlBar>();
        mat = transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<MeshRenderer>().material;
        GameManager.events.OnElevatorActivated += ActivateME;
        player = GameManager.player;
        IM = GameManager.input;
        ID = IM.GetID();
        Color col = new Color(0.3f, 0.3f, 0.3f);
        mat.color = col;
        CC = GetComponent<CapsuleCollider>();
        invisibleWalls.SetActive(false);
        invisibleWalls2.SetActive(false);
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
        if (col.tag == "Player" && spiritLevelBar.GetProgress() == 1)
        {
            invisibleWalls2.SetActive(true);
            invisibleWalls.SetActive(true);
            player.transform.parent = gameObject.transform;
<<<<<<< HEAD

            StartCoroutine(elevatorLift());
            CC.enabled = false;

=======
            CC.enabled = false; 
            StartCoroutine(elevatorLift());
>>>>>>> 228d3a62ca09c48a3597f1f956a5c51d56e13721
        }
        else if (InvisWall && col.tag == "Enemy")
        {
            col.GetComponent<Health>().decreaseHealth(100,col.transform.position-transform.position,3);
            GameManager.events.PlayerAttackHit(GameManager.player, col.gameObject, 10f);
        }

    }
    void OnTriggerStay(Collider col) 
    {
        if (InvisWall && col.tag == "Enemy")
        {
            col.GetComponent<Health>().decreaseHealth(100, col.transform.position - transform.position, 3);
            GameManager.events.PlayerAttackHit(GameManager.player, col.gameObject, 10f);
        }
    }
   

    IEnumerator elevatorLift()
    {
    
        yield return new WaitForSeconds(preLift);
        GameManager.events.ElevatorMoveStart();

        float newPos = gameObject.transform.position.y + Time.deltaTime * speed;
        float newPosStart = newPos;
        GameManager.events.FadeToBlack();
        while (newPos < newPosStart + speed * underLift) // while the elevator is raised "underLift" meters up
        {
            newPos = gameObject.transform.position.y + Time.deltaTime * speed;
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, newPos, gameObject.transform.position.z);
            yield return null;
        }
        
        GameManager.progress++;
        PlayerPrefs.SetInt("Progress", GameManager.progress);
        GameManager.events.LoadNextlevel();
        print("progress playerpfres: " + PlayerPrefs.GetInt("Progress"));
        yield return null;
    }
}