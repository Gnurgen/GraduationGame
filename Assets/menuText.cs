using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class menuText : MonoBehaviour {

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnEnable()
    {
        GetComponent<Text>().text = GameManager.game.language == GameManager.Language.Danish ? "HOVED MENU" : "MAIN MENU";
    }
}
