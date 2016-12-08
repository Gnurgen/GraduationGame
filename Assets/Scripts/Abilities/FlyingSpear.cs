using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class FlyingSpear : MonoBehaviour {

    public GameObject spear;
    [SerializeField]
    private float baseDamage, flyingSpeed, drawLength, damage, pushForce, spearAltitude, turnRate, stunTime;
    [SerializeField]
    [Range(1, 10)]
    private int dragForce = 1;
    [SerializeField]
    private int dragTargets;



    public float currentCooldown;
    private EffectCurveDraw LR;
    private float currentDrawLength;
    private List<Vector3> drawnPoints;
    private int ID;

    private bool shouldDraw;

    // Use this for initialization
    void Start () {
        damage = baseDamage;
        currentCooldown = 0;
        currentDrawLength = 0;
        LR = GetComponent<EffectCurveDraw>();
        ID = GameManager.input.GetID();
	}
	
	// Update is called once per frame
	void Update () {
        currentCooldown -= Time.deltaTime;
	}

    public void UseAbility(Vector3 p)
    {
        currentDrawLength = 0;
        currentCooldown = 10000f;
        shouldDraw = true;
        drawnPoints = new List<Vector3>();
        LR.AddPoint(transform.position);
        LR.AddPoint(p);
        currentDrawLength += Vector3.Distance(transform.position, p);
        drawnPoints.Add(p);
        StartCoroutine(Ability());
    }

    IEnumerator Ability()
    {
        GameManager.input.OnFirstTouchMoveSub(GetMove, ID);
        GameManager.input.OnFirstTouchEndSub(GetEnd, ID);
        GameManager.input.TakeControl(ID);
        yield return StartCoroutine(DrawLine());
        GameManager.input.ReleaseControl(ID);
        GameManager.input.OnFirstTouchMoveUnsub(ID);
        GameManager.input.OnFirstTouchEndUnsub(ID);
       
        GetComponent<PlayerControls>().EndAbility(true);
        // Actually use the ability with the drawn points

        GameObject s = Instantiate(spear) as GameObject;
        GameManager.events.SpearDrawAbilityUsed(s);
        s.GetComponent<SpearControl>().SetParameters(LR.GetPoints(), LR.GetEffects(), flyingSpeed, damage, pushForce,dragForce, spearAltitude, turnRate, stunTime, dragTargets);
        LR.CleanUp();
    }



    IEnumerator DrawLine()
    {
        while (shouldDraw)
        {
            yield return null;
        }
        yield break;
    }

    void GetMove(Vector2 p)
    {
        Vector3 worldPoint = GameManager.input.GetWorldPoint(p);
        if (currentDrawLength + Vector3.Distance(drawnPoints[drawnPoints.Count-1], worldPoint) < drawLength)
        {
            currentDrawLength += Vector3.Distance(drawnPoints[drawnPoints.Count - 1], worldPoint);
            LR.AddPoint(worldPoint);
            drawnPoints.Add(worldPoint);
        }
        else if (drawLength - currentDrawLength > 0)
        {
            Vector3 k = drawnPoints[drawnPoints.Count - 1] + (worldPoint - drawnPoints[drawnPoints.Count - 1]).normalized * (drawLength - currentDrawLength);
            LR.AddPoint(k);
            drawnPoints.Add(k);
            currentDrawLength = drawLength;
        }
    }



    void GetEnd(Vector2 p)
    {
        Vector3 worldPoint = GameManager.input.GetWorldPoint(p);
        if (currentDrawLength + Vector3.Distance(drawnPoints[drawnPoints.Count - 1], worldPoint) < drawLength)
        {
            currentDrawLength += Vector3.Distance(drawnPoints[drawnPoints.Count - 1], worldPoint);
            LR.AddPoint(worldPoint);
            drawnPoints.Add(worldPoint);
        }
        else if (drawLength - currentDrawLength > 0)
        {
            Vector3 k = drawnPoints[drawnPoints.Count - 1] + (worldPoint - drawnPoints[drawnPoints.Count - 1]).normalized * (drawLength - currentDrawLength);
            LR.AddPoint(k);
            drawnPoints.Add(k);
            currentDrawLength = drawLength;
        }
        shouldDraw = false;
    }
}
