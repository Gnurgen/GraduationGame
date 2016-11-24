using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OpenCloseDoor : MonoBehaviour {
   public float speed = 1;
    private float newPos;
    public bool opening = false;
    public bool closing = false;
    GameObject invisibleWall;
    private GameObject player;
    GameObject thisTile;
    Ray ray;
    private RaycastHit hit;
    void Start() {
        player = GameManager.player;
        invisibleWall = transform.Find("InvisibleWall").gameObject;
    }

    void Update () {
        if (opening)
            openDoor();
        if (closing)
            closeDoor();
	}

    public void openDoor() {
        if (gameObject.transform.position.y > -7) { 

            invisibleWall.SetActive(false);
            newPos = gameObject.transform.position.y - Time.deltaTime * speed;
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, newPos, gameObject.transform.position.z);
        }
        else
            opening = false;
    }
    public void closeDoor(){
        if (gameObject.transform.position.y < 0)
        {
            newPos = gameObject.transform.position.y + Time.deltaTime * speed;
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, newPos, gameObject.transform.position.z);
        }
        else
            closing = false;
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {


            Collider[] allHits = Physics.OverlapSphere(player.transform.position, 0.001f);

            foreach (Collider c in allHits)
            {
                if(c.tag != "Player")
                {
                    thisTile = c.transform.gameObject;    
                }
            }
                           
            //Close doors
            GameObject room = thisTile.transform.parent.parent.gameObject;
            OpenCloseDoor[] scripts = room.GetComponentsInChildren<OpenCloseDoor>();
            foreach (OpenCloseDoor o in scripts)
            {
                Debug.Log("Close Doors");   
                o.closeDoor();
                invisibleWall.SetActive(true);
            }

        }
    }

}


