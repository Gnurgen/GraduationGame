using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;

[RequireComponent (typeof(Seeker))]
[RequireComponent (typeof(Rigidbody))]
public class MeleeAI : EnemyStats {

	public GameObject target;
    private Animator animator;
    //private Animation animation;
	private Seeker seeker;
    private CharacterController cc;
    private Vector3 targetPositionAtPath;
    private Path path;
    private int pathIndex;
    private float nextPointDistance = 2;
    private float recalcPathRange;
    private bool waitingForPath;
    private bool moving;
    private Vector3 yZero = new Vector3(1,0,1);
    private int behindPointCheck = 10;
    private float mySpeed;

   	// Use this for initialization
	void Start () {
       // animation = GetComponent<Animation>();
        animator = GetComponent<Animator>();
		seeker = GetComponent<Seeker> ();
        cc = GetComponent<CharacterController>();
        moving = false;
        StartCoroutine("StateHandler");
        mySpeed = moveSpeed;
	}


	IEnumerator StateHandler()
	{
        for(;;)
        {
            if(target == null)
            {
                yield return StartCoroutine("Idle");
            }
            if(target != null && Vector3.Distance(transform.position, target.transform.position) > attackDist)
            {
                yield return StartCoroutine("Chasing");
            }
            if(target != null && Vector3.Distance(transform.position, target.transform.position) < attackDist)
            {
                yield return StartCoroutine("Attacking");
            }
            yield return null;
        }
	}


	IEnumerator Idle()
	{
        for (;;)
        {
            Debug.Log("Idle");
            if (Vector3.Distance(transform.position, GameManager.player.transform.position) < aggroRange)
            {
                target = GameManager.player;
                yield break;
            }
            yield return null;
        }
	}

	IEnumerator Chasing()
	{
        // If there is a target and it is within aggro range
        for (;;)
        {
            // If there is no target, stop chasing;
            if (target == null)
            {
                yield break;
            }

            if(Vector3.Distance(transform.position, target.transform.position) > aggroRange)
            {
                target = null;
                yield break;
            }

            if (Vector3.Distance(transform.position, target.transform.position) < attackDist)
            {
                yield break;
            }
            // If there target got outside the aggro range, remvoe it and stop chasing
            //if(Vector3.Distance(transform.position, target.transform.position) > aggroRange)
            //{
            //    target = null;
            //    yield break;
            //}

            if (!waitingForPath)
            {
                if(path == null || Vector3.Distance(targetPositionAtPath, target.transform.position) > recalcPathRange)
                {
                    seeker.StartPath(transform.position, target.transform.position, ReceivePath);
                    waitingForPath = true;
                    goto skip;
                }

                if (pathIndex < path.vectorPath.Count - 1)
                {
                    if (Vector3.Distance(transform.position, path.vectorPath[pathIndex]) < nextPointDistance)
                    {
                        pathIndex++;
                    }
                }
                // Rotate towards the current path point and move towards it
                Vector3 dir = (path.vectorPath[pathIndex] - transform.position);
                dir = Quaternion.FromToRotation(transform.forward, dir).eulerAngles;
                dir.x = 0;
                dir.z = 0;
                if (dir.y > 180) //If point is to the right, convert degrees to minus
                    dir.y -= 360;
                if (dir.y > 1)
                    transform.Rotate(Vector3.up * turnRate * Time.fixedDeltaTime);
                else if (dir.y < -1)
                    transform.Rotate(Vector3.down * turnRate * Time.deltaTime);
                else
                    transform.Rotate(dir);
                transform.position += transform.forward * mySpeed * Time.fixedDeltaTime;
                
            }
            skip:
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
    }

	IEnumerator Attacking()
	{
		for(;;)
		{
			// Define attacking
            if(target == null)
            {
                yield break;
            }
            if(Vector3.Distance(transform.position, target.transform.position) > attackDist)
            {
                yield break;
            }

            Vector3 dir = (target.transform.position - transform.position).normalized;
            if (Vector3.Dot(dir, transform.forward) >= 0.8f)
            {
                animator.SetTrigger("Attack");
                Weapon.GetComponent<EnemyMeleeAttack>().Swing(true);
                while(animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                {
                    yield return null; 
                }
                Weapon.GetComponent<EnemyMeleeAttack>().Swing(false);
                yield break;
            }
            dir = Quaternion.FromToRotation(transform.forward, dir).eulerAngles;
            dir.x = 0;
            dir.z = 0;
            if (dir.y > 180) //If point is to the right, convert degrees to minus
                dir.y -= 360;
            if (dir.y > 1)
                transform.Rotate(Vector3.up * turnRate * Time.fixedDeltaTime);
            else if (dir.y < -1)
                transform.Rotate(Vector3.down * turnRate * Time.deltaTime);
            else
                transform.Rotate(dir);
                yield return null;
		}
	}

    void ReceivePath(Path path)
    {
        if(!path.error)
        {
            this.path = path;
            waitingForPath = false;
            pathIndex = 0;
            if (target != null)
            {
                targetPositionAtPath = target.transform.position;
            }
           // AnalysePath();
            moving = true;
        }
        else
        {
            if(target != null)
            {
                seeker.StartPath(transform.position, target.transform.position, ReceivePath);
            }
        }
    }

    void AnalysePath()
    {
        Vector3 dir = (path.vectorPath[pathIndex] - transform.position).normalized;
        if(Vector3.Dot(dir, transform.forward) < 0)
        {
            ++pathIndex;
            if (behindPointCheck+pathIndex>= path.vectorPath.Count)
                behindPointCheck = path.vectorPath.Count-pathIndex;
            for(int x = pathIndex; x<behindPointCheck+pathIndex; ++x)
            {
                dir = (path.vectorPath[x] - transform.position).normalized;
                if (Vector3.Dot(dir, transform.forward) < 0)
                    ++pathIndex;
            }
        }
    }
}
