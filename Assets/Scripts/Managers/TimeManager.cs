using UnityEngine;
using System.Collections;
using System;

public class TimeManager : MonoBehaviour {
    private float prevTimeScale, tarTimeScale, _normTime = 0;
    private bool _doTimeChange = false, _paused = false, _slow = false;
    public bool IsPaused
    {
        get
        {
            return _paused;
        }
    }
    public bool IsSlowed
    {
        get
        {
            return _slow;
        }
    }
    [SerializeField]
    private float _slowedTimeScale, _timeChangeDuration;

    private void Subscribe()
    {
        GameManager.events.OnMenuClose += Resume;
        GameManager.events.OnWheelOpen += SlowTime;
        GameManager.events.OnWheelSelect += NormalTime;
        GameManager.events.OnDrawComplete += NormalTime;
        //GameManager.events.OnEnemyDeath += FreezeTime;
    }

    private void FreezeTime(GameObject enemyID)
    {
        StartCoroutine(FreezeFrame(5));
    }

    private IEnumerator FreezeFrame(int frames)
    {
        Time.timeScale = 0.25f;
        int curFrames = 0;
        while (frames > curFrames)
        {
            Time.timeScale = 0.25f;
            curFrames++;
            yield return null;
        }

        Time.timeScale = 1;
    }

    void Update () {
        if(_doTimeChange)
        {
            _normTime +=  Time.unscaledDeltaTime / _timeChangeDuration; //normalize change curation based on unscale time
            if(_normTime>=1f) //Check that timescale will be set within bounds before assigning
            {
                Time.timeScale = tarTimeScale;
                _normTime = 0;
                _doTimeChange = false;
                return;
            }
            Time.timeScale = prevTimeScale + (tarTimeScale - prevTimeScale) * _normTime; //Change timescale linearly
        }
    }

    private void Pause()
    {
        _paused = true;
        prevTimeScale = Time.timeScale;
        Time.timeScale = 0f;
    }
    private void Resume()
    {
        _paused = false;
        Time.timeScale = prevTimeScale;
    }

    private void SlowTime()
    {
        _slow = true;
        _doTimeChange = true;
        prevTimeScale = Time.timeScale;
        tarTimeScale = _slowedTimeScale;
    }
    private void NormalTime(int i)
    {
        if (i != 10)
            return;
        _slow = false;
        _doTimeChange = true;
        prevTimeScale = Time.timeScale;
        tarTimeScale = 1f;

    }
    public void SetTimeScale(float t)
    {
        if (t < 0f)
            t = 0f;
        tarTimeScale = t;
        prevTimeScale = Time.timeScale;
        _doTimeChange = true;
    }
    public void SetTimeScaleInstant(float t)
    {
        if (t < 0f)
            t = 0f;
        Time.timeScale = t;
    }
}
