using UnityEngine;
using System.Collections;

public class ProjectileControl : MonoBehaviour {

    public float speed = 3f;

    private Vector3 direction;
    private PKFxFX effectControl;
    private RangedAI owner;
    private float damage;
    private float force;
    private bool subbed = false;

    public void Activate(Vector3 start, Vector3 target, RangedAI owner)
    {
        if(!subbed)
            GameManager.events.OnLoadNextLevel += KillSound;
        this.owner = owner;
        damage = owner.damage;
        force = owner.force;
        speed = owner.projectileSpeed;
        effectControl = gameObject.GetComponentInChildren<PKFxFX>();
        effectControl.StartEffect();
        direction = (new Vector3(target.x, 0, target.z) - new Vector3(start.x, 0, start.z)).normalized;
        transform.position = start;
        StartCoroutine(Launch());
    }

    IEnumerator Launch()
    {
        GameManager.events.EnemyRangedAttack(gameObject);
        transform.GetChild(0).transform.localPosition = direction * 0.1f * speed;
        float t = 30f;
        while(t > 0)
        {
            transform.position += direction * speed * Time.deltaTime;
            t -= Time.deltaTime;
            yield return null;
        }
        destroy();
        yield break;
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Player")
        {
            GameManager.events.EnemyAttackHit(gameObject, damage);
            col.gameObject.GetComponent<Health>().decreaseHealth(damage, col.transform.position - transform.position, force);
            GameObject eff = GameManager.pool.GenerateObject("p_EnemyRangedAttackHit");
            eff.transform.position = transform.position;
            eff.GetComponent<PKFxFX>().StartEffect();
            eff.GetComponent<PoolDelay>().DelayPool(0.5f);
            destroy();
        }
        else if(col.gameObject.tag == "Enemy")
        {
            // Do nothing
        }
        else
        {
            GameManager.events.EnemyRangedMiss(gameObject);
            GameObject eff = GameManager.pool.GenerateObject("p_EnemyRangedAttackHit");
            eff.transform.position = transform.position;
            eff.GetComponent<PKFxFX>().StartEffect();
            eff.GetComponent<PoolDelay>().DelayPool(0.5f);
            destroy();
        }
    }

    public void destroy()
    {
        effectControl.StopEffect();
        transform.position = new Vector3(0, -100000, 0);
        GameManager.pool.PoolObj(gameObject); 
    }

    void KillSound()
    {
        AkSoundEngine.PostEvent("Enemy_Ranged_Projectile_Stop", gameObject);
        AkSoundEngine.RenderAudio();
    }
}
