using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutorialDoor : MonoBehaviour {
    float speed = 1;
    private float newPos;
    //int ID;

    private void Start()
    {
        //ID = GameManager.input.GetID();

    }

    void Update () {
        closeDoor();
    }


    public void closeDoor()
    {
        if (gameObject.transform.parent.transform.position.y < 0)
        {
            //GameManager.input.TakeControl(ID);
            newPos = gameObject.transform.parent.transform.position.y + Time.deltaTime * speed;
            gameObject.transform.parent.transform.position = new Vector3(gameObject.transform.parent.transform.position.x, newPos, gameObject.transform.parent.transform.position.z);
        }
        else {
            //GameManager.input.ReleaseControl(ID);
           
        }
    }

}
