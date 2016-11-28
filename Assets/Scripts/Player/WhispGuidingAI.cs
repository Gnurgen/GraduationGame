﻿using UnityEngine;
using System.Collections;
using Pathfinding;

public class WhispGuidingAI : MonoBehaviour {

    // PUBLIC FIELDS
    [SerializeField]
    private float guidingRange;
    [SerializeField]
    private float pointSkipRange;

    // PRIVATE FIELDS
    private Seeker seeker;
    private Transform player;
    private Transform spear;
    private Transform elevator;
    private PKFxFX effectControl;
    private float scatter;
    private Path path;
    private bool waiting;
    private int index;
    private Vector3 guidingPoint;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        elevator = GameObject.FindWithTag("Elevator").transform;
        effectControl = GetComponent<PKFxFX>();
        player = GameManager.player.transform;
        spear = GameManager.spear.transform;
        StartCoroutine(Spawning());
    }

    IEnumerator Spawning()
    {
        effectControl.StopEffect();
        seeker.StartPath(player.position, elevator.position, ReceivePath);
        waiting = true;
        while (waiting || path == null)
        {
            if (!waiting)
            {
                seeker.StartPath(player.position, elevator.position, ReceivePath);
                waiting = true;
            }
            yield return null;
        }
        transform.position = spear.position;
        scatter = 2;
        effectControl.SetAttribute(new PKFxManager.Attribute("Scatter", scatter));
        effectControl.StartEffect();
        while (scatter < 100)
        {
            transform.position = spear.position;
            effectControl.SetAttribute(new PKFxManager.Attribute("Scatter", scatter));
            scatter += 3;
            yield return null;
        }
        transform.position = guidingPoint;
        while(scatter > 2)
        {
            transform.position = guidingPoint;
            effectControl.SetAttribute(new PKFxManager.Attribute("Scatter", scatter));
            scatter -= 3;
            yield return null;
        }
        scatter = 1;
        effectControl.SetAttribute(new PKFxManager.Attribute("Scatter", scatter));
        StartCoroutine(Guiding());
        yield break;
    }

    IEnumerator Guiding()
    {
        while(index < path.vectorPath.Count)
        {
            transform.position = guidingPoint;
            yield return null;
        }
        yield break;
    }

    IEnumerator Hiding()
    {
        yield break;
    }

    IEnumerator Reappearing()
    {
        yield break;
    }

    IEnumerator ActivatingElevator()
    {
        yield break;
    }

    void ReceivePath(Path path)
    {
        waiting = false;
        this.path = path;
        print(path.vectorPath.Count);
        index = 0;
        StartCoroutine(PathFollower());
    }

    IEnumerator PathFollower()
    {
        while(index < path.vectorPath.Count)
        {
            Vector3 dir = (path.vectorPath[index] - player.position).normalized;
            guidingPoint = player.position + new Vector3(dir.x * guidingRange, 1, dir.z * guidingRange);
            if(Vector3.Distance(transform.position, path.vectorPath[index]) < pointSkipRange)
            {
                index++;
            }
            yield return null;
        }
        yield break;
    }
}
