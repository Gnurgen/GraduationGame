﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConeDraw : MonoBehaviour {

    [SerializeField]
    private float maxConeLength, minConeLength, maxConeWidth, cancelAngle, coneAltitude, coneSpeed, damage, pushForce, stunTime, rampUp, maxDamageInrease, 
        maxParticleDrawSize = 4f, maxParticleFireSize = 6f;
    [Range(1, 10)]
    [SerializeField]
    private int pointResolution = 1;
    private InputManager im;
    private bool drawing = false, clockwise = true;
    private Vector3 start, end, cur, lookDir;
    private int coneResolution, ID, activeTris;
    private float length;
    private const float _2pi = Mathf.PI * 2;
    private bool dirSat = false;
    private bool doDraw = false;
    private int onlyHitLayermask;
    private float _currentCooldown, rotTimer = 0, normRamp, baseDamage;


    //NEW CONE
    private Quaternion myRot, cRot, Rot;
    public List<GameObject> conePart, coneHit, coneHitBig;
    private int drawnParts = 0, totDrawn = 0;
    private GameObject coneParticlePref, coneHitParticlePref, coneHitParticlePrefBig;
    private List<float> particleLength;
    private List<Vector3> particleDirection;
    private Vector3 particleFireColor, baseParticleColor = Vector3.forward, baseParticleDirection = Vector3.forward, particleFireDirection = -Vector3.forward*.5f;
    private float particleFireScale;
    private const string color = "CustomColor", scale = "GlobalScale", duration = "Duration", count = "Count", direction = "Direction", power = "Power", impact = "p_FlyingSpearImpact", bigImpact = "p_FlyingSpearBigImpact";
    private const float baseParticleCount = 10f, baseParticleScale = 2f, baseParticleDuration = 4f, particleFireDuration = .2f, particleFireCount = 20f, baseParticleSpeed = 5f, maxParticleCount = 25f,
        fireParticlePower = 1f, baseParticlePower = .5f;
    private bool abilityCharged = false;
    private int layerEnemy, layerEnemyhit;

    public float currentCooldown
    {
        get
        {
            return _currentCooldown;
        }
        set
        {
            _currentCooldown = value;
        }
    }
    //Collision checking
    Ray ray;
    RaycastHit hit;
    RaycastHit[] eHit;

    // Use this for initialization
    void Start () {
        baseDamage = damage;
        coneParticlePref = Resources.Load<GameObject>("Pool/p_ConeParticle");
        coneHitParticlePref = Resources.Load<GameObject>("Pool/p_FlyingSpearImpact");
        coneHitParticlePrefBig = Resources.Load<GameObject>("Pool/p_FlyingSpearBigImpact");
        coneResolution = pointResolution * 90;
        im = GameManager.input;
        ID = im.GetID();
        onlyHitLayermask = 1 << LayerMask.NameToLayer("ConeBlocker");
        conePart = new List<GameObject>();
        particleLength = new List<float>();
        particleDirection = new List<Vector3>();
        for (int x = 0; x<coneResolution; ++x)
        {
            GameManager.pool.PoolObj(Instantiate(coneHitParticlePref));
            GameManager.pool.PoolObj(Instantiate(coneHitParticlePrefBig));
            conePart.Add(GameManager.pool.PoolObj(Instantiate(coneParticlePref)));
            particleLength.Add(0f);
            particleDirection.Add(Vector3.zero);
        }
        layerEnemy = LayerMask.NameToLayer("Enemy");
        layerEnemyhit = LayerMask.NameToLayer("EnemyHit");
    }
	
    void Update()
    {
        if (doDraw && !abilityCharged)
        {
            normRamp = normRamp < 1f ? normRamp + Time.deltaTime / rampUp : 1;
            float rampedScale = 2f + normRamp / 2f;
            Vector3 curColor = new Vector3(normRamp, normRamp, 1f);
            for (int x = 0; x<activeTris+1; ++x)
            {
                if (normRamp == 1)
                {
                    particleFireColor = new Vector3(normRamp, 0, 0);
                    particleFireScale = maxParticleFireSize;
                    setParticleValue(maxParticleDrawSize, maxParticleCount, particleLength[x] / baseParticleSpeed/(maxParticleDrawSize- baseParticleScale)-0.2f, particleFireColor, conePart[x]);
                    GameManager.events.ConeAbilityCharged(gameObject);
                    damage = baseDamage + maxDamageInrease * normRamp;
                    abilityCharged = true;
                }
                else
                {
                    particleFireScale = maxParticleFireSize/2f + normRamp * 2f;
                    particleFireColor = new Vector3(normRamp, 0, 1f-normRamp);
                    setParticleValue(rampedScale, baseParticleCount, particleLength[x] / baseParticleSpeed-0.2f, curColor, conePart[x]);
                    damage = baseDamage + maxDamageInrease * normRamp;
                }
            }
            
        }
        _currentCooldown -= Time.deltaTime;
    }

    private void setParticleValue(float s, float c, float dur, Vector3 col, GameObject obj)
    {
        PKFxFX fx = obj.GetComponent<PKFxFX>();
        fx.GetAttribute(color).ValueFloat3 = col;
        fx.GetAttribute(scale).ValueFloat = s;
        fx.GetAttribute(duration).ValueFloat = dur;
        fx.GetAttribute(count).ValueFloat = c;
    }
    private void resetParticleValue(GameObject obj)
    {
        PKFxFX fx = obj.GetComponent<PKFxFX>();
        fx.GetAttribute(color).ValueFloat3 = baseParticleColor;
        fx.GetAttribute(scale).ValueFloat = baseParticleScale;
        fx.GetAttribute(duration).ValueFloat = baseParticleDuration;
        fx.GetAttribute(count).ValueFloat = baseParticleCount;
        fx.GetAttribute(direction).ValueFloat3 = baseParticleDirection;
        fx.GetAttribute(power).ValueFloat = baseParticlePower;
        fx.StopEffect();
        GameManager.pool.PoolObj(obj);
    }

    private void setParticleFireValue(GameObject obj)
    {
        PKFxFX fx = obj.GetComponent<PKFxFX>();
        fx.GetAttribute(color).ValueFloat3 = particleFireColor;
        fx.GetAttribute(scale).ValueFloat = 1;
        fx.GetAttribute(duration).ValueFloat = particleFireDuration;
        fx.GetAttribute(count).ValueFloat = particleFireCount;
        fx.GetAttribute(direction).ValueFloat3 = particleFireDirection;
        fx.GetAttribute(power).ValueFloat = fireParticlePower;
        fx.KillEffect();
        fx.StartEffect();
    }

   
    public void UseAbility(Vector3 p)
    {
        clockwise = true;
        dirSat = false;
        start = p;
        doDraw = false;
        drawnParts = 0;
        totDrawn = 0;
        abilityCharged = false;
        damage = baseDamage;
        lookDir = start - transform.transform.position;
        myRot.SetLookRotation(lookDir);
        cRot.eulerAngles = myRot.eulerAngles + Vector3.up * (180f - cancelAngle);
        Rot.eulerAngles = myRot.eulerAngles + Vector3.up * -90f;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        StartCoroutine(Ability());
    }

    IEnumerator Ability()
    {
        im.OnFirstTouchMoveSub(GetMove, ID);
        im.OnFirstTouchEndSub(GetEnd, ID);
        im.TakeControl(ID);
        drawing = true;
        yield return StartCoroutine(DrawCone());
        im.ReleaseControl(ID);
        im.OnFirstTouchMoveUnsub(ID);
        im.OnFirstTouchEndUnsub(ID);
        // Actually use the ability with the drawn points
       
        
        normRamp = 0;
        drawnParts = 0;
        totDrawn = 0;
        if (doDraw)//If ability was not cancelled
        {
            doDraw = false;
            GetComponent<PlayerControls>().EndAbility(true);
            GameManager.events.ConeAbilityUsed(GameManager.player);
            for (int x = 0; x < activeTris + 1; ++x)
            {
                conePart[x].transform.position = transform.position;
                conePart[x].transform.LookAt(transform.position+particleDirection[x]);
                setParticleFireValue(conePart[x]);
            }
            for (int x = 0; x<activeTris+1; ++x)
            {
                ray = new Ray(transform.position, (transform.position+Vector3.up*.5f) + particleDirection[x]);
                eHit = Physics.SphereCastAll(ray, 1f, particleDirection[x].magnitude, 1<<layerEnemy);
                for(int k = 0; k<eHit.Length; ++k)
                {
                    eHit[k].transform.gameObject.layer = layerEnemyhit;
                    if (eHit[k].transform.tag == "Boss")
                    {
                        StartCoroutine(ApplyConeEffect(eHit[k].transform.gameObject, Vector3.Distance(transform.position, eHit[k].transform.position) / coneSpeed));
                    }
                    else
                    {
                        StartCoroutine(ApplyConeEffect(eHit[k].transform.gameObject, Vector3.Distance(transform.position, eHit[k].transform.position + eHit[k].transform.GetComponent<EnemyStats>().velo * coneSpeed)/coneSpeed));
                    }
                }
            }
            int doneParticles = 0;
            while (doneParticles < activeTris)
            {
                for(int x = 0; x<activeTris+1; ++x)
                {
                    float coneTravel = (conePart[x].transform.position - transform.position).magnitude;
                    float coneDest = particleDirection[x].magnitude;
                    if (coneTravel>=coneDest)
                    {
                        resetParticleValue(conePart[x]);
                        doneParticles++;
                        continue;
                    }
                    else
                    {
                        conePart[x].transform.position += conePart[x].transform.forward * coneSpeed * Time.deltaTime;
                        conePart[x].GetComponent<PKFxFX>().GetAttribute(scale).ValueFloat =(particleFireScale * (coneTravel / coneDest));
                    }
                }
                yield return null;
            }
        }
        else
        {
            GetComponent<PlayerControls>().EndAbility(false);
            GameManager.events.ConeAbilityCancel(gameObject);
        }
        for (int x = 0; x < conePart.Count; ++x)
        {
            resetParticleValue(conePart[x]);
        }
        yield break;
    }
    IEnumerator ApplyConeEffect(GameObject go, float delayTime)
    {
        print(delayTime);
        yield return new WaitForSeconds(delayTime);
        if (go != null)
        {
            if (go.GetComponent<Health>().health <= 0)
            {
                go.layer = layerEnemy;
                yield break;
            }
            if (go.tag == "Enemy")
            {
                go.GetComponent<Rigidbody>().AddForce((go.transform.position - transform.position).normalized * pushForce, ForceMode.Impulse);
                go.GetComponent<EnemyStats>().decreaseHealth(damage, (go.transform.position - transform.position), pushForce);
                go.GetComponent<EnemyStats>().PauseFor(stunTime);
            }
            else
            {
                go.GetComponent<Health>().decreaseHealth(damage, Vector3.zero, 0);
            }
            go.layer = layerEnemy;
            GameManager.events.ConeAbilityHit(go);
            GameObject hitParticle = abilityCharged ? GameManager.pool.GenerateObject(bigImpact) : GameManager.pool.GenerateObject(impact);
            hitParticle.transform.position = go.transform.position;
            hitParticle.GetComponent<PKFxFX>().StartEffect();
            yield return new WaitForSeconds(1.0f);
            GameManager.pool.PoolObj(hitParticle);
            hitParticle.GetComponent<PKFxFX>().StopEffect();

        }
    }
    private bool coneDrawAnalysis(float y)
    {
        if (clockwise && y > 270)
            dirSat = false;
        else if (!clockwise && y < 90)
            dirSat = false;

        if (y < 270 && !dirSat)
        {
            clockwise = true;
            dirSat = true;
        }
        else if(y > 90 && !dirSat)
        {
            clockwise = false;
            dirSat = true;
        }
        return y > cancelAngle / 2f && clockwise|| y < 360f - cancelAngle / 2f && !clockwise;
    }

    void GetMove(Vector2 p)
    {
        cur = im.GetWorldPoint(p);
        float y = (int)Quaternion.FromToRotation((cur - transform.position), (start - transform.position)).eulerAngles.y;
        doDraw = coneDrawAnalysis(y);

        if (!doDraw)
        {
            damage = baseDamage;
            for (int x = 0; x < conePart.Count; ++x)
            {
                conePart[x].GetComponent<PKFxFX>().StopEffect();
                GameManager.pool.PoolObj(conePart[x]);
            }
            drawnParts = 0;
            totDrawn = 0;
            activeTris = 0;
        }
        else if (y > maxConeWidth && clockwise || y < 360 - maxConeWidth && !clockwise)
        {
            y = 180;
        }
        if (doDraw)
        {
            activeTris = (int)(clockwise ? y * (coneResolution / 360f) : (coneResolution) - (y * (coneResolution / 360f)));
            length = maxConeLength * (1 - (float)activeTris / coneResolution);
            if (length < minConeLength)
                length = minConeLength;
            int k, lim, inc, count = 0;
            if (clockwise)
            {
                k = 0;
                lim = activeTris + 1;
                inc = 1;
                myRot = cRot;
            }
            else
            {
                k = activeTris;
                lim = -1;
                inc = -1;
                myRot = Rot;
            }
            for (int l = k; l != lim; l += inc)
            {
                conePart[count].SetActive(true);
                float phi = clockwise ? coneResolution - l * _2pi / coneResolution : l * _2pi / coneResolution;
                Vector3 curPart = myRot * new Vector3(Mathf.Cos(phi), 0, -Mathf.Sin(phi));
                ray = new Ray(transform.position, curPart);
                if (Physics.Raycast(ray, out hit, length, onlyHitLayermask))
                {
                    Vector3 point = curPart * hit.distance;
                    point.y = 0.5f;
                    conePart[count].transform.position = transform.position + point;
                    conePart[count].transform.LookAt(transform.position);
                    particleLength[count] = hit.distance;
                    conePart[count].GetComponent<PKFxFX>().StartEffect();
                }
                else
                {
                    Vector3 point = curPart * length;
                    point.y = 0.5f;
                    conePart[count].transform.position = transform.position + point;
                    conePart[count].transform.LookAt(transform.position);
                    particleLength[count] = length;
                    conePart[count].GetComponent<PKFxFX>().StartEffect();
                }
                particleDirection[count] = conePart[count].transform.position - transform.position;
                count++;
            }
            if (totDrawn < activeTris)
                totDrawn = activeTris;
            if (drawnParts > activeTris)
            {
                for (int x = conePart.Count - 1; x != activeTris - 1; --x)
                {
                    conePart[x].GetComponent<PKFxFX>().StopEffect();
                    GameManager.pool.PoolObj(conePart[x]);
                }
                totDrawn = activeTris;
            }
            drawnParts = activeTris;
        }
        transform.LookAt(transform.position + particleDirection[particleDirection.Count / 2]);
    }

    void GetEnd(Vector2 p)
    {
        drawing = false;

    }

    IEnumerator DrawCone()
    {
        while (drawing)
        {
            yield return null;
        }
        yield break;
    }
}