using UnityEngine;
using System.Collections;

public class EnemyHealthBars : MonoBehaviour {
    private float maxSize = 0.8f;
    private float minSize = 0.2f;
    private float maxHealth = 30.0f;
    [Range(0.0f,30)]
    public float health;
    public float hightOfHealthbar;
    private float scale;
    public GameObject enemy;
    Vector3 position;

    void Start () {

    }
    void Update() {
        updateHealthBar();//Could actually just call this whenever enemy takes damage
        healthPosition();
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
}
