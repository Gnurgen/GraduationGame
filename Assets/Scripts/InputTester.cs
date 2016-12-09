using UnityEngine;
using System.Collections;

public class InputTester : MonoBehaviour {

    private InputManager im;
    private int id;

	// Use this for initialization
	void Start () {
        im = GameManager.input;
        id = im.GetID();
        im.OnFirstTouchBeginSub(FirstDown, id);
        im.OnFirstTouchMoveSub(FirstMove, id);
        im.OnFirstTouchEndSub(FirstEnd, id);
        im.OnTapSub(Tap, id);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void FirstDown(Vector2 p)
    {
        Debug.Log("First Down");
    }

    void FirstMove(Vector2 p)
    {
        Debug.Log("First Move");
    }

    void FirstEnd(Vector2 p)
    {
        Debug.Log("First End");
    }

    void SecondDown(Vector2 p)
    {
        Debug.Log("Second Down");
    }

    void SecondMove(Vector2 p)
    {
        Debug.Log("Second Move");
    }

    void SecondEnd(Vector2 p)
    {
        Debug.Log("Second End");
    }

    void Tap(Vector2 p)
    {
        Debug.Log("Tap");
    }

    void DoubleTap(Vector2 p)
    {
        Debug.Log("Double Tap");
    }

    void Swipe(InputManager.Swipe s)
    {
        Debug.Log("Swipe");
    }
}
