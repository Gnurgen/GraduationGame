using UnityEngine;
using System.Collections;

public class PlayerControls : MonoBehaviour {

    /// <summary> KRIS!!!
    /// Animationer for ability cancel skal addes - events er aktiverede.
    /// </summary>

   
    private InputManager im;
    private EventManager em;
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
    private int id;
    private bool shouldMove;
    private float currentDashDistance;
    private Vector3 MoveToPoint;
    private float currentDashCooldown;

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
        im = GameManager.input;
        em = GameManager.events;
        body = GetComponent<Rigidbody>();
        id = im.GetID();
        im.OnTapSub(Tap, id);
        im.OnFirstTouchBeginSub(Begin, id);
        im.OnFirstTouchMoveSub(Move, id);
        im.OnFirstTouchEndSub(End, id);
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
            if (Physics.Raycast(transform.position + Vector3.up * .2f, transform.forward, .3f, 1 << coneBlock))
                StartCoroutine(Idle());
        }
        body.velocity = Vector3.zero;
    }
    void Update()
    {
        currentDashCooldown -= Time.deltaTime;
    }

    IEnumerator Idle()
    {
        if(ClickFeedBack != null)
            ClickFeedBack.GetComponent<PKFxFX>().StopEffect();
        else
            ClickFeedBack = Instantiate(ClickFeedBack);
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
        em.PlayerMove(gameObject);
        while (state == State.Moving && Vector3.Dot(transform.forward, (MoveToPoint - transform.position).normalized)>0)
        {
            body.position += transform.forward * moveSpeed * Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate(); 
        }
        if (state == State.Ability)
        {
            StartCoroutine(Ability());
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
        while (state == State.Dashing && currentDashDistance < maxDashDistance && (transform.position - MoveToPoint).magnitude > alwaysWalk)
        {
            currentDashCooldown = dashCooldown;
            prevPos = transform.position;
            body.position += transform.forward * moveSpeed * dashSpeedMultiplier * Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
            currentDashDistance += (prevPos - transform.position).magnitude;
        }
        currentDashCooldown = dashCooldown;
        currentDashDistance = 0;
        em.PlayerDashEnd(gameObject);
        if (state == State.Ability)
        {
            StartCoroutine(Ability());
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
        touchStart = im.GetWorldPoint(p);
        prevstate = state;
        if ((touchStart - transform.position).magnitude < abilityTouchOffset && ability1.currentCooldown <= 0)
            ab1 = true;
        else if (ability2.currentCooldown <=0)
            ab2 = true;
    } 
    void Move(Vector2 p)
    {
        touchCur = im.GetWorldPoint(p);
        if ((touchCur - touchStart).magnitude >= abilityTouchMoveDistance)
        {
            
            body.velocity = Vector3.zero;
            state = State.Ability;
            if(ab1)
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
            ability1.currentCooldown = abilityCooldown;
            ability2.currentCooldown = abilityCooldown;
        }
        ab1 = false; ab2 = false;
        if (ResumeMovementAfterAbility)
        {
            state = prevstate;
        }
        else
        {
            
            state = State.Idle;
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
        if (state != State.Dashing || ControlDuringDash)
        {
            MoveToPoint = im.GetWorldPoint(p);
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

