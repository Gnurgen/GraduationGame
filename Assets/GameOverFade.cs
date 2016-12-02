using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameOverFade : MonoBehaviour {
/*
    [SerializeField]
    float fadeTime;
    [SerializeField]
    Color fadeColor;
    float curTime;
    bool fadeOut = false, done = false;
    Image img;

    [SerializeField]
    bool debug;
	// Use this for initialization
	void Start () {
        img = GetComponent<Image>();
        img.color = new Vector4(fadeColor.r, fadeColor.g, fadeColor.b, 0);
        GameManager.events.OnPlayerDeath += StartFade;
	}
	
	// Update is called once per frame
	void Update () {
        if (!done)
        {
            curTime += fadeOut ? Time.unscaledDeltaTime : -Time.unscaledDeltaTime;
            if (curTime >= fadeTime)
            {
                curTime = fadeTime;
                img.color = fadeColor;
                if(GameManager.game.activeCheckpoint != null)
                {
                    GameManager.events.Respawned();
                   
                    GameManager.player.GetComponent<Health>().health = GameManager.player.GetComponent<Health>().maxHealth;
                    GameManager.GameOver(true);
                }
                else
                    GameManager.GameOver(false);
                fadeOut = false;
            }
            else if (curTime < 0)
            {
                img.color = new Vector4(fadeColor.r, fadeColor.g, fadeColor.b, 0);
                done = true;
            }
            else
                img.color = new Vector4(fadeColor.r, fadeColor.g, fadeColor.b, curTime / fadeTime);
        }
        if (debug)
            StartFade(gameObject);
	}
    void StartFade(GameObject go)
    {
        curTime = 0;
        debug = false;
        done = false;
        fadeOut = true;
    }
    */
}
