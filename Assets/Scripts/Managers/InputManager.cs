﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputManager : MonoBehaviour {

	// ----- Method wrappers for subscribing to input -----
	public delegate void Vector2Delegate(Vector2 points);
	public delegate void SwipeDelegate(Swipe swipe);

	// ----- Mouse Variables -----
	private bool mouseIsDown;
	// Previous interaction
	private Vector3 mouseBegin;
	private float mouseBeginTime;
	private Vector3 lastValidMouseMovePoint;
	private Vector3 mouseEnd;
	private float mouseEndTime;
	private bool mouseTap;
	private bool mouseAnalysed;
    private int fingersTouching;
	// Current Interaction
	private bool previousMouseTap;
	private float previousMouseTapTime;

	// ----- Inspector Variables -----
	[SerializeField]
	private float distanceToMove = 20f;
	[SerializeField]
	private float tapTime = 0.5f;
	[SerializeField]
	private float tapDistance = 50f;
	[SerializeField]
	private float doubleTapTime = 0.5f;
	[SerializeField]
	private float swipeTime = 0.5f;
	[SerializeField]
	private float swipeMinDistance = 50f;

	// ----- Keeps track of current and previous input -----
	private TouchSession currentTouchSession;
	private TouchSession previousTouchSession;

	// ----- Limits the number of move points sent -----
	private Vector2 lastValidFirstMovePoint;
	private Vector2 lastValidSecondMovePoint;

	// ----- Control Variables -----
	private static int idCount;
	public int owner;
    private int index;
	private List<MethodVectorID> firstTouchBeginMethods;
	private List<MethodVectorID> firstTouchMoveMethods;
	private List<MethodVectorID> firstTouchEndMethods;
	private List<MethodVectorID> secondTouchBeginMethods;
	private List<MethodVectorID> secondTouchMoveMethods;
	private List<MethodVectorID> secondTouchEndMethods;
	private List<MethodVectorID> tapMethods;
	private List<MethodVectorID> doubleTapMethods;
	private List<MethodSwipeID>  swipeMethods;
    private List<MethodVectorID> firstTouchBeginMethodsAdd;
    private List<MethodVectorID> firstTouchMoveMethodsAdd;
    private List<MethodVectorID> firstTouchEndMethodsAdd;
    private List<MethodVectorID> secondTouchBeginMethodsAdd;
    private List<MethodVectorID> secondTouchMoveMethodsAdd;
    private List<MethodVectorID> secondTouchEndMethodsAdd;
    private List<MethodVectorID> tapMethodsAdd;
    private List<MethodVectorID> doubleTapMethodsAdd;
    private List<MethodSwipeID> swipeMethodsAdd;
    private List<int> firstTouchBeginMethodsRemove;
    private List<int> firstTouchMoveMethodsRemove;
    private List<int> firstTouchEndMethodsRemove;
    private List<int> secondTouchBeginMethodsRemove;
    private List<int> secondTouchMoveMethodsRemove;
    private List<int> secondTouchEndMethodsRemove;
    private List<int> tapMethodsRemove;
    private List<int> doubleTapMethodsRemove;
    private List<int> swipeMethodsRemove;

    // ----- Screen to world point Variables -----
    private Ray ray;
	private RaycastHit hit;

    public struct MethodVectorID
	{
		public int id;
		public Vector2Delegate method;
	}

	public struct MethodSwipeID
	{
		public int id;
		public SwipeDelegate method;
	}

	public struct Swipe
	{
		public Vector2 begin;
		public Vector2 end;
	}

	// Initialize the two touch sessions
	void Awake () 
	{
		currentTouchSession = new TouchSession ();
		previousTouchSession = new TouchSession ();
		firstTouchBeginMethods = new List<MethodVectorID> ();
		firstTouchMoveMethods = new List<MethodVectorID> ();
		firstTouchEndMethods = new List<MethodVectorID> ();
		secondTouchBeginMethods = new List<MethodVectorID> ();
		secondTouchMoveMethods = new List<MethodVectorID> ();
		secondTouchEndMethods = new List<MethodVectorID> ();
		tapMethods = new List<MethodVectorID> ();
		doubleTapMethods = new List<MethodVectorID> ();
		swipeMethods = new List<MethodSwipeID> ();

        firstTouchBeginMethodsAdd = new List<MethodVectorID>();
        firstTouchMoveMethodsAdd = new List<MethodVectorID>();
        firstTouchEndMethodsAdd = new List<MethodVectorID>();
        secondTouchBeginMethodsAdd = new List<MethodVectorID>();
        secondTouchMoveMethodsAdd = new List<MethodVectorID>();
        secondTouchEndMethodsAdd = new List<MethodVectorID>();
        tapMethodsAdd = new List<MethodVectorID>();
        doubleTapMethodsAdd = new List<MethodVectorID>();
        swipeMethodsAdd = new List<MethodSwipeID>();

        firstTouchBeginMethodsRemove = new List<int>();
        firstTouchMoveMethodsRemove = new List<int>();
        firstTouchEndMethodsRemove = new List<int>();
        secondTouchBeginMethodsRemove = new List<int>();
        secondTouchMoveMethodsRemove = new List<int>();
        secondTouchEndMethodsRemove = new List<int>();
        tapMethodsRemove = new List<int>();
        doubleTapMethodsRemove = new List<int>();
        swipeMethodsRemove = new List<int>();

        owner = -1;
		idCount = 0;
		previousMouseTap = false;
		mouseTap = false;
		mouseAnalysed = true;
        fingersTouching = 0;
	}

	void Update () { 
		foreach (Touch t in Input.touches) {
			switch (t.phase) {
			case TouchPhase.Began:
                    fingersTouching += 1;
				if (currentTouchSession.FirstTouchID () == -1) {
					currentTouchSession.AddFirstTouchBegin (t);
					lastValidFirstMovePoint = t.position;
					OnFirstTouchBegin (t);
				} else if (currentTouchSession.SecondTouchID () == -1) {
					currentTouchSession.AddSecondTouchBegin (t);
					lastValidSecondMovePoint = t.position;
					OnSecondTouchBegin (t);
				}
				break;
			case TouchPhase.Moved:
				if (t.fingerId == currentTouchSession.FirstTouchID () && Vector2.Distance (t.position, lastValidFirstMovePoint) > distanceToMove && currentTouchSession.FirstFingerTouching()) {
					lastValidFirstMovePoint = t.position;
					OnFirstTouchMove (t);

				} else if (t.fingerId == currentTouchSession.SecondTouchID () && Vector2.Distance (t.position, lastValidSecondMovePoint) > distanceToMove && currentTouchSession.SecondFingerTouching()) {
					lastValidSecondMovePoint = t.position;
					OnSecondTouchMove (t);
				}
				break;
			case TouchPhase.Ended:
                    fingersTouching -= 1;
				if (t.fingerId == currentTouchSession.FirstTouchID () && currentTouchSession.FirstFingerTouching()) {
					currentTouchSession.AddFirstTouchEnd (t);
					OnFirstTouchEnd (t);
				} else if (t.fingerId == currentTouchSession.SecondTouchID () && currentTouchSession.SecondFingerTouching()) {
					currentTouchSession.AddSecondTouchEnd (t);
					OnSecondTouchEnd (t);
				}

                    if (fingersTouching == 0) // The last finger has lifted, do advanced input analysis
                    {
                        AdvancedInputAnalysis();
                        // Rotate the sessions so the current session becomes the previous one,
                        // and the previous one becomes the current one, but cleaned so its ready for new input.
                        TouchSession temp = previousTouchSession;
                        previousTouchSession = currentTouchSession;
                        currentTouchSession = temp;
                        currentTouchSession.CleanSession();
                    }

				break;
			}
		}

		// Support for mouse input.
		if (!mouseIsDown && !mouseAnalysed) {
			// Advanced analysis
			mouseAnalysed = true;
			mouseTap = IsMouseTap ();
			if (mouseTap) {
				OnMouseTap (mouseEnd);
				if (previousMouseTap && mouseEndTime - previousMouseTapTime < doubleTapTime) {
					OnMouseDoubleTap (mouseEnd);
				}
			}
			if (IsMouseSwipe ()) {
				OnMouseSwipe (mouseBegin, mouseEnd);
			}
			previousMouseTap = mouseTap;
			previousMouseTapTime = mouseEndTime;

		} else {
			if (Input.GetMouseButtonDown (0)) {
				mouseIsDown = true;
				mouseAnalysed = false;
				mouseBegin = Input.mousePosition;
				mouseBeginTime = Time.unscaledTime;
				lastValidMouseMovePoint = Input.mousePosition;
				OnMouseBegin (mouseBegin);
			} else if (Input.GetMouseButton (0)) {
				if (Vector3.Distance (Input.mousePosition, lastValidMouseMovePoint) > distanceToMove) {
					lastValidMouseMovePoint = Input.mousePosition;
					OnMouseMove (lastValidMouseMovePoint);
				}
			} else if (Input.GetMouseButtonUp (0)) {
				mouseEnd = Input.mousePosition;
				mouseEndTime = Time.unscaledTime;
				mouseIsDown = false;
				OnMouseEnd (mouseEnd);
			}
		}
	}

	void AdvancedInputAnalysis()
	{
		// Analyse first touch finger
		if(IsTap(currentTouchSession.GetFirstTouch()))
		{
			OnTap (currentTouchSession.GetFirstTouch().end);
			if(previousTouchSession.FirstTouchID() != -1 && IsTap(previousTouchSession.GetFirstTouch()) && IsDoubleTap(previousTouchSession.GetFirstTouch(), currentTouchSession.GetFirstTouch()))
            {
                OnDoubleTap (previousTouchSession.GetFirstTouch().end, currentTouchSession.GetFirstTouch().end);
			}
		} else if(IsSwipe(currentTouchSession.GetFirstTouch()))
        {
            OnSwipe (currentTouchSession.GetFirstTouch().begin, currentTouchSession.GetFirstTouch().end);
		}
		// Analyse second touch finger
		if(currentTouchSession.SecondTouchID() != -1)
		{
			if(IsTap(currentTouchSession.GetSecondTouch()))
			{
				OnTap (currentTouchSession.GetSecondTouch().end);
				if(previousTouchSession.SecondTouchID() != -1 && IsTap(previousTouchSession.GetSecondTouch()) && IsDoubleTap(previousTouchSession.GetSecondTouch(), currentTouchSession.GetSecondTouch()))
				{
					OnDoubleTap (previousTouchSession.GetSecondTouch().end, currentTouchSession.GetSecondTouch().end);
				}
			} else if(IsSwipe(currentTouchSession.GetSecondTouch()))
			{
				OnSwipe (currentTouchSession.GetSecondTouch().begin, currentTouchSession.GetSecondTouch().end);
			}
		}
	}

    /*
	 * Helper methods to determine when an interaction is a tap, doubletap or a swipe.
	*/
    #region Methods
    bool IsTap(SingleTouchSession sts)
	{
		return sts.endTime - sts.beginTime < tapTime && Vector2.Distance (sts.begin.position, sts.end.position) < tapDistance;
	}

	bool IsMouseTap()
	{
		return mouseEndTime - mouseBeginTime < swipeTime && Vector3.Distance (mouseBegin, mouseEnd) < tapDistance;
	}

	bool IsDoubleTap(SingleTouchSession sts1, SingleTouchSession sts2)
	{
		return sts2.endTime - sts1.beginTime < doubleTapTime && Vector2.Distance(sts1.end.position, sts2.end.position) < tapDistance;
	}

	bool IsSwipe(SingleTouchSession sts)
	{
		return sts.endTime -sts.beginTime < swipeTime && Vector2.Distance(sts.begin.position, sts.end.position) > swipeMinDistance;
	}

	bool IsMouseSwipe()
	{
		return mouseEndTime - mouseBeginTime < swipeTime && Vector3.Distance (mouseBegin, mouseEnd) > swipeMinDistance;
	}
    #endregion


    /*
     * Touch trigger methods.
    */
    #region Methods
    void OnFirstTouchBegin(Touch t)
	{
        if (firstTouchBeginMethodsAdd.Count > 0)
        {
            foreach (MethodVectorID mvi in firstTouchBeginMethodsAdd)
            {
                firstTouchBeginMethods.Add(mvi);
            }
            firstTouchBeginMethodsAdd.Clear();
        }

		if (firstTouchBeginMethods.Count > 0) {
            if(firstTouchBeginMethodsRemove.Count > 0)
            {
                foreach(int ID in firstTouchBeginMethodsRemove)
                {
                    index = -1;
                    for (int i = 0; i < firstTouchBeginMethods.Count; i++)
                    {
                        if (firstTouchBeginMethods[i].id == ID)
                        {
                            index = i;
                        }
                    }
                    if (index >= 0)
                    {
                        firstTouchBeginMethods.RemoveAt(index);
                    }
                }
                firstTouchBeginMethodsRemove.Clear();
            }

			if (owner < 0) {
				foreach (MethodVectorID mvi in firstTouchBeginMethods) {
					mvi.method (t.position);
				}
			} else {
				foreach (MethodVectorID mvi in firstTouchBeginMethods) {
					if (mvi.id == owner) {
						mvi.method (t.position);
					}
				}
			}
        }
	}

	void OnSecondTouchBegin(Touch t)
	{
        if (secondTouchBeginMethodsAdd.Count > 0)
        {
            foreach (MethodVectorID mvi in secondTouchBeginMethodsAdd)
            {
                secondTouchBeginMethods.Add(mvi);
            }
            secondTouchBeginMethodsAdd.Clear();
        }

		if (secondTouchBeginMethods.Count > 0) {
            if(secondTouchBeginMethodsRemove.Count > 0)
            {
                foreach(int ID in secondTouchBeginMethodsRemove)
                {
                    index = -1;
                    for (int i = 0; i < secondTouchBeginMethods.Count; i++)
                    {
                        if (secondTouchBeginMethods[i].id == ID)
                        {
                            index = i;
                        }
                    }
                    if (index >= 0)
                    {
                        secondTouchBeginMethods.RemoveAt(index);
                    }
                }
                secondTouchBeginMethodsRemove.Clear();
            }

			if (owner < 0) {
				foreach (MethodVectorID mvi in secondTouchBeginMethods) {
					mvi.method (t.position);
				}
			} else {
				foreach (MethodVectorID mvi in secondTouchBeginMethods) {
					if (mvi.id == owner) {
						mvi.method (t.position);
					}
				}
			}
        }
	}

	void OnFirstTouchMove(Touch t)
	{
        if (firstTouchMoveMethodsAdd.Count > 0)
        {
            foreach (MethodVectorID mvi in firstTouchMoveMethodsAdd)
            {
                firstTouchMoveMethods.Add(mvi);
            }
            firstTouchMoveMethodsAdd.Clear();
        }

		if (firstTouchMoveMethods.Count > 0) {
            if(firstTouchMoveMethodsRemove.Count > 0)
            {
                foreach(int ID in firstTouchMoveMethodsRemove)
                {
                    index = -1;
                    for (int i = 0; i < firstTouchMoveMethods.Count; i++)
                    {
                        if (firstTouchMoveMethods[i].id == ID)
                        {
                            index = i;
                        }
                    }
                    if (index >= 0)
                    {
                        firstTouchMoveMethods.RemoveAt(index);
                    }
                }
                firstTouchMoveMethodsRemove.Clear();
            }

            if (owner < 0) {
				foreach (MethodVectorID mvi in firstTouchMoveMethods) {
					mvi.method (t.position);
				}
			} else {
				foreach (MethodVectorID mvi in firstTouchMoveMethods) {
					if (mvi.id == owner) {
						mvi.method (t.position);
					}
				}
			}
        }
	}

	void OnSecondTouchMove(Touch t)
	{
        if (secondTouchMoveMethodsAdd.Count > 0)
        {
            foreach (MethodVectorID mvi in secondTouchMoveMethodsAdd)
            {
                secondTouchMoveMethods.Add(mvi);
            }
            secondTouchMoveMethodsAdd.Clear();
        }

		if (secondTouchMoveMethods.Count > 0) {
            if(secondTouchMoveMethodsRemove.Count > 0)
            {
                foreach(int ID in secondTouchMoveMethodsRemove)
                {
                    index = -1;
                    for (int i = 0; i < secondTouchMoveMethods.Count; i++)
                    {
                        if (secondTouchMoveMethods[i].id == ID)
                        {
                            index = i;
                        }
                    }
                    if (index >= 0)
                    {
                        secondTouchMoveMethods.RemoveAt(index);
                    }
                }
                secondTouchMoveMethodsRemove.Clear();
            }

            if (owner < 0) {
				foreach (MethodVectorID mvi in secondTouchMoveMethods) {
					mvi.method (t.position);
				}
			} else {
				foreach (MethodVectorID mvi in secondTouchMoveMethods) {
					if (mvi.id == owner) {
						mvi.method (t.position);
					}
				}
			}
        }
	}

	void OnFirstTouchEnd(Touch t)
	{
        if (firstTouchEndMethodsAdd.Count > 0)
        {
            foreach (MethodVectorID mvi in firstTouchEndMethodsAdd)
            {
                firstTouchEndMethods.Add(mvi);
            }
            firstTouchEndMethodsAdd.Clear();
        }

		if (firstTouchEndMethods.Count > 0) {
            if(firstTouchEndMethodsRemove.Count > 0)
            {
                foreach(int ID in firstTouchEndMethodsRemove)
                {
                    index = -1;
                    for (int i = 0; i < firstTouchEndMethods.Count; i++)
                    {
                        if (firstTouchEndMethods[i].id == ID)
                        {
                            index = i;
                        }
                    }
                    if (index >= 0)
                    {
                        firstTouchEndMethods.RemoveAt(index);
                    }
                }
                firstTouchEndMethodsRemove.Clear();
            }

			if (owner < 0) {
				foreach (MethodVectorID mvi in firstTouchEndMethods) {
					mvi.method (t.position);
				}
			} else {
				foreach (MethodVectorID mvi in firstTouchEndMethods) {
					if (mvi.id == owner) {
						mvi.method (t.position);
					}
				}
			}
        }
	}

	void OnSecondTouchEnd(Touch t)
	{
        if (secondTouchEndMethodsAdd.Count > 0)
        {
            foreach (MethodVectorID mvi in secondTouchEndMethodsAdd)
            {
                secondTouchEndMethods.Add(mvi);
            }
            secondTouchEndMethodsAdd.Clear();
        }

		if (secondTouchEndMethods.Count > 0) {
            if(secondTouchEndMethodsRemove.Count > 0)
            {
                foreach(int ID in secondTouchEndMethodsRemove)
                {
                    index = -1;
                    for (int i = 0; i < secondTouchEndMethods.Count; i++)
                    {
                        if (secondTouchEndMethods[i].id == ID)
                        {
                            index = i;
                        }
                    }
                    if (index >= 0)
                    {
                        secondTouchEndMethods.RemoveAt(index);
                    }
                }
                secondTouchEndMethodsRemove.Clear();
            }

			if (owner < 0) {
				foreach (MethodVectorID mvi in secondTouchEndMethods) {
					mvi.method (t.position);
				}
			} else {
				foreach (MethodVectorID mvi in secondTouchEndMethods) {
					if (mvi.id == owner) {
						mvi.method (t.position);
					}
				}
			}
        }
	}

	void OnTap(Touch t)
	{
        if (tapMethodsAdd.Count > 0)
        {
            foreach (MethodVectorID mvi in tapMethodsAdd)
            {
                tapMethods.Add(mvi);
            }
            tapMethodsAdd.Clear();
        }

		if (tapMethods.Count > 0) {
            if(tapMethodsRemove.Count > 0)
            {
                foreach(int ID in tapMethodsRemove)
                {
                    index = -1;
                    for (int i = 0; i < tapMethods.Count; i++)
                    {
                        if (tapMethods[i].id == ID)
                        {
                            index = i;
                        }
                    }
                    if (index >= 0)
                    {
                        tapMethods.RemoveAt(index);
                    }
                }
                tapMethodsRemove.Clear();
            }

			if (owner < 0) {
				foreach (MethodVectorID mvi in tapMethods) {
					mvi.method (t.position);
				}
			} else {
				foreach (MethodVectorID mvi in tapMethods) {
					if (mvi.id == owner) {
						mvi.method (t.position);
					}
				}
			}
        }
	}

	void OnDoubleTap(Touch tp, Touch tc)
	{
        if (doubleTapMethodsAdd.Count > 0)
        {
            foreach (MethodVectorID mvi in doubleTapMethodsAdd)
            {
                doubleTapMethods.Add(mvi);
            }
            doubleTapMethodsAdd.Clear();
        }

		if (doubleTapMethods.Count > 0) {
            if(doubleTapMethodsRemove.Count > 0)
            {
                foreach(int ID in doubleTapMethodsRemove)
                {
                    index = -1;
                    for (int i = 0; i < doubleTapMethods.Count; i++)
                    {
                        if (doubleTapMethods[i].id == ID)
                        {
                            index = i;
                        }
                    }
                    if (index >= 0)
                    {
                        doubleTapMethods.RemoveAt(index);
                    }
                }
                doubleTapMethodsRemove.Clear();
            }

			if (owner < 0) {
				foreach (MethodVectorID mvi in doubleTapMethods) {
					mvi.method (tc.position);
				}
			} else {
				foreach (MethodVectorID mvi in doubleTapMethods) {
					if (mvi.id == owner) {
						mvi.method (tc.position);
					}
				}
			}
        }
	}

	void OnSwipe(Touch p1, Touch p2)
	{
        if (swipeMethodsAdd.Count > 0)
        {
            foreach (MethodSwipeID msi in swipeMethodsAdd)
            {
                swipeMethods.Add(msi);
            }
            swipeMethodsAdd.Clear();
        }

		if (swipeMethods.Count > 0) {
            if(swipeMethodsRemove.Count > 0)
            {
                foreach(int ID in swipeMethodsRemove)
                {
                    index = -1;
                    for (int i = 0; i < swipeMethods.Count; i++)
                    {
                        if (swipeMethods[i].id == ID)
                        {
                            index = i;
                        }
                    }
                    if (index >= 0)
                    {
                        swipeMethods.RemoveAt(index);
                    }
                }
                swipeMethodsRemove.Clear();
            }

            Swipe s = new Swipe ();
			s.begin = p1.position;
			s.end = p2.position;
			if (owner < 0) {
				foreach (MethodSwipeID msi in swipeMethods) {
					msi.method (s);
				}
			} else {
				foreach (MethodSwipeID msi in swipeMethods) {
					if (msi.id == owner) {
						msi.method (s);
					}
				}
			}
        }
	}
    #endregion


    /*
	 * Mouse trigger methods.
    */
    #region Methods
    void OnMouseBegin(Vector3 p)
	{
        if (firstTouchBeginMethodsAdd.Count > 0)
        {
            foreach (MethodVectorID mvi in firstTouchBeginMethodsAdd)
            {
                firstTouchBeginMethods.Add(mvi);
            }
            firstTouchBeginMethodsAdd.Clear();
        }

		if (firstTouchBeginMethods.Count > 0) {
            if (firstTouchBeginMethodsRemove.Count > 0)
            {
                foreach (int ID in firstTouchBeginMethodsRemove)
                {
                    index = -1;
                    for (int i = 0; i < firstTouchBeginMethods.Count; i++)
                    {
                        if (firstTouchBeginMethods[i].id == ID)
                        {
                            index = i;
                        }
                    }
                    if (index >= 0)
                    {
                        firstTouchBeginMethods.RemoveAt(index);
                    }
                }
                firstTouchBeginMethodsRemove.Clear();
            }

			Vector2 point = new Vector2 (p.x,p.y);
			if (owner < 0) {
				foreach (MethodVectorID mvi in firstTouchBeginMethods) {
					mvi.method (point);
				}
			} else {
				foreach (MethodVectorID mvi in firstTouchBeginMethods) {
					if (mvi.id == owner) {
						mvi.method (point);
					}
				}
			}
        }
	}

	void OnMouseMove(Vector3 p)
	{
        if (firstTouchMoveMethodsAdd.Count > 0)
        {
            foreach (MethodVectorID mvi in firstTouchMoveMethodsAdd)
            {
                firstTouchMoveMethods.Add(mvi);
            }
            firstTouchMoveMethodsAdd.Clear();
        }

		if (firstTouchMoveMethods.Count > 0) {
            if (firstTouchMoveMethodsRemove.Count > 0)
            {
                foreach (int ID in firstTouchMoveMethodsRemove)
                {
                    index = -1;
                    for (int i = 0; i < firstTouchMoveMethods.Count; i++)
                    {
                        if (firstTouchMoveMethods[i].id == ID)
                        {
                            index = i;
                        }
                    }
                    if (index >= 0)
                    {
                        firstTouchMoveMethods.RemoveAt(index);
                    }
                }
                firstTouchMoveMethodsRemove.Clear();
            }

			Vector2 point = new Vector2 (p.x,p.y);
			if (owner < 0) {
				foreach (MethodVectorID mvi in firstTouchMoveMethods) {
					mvi.method (point);
				}
			} else {
				foreach (MethodVectorID mvi in firstTouchMoveMethods) {
					if (mvi.id == owner) {
						mvi.method (point);
					}
				}
			}
        }
	}

	void OnMouseEnd(Vector3 p)
	{
    
        if (firstTouchEndMethodsAdd.Count > 0)
        {
            foreach (MethodVectorID mvi in firstTouchEndMethodsAdd)
            {
                print("ADDER: " + mvi.id);
                firstTouchEndMethods.Add(mvi);
            }
            firstTouchEndMethodsAdd.Clear();
        }

		if (firstTouchEndMethods.Count > 0) {
            if (firstTouchEndMethodsRemove.Count > 0)
            {
                foreach (int ID in firstTouchEndMethodsRemove)
                {
                    index = -1;
                    for (int i = 0; i < firstTouchEndMethods.Count; i++)
                    {
                        if (firstTouchEndMethods[i].id == ID)
                        {
                            index = i;
                        }
                    }
                    if (index >= 0)
                    {
                        firstTouchEndMethods.RemoveAt(index);
                    }
                }
                firstTouchEndMethodsRemove.Clear();
            }

			Vector2 point = new Vector2 (p.x,p.y);
			if (owner < 0) {
				foreach (MethodVectorID mvi in firstTouchEndMethods) {
					mvi.method (point);
				}
			} else {
				foreach (MethodVectorID mvi in firstTouchEndMethods) {
					if (mvi.id == owner) {

						mvi.method (point);
					}
				}
			}
        }
	}

	void OnMouseTap(Vector3 p)
	{
        if (tapMethodsAdd.Count > 0)
        {
            foreach (MethodVectorID mvi in tapMethodsAdd)
            {
                tapMethods.Add(mvi);
            }
            tapMethodsAdd.Clear();
        }

		if (tapMethods.Count > 0) {
            if (tapMethodsRemove.Count > 0)
            {
                foreach (int ID in tapMethodsRemove)
                {
                    index = -1;
                    for (int i = 0; i < tapMethods.Count; i++)
                    {
                        if (tapMethods[i].id == ID)
                        {
                            index = i;
                        }
                    }
                    if (index >= 0)
                    {
                        tapMethods.RemoveAt(index);
                    }
                }
                tapMethodsRemove.Clear();
            }

			Vector2 point = new Vector2 (p.x,p.y);
			if (owner < 0) {
				foreach (MethodVectorID mvi in tapMethods) {
					mvi.method (point);
				}
			} else {
				foreach (MethodVectorID mvi in tapMethods) {
					if (mvi.id == owner) {
						mvi.method (point);
					}
				}
			}
        }
	}

	void OnMouseDoubleTap(Vector3 p)
	{
        if (doubleTapMethodsAdd.Count > 0)
        {
            foreach (MethodVectorID mvi in doubleTapMethodsAdd)
            {
                doubleTapMethods.Add(mvi);
            }
            doubleTapMethodsAdd.Clear();
        }

		if (doubleTapMethods.Count > 0) {
            if (doubleTapMethodsRemove.Count > 0)
            {
                foreach (int ID in doubleTapMethodsRemove)
                {
                    index = -1;
                    for (int i = 0; i < doubleTapMethods.Count; i++)
                    {
                        if (doubleTapMethods[i].id == ID)
                        {
                            index = i;
                        }
                    }
                    if (index >= 0)
                    {
                        doubleTapMethods.RemoveAt(index);
                    }
                }
                doubleTapMethodsRemove.Clear();
            }

			Vector2 point = new Vector2 (p.x,p.y);
			if (owner < 0) {
				foreach (MethodVectorID mvi in doubleTapMethods) {
					mvi.method (point);
				}
			} else {
				foreach (MethodVectorID mvi in doubleTapMethods) {
					if (mvi.id == owner) {
						mvi.method (point);
					}
				}
			}
        }
	}

	void OnMouseSwipe(Vector3 p1, Vector3 p2)
	{
        if (swipeMethodsAdd.Count > 0)
        {
            foreach (MethodSwipeID msi in swipeMethodsAdd)
            {
                swipeMethods.Add(msi);
            }
            swipeMethodsAdd.Clear();
        }

		if (swipeMethods.Count > 0) {
            if (swipeMethodsRemove.Count > 0)
            {
                foreach (int ID in swipeMethodsRemove)
                {
                    index = -1;
                    for (int i = 0; i < swipeMethods.Count; i++)
                    {
                        if (swipeMethods[i].id == ID)
                        {
                            index = i;
                        }
                    }
                    if (index >= 0)
                    {
                        swipeMethods.RemoveAt(index);
                    }
                }
                swipeMethodsRemove.Clear();
            }

			Swipe s = new Swipe ();
			s.begin = new Vector2 (p1.x,p1.y);
			s.end = new Vector2 (p2.x,p2.y);
			if (owner < 0) {
				foreach (MethodSwipeID msi in swipeMethods) {
					msi.method (s);
				}
			} else {
				foreach (MethodSwipeID msi in swipeMethods) {
					if (msi.id == owner) {
						msi.method (s);
					}
				}
			}
        }
	}
    #endregion

    /*
	 * 
	 * Public Methods 
	 * 
	 */
    #region Methods
    public int GetID()
	{
		int temp = idCount;
		idCount++;
		return temp;
	}

	public bool TakeControl(int ID)
	{
		if(owner < 0)
		{
			owner = ID;
			return true;
		} 
		else 
		{
			return false;
		}
	}

	public bool ReleaseControl(int ID)
	{
		if(owner == ID)
		{
			owner = -1;
			return true;
		}
		else
		{
			return false;
		}
	}

	public Vector3 GetWorldPoint(Vector2 pos)
	{
		ray = Camera.main.ScreenPointToRay (pos);
        int layermask = 1 << LayerMask.NameToLayer("Walkable");
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layermask))
			return hit.point;
		return GameManager.player.transform.position;
		//return Camera.main.ScreenToWorldPoint(new Vector3(pos.x, pos.y, 10));
	}
    #endregion

    /*
     * Subscibe and Unsubscribe methods
    */
    #region Methods
    public void OnFirstTouchBeginSub(Vector2Delegate vd, int ID)
	{
		bool exists = false;
		for(int i = 0; i < firstTouchBeginMethods.Count; i++)
		{
			if(firstTouchBeginMethods[i].id == ID)
			{
				exists = true;
			}
		}
		if(!exists)
		{
			MethodVectorID mvi = new MethodVectorID ();
			mvi.id = ID;
			mvi.method = vd;
			firstTouchBeginMethodsAdd.Add (mvi);
		}
	}

	public void OnFirstTouchBeginUnsub(int ID)
	{
        firstTouchBeginMethodsRemove.Add(ID);
	}

	public void OnFirstTouchMoveSub(Vector2Delegate vd, int ID)
	{
		bool exists = false;
		for(int i = 0; i < firstTouchMoveMethods.Count; i++)
		{
			if(firstTouchMoveMethods[i].id == ID)
			{
				exists = true;
			}
		}
		if(!exists)
		{
			MethodVectorID mvi = new MethodVectorID ();
			mvi.id = ID;
			mvi.method = vd;
            firstTouchMoveMethodsAdd.Add (mvi);
		}
	}

	public void OnFirstTouchMoveUnsub(int ID)
	{
        firstTouchMoveMethodsRemove.Add(ID);
	}

	public void OnFirstTouchEndSub(Vector2Delegate vd, int ID)
	{
		bool exists = false;
		for(int i = 0; i < firstTouchEndMethods.Count; i++)
		{
			if(firstTouchEndMethods[i].id == ID)
			{
				exists = true;
			}
		}
		if(!exists)
		{
			MethodVectorID mvi = new MethodVectorID ();
			mvi.id = ID;
			mvi.method = vd;
            firstTouchEndMethodsAdd.Add (mvi);
		}
	}

	public void OnFirstTouchEndUnsub(int ID)
	{
        firstTouchEndMethodsRemove.Add(ID);
	}

	public void OnSecondTouchBeginSub(Vector2Delegate vd, int ID)
	{
		bool exists = false;
		for(int i = 0; i < secondTouchBeginMethods.Count; i++)
		{
			if(secondTouchBeginMethods[i].id == ID)
			{
				exists = true;
			}
		}
		if(!exists)
		{
			MethodVectorID mvi = new MethodVectorID ();
			mvi.id = ID;
			mvi.method = vd;
            secondTouchBeginMethodsAdd.Add (mvi);
		}
	}

	public void OnSecondTouchBeginUnsub(int ID)
	{
        secondTouchBeginMethodsRemove.Add(ID);
	}

	public void OnSecondTouchMoveSub(Vector2Delegate vd, int ID)
	{
		bool exists = false;
		for(int i = 0; i < secondTouchMoveMethods.Count; i++)
		{
			if(secondTouchMoveMethods[i].id == ID)
			{
				exists = true;
			}
		}
		if(!exists)
		{
			MethodVectorID mvi = new MethodVectorID ();
			mvi.id = ID;
			mvi.method = vd;
            secondTouchMoveMethodsAdd.Add (mvi);
		}
	}

	public void OnSecondTouchMoveUnsub(int ID)
	{
        secondTouchMoveMethodsRemove.Add(ID);
	}

	public void OnSecondTouchEndSub(Vector2Delegate vd, int ID)
	{
		bool exists = false;
		for(int i = 0; i < secondTouchEndMethods.Count; i++)
		{
			if(secondTouchEndMethods[i].id == ID)
			{
				exists = true;
			}
		}
		if(!exists)
		{
			MethodVectorID mvi = new MethodVectorID ();
			mvi.id = ID;
			mvi.method = vd;
            secondTouchEndMethodsAdd.Add (mvi);
		}
	}

	public void OnSecondTouchEndUnsub(int ID)
	{
        secondTouchEndMethodsRemove.Add(ID);
	}

	public void OnTapSub(Vector2Delegate vd, int ID)
	{
		bool exists = false;
		for(int i = 0; i < tapMethods.Count; i++)
		{
			if(tapMethods[i].id == ID)
			{
				exists = true;
			}
		}
		if(!exists)
		{
			MethodVectorID mvi = new MethodVectorID ();
			mvi.id = ID;
			mvi.method = vd;
            tapMethodsAdd.Add (mvi);
		}
	}

	public void OnTapUnsub(int ID)
	{
        tapMethodsRemove.Add(ID);
	}

	public void OnDoubleTapSub(Vector2Delegate vd, int ID)
	{
		bool exists = false;
		for(int i = 0; i < doubleTapMethods.Count; i++)
		{
			if(doubleTapMethods[i].id == ID)
			{
				exists = true;
			}
		}
		if(!exists)
		{
			MethodVectorID mvi = new MethodVectorID ();
			mvi.id = ID;
			mvi.method = vd;
            doubleTapMethodsAdd.Add (mvi);
		}
	}

	public void OnDoubleTapUnsub(int ID)
	{
        doubleTapMethodsRemove.Add(ID);
	}

	public void OnSwipeSub(SwipeDelegate sd, int ID)
	{
		bool exists = false;
		for(int i = 0; i < swipeMethods.Count; i++)
		{
			if(swipeMethods[i].id == ID)
			{
				exists = true;
			}
		}
		if(!exists)
		{
			MethodSwipeID mvi = new MethodSwipeID ();
			mvi.id = ID;
			mvi.method = sd;
            swipeMethodsAdd.Add (mvi);
		}
	}

    public void OnSwipeUnsub(int ID)
	{
        swipeMethodsRemove.Add(ID);
	}
    #endregion


	public class TouchSession
	{

		private bool hasFirstTouch;
		private SingleTouchSession firstTouch;
        private bool firstTouching;
		private bool hasSecondTouch;
		private SingleTouchSession secondTouch;
        private bool secondTouching;

		public TouchSession()
		{
			hasFirstTouch = false;
            firstTouching = false;
			hasSecondTouch = false;
            secondTouching = false;
		}

		public void AddFirstTouchBegin(Touch t)
		{
			SingleTouchSession sts = new SingleTouchSession ();
			sts.begin = t;
			sts.beginTime = Time.unscaledTime;
			firstTouch = sts;
			hasFirstTouch = true;
            firstTouching = true;
		}

		public void AddFirstTouchEnd(Touch t)
		{
			firstTouch.end = t;
			firstTouch.endTime = Time.unscaledTime;
            firstTouching = false;
		}

		public void AddSecondTouchBegin(Touch t)
		{
			SingleTouchSession sts = new SingleTouchSession ();
			sts.begin = t;
			sts.beginTime = Time.unscaledTime;
			secondTouch = sts;
			hasSecondTouch = true;
            secondTouching = true;
		}

		public void AddSecondTouchEnd(Touch t)
		{
			secondTouch.end = t;
			secondTouch.endTime = Time.unscaledTime;
            secondTouching = false;
		}

		public int FirstTouchID()
		{
			return hasFirstTouch ? firstTouch.begin.fingerId : -1;
		}

		public int SecondTouchID()
		{
			return hasSecondTouch ? secondTouch.begin.fingerId : -1;
		}

		public SingleTouchSession GetFirstTouch()
		{
			return firstTouch;
		}

		public SingleTouchSession GetSecondTouch()
		{
			return secondTouch;
		}

		public void CleanSession()
		{
			hasFirstTouch = false;
			hasSecondTouch = false;
		}

		public bool IsCleanSession()
		{
			return !hasFirstTouch && !hasSecondTouch;
		}

        public bool FirstFingerTouching()
        {
            return firstTouching;
        }

        public bool SecondFingerTouching()
        {
            return secondTouching;
        }
	}

	public class SingleTouchSession{
		public Touch begin;
		public float beginTime;
		public Touch end;
		public float endTime;
	}
}