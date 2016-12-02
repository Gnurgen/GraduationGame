using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutorialTexts : MonoBehaviour {
    string[,] textList = new string[2, 6];
	
	void Start () {
        textList[1, 0] = "Tap for at gå. Hvis du tapper i kanterne af skærmen vil Kumo dashe den første del af vejen og gå resten. Kumo kan ikke tage skade, mens der bliver dashet.";
        textList[1, 1] = "Tegn en halvcirkel på skærmen for at lave en kegle. Løft fingeren fra skærmen for at aktivere angrebet. Keglen skubber fjender tilbage og stunner dem i få sekunder.";
        textList[1, 2] = "Når du har dræbt en fjende, vil en ånd blive sluppet løs og vil flyve ind i Kumos spyd. Når nok ånder er blevet indsamlet, vil de have nok energi til at aktivere en elevator til det næste niveau. Du kan se dine fremskridt i bjælken nederst på skærmen.";
        textList[1, 3] = "Hold en finger på Kumo og tegn udad og tegn en rute. Løft fingeren fra skærmen og spyddet vil flyve i den rute, du har tegnet. Fjender, som bliver ramt af spyddet, tager skade og bliver trukket med spyddet indtil den angrebet er afsluttet.";
        textList[1, 4] = "Når du har dræbt en fjende, vil en ånd blive sluppet løs og vil flyve ind i Kumos spyd. Når nok ånder er blevet indsamlet, vil de have nok energi til at aktivere en elevator til det næste niveau. Du kan se dine fremskridt i bjælken nederst på skærmen.";
        textList[1, 5] = "Når du har renset et rum for fjender er du i stand til at aktivere et spirit stone. Hvis du dør vil du genoplive ved denne spirit stone. Den indikerer også om du allerede har været gennem rummet eller ej. Du skal stå i nærheden af stenen for at aktivere det.";

        textList[0, 0] = "Tap to move. If you tap on the edges on the screen, Kumo will dash the first part of the way and walk the rest. Kumo is immune to damage while dashing.";
        textList[0, 1] = "Draw a half circle on the screen to make a cone. Release the finger to activate ability. The cone will knock back and sun enemies.";
        textList[0, 2] = "When you’ve slain an enemy a spirit will be released and will fly into Kumo’s spear. When enough spirits have been collected they will have enough spirit energy to activate an elevator to the next level.";
        textList[0, 3] = "Hold a finger on Kumo and draw outwards and draw a path. Release the finger and the spear will fly on the path that you have drawn. Enemies hit will take damage and will be stringed along until spear path end.";
        textList[0, 4] = "When you’ve slain an enemy a spirit will be released and will fly into Kumo’s spear. When enough spirits have been collected they will have enough spirit energy to activate an elevator to the next level.";
        textList[0, 5] = "When you’ve cleansed a room for enemies you are able to activate the spirit stone. If you die you will respawn at this spirit stone. It also indicates whether you’ve already been to this room or not. You have to stand in near proximity of the stone to activate it.";
    }
    public string getTextSnipped(int i) {
        return textList[GameManager.game.language == GameManager.Language.Danish ? 1 : 0, i];
    }

  
}
