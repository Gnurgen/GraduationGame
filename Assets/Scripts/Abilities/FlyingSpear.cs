using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class FlyingSpear : MonoBehaviour, IAbility {


    public float cooldown = 5;
    public float currentCooldown;
    public float drawTimer;
    public float drawLength;


    private InputManager IM;
    private LineRenderer LR;
    private float currentDrawTimer;
    private float currentDrawLength;
    private List<Vector3> drawnPoints;
    private int ID;

    private bool shouldDraw;
    private bool newPointAdded;

    // Use this for initialization
    void Start () {
        currentCooldown = 0;
        currentDrawLength = 0;
        currentDrawTimer = 0;
        IM = GameManager.input;
        LR = new LineRenderer();
        ID = IM.GetID();
	}
	
	// Update is called once per frame
	void Update () {
        currentCooldown -= Time.deltaTime;
        currentDrawTimer -= Time.deltaTime;
	}

    public float Cooldown()
    {

        return currentCooldown < 0 ? 0 : currentCooldown;
    }

    public void UseAbility()
    {
        IM.OnFirstTouchBeginSub(GetDown, ID);
        IM.OnFirstTouchMoveSub(GetMove, ID);
        IM.OnFirstTouchEndSub(GetEnd, ID);
        IM.TakeControl(ID);

        drawnPoints = new List<Vector3>();
        currentDrawLength = 0;
        shouldDraw = true;
        newPointAdded = false;
        while(shouldDraw)
        {
            if (newPointAdded)
            {
                LR.SetPositions(drawnPoints.ToArray());
                newPointAdded = false;
            }
        }
        IM.ReleaseControl(ID);
        IM.OnFirstTouchBeginUnsub(ID);
        IM.OnFirstTouchMoveUnsub(ID);
        IM.OnFirstTouchEndUnsub(ID);
        // Actually use the ability with the drawn points
        Debug.Log("THROW!!!!");
    }

    void GetDown(Vector2 p)
    {
        Vector3 worldPoint = IM.GetWorldPoint(p);
        if(currentDrawLength + Vector3.Distance(transform.position, worldPoint) < drawLength)
        {
            drawnPoints.Add(transform.position);
            drawnPoints.Add(worldPoint);
            currentDrawLength += Vector3.Distance(transform.position, worldPoint);
            newPointAdded = true;
        }
    }

    void GetMove(Vector2 p)
    {
        Vector3 worldPoint = IM.GetWorldPoint(p);
        if (currentDrawLength + Vector3.Distance(transform.position, worldPoint) < drawLength)
        {
            drawnPoints.Add(worldPoint);
            currentDrawLength += Vector3.Distance(transform.position, worldPoint);
            newPointAdded = true;
        }
    }

    void GetEnd(Vector2 p)
    {
        Vector3 worldPoint = IM.GetWorldPoint(p);
        if (currentDrawLength + Vector3.Distance(transform.position, worldPoint) < drawLength)
        {
            drawnPoints.Add(worldPoint);
            currentDrawLength += Vector3.Distance(transform.position, worldPoint);
            newPointAdded = true;
        }
        shouldDraw = false;
    }
}
