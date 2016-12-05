using UnityEngine;
using System.Collections;
using System;

public class PlayerDeathEvent : MonoBehaviour {

    private bool fadeblack = false;

	// Use this for initialization
	void Start () {
        GameManager.events.OnPlayerDeath += PlayerDeath;
        GameManager.events.OnFadedBlackScreen += Faded2Black;
        GameManager.events.OnFadedTransparentScreen += Faded2Trans;
    }

    private void Faded2Trans()
    {
        fadeblack = false;
    }

    private void Faded2Black()
    {
        fadeblack = true;
    }

    private void PlayerDeath(GameObject Id)
    {
        StartCoroutine(DeathEvent());
    }

    private IEnumerator DeathEvent()
    {
        GameManager.time.SetTimeScaleInstant(0.5f);
        FindObjectOfType<Camera>().fieldOfView = 10;
        yield return new WaitForSeconds(2f);
        GameManager.events.FadeToBlack();
        yield return new WaitUntil(() => fadeblack == true);
        if(GameManager.game.activeCheckpoint == null)
        {
            FindObjectOfType<Camera>().fieldOfView = 20;
            GameManager.time.SetTimeScaleInstant(1f);
            GameManager.events.LoadNextlevel();
            yield break;
        }
        else
        {
            FindObjectOfType<Camera>().fieldOfView = 20;
            GameManager.time.SetTimeScaleInstant(1f);
            GameManager.player.transform.position = GameManager.game.activeCheckpoint.transform.position + Vector3.forward;
            GameManager.player.SetActive(true);
        }
        GameManager.events.FadeFromBlackToTransparent();
    }



    // Update is called once per frame
    void Update () {
	
	}
}
