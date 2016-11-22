using UnityEngine;
using System.Collections;

public class MeteorShower : MonoBehaviour {


    public float MaxRangeOfAttack = 10;
    public float MinRangeOfAttack = 1;
    public float AreaOfBoulder = 4;
    public float FallAnimationSeconds = 0.3f;
    public float FallTime = 4;
    public float SecondsOfExpanding= 1;
    public float HeightOfFall = 10;
    public GameObject Boulder, Fallarea;
    public float Damage;
    [SerializeField]
    private float meteorForce;

    // Use this for initialization
    void Start () {
	
	}
	
    public void Activate(int meteors)
    {
        for(int i = 0; i < meteors; i++)
        {
            StartCoroutine("BoulderFall");
        }
    }
    IEnumerator BoulderFall()
    {
        Vector2 pos = Random.insideUnitCircle * (MaxRangeOfAttack - MinRangeOfAttack);
        Vector3 actual = GameManager.player.transform.position;
        GameObject fallArea = Instantiate(Fallarea, actual, Quaternion.identity) as GameObject;
        float step = SecondsOfExpanding;
        while(step > 0) // increase size of fallArea to areaBoulder in SecondsOfExpanding
        {
            step -= 1 * Time.deltaTime;
            fallArea.transform.localScale = new Vector3((SecondsOfExpanding - step) / SecondsOfExpanding * AreaOfBoulder, .1f, (SecondsOfExpanding - step) / SecondsOfExpanding * AreaOfBoulder);
            yield return null;
        }
        step = FallTime - SecondsOfExpanding;

        while(step>FallAnimationSeconds) // wait for boulder to spawn
        {
            step -= 1 * Time.deltaTime;
            yield return null;
        }

        GameObject boulder = Instantiate(Boulder, actual+ Vector3.up * HeightOfFall, Quaternion.identity) as GameObject;
       
        while(step>0) // Let the boulder fall HeightOfFall distance in FallAnimationSeconds
        {
            step -= 1 * Time.deltaTime;
            boulder.transform.position = Vector3.Lerp(actual + Vector3.up * HeightOfFall, actual, 1 - step / FallAnimationSeconds);
            yield return null;
        }
        Collider[] hit = Physics.OverlapSphere(fallArea.transform.position, AreaOfBoulder);
        for (int i = 0; i < hit.Length; i++)
        {
            if(hit[i].tag == "Player")
            {
                GameManager.events.EnemyAttackHit(gameObject, Damage);
                hit[i].GetComponent<Health>().decreaseHealth(Damage, (GameManager.player.transform.position-actual), meteorForce);
            }
        }
        
        Destroy(fallArea);
    }
}
