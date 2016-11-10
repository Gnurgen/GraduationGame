using UnityEngine;
using System.Collections;

public class PlayerControls : MonoBehaviour {

    private Animator anim;
    private NavMeshAgent navMeshAgent;
    InputManager IM;
    public AnimationCurve DashCurve;
    public bool dashing, attacking;
    public float dashingSpeed;
    float dashingDis, dashingStartDis;
    float cameraRotation;
	int ID;

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
	}
	
	void Awake()
    {
        anim = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navmeshSpeed = navMeshAgent.speed;
    }

    void MoveTo(Vector2 point)
    {
        if(!dashing && !attacking)
        {
            anim.SetBool("Run",true);
			navMeshAgent.destination = IM.GetWorldPoint (point);
        }
    }
    void DashTo(Vector2 point)
    {
        if (!dashing && !attacking)
        {
            GameManager.events.PlayerDashBegin(gameObject);
            anim.SetTrigger("Dash");
            dashing = true;
			dashingStartDis = Vector3.Distance(transform.position, IM.GetWorldPoint(point));
            navMeshAgent.destination = point;
        }
    }
	void AttackDir(InputManager.Swipe swipe)
    {
        if (!dashing && !attacking)
        {
            anim.SetTrigger("Attack");
            navMeshAgent.ResetPath();
			transform.LookAt(transform.position + IM.GetWorldPoint(swipe.end) - IM.GetWorldPoint (swipe.begin));
			ray = transform.position + IM.GetWorldPoint (swipe.end) - IM.GetWorldPoint (swipe.begin);
            attacking = true;
        }
    }
    void Update()
    {
        if(dashing)
        {
            dashingDis = Vector3.Distance(transform.position, navMeshAgent.destination);
            navMeshAgent.speed = navMeshAgent.speed + dashingSpeed * DashCurve.Evaluate(1 - dashingDis/dashingStartDis);
            if (dashingDis <= 0.02f)
            {
                
                dashing = false;
                navMeshAgent.speed = navmeshSpeed;
                anim.SetBool("Run",false);
            }
        }
        if(navMeshAgent.remainingDistance < 0.1f)
        {
            anim.SetBool("Run", false);
        }

		Debug.DrawRay (transform.position, ray);
    }
  
}
