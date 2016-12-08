using UnityEngine;
using System.Collections;

public class PlayerControls : MonoBehaviour {

    /// <summary> KRIS!!!
    /// Animationer for ability cancel skal addes - events er aktiverede.
    /// </summary>

   
    public enum State {Idle, Moving, Dashing, Ability};
    public State state;
    State prevstate;


    // ----- Inspector -----
    [SerializeField]
    private float moveSpeed = 4, minDashDistance = 3, maxDashDistance = 6, dashCooldown = 3, abilityCooldown = 1;
    [SerializeField]
    [Range(1, 5)]
    private int dashSpeedMultiplier = 2, alwaysWalk = 2;
    [SerializeField]
    private bool ControlDuringDash = false, AbilitiesDuringDash = false;
    [SerializeField]
    private float abilityTouchOffset, abilityTouchMoveDistance;
    [SerializeField]
    private AnimationCurve adsr;
    private bool ResumeMovementAfterAbility = false;
    public GameObject ClickFeedBack;

    private Vector3 prevPos;
    public int id;
    private bool shouldMove;
    private float currentDashDistance;
    private Vector3 MoveToPoint;
    private float currentDashCooldown;
    private float touchCooldown = .125f, curTouchCooldown;

    // --- Abilities ---
    private Vector3 touchStart, touchEnd, touchCur;
    private bool ab1 = false, ab2 = false;
    FlyingSpear ability1;
    ConeDraw ability2;

    private Rigidbody body;
    RaycastHit[] hit;
    int coneBlock = 1;
    private bool hitWall = false;

    // Use this for initialization
    void Start () {
        //ClickFeedBack = Instantiate(ClickFeedBack);
        body = GetComponent<Rigidbody>();
        id = GameManager.input.GetID();
        GameManager.input.OnTapSub(Tap, id);
        GameManager.input.OnFirstTouchBeginSub(Begin, id);
        GameManager.input.OnFirstTouchMoveSub(Move, id);
        GameManager.input.OnFirstTouchEndSub(End, id);
        StartCoroutine(Idle());
        shouldMove = true;
        currentDashDistance = 0;
        currentDashCooldown = 0;
        ability1 = GetComponent<FlyingSpear>();
        ability2 = GetComponent<ConeDraw>();
        coneBlock = LayerMask.NameToLayer("ConeBlocker");
	}
	

    void FixedUpdate()
    {
        if (state == State.Dashing || state == State.Moving)
        {
            float colCheckDist = state == State.Dashing ? (transform.forward * moveSpeed * dashSpeedMultiplier * Time.fixedDeltaTime).magnitude : (transform.forward * moveSpeed * Time.fixedDeltaTime).magnitude;
            if (Physics.Raycast(transform.position + Vector3.up * .2f, transform.forward, colCheckDist, 1 << coneBlock))
            {
                if (state == State.Dashing)
                    StartCoroutine(Moving());
                else
                    StartCoroutine(Idle());
            }
        }
        body.velocity = Vector3.zero;
    }
    void Update()
    {
        currentDashCooldown -= Time.deltaTime;
        curTouchCooldown -= Time.deltaTime;
    }

    IEnumerator Idle()
    {
        if(ClickFeedBack != null)
            ClickFeedBack.GetComponent<PKFxFX>().StopEffect();
        else
            ClickFeedBack = Instantiate(ClickFeedBack);
        state = State.Idle;
        GameManager.events.PlayerIdle(gameObject);
        while(state == State.Idle)
        {
            yield return null;
        }
        if (state == State.Ability)
        {
            yield break;
        }
        yield break;
    }

