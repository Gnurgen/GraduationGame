using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConeDraw : MonoBehaviour {

    [SerializeField]
    private float maxConeLength, minConeLength, maxConeWidth, cancelAngle, coneAltitude, coneSpeed, damage, pushForce, stunTime, rampUp, maxDamageInrease;
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
    public List<GameObject> conePart;
    private int drawnParts = 0, totDrawn = 0;
    private GameObject coneParticlePref;
    List<float> particleLength;
    List<Vector3> particleDirection;
    Vector3 fireValue;
    float fireScale;

    //ROTATE UV MAP
    //private Texture2D target;
    //private Texture2D rotImage;
    //private float texW, texH, texCOS, texSIN, rotVal;
    //private int wTex, hTex;


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

    // Use this for initialization
    void Start () {
        baseDamage = damage;
        coneParticlePref = Resources.Load<GameObject>("Pool/p_ConeParticle");
        coneResolution = pointResolution * 90;
        im = GameManager.input;
        ID = im.GetID();
        onlyHitLayermask = 1 << LayerMask.NameToLayer("ConeBlocker");
        conePart = new List<GameObject>();
        particleLength = new List<float>();
        particleDirection = new List<Vector3>();
        for (int x = 0; x<coneResolution; ++x)
        {
            conePart.Add(GameManager.pool.PoolObj(Instantiate(coneParticlePref)));
            particleLength.Add(0f);
            particleDirection.Add(Vector3.zero);
        }
    }
	
    void Update()
    {
        if (doDraw)
        {
            normRamp = normRamp < 1f ? normRamp + Time.deltaTime / rampUp : 1;
            for(int x = 0; x<activeTris+1; ++x)
            {
                if (normRamp == 1)
                {
                    fireValue = new Vector3(normRamp, 0, 0);
                    fireScale = 6;
                    conePart[x].GetComponent<PKFxFX>().GetAttribute("CustomColor").ValueFloat3 = fireValue;
                    conePart[x].GetComponent<PKFxFX>().GetAttribute("GlobalScale").ValueFloat = 4;
                    conePart[x].GetComponent<PKFxFX>().GetAttribute("Duration").ValueFloat = particleLength[x] / 6f;
                    conePart[x].GetComponent<PKFxFX>().GetAttribute("Count").ValueFloat = 15;
                }
                else
                {
                    fireScale = 3 + normRamp * 2f;
                    fireValue = new Vector3(normRamp, 0, 1f-normRamp);
                    conePart[x].GetComponent<PKFxFX>().GetAttribute("CustomColor").ValueFloat3 = new Vector3(normRamp, normRamp, 1f);
                    conePart[x].GetComponent<PKFxFX>().GetAttribute("GlobalScale").ValueFloat = 2f + normRamp/2f;
                    conePart[x].GetComponent<PKFxFX>().GetAttribute("Duration").ValueFloat = particleLength[x] / (3f + normRamp);
                    conePart[x].GetComponent<PKFxFX>().GetAttribute("Count").ValueFloat = 10;
                }
            }
            damage = baseDamage + maxDamageInrease * normRamp;
        }
        _currentCooldown -= Time.deltaTime;
    }

 /*   Texture2D rotateTexture(Texture2D tex, float angle)
    {

        float x2, y2;
        angle = angle / 180f * Mathf.PI;
        texCOS = Mathf.Cos(angle);
        texSIN = Mathf.Sin(angle);
        float texWS = texW * texSIN;
        float texWC = texW * texCOS;
        float texHC = texH * texCOS;
        float texHS = texH * texSIN;
        float x1 = (-texWC + texHS) + texW;
        float y1 = (-texWS + -texHC) + texH;
        float dx_x = texCOS;
        float dx_y = texSIN;
        float dy_x = -texSIN;
        float dy_y = texCOS;
        for (int x = 0; x < tex.width; x++)
        {
            x2 = x1;
            y2 = y1;
            for (int y = 0; y < tex.height; y++)
            {         
                x2 += dx_x;
                y2 += dx_y;
                rotImage.SetPixel(x, y, tex.GetPixel((int)x2, (int)y2));
            }

            x1 += dy_x;
            y1 += dy_y;

        }
        rotImage.Apply();
        return rotImage;
    } */

   
    public void UseAbility(Vector3 p)
    {
        clockwise = true;
        dirSat = false;
        start = p;
        doDraw = false;
        drawnParts = 0;
        totDrawn = 0;
        
        //rotVal = 0;
        lookDir = start - transform.transform.position;
        myRot.SetLookRotation(lookDir);
        cRot.eulerAngles = myRot.eulerAngles + Vector3.up * (180f - cancelAngle);
        Rot.eulerAngles = myRot.eulerAngles + Vector3.up * -90f;
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
        damage = baseDamage;
        for (int x = 0; x < conePart.Count; ++x)
        {
            conePart[x].GetComponent<PKFxFX>().GetAttribute("CustomColor").ValueFloat3 = new Vector3(0, 0, 1);
            conePart[x].GetComponent<PKFxFX>().GetAttribute("GlobalScale").ValueFloat = 2f;
            conePart[x].GetComponent<PKFxFX>().StopEffect();
            GameManager.pool.PoolObj(conePart[x]);
        }
        normRamp = 0;
        drawnParts = 0;
        totDrawn = 0;
        if (doDraw)//If abality was not cancelled
        {
            GetComponent<PlayerControls>().EndAbility(true);
            GameManager.events.ConeAbilityUsed(GameManager.player);
            StartCoroutine(particleProjectile());
        }
        else
        {
            GetComponent<PlayerControls>().EndAbility(false);
            GameManager.events.ConeAbilityCancel(gameObject);
        }
        yield break;
    }

    IEnumerator particleProjectile()
    {
        doDraw = false;
        yield return new WaitForSeconds(0.2f);
        for(int x = 0; x<activeTris+1; ++x)
        {
            conePart[x].transform.position = transform.position;
            conePart[x].transform.LookAt(particleDirection[x]);
            conePart[x].GetComponent<PKFxFX>().GetAttribute("CustomColor").ValueFloat3 = fireValue;
            conePart[x].GetComponent<PKFxFX>().GetAttribute("GlobalScale").ValueFloat = fireScale;
            conePart[x].GetComponent<PKFxFX>().GetAttribute("Duration").ValueFloat = particleLength[x]/(3f+fireScale);
            conePart[x].GetComponent<PKFxFX>().GetAttribute("Count").ValueFloat = 20;
            conePart[x].SetActive(true);
            conePart[x].GetComponent<PKFxFX>().StartEffect();
        }
        yield return new WaitForSeconds(particleLength[0] / (3f + fireScale));
        for (int x = 0; x < activeTris + 1; ++x)
        {
            conePart[x].GetComponent<PKFxFX>().StopEffect();
            conePart[x].GetComponent<PKFxFX>().GetAttribute("CustomColor").ValueFloat3 = new Vector3(0, 0, 1);
            conePart[x].GetComponent<PKFxFX>().GetAttribute("GlobalScale").ValueFloat = 2f;
            conePart[x].GetComponent<PKFxFX>().GetAttribute("Count").ValueFloat = 10;
            GameManager.pool.PoolObj(conePart[x]);
        }
        yield break;
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
        float y =(int) Quaternion.FromToRotation((cur - transform.position), (start - transform.position)).eulerAngles.y;
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
        else if (y > maxConeWidth && clockwise || y<360-maxConeWidth && !clockwise)
        {
            y = 180;
        }
        if(doDraw)
        {
            activeTris = (int)(clockwise ? y * (coneResolution / 360f) : (coneResolution) - (y * (coneResolution / 360f)));
            length = maxConeLength * (1 - (float)activeTris / coneResolution);
            if (length < minConeLength)
                length = minConeLength;
            int k, lim, inc, count = 0;
            if (clockwise)
            {
                k = 0;
                lim = activeTris+1;
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
            for (int l = k; l!=lim; l+=inc)
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
                    conePart[count].GetComponent<PKFxFX>().GetAttribute("Duration").ValueFloat = hit.distance;
                    conePart[count].GetComponent<PKFxFX>().StartEffect();
                }
                else
                {
                    Vector3 point = curPart * length;
                    point.y = 0.5f;
                    conePart[count].transform.position = transform.position + point;
                    conePart[count].transform.LookAt(transform.position);
                    particleLength[count] = length;
                    conePart[count].GetComponent<PKFxFX>().GetAttribute("Duration").ValueFloat = length;
                    conePart[count].GetComponent<PKFxFX>().StartEffect();
                }
                particleDirection[count] = transform.position + (curPart * particleLength[count] - transform.position);
                count++;
            }
            if (totDrawn < activeTris)
                totDrawn = activeTris;
            if(drawnParts>activeTris)
            {
                for(int x = conePart.Count-1; x!=activeTris-1; --x)
                {
                    conePart[x].GetComponent<PKFxFX>().StopEffect();
                    GameManager.pool.PoolObj(conePart[x]);
                }
                totDrawn = activeTris;
            }
            drawnParts = activeTris;
        }
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