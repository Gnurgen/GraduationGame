﻿using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class FlyingSpear : MonoBehaviour, IAbility {

    public GameObject spear;
    public float baseDamage;
    [Range(0, 100)]
    public float DmgIncreasePerLvlPercentage;
    public float flyingSpeed;
    public float cooldown = 5;
    public float drawTimer;
    public float drawLength;


    private InputManager IM;
    private EventManager EM;
    private CurveDraw LR;
    public float damage;
    public float currentCooldown;
    public float currentDrawTimer;
    public float currentDrawLength;
    private List<Vector3> drawnPoints;
    private int ID;

    private bool shouldDraw;
    private bool newPointAdded;
    private int pointIndex;

    // Use this for initialization
    void Start () {
        damage = baseDamage;
        currentCooldown = 0;
        currentDrawLength = 0;
        currentDrawTimer = 0;
        IM = GameManager.input;
        EM = GameManager.events;
        LR = GetComponent<CurveDraw>();
        ID = IM.GetID();
        EM.OnLevelUp += levelUpSpear;
	}
	
	// Update is called once per frame
	void Update () {
        currentCooldown -= Time.deltaTime;
        currentDrawTimer -= Time.deltaTime;
	}

    public float Cooldown()
    {

        return currentCooldown < 0 ? 1 : (cooldown - currentCooldown) / cooldown;
    }

    public void UseAbility()
    {
        if (currentCooldown < 0)
        {
            StartCoroutine("Ability");
        }
    }

    IEnumerator Ability()
    {
        IM.OnFirstTouchBeginSub(GetDown, ID);
        IM.OnFirstTouchMoveSub(GetMove, ID);
        IM.OnFirstTouchEndSub(GetEnd, ID);
        IM.TakeControl(ID);

        drawnPoints = new List<Vector3>();
        currentDrawLength = 0;
        shouldDraw = true;
        newPointAdded = false;
        currentDrawTimer = drawTimer;

        yield return StartCoroutine("DrawLine");

        IM.ReleaseControl(ID);
        IM.OnFirstTouchBeginUnsub(ID);
        IM.OnFirstTouchMoveUnsub(ID);
        IM.OnFirstTouchEndUnsub(ID);
        // Actually use the ability with the drawn points

        GameObject s = Instantiate(spear) as GameObject;
        s.GetComponent<SpearController>().SetParameters(LR.GetPoints(), flyingSpeed, damage);
        LR.CleanUp();
        GameManager.events.DrawComplete(10); // takes input 10 because it is complete.
        currentCooldown = cooldown;
    }

    IEnumerator DrawLine()
    {
        while (shouldDraw)
        {
            if(currentDrawTimer < 0)
            {
                yield break;
            }
            yield return null;
        }
        yield break;
    }

    void GetDown(Vector2 p)
    {
        Vector3 worldPoint = IM.GetWorldPoint(p);
        if(currentDrawLength + Vector3.Distance(transform.position, worldPoint) < drawLength)
        {
            LR.AddPoint(transform.position);
            LR.AddPoint(worldPoint);
            currentDrawLength += Vector3.Distance(transform.position, worldPoint);
            newPointAdded = true;
        }
    }

    void GetMove(Vector2 p)
    {
        Vector3 worldPoint = IM.GetWorldPoint(p);
        if (currentDrawLength + Vector3.Distance(transform.position, worldPoint) < drawLength)
        {
            LR.AddPoint(worldPoint);
            currentDrawLength += Vector3.Distance(transform.position, worldPoint);
            newPointAdded = true;
        }
    }

    void GetEnd(Vector2 p)
    {
        Vector3 worldPoint = IM.GetWorldPoint(p);
        if (currentDrawLength + Vector3.Distance(transform.position, worldPoint) < drawLength)
        {
            LR.AddPoint(worldPoint);
            currentDrawLength += Vector3.Distance(transform.position, worldPoint);
            newPointAdded = true;
        }
        shouldDraw = false;
    }
    void levelUpSpear(int level) {
        damage += damage * (DmgIncreasePerLvlPercentage / 100);
    }
}
