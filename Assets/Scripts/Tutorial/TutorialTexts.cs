using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutorialTexts : MonoBehaviour {
    string[,] textList = new string[2, 3];
	
	void Start () {
        textList[1, 0] = "Tegn en halvcirkel på skærmen for at lave en Åndevifte.";
        textList[1, 1] = "Hold en finger på karakteren og tegn en rute på skærmen for at bruge en Flyvende Flamme.";
        textList[1, 2] = "Når du har dræbt en fjende, vil en ånd blive indsamlet. Ånden vil fylde et blåt ånde-meter i bunden af skærmen. Når ånde-meteret er fyldt vil en elevator blive aktiveret.";

        textList[0, 0] = "Draw a half circle on the screen to make a Spirit Fan.";
        textList[0, 1] = "Hold a finger on the character and begin to draw a path on the screen to use a Flying Flame.";
        textList[0, 2] = "When an enemy is dead a wisp will be collected. The wisp will fill a blue spirit bar at the bottom of the screen. When the spirit bar is full an elevator will activate.";
    }
    public string getTextSnipped(int i) {
        return textList[GameManager.game.language == GameManager.Language.Danish ? 1 : 0, i];        
    }

  
}
