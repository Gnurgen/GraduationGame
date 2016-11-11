using UnityEngine;
using System.Collections;
using Pathfinding;

public class PlayerControls : MonoBehaviour {

    private Animator anim;
    private InputManager IM;
    public AnimationCurve DashCurve;
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
    private float nextPointDistance = 1;
    private bool waitingForPath;
    private int pathIndex;
    private bool shouldMove;
    private Vector3 target;
    private float currentDashCooldown;
    private float currentAttackSpeed;

	//test
	Vector3 ray;


    private float navmeshSpeed;

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
        anim = GetComponent<Animator>();
    }

    void MoveTo(Vector2 point)
    {
        if(!dashing && !attacking)
        {
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
        if (waitingForPath && shouldMove)
        {
            Vector3 dir = (target - transform.position).normalized;
            dir *= moveSpeed * Time.fixedDeltaTime;
            transform.position += dir;
            //cc.SimpleMove(dir);
        }
        else if (!waitingForPath && shouldMove)
        {
            Debug.Log("Moving");
            if (pathIndex < path.vectorPath.Count - 1)
            {
                if (Vector3.Distance(transform.position, path.vectorPath[pathIndex]) < nextPointDistance)
                {
                    pathIndex++;
                }
            }
            Vector3 dir = Vector3.Normalize(path.vectorPath[pathIndex] - transform.position);
            dir *= moveSpeed * Time.fixedDeltaTime;
            transform.position += dir;
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
        for(;;)
        {
            if(Vector3.Distance(transform.position, target) < 1)
            {
                dashing = false;
                yield break;
            }
            Vector3 dir = (target - transform.position).normalized;
            dir *= moveSpeed * dashingSpeed * Time.deltaTime;
            transform.position += dir;
            yield return null;
        }
    }
}
