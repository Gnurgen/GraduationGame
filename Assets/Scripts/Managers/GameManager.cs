using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager {
    private const int START_SCENE = 0;
    private const int GAME_SCENE = 1;
    private const string GAMEOVER_SCENE = "GameOver";

    // Number of levels with procedural map generation. Used for Loadingscreen 
    private static int _numberOfLevels = 2;
    
     
    private static GameManager _instance;

    private AudioManager _audioManager;
    private EventManager _eventManager;
    private InputManager _inputManager;
    private TimeManager _timeManager;
    private GameObject _player;
    private GameObject _spear;
    private PoolManager _poolManager; 
    private GameObject _managers;
  
    private Menu _menu;
    private static int _score, _experience, _playerLevel, _progress;
    private static bool _useVibrations;

    public GameManager()
    {
        Debug.Log("GameManager constructed");
        _instance = this;
        _managers = GameObject.Find("Managers");
        if (_managers != null)
        {
            _managers.SendMessage("Subscribe");
            events.OnLevelUp += PlayerLevelUp;
            events.OnMenuOpen += showMenu;
            events.OnMenuClose += hideMenu;
            events.OnLoadNextLevel += LoadNextLevel;
        }
        //menu.gameObject.SetActive(false);

    }




    public static int score
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
    public static int experience
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
                experience = _experience - ExperienceForNextLevel();
                events.LevelUp(1);
            }
        }
    }
    public static int playerLevel
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

    public static int progress
    {
        get
        {
            return _progress;
        }
        set
        {
            _progress = value;
        }
    }
    public static int numberOfLevels
    {
        get
        {
            return _numberOfLevels;
        }
        set
        {
            _numberOfLevels = value;
        }
    }
    public static bool useVibrations
    {
        get
        {
            return _useVibrations;
        }
        set
        {
            _useVibrations = value;
        }
    }

    private Settings settings = new Settings(Language.None);

    private float prevTimeScale;
   

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
                new GameManager();
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

    public TimeManager timeManager
    {
        get
        {
            if(_timeManager == null)
                _timeManager = Object.FindObjectOfType(typeof(TimeManager)) as TimeManager;
            return _timeManager;
        }
    }
    public static TimeManager time
    {
        get
        {
            return game.timeManager;
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
            if (_instance == null)
                new GameManager();
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
    public PoolManager poolManager
    {
        get
        {
            if (_poolManager == null)
                _poolManager = Object.FindObjectOfType(typeof(PoolManager)) as PoolManager;
            return _poolManager;
        }
    }

    public static PoolManager pool
    {
        get
        {
            return game.poolManager;
        }
    }

    public GameObject Player
    {
        get
        {
            if (_player == null)
                _player = GameObject.FindWithTag("Player");
            return _player;
        }
    }
    public static GameObject player
    {
        get
        {
            return game.Player;
        }
    }

    public GameObject Spear
    {
        get
        {
            if (_spear == null)
                _spear = GameObject.Find("spear_tip");
            return _spear;
        }
    }
    public static GameObject spear
    {
        get
        {
            return game.Spear;
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

    private void showMenu()
    {
        menu.gameObject.SetActive(true);
    }

    private void hideMenu()
    {
        menu.gameObject.SetActive(false);
    }

    private void PlayerLevelUp(int i)
    {
        playerLevel += i;
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

    public static int ExperienceForNextLevel() //Make an experience per level-algorithm
    {
        int exp = 4 + playerLevel * (playerLevel - 1);
        return exp;
    }

    public static void LoadNextLevel()
    {
        progress = PlayerPrefs.GetInt("Progress");
        _instance = null;
        SceneManager.LoadScene("LoadingScreen", LoadSceneMode.Single);
        // LOADING SCREEN TAKES IT FROM HERE

/*
        if (progress == 0)
        {
            AsyncOperation tut = SceneManager.LoadSceneAsync("Tutorial", LoadSceneMode.Additive);
            
        }
        else if (progress <= numberOfLevels) // Number of levels before Boss level 
        {
            SceneManager.LoadSceneAsync("Final",LoadSceneMode.Additive);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("Final"));
        }
        else
        {
            SceneManager.LoadSceneAsync("BossLevel", LoadSceneMode.Additive);
        }
        */
    }

}
