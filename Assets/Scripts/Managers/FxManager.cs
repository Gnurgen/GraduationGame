using UnityEngine;
using System.Collections;

public class FxManager : MonoBehaviour {


    public GameObject playerAttack;
    public GameObject enemyMeleeAttack;
    public GameObject enemyRangedAttack;

	// Use this for initialization
	void Start () {
        GameManager.events.OnPlayerAttack += PlayerAttackEffect;
        GameManager.events.OnEnemyAttack += EnemyMeleeAttackEffect;
        GameManager.events.OnEnemyRangedAttack += EnemyRangedAttackEffect;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void PlayerAttackEffect(GameObject unit)
    {
        GameObject ef = Instantiate(playerAttack) as GameObject;
        ef.transform.position = unit.transform.position + unit.transform.forward.normalized;
        StartCoroutine(DestroyAfter(ef, 2));
    }

    void EnemyMeleeAttackEffect(GameObject unit)
    {
        GameObject ef = Instantiate(enemyMeleeAttack) as GameObject;
        ef.transform.position = unit.transform.position + unit.transform.forward.normalized;
        StartCoroutine(DestroyAfter(ef, 2));
    }

    void EnemyRangedAttackEffect(GameObject unit)
    {
        GameObject ef = Instantiate(enemyRangedAttack) as GameObject;
        ef.transform.position = unit.transform.position + unit.transform.forward.normalized;
        StartCoroutine(DestroyAfter(ef, 3));
    }

    IEnumerator DestroyAfter(GameObject obj, float time)
    {
        if(time > 0)
        {
            yield return new WaitForSeconds(time);
        }
        Destroy(obj);
    }
}
