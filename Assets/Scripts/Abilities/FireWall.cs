using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class FireWall : MonoBehaviour {


    [SerializeField]
    private GameObject lineEffect;
    [SerializeField]
    private float cooldown = 5f;
    [SerializeField]
    private float drawLength = 50;
    [SerializeField]
    private float ignitionSpeed = 0.1f;

    private float currentDrawLength;
    private float currentCooldown;
    private List<Vector3> line;
    private List<GameObject> effects;
    private int effectIndex;
    private bool drawing;

    private InputManager im;
    private int id;


    public float Cooldown()
    {
        return currentCooldown < 0 ? 0 : currentCooldown;
    }

    public void UseAbility(Vector3 start)
    {
        if(currentCooldown < 0)
        {
            line.Clear();
            line.Add(start);
            drawing = true;
            currentDrawLength = 0;
            effectIndex = 0;
            StartCoroutine(Ability());
        }
    }
    
    void Start () {
        line = new List<Vector3>();
        effects = new List<GameObject>();
        im = GameManager.input;
        id = im.GetID();
        drawing = false;
        effectIndex = 0;
    }
	
	void Update () {
        currentCooldown -= Time.deltaTime;
	}

    IEnumerator Ability()
    {
        im.OnFirstTouchMoveSub(GetMove, id);
        im.OnFirstTouchEndSub(GetEnd, id);
        im.TakeControl(id);
        yield return StartCoroutine(DrawLine());
        im.ReleaseControl(id);
        im.OnFirstTouchMoveUnsub(id);
        im.OnFirstTouchEndUnsub(id);
        currentCooldown = cooldown;
        StartCoroutine(LightFire());
        yield break;
    }

    IEnumerator DrawLine()
    {
        while (drawing)
        {
            yield return null;
        }
        yield break;
    }

    IEnumerator LightFire()
    {
        print(effects.Count);
        foreach (GameObject go in effects)
        {
            go.GetComponent<PKFxFX>().SetAttribute(new PKFxManager.Attribute("GlobalScale", 2f));
            go.GetComponent<PKFxFX>().SetAttribute(new PKFxManager.Attribute("FireColor", new Vector3(1f, 0.25f, 0f)));
            go.GetComponent<PKFxFX>().SetAttribute(new PKFxManager.Attribute("Smoke", 0f));
            yield return new WaitForSeconds(ignitionSpeed);
        }
        StartCoroutine(RemoveFire());
        yield break;
    }

    IEnumerator RemoveFire()
    {
        yield return new WaitForSeconds(currentCooldown);
        foreach(GameObject go in effects)
        {
            go.GetComponent<PKFxFX>().StopEffect();
        }
        yield break;
    }

    void GetMove(Vector2 p)
    {
        Vector3 worldPoint = im.GetWorldPoint(p);
        if(currentDrawLength + Vector3.Distance(line[line.Count - 1], worldPoint) < drawLength)
        {
            currentDrawLength += Vector3.Distance(line[line.Count - 1], worldPoint);
            line.Add(worldPoint);
            DrawSegment(line[line.Count - 2], line[line.Count - 1]);
        }
        else
        {
            Vector3 k = line[line.Count - 1] + (worldPoint - line[line.Count - 1]).normalized * (drawLength - currentDrawLength);
            line.Add(k);
            DrawSegment(line[line.Count - 2], line[line.Count - 1]);
            currentDrawLength = drawLength;
        }
    }

    void GetEnd(Vector2 p)
    {
        drawing = false;
    }

    void DrawSegment(Vector3 from, Vector3 to)
    {
        Vector3 dir = to - from;
        Vector3 pos = (dir / 2) + from;
        if (effectIndex < effects.Count)
        {
            effects[effectIndex].transform.position = pos;
            effects[effectIndex].GetComponent<PKFxFX>().SetAttribute(new PKFxManager.Attribute("GlobalScale", 0.7f));
            effects[effectIndex].GetComponent<PKFxFX>().SetAttribute(new PKFxManager.Attribute("Fire", dir.magnitude));
            effects[effectIndex].GetComponent<PKFxFX>().SetAttribute(new PKFxManager.Attribute("FireColor", new Vector3(0.25f, 0.25f, 1f)));
            effects[effectIndex].GetComponent<PKFxFX>().SetAttribute(new PKFxManager.Attribute("Smoke", 0f));
            effects[effectIndex].GetComponent<PKFxFX>().SetAttribute(new PKFxManager.Attribute("Area", new Vector3(0, 0, dir.magnitude * 0.5f)));
            effects[effectIndex].GetComponent<PKFxFX>().StartEffect();
            effects[effectIndex].transform.LookAt(to);
        }
        else
        {
            GameObject newLine = Instantiate(lineEffect, pos, Quaternion.identity) as GameObject;
            newLine.GetComponent<PKFxFX>().SetAttribute(new PKFxManager.Attribute("GlobalScale", 0.7f));
            newLine.GetComponent<PKFxFX>().SetAttribute(new PKFxManager.Attribute("Fire", dir.magnitude));
            newLine.GetComponent<PKFxFX>().SetAttribute(new PKFxManager.Attribute("FireColor", new Vector3(0.25f, 0.25f, 1f)));
            newLine.GetComponent<PKFxFX>().SetAttribute(new PKFxManager.Attribute("Smoke", 0f));
            newLine.GetComponent<PKFxFX>().SetAttribute(new PKFxManager.Attribute("Area", new Vector3(0, 0, dir.magnitude * 0.5f)));
            newLine.GetComponent<PKFxFX>().StartEffect();
            newLine.transform.LookAt(to);
            effects.Add(newLine);

        }
        effectIndex += 1;
    }
}
