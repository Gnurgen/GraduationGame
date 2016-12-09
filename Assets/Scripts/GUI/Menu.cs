using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {
  
    
    //Manager 
    int ID;

    public void terminateTouch()
    {
        ID = GameManager.input.GetID();
        GameManager.input.TakeControl(ID);

    }
    public void allowTouch()
    {
        ID = GameManager.input.GetID();
        GameManager.input.ReleaseControl(ID);

    }
   
}
