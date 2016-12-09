using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutorialTexts : MonoBehaviour {
    string[,] textList = new string[2, 3];
	
	void Start () {
        textList[1, 0] = "Tegn en halvcirkel for at lave en Åndevifte";
        textList[1, 1] = "Hold en finger på karakteren og tegn for at bruge Flyvende Flamme";
        textList[1, 2] = "Når en fjende er dræbt vil en ånd blive indsamlet,som fylder et blåt ånde-meter op, som aktiverer en elevator";

        textList[0, 0] = "Draw a half circle to make a Spirit Fan";
        textList[0, 1] = "Hold a finger on the character and draw to use Flying Flame";
        textList[0, 2] = "When an enemy is dead a spirit is collected and fills a blue spirit bar, which activates an elevator";
    }
    public string getTextSnipped(int i) {
        return textList[GameManager.game.language == GameManager.Language.Danish ? 1 : 0, i];        
    }
    void OnEnable()
    {
        textList[1, 0] = "Tegn en halvcirkel for at lave en Åndevifte";
        textList[1, 1] = "Hold en finger på karakteren og tegn for at bruge Flyvende Flamme";
        textList[1, 2] = "Når en fjende er dræbt vil en ånd blive indsamlet,som fylder et blåt ånde-meter op, som aktiverer en elevator";

        textList[0, 0] = "Draw a half circle to make a Spirit Fan";
        textList[0, 1] = "Hold a finger on the character and draw to use Flying Flame";
        textList[0, 2] = "When an enemy is dead a spirit is collected and fills a blue spirit bar, hich activates an elevator";
    }

}
