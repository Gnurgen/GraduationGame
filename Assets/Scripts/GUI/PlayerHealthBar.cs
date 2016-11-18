using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealthBar : MonoBehaviour {
    //Publics for Game Designer
    public float timeBetweenFade = 2.0f;
    public float fadeInDuration = 1.0f;
    public float fadeOutDuration = 10.0f;

    //Private stuff
    public float currentHealth, maxHealth;
    private GameObject player;
    private float scale;
    private float minSize = 0;
    private float maxSize = 1;
    private GameObject imgLeft, imgRight;

    //Input manager and Event manager
    private InputManager IM;
    private EventManager EM;
    int ID;

    void Start() {
        IM = GameManager.input;
        ID = IM.GetID();
        EM = GameManager.events;

        player = GameObject.Find("Kumo");
        imgLeft = gameObject.transform.GetChild(0).gameObject;
        imgRight = gameObject.transform.GetChild(1).gameObject;
        maxHealth = currentHealth = player.GetComponent<Health>().health;
        imgLeft.GetComponent<Image>().CrossFadeAlpha(0, 2, true);
        imgRight.GetComponent<Image>().CrossFadeAlpha(0, 2, true);

        EM.OnEnemyAttackHit += updatePlayerHealth;
        //EM.OnLevelUp += levelupHealthbar;
    }
    
    void updatePlayerHealth(GameObject ID, float dmg) { 
        currentHealth = player.GetComponent<Health>().health;
        scale = minSize + ((maxSize - minSize) * (1 - ((maxHealth - currentHealth) / maxHealth)));
        if (currentHealth >= 0)
        {
            imgLeft.GetComponent<Image>().fillAmount = scale;
            imgRight.GetComponent<Image>().fillAmount = scale;
            StartCoroutine(fadeHealthBar());
        }
    }

     
    IEnumerator fadeHealthBar()
    {
       imgLeft.GetComponent<Image>().CrossFadeAlpha(1, fadeInDuration, true);
       imgRight.GetComponent<Image>().CrossFadeAlpha(1, fadeInDuration, true);
       yield return new WaitForSeconds(timeBetweenFade);
       imgLeft.GetComponent<Image>().CrossFadeAlpha(0, fadeOutDuration, true);
       imgRight.GetComponent<Image>().CrossFadeAlpha(0, fadeOutDuration, true);
    }

    void levelupHealthbar(int lvl) {
        maxHealth = player.GetComponent<Health>().maxHealth;
        currentHealth = maxHealth;
        updatePlayerHealth(gameObject, 0);
    }
}
