using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;

[RequireComponent (typeof(Seeker))]
[RequireComponent (typeof(Rigidbody))]
public class MeleeAI : EnemyStats {
    

	public GameObject target;
    private float targetDist;
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
    private int behindPointCheck = 10;
    private float mySpeed;
    private Vector3 startPosition;

   	// Use this for initialization
	void Start () {
       // animation = GetComponent<Animation>();
        animator = GetComponent<Animator>();
		seeker = GetComponent<Seeker> ();
        StartCoroutine(Waiting(3));
        mySpeed = moveSpeed;
        startPosition = transform.position;
	}

    public void Taunt(GameObject newTarget)
    {
        target = newTarget;
    }

    IEnumerator Waiting(float sec)
    {
        while(sec > 0)
        {
            sec -= Time.deltaTime;
            yield return null;
        }
        StartCoroutine(Idle());
        yield break;
    }


	IEnumerator Idle()
	{
        for (;;)
        {
            if(target != null)
            {
                path = null;
                seeker.StartPath(transform.position, target.transform.position, ReceivePath);
                waitingForPath = true;
                StartCoroutine(Chasing());
                yield break;
            }
            // If the player has moved within aggro range, start chasing him
            if (Vector3.Distance(transform.position, GameManager.player.transform.position) < aggroRange)
            {
                target = GameManager.player;
                path = null;
                seeker.StartPath(transform.position, target.transform.position, ReceivePath);
                waitingForPath = true;
                StartCoroutine(Chasing());
                yield break;
            }
            yield return null;
        }
	}

    IEnumerator Reset()
    {
        seeker.StartPath(transform.position, startPosition, ReceivePath);
        waitingForPath = true;
        while (waitingForPath)
        {
            yield return null;
        }
        for(;;)
        {
            if(Vector3.Distance(transform.position, GameManager.player.transform.position) < aggroRange)
            {
                StartCoroutine(Chasing());
                yield break;
            }

            if (Vector3.Distance(transform.position, path.vectorPath[pathIndex]) < nextPointDistance)
            {
                pathIndex++;
                // If the previous target point was the final one in the path, go to idle
                if (pathIndex == path.vectorPath.Count)
                {
                    StartCoroutine(Idle());
                    yield break;
                }
            }

            // Rotate and move towards the current target point in the path
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

            yield return new WaitForSeconds(0.02f);
        }
    }

	IEnumerator Chasing()
	{
        for(;;)
        {
            // If there is no longer a target go back to idle
            if(target == null)
            {
                StartCoroutine(Idle());
                yield break;
            }
            targetDist = Vector3.Distance(transform.position, target.transform.position);
            // If the target has moved outside the aggro range, go to idle, if withing attack range, go to attacking
            if (targetDist > aggroRange)
            {
                StartCoroutine(Reset());
                yield break;
            } else if(targetDist < attackDist)
            {
                StartCoroutine(Attacking());
                yield break;
            }

            if (!waitingForPath && Vector3.Distance(targetPositionAtPath, target.transform.position) > recalcPathRange)
            {
                waitingForPath = true;
                seeker.StartPath(transform.position, target.transform.position, ReceivePath);
            }

            // If there is a path, follow it
            if (path != null)
            {
                // If too close to the current target point in the path, move on to the next
                if (Vector3.Distance(transform.position, path.vectorPath[pathIndex]) < nextPointDistance)
                {
                    pathIndex++;
                    // If the previous target point was the final one in the path, go to idle
                    if (pathIndex == path.vectorPath.Count)
                    {
                        StartCoroutine(Idle());
                        yield break;
                    }
                }

                // Rotate and move towards the current target point in the path
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
            yield return new WaitForSeconds(0.02f);
        }
    }

	IEnumerator Attacking()
    {
        for (;;)
        {
            // If the target is no longer valid, go to idle
            if(target == null)
            {
                StartCoroutine(Idle());
                yield break;
            }
            targetDist = Vector3.Distance(transform.position, target.transform.position);
            // If the target has moved outside attack distance, go to chasing it
            if (targetDist > attackDist)
            {
                StartCoroutine(Chasing());
                yield break;
            }

            // Check if facing the target, if not turn towards it, otherwise attack
            Vector3 dir = (target.transform.position - transform.position).normalized;
            if (Vector3.Dot(dir, transform.forward) >= 0.8f)
            {
                animator.SetTrigger("Attack");
                Weapon.GetComponent<EnemyMeleeAttack>().Swing(true);
                while (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                {
                    yield return null;
                }
                Weapon.GetComponent<EnemyMeleeAttack>().Swing(false);
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
