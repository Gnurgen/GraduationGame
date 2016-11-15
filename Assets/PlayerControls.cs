using UnityEngine;
using System.Collections;
using Pathfinding;

public class PlayerControls : MonoBehaviour {

   
    private InputManager im;
    private EventManager em;
    public enum State {Idle, Attacking, Moving, Dashing, Ability};
    public State state;


    // ----- Inspector -----
    public float moveSpeed = 4;
    public float dashDuration = 0.5f;
    public float dashSpeedMultiplier = 3;
    public float attackDuration = 0.3f;

    private Path path;
    private int pathIndex;
    private Vector3 moveDir;
    private Vector2 middleScreen;
    private Seeker seeker;
    private int id;
    private Camera mainCamera;
    private bool shouldMove;

    // Use this for initialization
    void Start () {
        im = FindObjectOfType<InputManager>();
        em = FindObjectOfType<EventManager>();
        em.OnWheelOpen += disableMovement;
        em.OnWheelSelect += enableMovement;
        seeker = GetComponent<Seeker>();
        id = im.GetID();
        im.OnFirstTouchBeginSub(Begin, id);
        im.OnFirstTouchMoveSub(Move, id);
        im.OnFirstTouchEndSub(End, id);
        im.OnDoubleTapSub(DoubleTap, id);
        im.OnSwipeSub(Swipe, id);
        StartCoroutine(Idle());
        middleScreen = new Vector2(Screen.width / 2, Screen.height / 2);
        Debug.Log(middleScreen);
        mainCamera = FindObjectOfType<Camera>();
        shouldMove = true;
	}
	
	void Awake()
    {
       
    }

    IEnumerator Idle()
    {
        state = State.Idle;
        em.PlayerIdle(gameObject);
        path = null;
        while(state == State.Idle)
        {
            yield return new WaitForSeconds(0.02f);
        }
        yield break;
    }

    IEnumerator Moving()
    {
        state = State.Moving;
        em.PlayerMove(gameObject);
        while (state == State.Moving && shouldMove)
        {
            transform.position += transform.forward * moveSpeed * Time.fixedDeltaTime;
            yield return new WaitForSeconds(0.02f);
        }
        StartCoroutine(Idle());
        yield break;
    }

    IEnumerator Attacking(InputManager.Swipe s)
    {
        state = State.Attacking;
        em.PlayerAttack(gameObject);
        float currentAttackDuration = attackDuration;
        while(currentAttackDuration > 0)
        {
            currentAttackDuration -= Time.fixedDeltaTime;
            yield return new WaitForSeconds(0.02f);
        }
        StartCoroutine(Idle());
        yield return null;
    }

    IEnumerator Dashing(Vector3 p)
    {
        state = State.Dashing;
        em.PlayerDashBegin(gameObject);
        float currentDashDuration = dashDuration;
        while(currentDashDuration > 0)
        {
            currentDashDuration -= Time.fixedDeltaTime;
            transform.position += transform.forward * moveSpeed * dashSpeedMultiplier * Time.fixedDeltaTime;
            yield return new WaitForSeconds(0.02f);
        }
        em.PlayerDashEnd(gameObject);
        StartCoroutine(Idle());
        yield return null;
    }

    void Begin(Vector2 p)
    {
        if(state != State.Dashing && state != State.Attacking)
        {
            Vector2 tempDir = p - middleScreen;
            moveDir = new Vector3(tempDir.x, 0, tempDir.y);
            moveDir = Camera.main.transform.TransformDirection(moveDir).normalized;
            moveDir.y = 0;
            moveDir = Quaternion.FromToRotation(transform.forward, moveDir).eulerAngles;
            moveDir.x = 0;
            moveDir.z = 0;
            transform.Rotate(moveDir);
            if (state == State.Idle)
            {
                StartCoroutine(Moving());
            }
        }
    } 
    void Move(Vector2 p)
    {
        if (state != State.Dashing && state != State.Attacking)
        {
            Vector2 tempDir = p - middleScreen;
            moveDir = new Vector3(tempDir.x, 0, tempDir.y);
            moveDir = Camera.main.transform.TransformDirection(moveDir).normalized;
            moveDir.y = 0;
            moveDir = Quaternion.FromToRotation(transform.forward, moveDir).eulerAngles;
            moveDir.x = 0;
            moveDir.z = 0;
            transform.Rotate(moveDir);
            if (state == State.Idle)
            {
                StartCoroutine(Moving());
            }
        }
    }

    void End(Vector2 p)
    {
        if (state != State.Dashing && state != State.Attacking)
        {
            StartCoroutine(Idle());
        }
    }

    void disableMovement()
    {
        shouldMove = false;
    }

    void enableMovement(int i)
    {
        shouldMove = true;
    }

    void Tap(Vector2 p)
    {

    }

    void DoubleTap(Vector2 p)
    {
        if(state == State.Idle || state == State.Moving)
        {
            StartCoroutine(Dashing(im.GetWorldPoint(p)));
        }
    }

    void Swipe(InputManager.Swipe s)
    {
        if(state == State.Idle || state == State.Moving)
        {
            StartCoroutine(Attacking(s));
        }
    }

    /*// Used to receive a new path and start calculating a new path
    void ReceivePath(Path path)
    {
        if (!path.error && state == State.Moving)
        {
            this.path = path;
            pathIndex = 0;
            if(Vector3.Distance(transform.position, target) > 2)
            {
                target = im.GetWorldPoint(screenTarget);
                seeker.StartPath(transform.position, target, ReceivePath);
            }
        }
    }/*




    /*
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
            transform.LookAt(transform.position + IM.GetWorldPoint(swipe.end) - IM.GetWorldPoint(swipe.begin));
            Collider[] hit = Physics.OverlapSphere(transform.position, attackRange);

            foreach(Collider c in hit)
            {
                if(c.tag == "Melee")
                {
                    c.GetComponent<Health>().decreaseHealth(damage);
                    GameManager.events.PlayerAttackHit(gameObject, c.gameObject, damage);
                }
            }


            currentAttackSpeed = attackSpeed;
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
    }*/

}
