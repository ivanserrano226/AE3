using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { Ready, Game, Victory, Defeat }
public class GameManager : MonoBehaviour
{
    public static bool isPaused = false;
    public GameObject pauseMenuUI;
    public static GameManager Instance { get; private set; }
    [SerializeField] private AudioClip _shootSound;
    [SerializeField] private AudioClip _bossMusic;
    private float _timeRemaining = 180.0f;
    private bool _isCountdownPaused = false;
    public float TimeRemaining => _timeRemaining;
    public AudioClip ShootSound => _shootSound;
    public GameState CurrentGameState { get; private set; }
    public int EnemyCount { get; private set; }
    public event Action OnCountdownFinished;
    public event Action OnEnemyKilledEvent;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        CurrentGameState = GameState.Game;
        StartCoroutine(Countdown()); 
    }

    public void PauseCountdown()
    {
        _isCountdownPaused = true;
    }

    public void ResumeCountdown()
    {
        _isCountdownPaused = false;
    }

    IEnumerator Countdown()
    {
        while (_timeRemaining > 0 && !_isCountdownPaused)
        {
            yield return new WaitForSeconds(1.0f);
            _timeRemaining--;
        }
        if (_timeRemaining <= 0)
        {
            OnCountdownFinished?.Invoke();
            ChangeSong(_bossMusic);
        }
    }
        void Update()
        {
            // Si se presiona la tecla ESC, pausar o reanudar el juego
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!isPaused)
                {
                    //PauseGame();
                }
                else
                {
                    //ResumeGame();
                }
            }
            // Si se presiona la tecla CTRL + ESC, regresar al menú principal
            if(Input.GetKeyDown(KeyCode.LeftControl)&& isPaused)
            {
                SceneManager.LoadScene("MainMenu");
            }
        }

    public void ResumeGame()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }

        Time.timeScale = 1f;
        isPaused = false;
        ResumeCountdown();
    }

    public void PauseGame()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(true);
        }

        Time.timeScale = 0f;
        isPaused = true;
        PauseCountdown();
    }
    
    public void ChangeSong(AudioClip newSong)
    {
        GetComponent<AudioSource>().clip = newSong;
        GetComponent<AudioSource>().Play();
    }
    public void OnEnemyKilled()
    {
        EnemyCount++;
        OnEnemyKilledEvent?.Invoke();
    }
    public void AddTime(int extraTime)
{
    _timeRemaining += extraTime;
    Debug.Log("Tiempo aumentado: " + extraTime + " segundos.");
}

}
