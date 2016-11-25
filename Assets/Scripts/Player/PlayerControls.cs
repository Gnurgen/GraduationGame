using UnityEngine;
using System.Collections;
using Pathfinding;
using System;

public class PlayerControls : MonoBehaviour {

   
    private InputManager im;
    private EventManager em;
    public enum State {Idle, Moving, Dashing, Ability};
    public State state;
    State prevstate;


    // ----- Inspector -----
    public float moveSpeed = 4;
    [SerializeField]
    private float minDashDistance = 3, maxDashDistance = 6, alwaysWalk = 1;
    public float dashSpeedMultiplier = 3;
    public float dashCooldown = 3;
    [SerializeField]
    private bool ControlDuringDash = false, AbilitiesDuringDash = false;
    [SerializeField]
    private float abilityTouchOffset, abilityTouchMoveDistance;
    [SerializeField]
    private bool ResumeMovementAfterAbility = false;
    public float Damage = 1;
    public float meeleeForce = 1;
    public float MovePointCooldown = 1;
    public GameObject ClickFeedBack;

    private float currentMovePointCooldown = 0;
    private Vector3 prevPos;
    private int id;
    private bool shouldMove;
    private float currentDashDistance;
    private Vector3 MoveToPoint;
    private float currentDashCooldown;
    private bool isMoving = false;

    // --- Abilities ---
    private Vector3 touchStart, touchEnd, touchCur;
    private Vector2 playerScreenPos;
    private float screenRes;
    private float normAbilityTouchOffset;
    private bool ab1 = false, ab2 = false;
    FlyingSpear ability1;
    ConeDraw ability2;

    private Rigidbody body;

    // Use this for initialization
    void Start () {
        im = GameManager.input;
        em = GameManager.events;
        body = GetComponent<Rigidbody>();
        id = im.GetID();
        im.OnTapSub(Tap, id);
        im.OnFirstTouchBeginSub(Begin, id);
        im.OnFirstTouchMoveSub(Move, id);
        im.OnFirstTouchEndSub(End, id);
        //im.TakeControl(id);
        StartCoroutine(Idle());
        playerScreenPos = new Vector2(Screen.width / 2, Screen.height / 2);
        shouldMove = true;
        currentMovePointCooldown = 0;
        currentDashDistance = 0;
        currentDashCooldown = 0;
        ability1 = GetComponent<FlyingSpear>();
        ability2 = GetComponent<ConeDraw>();
        //normalize for screen resolution
	}
	

    void FixedUpdate()
    {
        currentDashCooldown += currentDashCooldown < 0 ? 0 : -Time.fixedDeltaTime;
        body.velocity = Vector3.zero;
    }

    IEnumerator Idle()
    {
        isMoving = false;
        ClickFeedBack.GetComponent<PKFxFX>().StopEffect();
        state = State.Idle;
        em.PlayerIdle(gameObject);
        while(state == State.Idle)
        {
            yield return null;
        }
        if (state == State.Ability)
        {
            StartCoroutine(Ability());
            yield break;
        }
        yield break;
    }

    IEnumerator Moving()
    {
        state = State.Moving;
        isMoving = true;
        em.PlayerMove(gameObject);
        while (state == State.Moving && Vector3.Dot(transform.forward, (MoveToPoint - transform.position).normalized) > 0)
        {
            body.position += transform.forward * moveSpeed * Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate(); 
        }
        if (state == State.Ability)
        {
            StartCoroutine(Ability());
            yield break;
        }
        StartCoroutine(Idle());
        yield break;
    }

    IEnumerator Ability()
    {
        while (state == State.Ability)
        {

            yield return null;
        }
        switch (state)
        {
            case State.Dashing: StartCoroutine(Dashing());
                break;
            case State.Moving: StartCoroutine(Moving());
                break;
            case State.Idle: StartCoroutine(Idle());
                break;
        }
        yield break;
    }

    IEnumerator Dashing()
    {
        state = State.Dashing;
        em.PlayerDashBegin(gameObject);
        while (state == State.Dashing && currentDashDistance < maxDashDistance && NotPassedPoint(transform.position, MoveToPoint) && Distance(transform.position, MoveToPoint)>alwaysWalk)
        {
            prevPos = transform.position;
            body.position += transform.forward * moveSpeed * dashSpeedMultiplier * Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
            currentDashDistance += Distance(prevPos, transform.position);
        }
        em.PlayerDashEnd(gameObject);
        if (state == State.Ability)
        {
            StartCoroutine(Ability());
            yield break;
        }
        currentDashDistance = 0;
        currentDashCooldown = dashCooldown;
        if (NotPassedPoint(transform.position, MoveToPoint))
            StartCoroutine(Moving());
        else
            StartCoroutine(Idle());
        yield break;
    }


    private bool NotPassedPoint(Vector3 pos, Vector3 tar)
    {
        if ((pos - tar).magnitude < minDashDistance)
            return Vector3.Dot(transform.forward, (tar - pos).normalized) > 0f;
        else return true;
    }


    void Begin(Vector2 p)
    {
        abilityTouchMoveDistance = 0;
        touchStart = im.GetWorldPoint(p);
        prevstate = state;
        if ((p - playerScreenPos).magnitude < abilityTouchOffset && ability1.Cooldown() <= 0)
            ab1 = true;
        else if (ability2.Cooldown()<=0)
            ab2 = true;
    } 
    void Move(Vector2 p)
    {
        touchCur = im.GetWorldPoint(p);
        if(Distance(touchCur, touchStart) >= abilityTouchMoveDistance)
        {
            state = State.Ability;
            if(ab1)
            {
                ability1.UseAbility(touchStart);
                em.SpearDrawAbilityStart(gameObject);
            }
            else
            {
                ability2.UseAbility(touchStart);
               
                GameManager.events.ConeAbilityStart(gameObject);
            }
        }
    }

    void End(Vector2 p)
    {
        ab1 = false; ab2 = false;
    }

    public void EndAbility()
    {
        if(state==State.Ability)
        {
            if (ab1)
                em.SpearDrawAbilityEnd(gameObject);
            else
                em.ConeAbilityEnd(gameObject);
        }
        ab1 = false; ab2 = false;
        if (ResumeMovementAfterAbility)
            state = prevstate;
        else
            state = State.Idle;
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
        if (state != State.Dashing || ControlDuringDash)
        {
            MoveToPoint = im.GetWorldPoint(p);
            MoveToPoint.y = transform.position.y;
            ClickFeedBack.GetComponent<PKFxFX>().StopEffect();
            ClickFeedBack.transform.position = MoveToPoint;
            ClickFeedBack.GetComponent<PKFxFX>().StartEffect();
            transform.LookAt(MoveToPoint);
            if (Distance(transform.position, MoveToPoint) > minDashDistance && currentDashCooldown <= 0 && state!=State.Dashing)
                StartCoroutine(Dashing());
            else if (!isMoving)
                StartCoroutine(Moving());
        }
      
    }
    private float Distance(Vector3 a, Vector3 b)
    {
        return (a - b).magnitude;
    }

   
}

