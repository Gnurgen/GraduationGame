using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;

[RequireComponent (typeof(Seeker))]
[RequireComponent (typeof(CharacterController))]
[RequireComponent (typeof(Rigidbody))]
public class MeleeAI : EnemyStats {


	public GameObject target;
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

   	// Use this for initialization
	void Start () {
		seeker = GetComponent<Seeker> ();
        cc = GetComponent<CharacterController>();
        moving = false;
        StartCoroutine("StateHandler");
	}

	IEnumerator StateHandler()
	{
        for(;;)
        {
            if(target == null)
            {
                yield return StartCoroutine("Idle");
            }
            if(target != null && Vector3.Distance(transform.position, target.transform.position) > attackRange)
            {
                yield return StartCoroutine("Chasing");
            }
            if(target != null && Vector3.Distance(transform.position, target.transform.position) < attackRange)
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
            Debug.Log("Chasing");
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

            if (Vector3.Distance(transform.position, target.transform.position) < attackRange)
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
                dir *= moveSpeed * 0.02f;
                //transform.LookAt(path.vectorPath[pathIndex] );
                //Debug.Log(path.vectorPath[pathIndex]);
                //transform.position = Vector3.MoveTowards(transform.position, path.vectorPath[pathIndex], 0.02f * moveSpeed);
                //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.FromToRotation(Vector3.Scale(transform.position, yZero)+Vector3.up, Vector3.Scale(path.vectorPath[pathIndex], yZero)+Vector3.up), Time.fixedDeltaTime * turnRate);
                // Quaternion.RotateTowards(transform.rotation, Quaternion.FromToRotation(Vector3.Scale(transform.position, yZero) + Vector3.up, Vector3.Scale(path.vectorPath[pathIndex], yZero) + Vector3.up), turnRate * 0.02f);
                // transform.position += transform.forward * moveSpeed * 0.02f;
                cc.SimpleMove(dir);
                
            }
            skip:
            yield return new WaitForSeconds(0.02f);
        }
    }

	IEnumerator Attacking()
	{
		for(;;)
		{
            Debug.Log("Attacking");
			// Define attacking
            if(target == null)
            {
                yield break;
            }
            if(Vector3.Distance(transform.position, target.transform.position) > attackRange)
            {
                yield break;
            }


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
            while(IsBehind(path.vectorPath[pathIndex]))
            {
                pathIndex++;
            }
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

    bool IsBehind(Vector3 p)
    {
        return false;
    }
}
