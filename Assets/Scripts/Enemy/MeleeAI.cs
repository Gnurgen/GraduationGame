using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;

[RequireComponent (typeof(Seeker))]
[RequireComponent (typeof(CharacterController))]
public class MeleeAI : EnemyStats {


	public GameObject target;
	private Seeker seeker;
    private CharacterController cc;
    private Vector3 targetPositionAtPath;
    private Path path;
    private int pathIndex;
    private float nextPointDistance = 1;
    private float recalcPathRange;
    private bool waitingForPath;

    private int calls = 0, updcalls = 0;

	// Use this for initialization
	void Start () {
		seeker = GetComponent<Seeker> ();
        cc = GetComponent<CharacterController>();
        StartCoroutine("StateHandler");
	}

    void Update()
    {
        updcalls++;
    }

	IEnumerator StateHandler()
	{
        for(;;)
        {
            if(target == null)
            {
                StartCoroutine("Idle");
            }
            if(target != null && Vector3.Distance(transform.position, target.transform.position) > attackRange)
            {
                StartCoroutine("Chasing");
            }
            if(target != null && Vector3.Distance(transform.position, target.transform.position) < attackRange)
            {
                StartCoroutine("Attacking");
            }
            yield return null;
        }
	}


	IEnumerator Idle()
	{
        for (;;)
        {
            if(Vector3.Distance(transform.position, GameManager.player.transform.position) < aggroRange)
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
            if(target == null)
            {
                yield break;
            }

            // If there target got outside the aggro range, remvoe it and stop chasing
            if(Vector3.Distance(transform.position, target.transform.position) > aggroRange)
            {
                target = null;
                yield break;
            }

            if(!waitingForPath)
            {
                if(path == null || Vector3.Distance(targetPositionAtPath, target.transform.position) > recalcPathRange)
                {
                    seeker.StartPath(transform.position, target.transform.position, ReceivePath);
                    waitingForPath = true;
                }
                else
                {
                    if (Vector3.Distance(transform.position, path.vectorPath[pathIndex]) < nextPointDistance)
                    {
                        pathIndex++;
                    }

                    if(pathIndex < path.vectorPath.Count)
                    {
                        // Rotate towards the current path point and move towards it
                        Vector3 dir = (path.vectorPath[pathIndex] - transform.position);
                        dir = Vector3.Normalize(dir);
                        Debug.Log(updcalls + " : " + calls);
                        calls++;
                        //dir *= moveSpeed * Time.deltaTime;
                        //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.FromToRotation(transform.position, path.vectorPath[pathIndex]), Time.deltaTime * turnRate);
                        transform.position += dir * moveSpeed * Time.deltaTime;
                            
                            //Vector3.MoveTowards(transform.position, path.vectorPath[pathIndex], moveSpeed * Time.deltaTime);
                        
                        //cc.SimpleMove(dir / cc.velocity.magnitude);
            
                    }
                }
            }

            yield return new WaitForSeconds(Time.unscaledDeltaTime);
        }
    }

	IEnumerator Attacking()
	{
		while (target != null && target != null && Vector3.Distance (transform.position, target.transform.position) < attackRange)
		{
			// Define attacking

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
