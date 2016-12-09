using UnityEngine;
using System.Collections;
using System;

public class PlayerDeathEvent : MonoBehaviour {

    private bool fadeblack = false;
    private float fov = 20;
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
        AkSoundEngine.SetState("Game_State", "Death");
        GameManager.time.SetTimeScaleInstant(0.1f);
        yield return new WaitForSecondsRealtime(.5f);
        while (fov > 10)
        {
            GameManager.time.SetTimeScaleInstant(0.5f);
            fov -= 1f;
            FindObjectOfType<Camera>().fieldOfView = fov;
            yield return new WaitForFixedUpdate();
        }
        GameManager.events.FadeToBlack();
        fov = 20;
        yield return new WaitUntil(() => fadeblack == true);
        GameObject[] en = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < en.Length; i++)
        {
            en[i].SetActive(false);
        }
        FindObjectOfType<Camera>().fieldOfView = fov;
        GameManager.time.SetTimeScaleInstant(1f);
        AkSoundEngine.StopAll();
        GameManager.events.LoadNextlevel();
    }
}
