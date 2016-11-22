using UnityEngine;
using System.Collections;

public class MeteorShower : MonoBehaviour {


    public float AreaAroundPlayerRadius = 2;
    
    public float AreaOfBoulder = 4;
    public float FallAnimationSeconds = 0.3f;
    public float FallTime = 5;
    
    public float HeightOfFall = 10;
    public GameObject Boulder, Fallarea;
    public float Damage;

    private bool Face = false, MoveHead = false;
    [SerializeField]
    private float meteorForce, turnSpeed;

    // Use this for initialization
    void Start () {
	
	}
	
    public void Activate(int meteors)
    {
        for(int i = 0; i < meteors; i++)
        {
            MoveHead = true;
        }
    }
    void Update()
    {
        if (MoveHead)
        {

            Vector3 face = Face ? transform.forward : -transform.forward;
            Vector3 dir = Quaternion.FromToRotation(face, GameManager.player.transform.position - transform.position).eulerAngles;
            dir.x = 0;
            dir.z = 0;
            if (dir.y < 5 || dir.y > 355)
            {
                transform.Rotate(dir);
                StartCoroutine(BoulderFall());
                MoveHead = false;
            }
            else if (dir.y > 180)
                transform.Rotate(-dir.normalized * turnSpeed * Time.deltaTime);
            else
                transform.Rotate(dir.normalized * turnSpeed * Time.deltaTime);
        }
    }
    IEnumerator BoulderFall()
    {
        
        Vector2 pos = Random.insideUnitCircle * AreaAroundPlayerRadius;
        Vector3 actual = GameManager.player.transform.position + new Vector3(pos.x,0,pos.y);
        GameObject fallArea = Instantiate(Fallarea, actual, Quaternion.identity) as GameObject;
        GameManager.events.BossMeteorActivation(fallArea); // fallArea for Sounds (the impact sound is this event)
        float step =0;
      
        step = FallTime;
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
        GameManager.events.BossMeteorImpact(boulder);
        Collider[] hit = Physics.OverlapSphere(fallArea.transform.position, AreaOfBoulder);
        for (int i = 0; i < hit.Length; i++)
        {
            if(hit[i].tag == "Player")
            {
                GameManager.events.EnemyAttackHit(gameObject, Damage);
                hit[i].GetComponent<Health>().decreaseHealth(Damage, (GameManager.player.transform.position-actual), meteorForce);
            }
        }
        Face = !Face;
        Destroy(boulder);
        Destroy(fallArea);
    }
}
