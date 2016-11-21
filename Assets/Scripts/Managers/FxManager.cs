using UnityEngine;
using System.Collections;

public class FxManager : MonoBehaviour {

    // Player effects
    public GameObject playerMeleeAttack;
    public GameObject playerDashBegin;
    public GameObject playerDashEnd;
    public GameObject flyingSpearHit;
    public GameObject coneHit;
    public GameObject playerDeath;

    // Boss effects
    public GameObject bossActivation;
    public GameObject bossLaserActivation;
    public GameObject bossMeteorActivation;
    public GameObject bossMeteorImpact;
    public GameObject bossPhaseChange;
    public GameObject bossDeath;

    // Basic enemy effects
    public GameObject enemyMeleeAttack;
    public GameObject enemyRangedAttack;
    public GameObject enemyDeath;

	// Use this for initialization
	void Start () {
        // Subscribe to player events with effects
        GameManager.events.OnPlayerAttack += PlayerMeleeAttackEffect;
        GameManager.events.OnPlayerDashBegin += PlayerDashBeginEffect;
        GameManager.events.OnPlayerDashEnd += PlayerDashEndEffect;
        // Insert ability effects here
        GameManager.events.OnPlayerDeath += PlayerDeathEffect;

        // Subscribe to boss events with effects
        GameManager.events.OnBossActivated += BossActivationEffect;
        GameManager.events.OnBossLaserActivation += BossLaserActivationEffect;
        GameManager.events.OnBossMeteorActivation += BossMeteorActivationEffect;
        GameManager.events.OnBossMeteorImpact += BossMeteorImpactEffect;
        GameManager.events.OnBossPhaseChange += BossPhaseChangeEffect;
        GameManager.events.OnBossDeath += BossDeathEffect;

        // Subscribe to basic enemy events with effects
        GameManager.events.OnEnemyAttack += EnemyMeleeAttackEffect;
        GameManager.events.OnEnemyRangedAttack += EnemyRangedAttackEffect;
        GameManager.events.OnEnemyDeath += EnemyDeathEffect;
	}

    /* -------------------------------------------------------------
     * -------------------------------------------------------------
     * ---------------------- PLAYER EFFECTS -----------------------
     * -------------------------------------------------------------
     * -------------------------------------------------------------
    */

    void PlayerMeleeAttackEffect(GameObject unit)
    {
        GameObject ef = Instantiate(playerMeleeAttack) as GameObject;
        ef.transform.position = unit.transform.position + unit.transform.forward.normalized;
        StartCoroutine(DestroyAfter(ef, 2));
    }

    void PlayerDashBeginEffect(GameObject unit)
    {
        GameObject ef = Instantiate(playerDashBegin) as GameObject;
        ef.transform.position = unit.transform.position + unit.transform.forward.normalized;
        StartCoroutine(DestroyAfter(ef, 2));
    }

    void PlayerDashEndEffect(GameObject unit)
    {
        GameObject ef = Instantiate(playerDashEnd) as GameObject;
        ef.transform.position = unit.transform.position + unit.transform.forward.normalized;
        StartCoroutine(DestroyAfter(ef, 2));
    }

    void PlayerDeathEffect(GameObject unit)
    {
        GameObject ef = Instantiate(playerDeath) as GameObject;
        ef.transform.position = unit.transform.position + unit.transform.forward.normalized;
        StartCoroutine(DestroyAfter(ef, 2));
    }


    /* -------------------------------------------------------------
     * -------------------------------------------------------------
     * ----------------------- BOSS EFFECTS ------------------------
     * -------------------------------------------------------------
     * -------------------------------------------------------------
    */

    void BossActivationEffect(GameObject unit)
    {
        GameObject ef = Instantiate(bossActivation) as GameObject;
        ef.transform.position = unit.transform.position + unit.transform.forward.normalized;
        StartCoroutine(DestroyAfter(ef, 2));
    }

    void BossLaserActivationEffect(GameObject unit)
    {
        GameObject ef = Instantiate(bossLaserActivation) as GameObject;
        ef.transform.position = unit.transform.position + unit.transform.forward.normalized;
        StartCoroutine(DestroyAfter(ef, 2));
    }

    void BossMeteorActivationEffect(GameObject unit)
    {
        GameObject ef = Instantiate(bossMeteorActivation) as GameObject;
        ef.transform.position = unit.transform.position + unit.transform.forward.normalized;
        StartCoroutine(DestroyAfter(ef, 2));
    }

    void BossMeteorImpactEffect(GameObject unit)
    {
        GameObject ef = Instantiate(bossMeteorImpact) as GameObject;
        ef.transform.position = unit.transform.position + unit.transform.forward.normalized;
        StartCoroutine(DestroyAfter(ef, 2));
    }

    void BossPhaseChangeEffect(GameObject unit)
    {
        GameObject ef = Instantiate(bossPhaseChange) as GameObject;
        ef.transform.position = unit.transform.position + unit.transform.forward.normalized;
        StartCoroutine(DestroyAfter(ef, 2));
    }

    void BossDeathEffect(GameObject unit)
    {
        GameObject ef = Instantiate(bossDeath) as GameObject;
        ef.transform.position = unit.transform.position + unit.transform.forward.normalized;
        StartCoroutine(DestroyAfter(ef, 2));
    }

    /* -------------------------------------------------------------
     * -------------------------------------------------------------
     * -------------------- BASIC ENEMY EFFECTS --------------------
     * -------------------------------------------------------------
     * -------------------------------------------------------------
    */

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
        StartCoroutine(DestroyAfter(ef, 2));
    }

    void EnemyDeathEffect(GameObject unit)
    {
        GameObject ef = Instantiate(enemyDeath) as GameObject;
        ef.transform.position = unit.transform.position + unit.transform.forward.normalized;
        StartCoroutine(DestroyAfter(ef, 2));
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
