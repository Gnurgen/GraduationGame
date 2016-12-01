using UnityEngine;
using System.Collections;
using System;

public class FxManager : MonoBehaviour {

    // Player effects
    public GameObject playerMeleeAttack;
    public GameObject playerMeleeHit;
    public GameObject playerDashBegin;
    public GameObject playerDashEnd;
    public GameObject flyingSpearHit;
    public GameObject coneHit;
    public GameObject playerDeath;
    public GameObject playerPickup;
    public GameObject playerSpearAttack;
   

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
    public GameObject enemyMeleeAttackHit;
    public GameObject enemyRangedAttackHit;
    public GameObject enemyDeath;
    public GameObject RagdollDespawn;

    //variable caching
    private Transform spearTip;

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
        if (flyingSpearHit != null)
            GameManager.events.OnSpearDrawAbilityHit += SpearHitEffect;

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
        if (RagdollDespawn != null)
            GameManager.events.OnEnemyRagdollDespawn += RagdollDespawnEffect;
        if (playerPickup != null)
            GameManager.events.OnResourcePickup += ResourcePickup;
	}

   


    /* -------------------------------------------------------------
     * -------------------------------------------------------------
     * ---------------------- PLAYER EFFECTS -----------------------
     * -------------------------------------------------------------
     * -------------------------------------------------------------
    */

    void PlayerMeleeAttackEffect(GameObject unit)
    {
        GameObject ef =  GameManager.pool.GenerateObject(playerMeleeAttack.tag);
        ef.transform.position = new Vector3(ef.transform.position.x, 1.0f, ef.transform.position.z);
        StartCoroutine(DestroyAfter(ef, 2));
        ef.GetComponent<PKFxFX>().StartEffect();
    }

    void PlayerMeleeHitEffect(GameObject unit1, GameObject unit2, float dmg)
    {
        GameObject ef =  GameManager.pool.GenerateObject(playerMeleeHit.tag); 
        ef.transform.position = unit2.transform.position;
        StartCoroutine(DestroyAfter(ef, 2));
        ef.GetComponent<PKFxFX>().StartEffect();

    }

    void PlayerDashBeginEffect(GameObject unit)
    {
        GameObject ef =  GameManager.pool.GenerateObject(playerDashBegin.tag); 
        ef.transform.position = unit.transform.position;
        StartCoroutine(DestroyAfter(ef, 2));
        ef.GetComponent<PKFxFX>().StartEffect();
    }

    void PlayerDashEndEffect(GameObject unit)
    {
        GameObject ef =  GameManager.pool.GenerateObject(playerDashEnd.tag); 
        ef.transform.position = unit.transform.position;
        ef.GetComponent<PKFxFX>().StartEffect();

        StartCoroutine(DestroyAfter(ef, 2));
    }

    void PlayerDeathEffect(GameObject unit)
    {
        GameObject ef =  GameManager.pool.GenerateObject(playerDeath.tag); 
        ef.transform.position = unit.transform.position;
        ef.GetComponent<PKFxFX>().StartEffect();
        StartCoroutine(DestroyAfter(ef, 2));
    }
    private void ResourcePickup(GameObject unit, int amount)
    {
        GameObject ef = GameManager.pool.GenerateObject(playerPickup.tag);
        ef.transform.SetParent(GameManager.player.transform);
        ef.transform.localPosition = Vector3.zero;
        
        ef.GetComponent<PKFxFX>().StartEffect();
        StartCoroutine(DestroyAfter(ef, 2));
    }

    private void SpearEffect()
    {
        GameObject ef = GameManager.pool.GenerateObject(playerSpearAttack.tag);
        ef.transform.position = spearTip.position;
        ef.GetComponent<PKFxFX>().StartEffect();
        StartCoroutine(DestroyAfter(ef, 2));
    }

    void SpearHitEffect(GameObject id)
    {
        
    }


    /* -------------------------------------------------------------
     * -------------------------------------------------------------
     * ----------------------- BOSS EFFECTS ------------------------
     * -------------------------------------------------------------
     * -------------------------------------------------------------
    */

    void BossActivationEffect(GameObject unit)
    {
        GameObject ef =  GameManager.pool.GenerateObject(bossActivation.tag); 
        ef.transform.position = unit.transform.position;
        ef.GetComponent<PKFxFX>().StartEffect();
        StartCoroutine(DestroyAfter(ef, 2));
    }

    void BossLaserActivationEffect(GameObject unit)
    {
        GameObject ef =  GameManager.pool.GenerateObject(bossLaserActivation.tag); 
        ef.transform.position = unit.transform.position;
        ef.GetComponent<PKFxFX>().StartEffect();
        StartCoroutine(DestroyAfter(ef, 2));
    }

    void BossMeteorActivationEffect(GameObject unit)
    {
        GameObject ef =  GameManager.pool.GenerateObject(bossMeteorActivation.tag); 
        ef.transform.position = unit.transform.position ;
        ef.GetComponent<PKFxFX>().StartEffect();
        StartCoroutine(DestroyAfter(ef, 2));
    }

    void BossMeteorImpactEffect(GameObject unit)
    {
        GameObject ef =  GameManager.pool.GenerateObject(bossMeteorImpact.tag); 
        ef.transform.position = unit.transform.position;
        ef.GetComponent<PKFxFX>().StartEffect();
        StartCoroutine(DestroyAfter(ef, 2));
    }

    void BossPhaseChangeEffect(GameObject unit)
    {
        GameObject ef =  GameManager.pool.GenerateObject(bossPhaseChange.tag); 
        ef.transform.position = unit.transform.position;
        ef.GetComponent<PKFxFX>().StartEffect();
        StartCoroutine(DestroyAfter(ef, 2));
    }

    void BossDeathEffect(GameObject unit)
    {
        GameObject ef =  GameManager.pool.GenerateObject(bossDeath.tag); 
        ef.transform.position = unit.transform.position;
        ef.GetComponent<PKFxFX>().StartEffect();
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
            GameObject ef =  GameManager.pool.GenerateObject(enemyMeleeAttack.tag); 
            ef.transform.position = GameManager.player.transform.position + Vector3.up;
            ef.GetComponent<PKFxFX>().StartEffect();
            StartCoroutine(DestroyAfter(ef, 2));
        }
    }

    void EnemyRangedAttackEffect(GameObject unit, float val)
    {
        if(unit.GetComponent<RangedAI>() != null)
        {
            GameObject ef =  GameManager.pool.GenerateObject(enemyRangedAttack.tag); 
            ef.transform.position = GameManager.player.transform.position ;
        ef.GetComponent<PKFxFX>().StartEffect();
            StartCoroutine(DestroyAfter(ef, 2));
        }
    }

    void EnemyDeathEffect(GameObject unit)
    {
        GameObject ef =  GameManager.pool.GenerateObject(enemyDeath.tag); 
        ef.transform.position = unit.transform.position;
        ef.GetComponent<PKFxFX>().StartEffect();
        StartCoroutine(DestroyAfter(ef, 2));
    }

    private void RagdollDespawnEffect(GameObject unit) 
    {

        GameObject whisp = GameManager.pool.GenerateObject("Whisp");
        Vector3 pos = unit.transform.GetChild(0).GetChild(10).position;
        whisp.transform.position = new Vector3(pos.x, 1, pos.z);
        GameObject ef = GameManager.pool.GenerateObject(RagdollDespawn.tag);
        ef.transform.position = pos; // Special case for ragdoll position
        ef.GetComponent<PKFxFX>().StartEffect();
        StartCoroutine(DestroyAfter(ef, 2));
    }


    IEnumerator DestroyAfter(GameObject obj, float time) // POOL INSTEAD OF DESTROY (fixing)
    {
        if(time > 0)
        {
            yield return new WaitForSeconds(time);
        }
        obj.GetComponent<PKFxFX>().StopEffect();
        GameManager.pool.PoolObj(obj);
    }

}
