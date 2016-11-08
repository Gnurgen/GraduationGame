using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager {
    private const int START_SCENE = 0;
    private const int GAME_SCENE = 1;

    private static GameManager _instance;

    private AudioManager _audioManager;
    private EventManager _eventManager;
    private InputManager _inputManager;
    private GameObject _player;
    private Menu _menu;
    private int _score, _experience, _playerLevel;

    public int Score
    {
        get
        {
            return _score;
        }
        set
        {
            _score = value;
        }
    }
    public int Experience
    {
        get
        {
            return _experience;
        }
        set
        {
            _experience = value;
            if (_experience > ExperienceForNextLevel())
            {
               // events.level();
            }
        }
    }
    public int PlayerLevel
    {
        get
        {
            return _playerLevel;
        }
        set
        {
            _playerLevel = value;
        }
    }

    private Settings settings = new Settings(Language.None);

    private float prevTimeScale;
    private bool paused = false;

    private struct Settings{
        public Language language;

        public Settings(Language language)
        {
            this.language = language;
        }
    };

    public enum Language { None, English, Danish };

    public static GameManager game
    {
        get
        {
            if (_instance == null)
                _instance = new GameManager();
            return _instance;
        }
    }

    public AudioManager audioManager
    {
        get
        {
            if (_audioManager == null)
                _audioManager = Object.FindObjectOfType(typeof(AudioManager)) as AudioManager;
            return _audioManager;
        }
    }

    public static AudioManager audio
    {
        get
        {
            return game.audioManager;
        }
    }

    public EventManager eventManager
    {
        get
        {
            if (_eventManager == null)
                _eventManager = Object.FindObjectOfType(typeof(EventManager)) as EventManager;
            return _eventManager;
        }
    }

    public static EventManager events
    {
        get
        {
            return game.eventManager;
        }
    }

    public InputManager inputManager
    {
        get
        {
            if (_inputManager == null)
                _inputManager = Object.FindObjectOfType(typeof(InputManager)) as InputManager;
            return _inputManager;
        }
    }

    public static InputManager input
    {
        get
        {
            return game.inputManager;
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

    private Menu menu
    {
        get
        {
            if (_menu == null)
                _menu = Object.FindObjectOfType(typeof(Menu)) as Menu;
            return _menu;
        }
    }

    public void showMenu()
    {

    }

    public void hideMenu()
    {

    }

    public Language language
    {
        get
        {
            if (settings.language == Language.None)
                settings.language = (Language)PlayerPrefs.GetInt("language", 1);
            return settings.language;
        }
        set
        {
            if (value != Language.None)
                PlayerPrefs.SetInt("language", (int)value);
            settings.language = value;
        }
    }

    public int ExperienceForNextLevel()
    {
        int exp = 0;
        return exp;
    }
}