    IEnumerator Moving()
    {
        if (state == State.Moving)
            yield break;
        if (Physics.Raycast(transform.position + Vector3.up * .2f, transform.forward, (transform.forward * moveSpeed * Time.fixedDeltaTime).magnitude, 1 << coneBlock))
        {
            StartCoroutine(Idle());
            yield break;
        }
        state = State.Moving;
        GameManager.events.PlayerMove(gameObject);
        while (state == State.Moving && Vector3.Dot(transform.forward, (MoveToPoint - transform.position).normalized)>0)
        {
            body.position += transform.forward * moveSpeed * Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate(); 
        }
        if (state == State.Ability)
        {
            yield break;
        }
        else if (state == State.Moving)
        {
            StartCoroutine(Idle());
        }
        yield break;
    }

    IEnumerator Ability()
    {
        while (state == State.Ability)
        {

            yield return null;
        }
        switch (prevstate)
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
        if (state == State.Dashing)
        yield break;
        if (Physics.Raycast(transform.position + Vector3.up * .2f, transform.forward, (transform.forward*moveSpeed*dashSpeedMultiplier*Time.fixedDeltaTime).magnitude, 1 << coneBlock))
        {
            StartCoroutine(Moving());
            yield break;
        }
        state = State.Dashing;
        GameManager.events.PlayerDashBegin(gameObject);
        while (state == State.Dashing && currentDashDistance < maxDashDistance && (transform.position - MoveToPoint).magnitude > alwaysWalk && 
            Vector3.Dot(transform.forward, (MoveToPoint - transform.position).normalized) > 0)
        {
            currentDashCooldown = dashCooldown;
            prevPos = transform.position;
            body.position += transform.forward * moveSpeed * dashSpeedMultiplier * Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
            currentDashDistance += (prevPos - transform.position).magnitude;
        }
        currentDashCooldown = dashCooldown;
        currentDashDistance = 0;
        GameManager.events.PlayerDashEnd(gameObject);
        if (state == State.Ability)
        {
            yield break;
        }
        if ((transform.position - MoveToPoint).magnitude > .2f && state!=State.Idle)
        {
            StartCoroutine(Moving());
        }
        else
        {
            StartCoroutine(Idle());
        }
        yield break;
    }

    void Begin(Vector2 p)
    {
        touchStart = GameManager.input.GetWorldPoint(p);
        prevstate = ResumeMovementAfterAbility? state : State.Idle;
        if(AbilitiesDuringDash || state != State.Dashing)
        {
            if ((touchStart - transform.position).magnitude < abilityTouchOffset && ability1.currentCooldown <= 0)
                ab1 = true;
            else if (ability2.currentCooldown <=0)
                ab2 = true;
        }
    } 
    void Move(Vector2 p)
    {
        touchCur = GameManager.input.GetWorldPoint(p);
        if ((touchCur - touchStart).magnitude >= abilityTouchMoveDistance && (ab1 || ab2))
        {
            
            body.velocity = Vector3.zero;
            StartCoroutine(Ability());
            if (ab1)
            {
                ability1.UseAbility(touchStart);
                GameManager.events.SpearDrawAbilityStart(gameObject);
            }
            else if(ab2)
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

    public void EndAbility(bool used)
    {
        if (used)
        {
            //ability1.currentCooldown = abilityCooldown;
            ability2.currentCooldown = abilityCooldown;
        }
        ab1 = false; ab2 = false;
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
        if ((state != State.Dashing || ControlDuringDash) && gameObject.activeSelf)
        {
            curTouchCooldown = touchCooldown;
            MoveToPoint = GameManager.input.GetWorldPoint(p);
            MoveToPoint.y = transform.position.y;
            ClickFeedBack.GetComponent<PKFxFX>().StopEffect();
            ClickFeedBack.transform.position = MoveToPoint;
            ClickFeedBack.GetComponent<PKFxFX>().StartEffect();
            transform.LookAt(MoveToPoint);
            if ((transform.position-MoveToPoint).magnitude >= minDashDistance && currentDashCooldown <= 0 && state!=State.Dashing && currentDashDistance == 0)
            {
                StartCoroutine(Dashing());
            }
            else if (state!=State.Moving && state != State.Dashing)
            {
                StartCoroutine(Moving());
            }
        }
    }
}

