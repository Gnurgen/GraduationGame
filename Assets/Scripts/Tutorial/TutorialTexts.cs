using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutorialTexts : MonoBehaviour {
    string[,] textList = new string[2, 2];
	
	void Start () {
        textList[1, 0] = "Tegn en halvcirkel på skærmen for at lave en kegle.";
        textList[1, 1] = "Hold en finger på Kumo og tegn en rute på skærmen for at bruge en flyvende flamme.";
       
        textList[0, 0] = "Draw a half circle on the screen to make a cone";
        textList[0, 1] = "Hold a finger on Kumo and begin to draw a path on the screen to use a flying flame.";
      
    }
    public string getTextSnipped(int i) {
        return textList[GameManager.game.language == GameManager.Language.Danish ? 1 : 0, i];        
    }

  
}
