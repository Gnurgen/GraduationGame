using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutorialTexts : MonoBehaviour {
    string[,] textList = new string[2, 2];
	
	void Start () {
        textList[1, 0] = "Tegn en halvcirkel på skærmen for at lave en Åndevifte.";
        textList[1, 1] = "Hold en finger på karakteren og tegn en rute på skærmen for at bruge en Flyvende Flamme.";
       
        textList[0, 0] = "Draw a half circle on the screen to make a Spirit Fan.";
        textList[0, 1] = "Hold a finger on the character and begin to draw a path on the screen to use a Flying Flame.";
      
    }
    public string getTextSnipped(int i) {
        return textList[GameManager.game.language == GameManager.Language.Danish ? 1 : 0, i];        
    }

  
}
