using UnityEngine;
using System.Collections;

public class PlayerControls : MonoBehaviour {

    private Animator anim;
    private NavMeshAgent navMeshAgent;
    InputManager IM;
    public AnimationCurve DashCurve;
    bool dashing, attacking;
    public float dashingSpeed;
    float dashingDis, dashingStartDis;
    float cameraRotation;


    private float navmeshSpeed;

    // Use this for initialization
    void Start () {
        IM = FindObjectOfType<InputManager>();
        IM.OnTap += MoveTo;
        IM.OnSwipe += AttackDir;
        IM.OnDoubleTap += DashTo;
        cameraRotation = FindObjectOfType<Camera>().transform.rotation.y;
	}
	
	void Awake()
    {
        anim = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navmeshSpeed = navMeshAgent.speed;
    }

    void MoveTo(Vector3 point)
    {
        if(!dashing && !attacking)
        {
            anim.SetBool("Run",true);
            navMeshAgent.destination = point;
        }
    }
    void DashTo(Vector3 point)
    {
        if (!dashing && !attacking)
        {
            anim.SetTrigger("Dash");
            dashing = true;
            dashingStartDis = Vector3.Distance(transform.position, point);
            navMeshAgent.destination = point;
        }
    }
    void AttackDir(Vector3 point)
    {
        if (!dashing && !attacking)
        {
            anim.SetTrigger("Attack");
            navMeshAgent.Stop();
            transform.LookAt(transform.position + point);
            transform.Rotate(Vector3.up, cameraRotation);
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

    }
  
}
