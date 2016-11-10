using UnityEngine;
using System.Collections;

public class EnemyHealthBars : MonoBehaviour {
    private float maxSize = 0.8f;
    private float minSize = 0.2f;
    private float maxHealth = 30.0f;
    [Range(0.0f,30)]
    public float health;
    private float scale;

	void Start () {

    }
    void Update() {
        updateHealthBar();//Could actually just call this whenever enemy takes damage
    }

    void updateHealthBar() {
        scale = minSize+((maxSize-minSize)*(1-((maxHealth-health)/maxHealth))); 
        gameObject.transform.localScale = new Vector3(scale, scale, 0); ;
    }
}
