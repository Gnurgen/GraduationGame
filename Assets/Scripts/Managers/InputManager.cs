using UnityEngine;
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

	// ----- Limits the number of move points sent -----
	private Vector2 lastValidFirstMovePoint;
	private Vector2 lastValidSecondMovePoint;

	// ----- Control Variables -----
	private static int idCount;
	private int owner;
    private int index;
	private List<MethodVectorID> firstTouchBeginMethods;
	private List<MethodVectorID> firstTouchMoveMethods;
	private List<MethodVectorID> firstTouchEndMethods;
	private List<MethodVectorID> tapMethods;
	private List<MethodVectorID> doubleTapMethods;
	private List<MethodSwipeID>  swipeMethods;
    private List<MethodVectorID> firstTouchBeginMethodsAdd;
    private List<MethodVectorID> firstTouchMoveMethodsAdd;
    private List<MethodVectorID> firstTouchEndMethodsAdd;
    private List<MethodVectorID> tapMethodsAdd;
    private List<MethodVectorID> doubleTapMethodsAdd;
    private List<MethodSwipeID> swipeMethodsAdd;
    private List<int> firstTouchBeginMethodsRemove;
    private List<int> firstTouchMoveMethodsRemove;
    private List<int> firstTouchEndMethodsRemove;
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
        Input.multiTouchEnabled = false;
		currentTouchSession = new TouchSession ();
		firstTouchBeginMethods = new List<MethodVectorID> ();
		firstTouchMoveMethods = new List<MethodVectorID> ();
		firstTouchEndMethods = new List<MethodVectorID> ();
		tapMethods = new List<MethodVectorID> ();
		doubleTapMethods = new List<MethodVectorID> ();
		swipeMethods = new List<MethodSwipeID> ();

        firstTouchBeginMethodsAdd = new List<MethodVectorID>();
        firstTouchMoveMethodsAdd = new List<MethodVectorID>();
        firstTouchEndMethodsAdd = new List<MethodVectorID>();
        tapMethodsAdd = new List<MethodVectorID>();
        doubleTapMethodsAdd = new List<MethodVectorID>();
        swipeMethodsAdd = new List<MethodSwipeID>();

        firstTouchBeginMethodsRemove = new List<int>();
        firstTouchMoveMethodsRemove = new List<int>();
        firstTouchEndMethodsRemove = new List<int>();
        tapMethodsRemove = new List<int>();
        doubleTapMethodsRemove = new List<int>();
        swipeMethodsRemove = new List<int>();

        owner = -1;
		idCount = 0;
		mouseTap = false;
		mouseAnalysed = true;
	}

	void Update () {
        print(owner);
        for(int i = 0; i < Input.touches.Length; i++)
        {
            if (currentTouchSession.TouchID() == Input.touches[i].fingerId || currentTouchSession.IsCleanSession())
            {
                switch (Input.touches[i].phase)
                {
                case TouchPhase.Began:
                    currentTouchSession.AddTouchBegin(Input.touches[i]);
                    lastValidFirstMovePoint = Input.touches[i].position;
                    OnFirstTouchBegin(Input.touches[i]);
                    break;
                case TouchPhase.Moved:
                    if (Vector2.Distance(Input.touches[i].position, lastValidFirstMovePoint) > distanceToMove)
                    {
                            lastValidFirstMovePoint = Input.touches[i].position;
                        OnFirstTouchMove(Input.touches[i]);
                    }
                    break;
                case TouchPhase.Ended:
                        currentTouchSession.AddTouchEnd(Input.touches[i]);
                    OnFirstTouchEnd(Input.touches[i]);
                    AdvancedInputAnalysis();
                    currentTouchSession.CleanSession();
                    break;
                }
            }
        }
		/*foreach (Touch t in Input.touches) {
			switch (t.phase) {
			case TouchPhase.Began:
                    fingersTouching += 1;
				if (currentTouchSession.FirstTouchID () == -1) {
					currentTouchSession.AddFirstTouchBegin (t);
					lastValidFirstMovePoint = t.position;
					OnFirstTouchBegin (t);
				}
				break;
			case TouchPhase.Moved:
				if (t.fingerId == currentTouchSession.FirstTouchID () && Vector2.Distance (t.position, lastValidFirstMovePoint) > distanceToMove && currentTouchSession.FirstFingerTouching()) {
					lastValidFirstMovePoint = t.position;
					OnFirstTouchMove (t);

				} 
				break;
			case TouchPhase.Ended:
                    fingersTouching -= 1;
				if (t.fingerId == currentTouchSession.FirstTouchID () && currentTouchSession.FirstFingerTouching()) {
					currentTouchSession.AddFirstTouchEnd (t);
					OnFirstTouchEnd (t);
				} 
                if (fingersTouching == 0) // The last finger has lifted, do advanced input analysis
                {
                    AdvancedInputAnalysis();
                    // Rotate the sessions so the current session becomes the previous one,
                    // and the previous one becomes the current one, but cleaned so its ready for new input.
                    currentTouchSession.CleanSession();
                }

				break;
			}
            
		}*/

		// Support for mouse input.
		if (!mouseIsDown && !mouseAnalysed) {
			// Advanced analysis
			mouseAnalysed = true;
			mouseTap = IsMouseTap ();
			if (mouseTap) {
				OnMouseTap (mouseEnd);
			}
			if (IsMouseSwipe ()) {
				OnMouseSwipe (mouseBegin, mouseEnd);
			}
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
    #region Add/Remove subscribers
    void LateUpdate()
    {
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

        if (firstTouchBeginMethodsAdd.Count > 0)
        {
            foreach (MethodVectorID mvi in firstTouchBeginMethodsAdd)
            {
                firstTouchBeginMethods.Add(mvi);
            }
            firstTouchBeginMethodsAdd.Clear();
        }

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

        if (firstTouchMoveMethodsAdd.Count > 0)
        {
            foreach (MethodVectorID mvi in firstTouchMoveMethodsAdd)
            {
                firstTouchMoveMethods.Add(mvi);
            }
            firstTouchMoveMethodsAdd.Clear();
        }

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

        if (firstTouchEndMethodsAdd.Count > 0)
        {
            foreach (MethodVectorID mvi in firstTouchEndMethodsAdd)
            {
                firstTouchEndMethods.Add(mvi);
            }
            firstTouchEndMethodsAdd.Clear();
        }

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

        if (tapMethodsAdd.Count > 0)
        {
            foreach (MethodVectorID mvi in tapMethodsAdd)
            {
                tapMethods.Add(mvi);
            }
            tapMethodsAdd.Clear();
        }

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

        if (doubleTapMethodsAdd.Count > 0)
        {
            foreach (MethodVectorID mvi in doubleTapMethodsAdd)
            {
                doubleTapMethods.Add(mvi);
            }
            doubleTapMethodsAdd.Clear();
        }

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

        if (swipeMethodsAdd.Count > 0)
        {
            foreach (MethodSwipeID msi in swipeMethodsAdd)
            {
                swipeMethods.Add(msi);
            }
            swipeMethodsAdd.Clear();
        }
    }
    #endregion

    void AdvancedInputAnalysis()
	{
		// Analyse first touch finger
		if(IsTap(currentTouchSession.GetTouch()))
		{
			OnTap (currentTouchSession.GetTouch().end);
		} else if(IsSwipe(currentTouchSession.GetTouch()))
        {
            OnSwipe (currentTouchSession.GetTouch().begin, currentTouchSession.GetTouch().end);
		}
	}

    /*
	 * Helper methods to determine when an interaction is a tap or a swipe.
	*/
    #region Methods
    bool IsTap(SingleTouchSession sts)
	{
		return sts.endTime - sts.beginTime < tapTime && Vector2.Distance (sts.begin.position, sts.end.position) < tapDistance;
	}

	bool IsSwipe(SingleTouchSession sts)
	{
		return sts.endTime -sts.beginTime < swipeTime && Vector2.Distance(sts.begin.position, sts.end.position) > swipeMinDistance;
	}

    bool IsMouseTap()
    {
        return mouseEndTime - mouseBeginTime < swipeTime && Vector3.Distance(mouseBegin, mouseEnd) < tapDistance;
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
		if (firstTouchBeginMethods.Count > 0) {
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

	void OnFirstTouchMove(Touch t)
	{
		if (firstTouchMoveMethods.Count > 0) {
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

	void OnFirstTouchEnd(Touch t)
	{
		if (firstTouchEndMethods.Count > 0) {
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

	void OnTap(Touch t)
    {
        if (tapMethods.Count > 0) {
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

	void OnSwipe(Touch p1, Touch p2)
	{
		if (swipeMethods.Count > 0) {
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
		if (firstTouchBeginMethods.Count > 0) {
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
		if (firstTouchMoveMethods.Count > 0) {
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
		if (firstTouchEndMethods.Count > 0) {
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
        if (tapMethods.Count > 0) {
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

	void OnMouseSwipe(Vector3 p1, Vector3 p2)
	{
		if (swipeMethods.Count > 0) {
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

    /*
     * Structure class
    */
    #region TouchSession
    public class TouchSession
	{

		private bool hasTouch;
		private SingleTouchSession touch;
        private bool touching;

		public TouchSession()
		{
			hasTouch = false;
            touching = false;
		}

		public void AddTouchBegin(Touch t)
		{
			SingleTouchSession sts = new SingleTouchSession ();
			sts.begin = t;
			sts.beginTime = Time.unscaledTime;
			touch = sts;
			hasTouch = true;
            touching = true;
		}

		public void AddTouchEnd(Touch t)
		{
			touch.end = t;
			touch.endTime = Time.unscaledTime;
            touching = false;
		}

		public int TouchID()
		{
			return hasTouch ? touch.begin.fingerId : -1;
		}

		public SingleTouchSession GetTouch()
		{
			return touch;
		}

		public void CleanSession()
		{
			hasTouch = false;
		}

		public bool IsCleanSession()
		{
			return !hasTouch;
		}

        public bool FirstFingerTouching()
        {
            return touching;
        }
	}
    #endregion

    public class SingleTouchSession{
		public Touch begin;
		public float beginTime;
		public Touch end;
		public float endTime;
	}
}