using UnityEngine;
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
    private bool _dTap = false, _isSwipe = false, _isDrag = false;
    private Camera myCam;
    private Ray ray;
    private RaycastHit hit;
    private Vector3 worldPoint, swipeDir;

    public delegate void InputDelegate(Vector3 p);
    public event InputDelegate OnDrag, OnTap, OnDoubleTap, OnSwipe, OnTouchBegin, OnTouchEnd;


    //public delegate void 
    
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
                PostTouchBeginEvent();
            }
            if (Input.touches[0].phase == TouchPhase.Ended)
            {
                if (curState == InputState.move)
                {
                    if (_dTapTimer<=_doubleTapTime && Vector2.Distance(normPos(origPos), normPos(endPos)) <= _minDoubleTapDist)
                        PostDoubleTapEvent();
                }
                endPos = Input.touches[0].position;
                if (!_isSwipe && !_isDrag)
                    PostTapEvent();
                else if (_isSwipe)
                    PostSwipeEvent();
                _dTapTimer = 0;
                _isSwipe = false;
                _isDrag = false;
                PostTouchEndEvent();
            }
            if (Input.touches[0].phase == TouchPhase.Moved)
            {
                _dragSpeed = normPos(Input.touches[0].deltaPosition).magnitude / Time.deltaTime;
                _dragDist += _dragSpeed * Time.deltaTime;
                curPos = Input.touches[0].position;
                if (_dragDist >= _distPointResolution && curState == InputState.draw)
                    PostNextDragPoint();
                else if (curState == InputState.move && _dragSpeed >= _minSwipeSpeed && _dragDist >= _minSwipeDist)
                {
                    _isSwipe = true;
                    PostSwipeEvent();
                }
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

    private Vector3 getWorldPoint(Vector2 p)
    {
        ray = Camera.main.ScreenPointToRay(p);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            worldPoint = hit.point;
        return worldPoint;
    }

    private void PostNextDragPoint()
    {
        print("PostNextDragPoint");
        _dragDist = 0;
        _isDrag = true;
        if (OnDrag != null)
            OnDrag(getWorldPoint(curPos));

    }

    private void PostDoubleTapEvent()
    {
        print("PostDoubleTapEvent");
        if (OnDoubleTap != null)
            OnDoubleTap(getWorldPoint(origPos));
    }

    private void PostSwipeEvent()
    {
        print("PostSwipeEvent");
        _dragDist = 0;
        swipeDir = curPos - origPos;
        swipeDir.z = swipeDir.y;
        swipeDir.y = 0;
        if (OnSwipe != null)
            OnSwipe(swipeDir);
    }

    private void PostTapEvent()
    {
        print("PostTapEvent");
        if (OnTap != null)
            OnTap(getWorldPoint(origPos));
    }

    private void PostTouchBeginEvent()
    {
        if (OnTouchBegin != null)
            OnTouchBegin(getWorldPoint(origPos));
    }
    private void PostTouchEndEvent()
    {
        if (OnTouchEnd != null)
            OnTouchEnd(getWorldPoint(endPos));
    }
}
