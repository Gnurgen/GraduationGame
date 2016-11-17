using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class tagmigtilbage : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void startgameplz()
    {
        SceneManager.LoadScene(0);

        GameManager.game.audioManager.SendMessage("Subscribe");
        GameManager.game.eventManager.SendMessage("Subscribe");
        GameManager.game.inputManager.SendMessage("Subscribe");
        GameManager.game.poolManager.SendMessage("Subscribe");
        GameManager.game.timeManager.SendMessage("Subscribe");

    }
}
