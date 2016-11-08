using UnityEngine;
using System.Collections;

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

    public void Pause()
    {
        _paused = true;
        prevTimeScale = Time.timeScale;
        Time.timeScale = 0f;
    }
    public void Resume()
    {
        _paused = false;
        Time.timeScale = prevTimeScale;
    }

    public void SlowTime()
    {
        _slow = true;
        _doTimeChange = true;
        prevTimeScale = Time.timeScale;
        tarTimeScale = _slowedTimeScale;
    }
    public void NormalTime()
    {
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
}
