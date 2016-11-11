using UnityEngine;
using System.Collections;
using Pathfinding;

public class PlayerControls : MonoBehaviour {

   
    private InputManager IM;
   
    public bool dashing, attacking;
    public float dashingSpeed;
    public float moveSpeed;
    float dashingDis, dashingStartDis;
    float cameraRotation;
	int ID;
    public float attackSpeed;
    public float dashCooldown;

    // Pathfinding
    private Path path;
    private Seeker seeker;
    private float nextPointDistance = 1.2f;
    private bool waitingForPath;
    private int pathIndex;
    private bool shouldMove;
    private Vector3 target;
    private float currentDashCooldown;
    private float currentAttackSpeed;
    private Vector3 dir;

    // Use this for initialization
    void Start () {
        IM = FindObjectOfType<InputManager>();
		IM.OnTapSub (MoveTo, ID);
		IM.OnSwipeSub (AttackDir, ID);
		IM.OnDoubleTapSub (DashTo, ID);
        cameraRotation = FindObjectOfType<Camera>().transform.rotation.y;
        seeker = GetComponent<Seeker>();
        shouldMove = false;
	}
	
	void Awake()
    {
       
    }

    void MoveTo(Vector2 point)
    {
        if(!dashing && !attacking)
        {
            GameManager.events.PlayerMove(gameObject);
            shouldMove = true;
            target = IM.GetWorldPoint(point);
            seeker.StartPath(transform.position, target, ReceivePath);
            waitingForPath = true;
        }
        //if(!dashing && !attacking)
        //{
        //    anim.SetBool("Run",true);
        //}
    }
    void DashTo(Vector2 point)
    {
        if(currentDashCooldown < 0)
        {
            GameManager.events.PlayerDashBegin(gameObject);
            shouldMove = false;
            target = IM.GetWorldPoint(point);
            dashing = true;
            StartCoroutine("Dash");
            currentDashCooldown = dashCooldown;
        }

       // if (!dashing && !attacking)
        //{
        //    gamemanager.events.playerdashbegin(gameobject);
        //    anim.settrigger("dash");
        //    dashing = true;
		//	dashingstartdis = vector3.distance(transform.position, im.getworldpoint(point));
        //}
    }
	void AttackDir(InputManager.Swipe swipe)
    {
        if(currentAttackSpeed < 0)
        {
            GameManager.events.PlayerAttack(gameObject);
            attacking = true;
            // do attack;
            Debug.Log("Attack blob");
            transform.LookAt(transform.position + IM.GetWorldPoint(swipe.end) - IM.GetWorldPoint(swipe.begin));

            attacking = false;
        }
        //if (!dashing && !attacking)
        //{
        //    anim.SetTrigger("Attack");
		//	transform.LookAt(transform.position + IM.GetWorldPoint(swipe.end) - IM.GetWorldPoint (swipe.begin));
		//	ray = transform.position + IM.GetWorldPoint (swipe.end) - IM.GetWorldPoint (swipe.begin);
        //    attacking = true;
        //}
    }
    void FixedUpdate()
    {
        currentDashCooldown -= Time.fixedDeltaTime;
        currentAttackSpeed -= Time.fixedDeltaTime;
        
        if (waitingForPath && shouldMove && false) // DET ER IKKE MED NU
        {
            if(Vector3.Distance(transform.position, target) > nextPointDistance)
            {
                dir = (target - transform.position).normalized;
                dir = Quaternion.FromToRotation(transform.forward, dir).eulerAngles;
                dir.x = 0;
                dir.z = 0;
                if (dir.y > 180) //If point is to the right, convert degrees to minus
                    dir.y -= 360;
                transform.Rotate(dir);
                transform.position += transform.forward * moveSpeed * Time.fixedDeltaTime;
            }
            else
            {
                GameManager.events.PlayerIdle(gameObject);
                shouldMove = false;
            }
        }
        else if (!waitingForPath && shouldMove)
        {
           
            if (pathIndex < path.vectorPath.Count - 1)
            {
                if (Vector3.Distance(transform.position, path.vectorPath[pathIndex]) < nextPointDistance)
                {
                    pathIndex++;
                }
            }
            else if (Vector3.Distance(transform.position, path.vectorPath[pathIndex]) < nextPointDistance)
            {
                shouldMove = false;
                GameManager.events.PlayerIdle(gameObject);
            }
            dir = (path.vectorPath[pathIndex] - transform.position).normalized;
            dir = Quaternion.FromToRotation(transform.forward, dir).eulerAngles;
            dir.x = 0;
            dir.z = 0;
            if (dir.y > 180) //If point is to the right, convert degrees to minus
                dir.y -= 360;
            transform.Rotate(dir);
            transform.position += transform.forward * moveSpeed * Time.fixedDeltaTime;

        }
    }

    void ReceivePath(Path path)
    {
        if (!path.error)
        {
            this.path = path;
            waitingForPath = false;
            pathIndex = 0;
            while (IsBehind(path.vectorPath[pathIndex]))
            {
                pathIndex++;
            }
        }
    }

    bool IsBehind(Vector3 p)
    {
        return false;
    }

    IEnumerator Dash()
    {
        print("dash");
       
        for(;;)
        {
            if(Vector3.Distance(transform.position, target) < nextPointDistance)
            {
                GameManager.events.PlayerDashEnd(gameObject);
                dashing = false;
                yield break;
            }

            dir = (target - transform.position).normalized;
            dir = Quaternion.FromToRotation(transform.forward, dir).eulerAngles;
            dir.x = 0;
            dir.z = 0;
            transform.Rotate(dir);
            transform.position += transform.forward  * dashingSpeed * Time.fixedDeltaTime;

            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
    }
}
