using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyHealthBars : MonoBehaviour {
    private float maxSize = 0.8f;
    private float minSize = 0.2f;
    private float maxHealth;
    private float health;
    private float hightOfHealthbar = 300;
    private float scale;
    private GameObject enemy;
    private int type;
    Vector3 position;

    void Awake()
    {
        transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
    }

    void Start () {

    }
    void Update()
    {
        if (enemy != null)
        {
            healthPosition();
            updateHealthBar();//Could actually just call this whenever enemy takes damage
        }
        else
            gameObject.SetActive(false);
    }

    void updateHealthBar() {
        health = enemy.GetComponent<Health>().health;
        scale = 1 - ((maxHealth - health) / maxHealth);
        gameObject.GetComponent<Image>().fillAmount = scale;
        if (health <= 0)
            gameObject.SetActive(false);
    }

    void healthPosition() {
        position = enemy.transform.position;  //stop
        gameObject.transform.position = position;
        gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y+ hightOfHealthbar, 0);
    }
    public void getMyEnemy(GameObject myEnemy, int myType) {
        print(myEnemy);
        enemy = myEnemy;
        type = myType;
        maxHealth = enemy.GetComponent<Health>().health;
    }
}
