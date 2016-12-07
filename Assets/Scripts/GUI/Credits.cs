﻿using UnityEngine;
using System.Collections;

public class Credits : MonoBehaviour {
    private float newPos;
    [SerializeField]
    private float speed = 5;

    public void startScrolling() {
        StartCoroutine(scrolling());
    }
    IEnumerator scrolling() {
        gameObject.transform.localPosition = new Vector3(0, -500, 0);
        newPos = gameObject.transform.localPosition.y;
        while (newPos < 7500)
        {
            newPos += speed * Time.deltaTime;
            gameObject.transform.localPosition = new Vector3(0, newPos, 0);
            yield return null;
        }
        yield return null;
    }
}
