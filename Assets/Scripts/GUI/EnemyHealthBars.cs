using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyHealthBars : MonoBehaviour {
    private float maxSize = 0.8f;
    private float minSize = 0.2f;
    private float maxHealth;
    private float health;
    private float hightOfHealthbar = 300;
    private float scale, distance;
    private GameObject enemy, mainCamera;
    private int type;
    Vector3 position;

    void Awake()
    {
        transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
    }

    void Start () {
        mainCamera = GameObject.Find("Main Camera");
        distance = Vector3.Distance(mainCamera.transform.position, GameObject.Find("Canvas").transform.position);                
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
        Vector3 enemyToCamera = enemy.transform.position - mainCamera.transform.position;
        Ray myRay = new Ray(mainCamera.transform.position, enemyToCamera);
        Vector3 posi = myRay.origin + (myRay.direction * 3);
        position = posi;
        gameObject.transform.position = position;
        gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y + hightOfHealthbar, gameObject.transform.localPosition.z);

    }
    public void getMyEnemy(GameObject myEnemy, int myType) {
        enemy = myEnemy;
        type = myType;
        maxHealth = enemy.GetComponent<Health>().health;
    }
}
