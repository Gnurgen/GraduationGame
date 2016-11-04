using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager {
    private const int START_SCENE = 0;
    private const int GAME_SCENE = 1;

    private static GameManager _instance;

    private AudioManager _audioManager;
    private EventManager _eventManager;
    private GameObject _player;
    private Menu _menu;
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

    public static GameManager instance
    {
        get
        {
            if (_instance == null)
                _instance = new GameManager();
            return _instance;
        }
    }

    public AudioManager audio
    {
        get
        {
            if (_audioManager == null)
                _audioManager = Object.FindObjectOfType(typeof(AudioManager)) as AudioManager;
            return _audioManager;
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

    private Menu menu
    {
        get
        {
            if (_menu == null)
                _menu = Object.FindObjectOfType(typeof(Menu)) as Menu;
            return _menu;
        }
    }

    private void setGameSpeed(float speed)
    {
        if (!paused)
            Time.timeScale = speed;
    }

    private void pauseGame()
    {
        prevTimeScale = Time.timeScale;
        Time.timeScale = 0.0f;
        paused = true;
    }

    private void resumeGame()
    {
        Time.timeScale = prevTimeScale;
        paused = false;
    }

    public void showMenu()
    {
        pauseGame();
    }

    public void hideMenu()
    {
        resumeGame();
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

}
