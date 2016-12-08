using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyHealthBars : MonoBehaviour {

    public float timeBetweenFade = 2.0f;
    public float fadeInDuration = 0.2f;
    public float fadeOutDuration = 5.0f;

    private float maxSize = 0.8f;
    private float minSize = 0.2f;
    private float maxHealth;
    private float health;
    private float hightOfHealthbar = 300;
    private GameObject enemy, mainCamera;
    private int type;

  

    void Awake()
    {
        transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
    }

    void Start () {
        mainCamera = GameObject.Find("Main Camera");            
        gameObject.GetComponent<Image>().CrossFadeAlpha(0, fadeOutDuration, true);

        
    }
    void Update()
    {
        if (enemy != null)
            healthPosition();
        else
            gameObject.SetActive(false);
    }

   public void updateHealthBar() {
        StartCoroutine(fadeHealthBar());
        health = enemy.GetComponent<Health>().health;
        float fill = 1f - ((maxHealth - health) / maxHealth);
        gameObject.GetComponent<Image>().fillAmount = fill >= 0.2f ? fill : 0.2f;
        if (health <= 0)
            gameObject.SetActive(false);
    }

    void healthPosition() {
        gameObject.transform.position = mainCamera.transform.position + (enemy.transform.position - mainCamera.transform.position).normalized * 3;
        gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y + hightOfHealthbar, gameObject.transform.localPosition.z);
    }


    public void getMyEnemy(GameObject myEnemy) {
        enemy = myEnemy;
        myEnemy.GetComponent<Health>().SetHealthBar(this);
        maxHealth = enemy.GetComponent<Health>().health;
        updateHealthBar();
    }


    IEnumerator fadeHealthBar()
    {
        gameObject.GetComponent<Image>().CrossFadeAlpha(1, fadeInDuration, true);
        yield return new WaitForSeconds(timeBetweenFade);
        gameObject.GetComponent<Image>().CrossFadeAlpha(0, fadeOutDuration, true);
    }
}
