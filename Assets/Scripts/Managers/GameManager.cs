using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager {
    private const int START_SCENE = 0;
    private const int GAME_SCENE = 1;

    private static GameManager _instance;

    private DataManager _data;
    private GameObject _player;
    private InputManager _inputManager;
    private EventManager _eventManager;

    private GameObject _wheel;
    private GameObject _skills;

    public enum Language { None, English, Danish };

    public static GameManager instance
    {
        get
        {
            if (_instance == null)
                _instance = new GameManager();
            return _instance;
        }
    }

    public DataManager data
    {
        get
        {
            if (_data == null)
                _data = new DataManager();
            return _data;
        }
    }

    public InputManager input
    {
        get
        {
            if (_inputManager == null)
                _inputManager = Object.FindObjectOfType(typeof(InputManager)) as InputManager;
            return _inputManager;
        }
    }

    public EventManager events
    {
        get
        {
            if (_eventManager == null)
                _eventManager = Object.FindObjectOfType(typeof(EventManager)) as EventManager;
            return _eventManager;
        }
    }

    public GameObject player
    {
        get
        {
            if (_player == null)
                _player = GameObject.FindWithTag("Player");
            return _player;
        }
    }

    public Language language
    {
        get
        {
            return data.language;
        }
        set
        {
            data.language = value;
        }
    }


}
