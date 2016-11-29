using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealthBar : MonoBehaviour {
    //Publics for Game Designer
 //   public float timeBetweenFade = 2.0f;
 //   public float fadeInDuration = 1.0f;
 //   public float fadeOutDuration = 10.0f;

    //Private stuff
    private float currentVal, maxVal;
    private GameObject actor;
    private float scale;
    private float minSize = 0;
    private float maxSize = 1;

    //Input manager and Event manager
    private InputManager IM;
    private EventManager EM;
    int ID;

    void Start() {
        IM = GameManager.input;
        ID = IM.GetID();
        EM = GameManager.events;

            
            actor = GameManager.player;
            EM.OnEnemyAttackHit += updateVal;
            EM.OnResourcePickup += updateVal;
            maxVal = currentVal = actor.GetComponent<Health>().health;
    }

    void updateVal(GameObject ID, float dmg) {

        StartCoroutine(UpdateHP());
    }
    void updateVal(GameObject ID, int heal)
    {
        StartCoroutine(UpdateHP());
    }
    
    

    IEnumerator UpdateHP()
    {
        yield return new WaitForEndOfFrame();
        currentVal = actor.GetComponent<Health>().health;
        scale = 1 - ((maxVal - currentVal) / maxVal);
        if (currentVal >= 0)
        {
            gameObject.GetComponent<Image>().fillAmount = scale;
        }
    }
    /*
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
    }*/
}
