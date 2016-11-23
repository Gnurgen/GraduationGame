using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager {
    private const int START_SCENE = 0;
    private const int GAME_SCENE = 1;
    private const string GAMEOVER_SCENE = "GameOver";

    private static GameManager _instance;

    private AudioManager _audioManager;
    private EventManager _eventManager;
    private InputManager _inputManager;
    private TimeManager _timeManager;
    private GameObject _player;
    private PoolManager _poolManager;
    private GameObject _managers;
    private Menu _menu;
    private static int _score, _experience, _playerLevel;

    public GameManager()
    {
        Debug.Log("GameManager constructed");
        _instance = this;
        _managers = GameObject.Find("Managers");
        _managers.SendMessage("Subscribe");
        //menu.gameObject.SetActive(false);
        events.OnLevelUp += PlayerLevelUp;
        events.OnMenuOpen += showMenu;
        events.OnMenuClose += hideMenu;
    }

    public static void GameOver(bool CheckPoint)
    {
        player.SetActive(true);
        //move player?? ++Setplayer active
        if (!CheckPoint)
        {
            _instance = null;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            
        }
        else
        {
            //player.transform.position
        }
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
}
