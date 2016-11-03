using UnityEngine;
using System.Collections;

public class DataManager{

    private GameManager.Language _language = GameManager.Language.None;

    public GameManager.Language language
    {
        get
        {
            if (_language == GameManager.Language.None)
                _language = (GameManager.Language)PlayerPrefs.GetInt("language", 1);
            return _language;
       }
        set
        {
            _language = value;
            PlayerPrefs.SetInt("language", (int)value);
        }
    }
}

