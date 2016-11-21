using UnityEngine;
using System.Collections;

public class FxManager : MonoBehaviour {

    // Player effects
    public GameObject playerMeleeAttack;
    public GameObject playerMeleeHit;
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
        if(playerMeleeAttack != null)
            GameManager.events.OnPlayerAttack += PlayerMeleeAttackEffect;
        if(playerMeleeHit != null)
            GameManager.events.OnPlayerAttackHit += PlayerMeleeHitEffect;
        if(playerDashBegin != null)
            GameManager.events.OnPlayerDashBegin += PlayerDashBeginEffect;
        if(playerDashEnd != null)
            GameManager.events.OnPlayerDashEnd += PlayerDashEndEffect;
        // Insert ability effects here
        if(playerDeath != null)
            GameManager.events.OnPlayerDeath += PlayerDeathEffect;

        // Subscribe to boss events with effects
        if(bossActivation != null)
            GameManager.events.OnBossActivated += BossActivationEffect;
        if(bossLaserActivation != null)
            GameManager.events.OnBossLaserActivation += BossLaserActivationEffect;
        if(bossMeteorActivation != null)
            GameManager.events.OnBossMeteorActivation += BossMeteorActivationEffect;
        if(bossMeteorImpact != null)
            GameManager.events.OnBossMeteorImpact += BossMeteorImpactEffect;
        if(bossPhaseChange != null)
            GameManager.events.OnBossPhaseChange += BossPhaseChangeEffect;
        if(bossDeath != null)
            GameManager.events.OnBossDeath += BossDeathEffect;

        // Subscribe to basic enemy events with effects
        if(enemyMeleeAttack != null)
            GameManager.events.OnEnemyAttackHit += EnemyMeleeAttackEffect;
        if(enemyRangedAttack != null)
            GameManager.events.OnEnemyAttackHit += EnemyRangedAttackEffect;
        if(enemyDeath != null)
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

    void PlayerMeleeHitEffect(GameObject unit1, GameObject unit2, float dmg)
    {
        GameObject ef = Instantiate(playerMeleeHit) as GameObject;
        ef.transform.position = unit2.transform.position;
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

    void EnemyMeleeAttackEffect(GameObject unit, float val)
    {
        if(unit.GetComponent<MeleeAI>() != null)
        {
            GameObject ef = Instantiate(enemyMeleeAttack) as GameObject;
            ef.transform.position = GameManager.player.transform.position + GameManager.player.transform.forward.normalized;
            StartCoroutine(DestroyAfter(ef, 2));
        }
    }

    void EnemyRangedAttackEffect(GameObject unit, float val)
    {
        if(unit.GetComponent<RangedAI>() != null)
        {
            GameObject ef = Instantiate(enemyRangedAttack) as GameObject;
            ef.transform.position = GameManager.player.transform.position + GameManager.player.transform.forward.normalized;
            StartCoroutine(DestroyAfter(ef, 2));
        }
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
