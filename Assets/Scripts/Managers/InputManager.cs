using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputManager : MonoBehaviour {

	// ----- Method wrappers for subscribing to input -----
	public delegate void Vector2Delegate(Vector2 points);
	public delegate void SwipeDelegate(Swipe swipe);
	enum State
	{
		Touch, Mouse
	};

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
	// Current Interaction
	private bool previousMouseTap;
	private float previousMouseTapTime;

	// ----- Inspector Variables -----
	[SerializeField]
	private State inputState = State.Touch;
	[SerializeField]
	private float distanceToMove = 20f;
	[SerializeField]
	private float tapTime = 0.5f;
	[SerializeField]
	private float tapDistance = 20f;
	[SerializeField]
	private float doubleTapTime = 0.5f;
	[SerializeField]
	private float swipeTime = 0.5f;
	[SerializeField]
	private float swipeMinDistance = 20f;

	// ----- Keeps track of current and previous input -----
	private TouchSession currentTouchSession;
	private TouchSession previousTouchSession;

	// ----- Limits the number of move points sent -----
	private Vector2 lastValidFirstMovePoint;
	private Vector2 lastValidSecondMovePoint;

	// ----- Control Variables -----
	private static int idCount;
	private int owner;
	private List<MethodVectorID> firstTouchBeginMethods;
	private List<MethodVectorID> firstTouchMoveMethods;
	private List<MethodVectorID> firstTouchEndMethods;
	private List<MethodVectorID> secondTouchBeginMethods;
	private List<MethodVectorID> secondTouchMoveMethods;
	private List<MethodVectorID> secondTouchEndMethods;
	private List<MethodVectorID> tapMethods;
	private List<MethodVectorID> doubleTapMethods;
	private List<MethodSwipeID>  swipeMethods;

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
		owner = -1;
		idCount = 0;
		previousMouseTap = false;
		mouseTap = false;
		mouseAnalysed = true;
	}

	void Update () {
		if (inputState == State.Touch) {
			// Support for touch input
			if (Input.touches.Length == 0) { // If no fingers are touching the screen, and current session is not clean, analyse it and clean it.
				if (!currentTouchSession.IsCleanSession ()) {
					// Do the analysis for tap and swipe.
					AdvancedInputAnalysis ();
					// Rotate the sessions so the current session becomes the previous one,
					// and the previous one becomes the current one, but cleaned so its ready for new input.
					TouchSession temp = previousTouchSession;
					previousTouchSession = currentTouchSession;
					currentTouchSession = temp;
					currentTouchSession.CleanSession ();
				}
			} else { // Finger are touching the screen, add their down and up to the current session, and pass the initial fingers up, move and down along.
				// Save and pass on the input here.
				foreach (Touch t in Input.touches) {
					switch (t.phase) {
					case TouchPhase.Began:
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
						if (t.fingerId == currentTouchSession.FirstTouchID () && Vector2.Distance (t.position, lastValidFirstMovePoint) > distanceToMove) {
							lastValidFirstMovePoint = t.position;
							OnFirstTouchMove (t);

						} else if (t.fingerId == currentTouchSession.SecondTouchID () && Vector2.Distance (t.position, lastValidSecondMovePoint) > distanceToMove) {
							lastValidSecondMovePoint = t.position;
							OnSecondTouchMove (t);
						}
						break;
					case TouchPhase.Ended:
						if (t.fingerId == currentTouchSession.FirstTouchID ()) {
							currentTouchSession.AddFirstTouchEnd (t);
							OnFirstTouchEnd (t);
						} else if (t.fingerId == currentTouchSession.SecondTouchID ()) {
							currentTouchSession.AddSecondTouchEnd (t);
							OnSecondTouchEnd (t);
						}
						break;
					}
				}
			}
		}
		else 
		{
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

	void OnSecondTouchBegin(Touch t)
	{
		if (secondTouchBeginMethods.Count > 0) {
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

	void OnSecondTouchMove(Touch t)
	{
		if (secondTouchMoveMethods.Count > 0) {
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

	void OnSecondTouchEnd(Touch t)
	{
		if (secondTouchEndMethods.Count > 0) {
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

	void OnDoubleTap(Touch tp, Touch tc)
	{
		if (doubleTapMethods.Count > 0) {
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


	/*
	 * 
	 * Mouse trigger methods.
	 * 
	 * 
	*/
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

	void OnMouseDoubleTap(Vector3 p)
	{
		if (doubleTapMethods.Count > 0) {
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

	/*
	 * 
	 * Public Methods 
	 * 
	 */

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
		if (Physics.Raycast (ray, out hit, Mathf.Infinity))
			return hit.point;
		return Vector3.down*100;
		//return Camera.main.ScreenToWorldPoint(new Vector3(pos.x, pos.y, 10));
	}

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
			firstTouchBeginMethods.Add (mvi);
		}
	}

	public void OnFirstTouchBeginUnsub(int ID)
	{
		int index = -1;
		for(int i = 0; i < firstTouchBeginMethods.Count; i++)
		{
			if(firstTouchBeginMethods[i].id == ID)
			{
				index = i;
			}
		}
		if(index >= 0)
		{
			firstTouchBeginMethods.RemoveAt (index);
		}
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
			firstTouchMoveMethods.Add (mvi);
		}
	}

	public void OnFirstTouchMoveUnsub(int ID)
	{
		int index = -1;
		for(int i = 0; i < firstTouchMoveMethods.Count; i++)
		{
			if(firstTouchMoveMethods[i].id == ID)
			{
				index = i;
			}
		}
		if(index >= 0)
		{
			firstTouchMoveMethods.RemoveAt (index);
		}
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
			firstTouchEndMethods.Add (mvi);
		}
	}

	public void OnFirstTouchEndUnsub(int ID)
	{
		int index = -1;
		for(int i = 0; i < firstTouchEndMethods.Count; i++)
		{
			if(firstTouchEndMethods[i].id == ID)
			{
				index = i;
			}
		}
		if(index >= 0)
		{
			firstTouchEndMethods.RemoveAt (index);
		}
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
			secondTouchBeginMethods.Add (mvi);
		}
	}

	public void OnSecondTouchBeginUnsub(int ID)
	{
		int index = -1;
		for(int i = 0; i < secondTouchBeginMethods.Count; i++)
		{
			if(secondTouchBeginMethods[i].id == ID)
			{
				index = i;
			}
		}
		if(index >= 0)
		{
			secondTouchBeginMethods.RemoveAt (index);
		}
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
			secondTouchMoveMethods.Add (mvi);
		}
	}

	public void OnSecondTouchMoveUnsub(int ID)
	{
		int index = -1;
		for(int i = 0; i < secondTouchMoveMethods.Count; i++)
		{
			if(secondTouchMoveMethods[i].id == ID)
			{
				index = i;
			}
		}
		if(index >= 0)
		{
			secondTouchMoveMethods.RemoveAt (index);
		}
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
			secondTouchEndMethods.Add (mvi);
		}
	}

	public void OnSecondTouchEndUnsub(int ID)
	{
		int index = -1;
		for(int i = 0; i < secondTouchEndMethods.Count; i++)
		{
			if(secondTouchEndMethods[i].id == ID)
			{
				index = i;
			}
		}
		if(index >= 0)
		{
			secondTouchEndMethods.RemoveAt (index);
		}
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
			tapMethods.Add (mvi);
		}
	}

	public void OnTapUnsub(int ID)
	{
		int index = -1;
		for(int i = 0; i < tapMethods.Count; i++)
		{
			if(tapMethods[i].id == ID)
			{
				index = i;
			}
		}
		if(index >= 0)
		{
			tapMethods.RemoveAt (index);
		}
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
			doubleTapMethods.Add (mvi);
		}
	}

	public void OnDoubleTapUnsub(int ID)
	{
		int index = -1;
		for(int i = 0; i < doubleTapMethods.Count; i++)
		{
			if(doubleTapMethods[i].id == ID)
			{
				index = i;
			}
		}
		if(index >= 0)
		{
			doubleTapMethods.RemoveAt (index);
		}
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
			swipeMethods.Add (mvi);
		}
	}

	public void OnSwipeUnsub(int ID)
	{
		int index = -1;
		for(int i = 0; i < swipeMethods.Count; i++)
		{
			if(swipeMethods[i].id == ID)
			{
				index = i;
			}
		}
		if(index >= 0)
		{
			swipeMethods.RemoveAt (index);
		}
	}


	public class TouchSession
	{

		private bool hasFirstTouch;
		private SingleTouchSession firstTouch;
		private bool hasSecondTouch;
		private SingleTouchSession secondTouch;

		public TouchSession()
		{
			hasFirstTouch = false;
			hasSecondTouch = false;

		}

		public void AddFirstTouchBegin(Touch t)
		{
			SingleTouchSession sts = new SingleTouchSession ();
			sts.begin = t;
			sts.beginTime = Time.unscaledTime;
			firstTouch = sts;
			hasFirstTouch = true;
		}

		public void AddFirstTouchEnd(Touch t)
		{
			firstTouch.end = t;
			firstTouch.endTime = Time.unscaledTime;
		}

		public void AddSecondTouchBegin(Touch t)
		{
			SingleTouchSession sts = new SingleTouchSession ();
			sts.begin = t;
			sts.beginTime = Time.unscaledTime;
			secondTouch = sts;
			hasSecondTouch = true;
		}

		public void AddSecondTouchEnd(Touch t)
		{
			secondTouch.end = t;
			secondTouch.endTime = Time.unscaledTime;
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
	}

	public class SingleTouchSession{
		public Touch begin;
		public float beginTime;
		public Touch end;
		public float endTime;
	}
}