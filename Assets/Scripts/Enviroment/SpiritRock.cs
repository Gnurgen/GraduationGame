using UnityEngine;
using System.Collections;

public class SpiritRock : MonoBehaviour {
    public GameObject blueFlame;
    public GameObject[] spiritRocks;
    private bool isLit = false;
    private GameObject room;
  
    void Start() {
        GameManager.events.OnMapGenerated += getAllSpiritStones;
        room = gameObject.transform.parent.gameObject;
        getAllSpiritStones();
    }

    void OnTriggerEnter(Collider col)
    {
        Health[] enemies = gameObject.transform.parent.gameObject.GetComponentsInChildren<Health>();

        if (col.tag == "Player" && isLit == false && enemies.Length == 0)
        {
       
            turnOffAllFlames();
            startFlame();
            
        }
    }

    public void stopFlame() {
        blueFlame.GetComponent<PKFxFX>().StopEffect();
        isLit = false;
    }

    public void startFlame()
    {
        blueFlame.GetComponent<PKFxFX>().StartEffect();
        isLit = true;
        GameManager.game.activeCheckpoint = gameObject;
    }

    void turnOffAllFlames() {
        for (int i = 0; i < spiritRocks.Length; i++) {
            spiritRocks[i].GetComponent<SpiritRock>().stopFlame();         
        }
    }
    void getAllSpiritStones()
    {
        spiritRocks = GameObject.FindGameObjectsWithTag("CheckPoint");
    }

        
}
