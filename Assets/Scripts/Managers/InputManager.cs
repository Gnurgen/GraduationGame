﻿using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour
{
    public enum InputState {draw, move, menu};
    public InputState curState;
    [SerializeField]
    private float _minSwipeSpeed, _minSwipeDist, _doubleTapTime, _minDoubleTapDist, _distPointResolution;
    private Vector2 origPos, endPos, curPos;
    private float _dragDist, _dragSpeed, _dTapTimer;
    private int h, w;
    private bool _dTap = false, _isSwipe = false;
    Camera myCam;

    public delegate void DragDelegate(Vector3 p);
    public event DragDelegate OnDrag;

    void Start()
    {
        myCam = FindObjectOfType<Camera>();
        h = Screen.height;
        w = Screen.width;
    }



    void Update()
    {
        if (curState != InputState.menu && Input.touchCount > 0)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                origPos = Input.touches[0].position;
                curPos = origPos;
            }
            if (Input.touches[0].phase == TouchPhase.Ended)
            {
                if (curState == InputState.move)
                {
                    if (_dTapTimer<=_doubleTapTime && Vector2.Distance(normPos(origPos), normPos(endPos)) <= _minDoubleTapDist)
                        PostDoubleTapEvent();
                }
                endPos = Input.touches[0].position;
                if (!_isSwipe)
                    PostTapEvent();
                else if (_isSwipe)
                    PostSwipeEvent();
                _dTapTimer = 0;
            }
            if (Input.touches[0].phase == TouchPhase.Moved)
            {
                _dragSpeed = normPos(Input.touches[0].deltaPosition).magnitude / Time.deltaTime;
                _dragDist += _dragSpeed * Time.deltaTime;
                curPos = Input.touches[0].position;
                if (_dragDist >= _distPointResolution && curState == InputState.draw)
                    PostNextDragPoint();
                else if (curState == InputState.move && _dragSpeed >= _minSwipeSpeed && _dragDist >= _minSwipeDist)
                    _isSwipe = true;
            }
        }
        if (_dTapTimer<=_doubleTapTime)
            _dTapTimer += Time.deltaTime;
    }

    private Vector2 normPos(Vector2 vec)
    {
        vec = new Vector2(vec.x / w, vec.y / h);
        return vec;
    }

    private void PostNextDragPoint()
    {
        print("PostNextDragPoint");
        _dragDist = 0;
        Vector3 inputPos = curPos;
        inputPos.z = 1f;
        Vector3 worldPos = myCam.ScreenToWorldPoint(inputPos);
        if (OnDrag != null)
            OnDrag(worldPos);

    }

    private void PostDoubleTapEvent()
    {
        print("PostDoubleTapEvent");
    }

    private void PostSwipeEvent()
    {
        print("PostSwipeEvent");
        _dragDist = 0;
        _isSwipe = false;
    }

    private void PostTapEvent()
    {
        print("PostTapEvent");
    }
}
