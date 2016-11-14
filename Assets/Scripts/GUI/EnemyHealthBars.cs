using UnityEngine;
using System.Collections;

public class EnemyHealthBars : MonoBehaviour {
    private float maxSize = 0.8f;
    private float minSize = 0.2f;
    private float maxHealth = 30.0f;
    [Range(0.0f,30)]
    public float health;
    private float hightOfHealthbar = 180;
    private float scale;
    private GameObject enemy;
    private int type;
    Vector3 position;
    private bool activated = false;
    void Start () {

    }
    void Update() {
        if (activated == true)
        {
            healthPosition();
            updateHealthBar();//Could actually just call this whenever enemy takes damage
        }
    }

    void updateHealthBar() {
        scale = minSize+((maxSize-minSize)*(1-((maxHealth-health)/maxHealth))); 
        gameObject.transform.localScale = new Vector3(scale, scale, 0); ;
    }

    void healthPosition() {
        position = enemy.transform.position;
        gameObject.transform.position = position;
        gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y+ hightOfHealthbar, 0);
    }
    public void getMyEnemy(GameObject myEnemy, int myType) {
        enemy = myEnemy;
        type = myType;
        activated = true;   
    }
}
