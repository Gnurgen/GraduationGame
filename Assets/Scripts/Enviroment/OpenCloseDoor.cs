using UnityEngine;
using System.Collections;

public class OpenCloseDoor : MonoBehaviour {
   public float speed = 1;
    private float newPos;
    public bool opening = false;
    public bool closing = false;

	
    void Update () {
        if (opening)
            openDoor();
        if (closing)
            closeDoor();
	}

    public void openDoor() {
        if (gameObject.transform.position.y > -7)
        {
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
    
}
