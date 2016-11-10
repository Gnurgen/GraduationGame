using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {
  
    
    //Manager 
    private InputManager IM;
    int ID;

    public void terminateTouch()
    {
        ID = GameManager.input.GetID();
        IM.TakeControl(ID);

    }
    public void allowTouch()
    {
        ID = GameManager.input.GetID();
        IM.ReleaseControl(ID);

    }
   
}
