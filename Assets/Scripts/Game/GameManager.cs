using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { Ready, Countdown, Boss, Victory, Defeat }
public class GameManager : MonoBehaviour
{
     public static bool isPaused = false;
    public GameObject pauseMenuUI;
    public static GameManager Instance { get; private set; }
    public GameState CurrentGameState { get; private set; }
    private float _timeRemaining = 180.0f;
    private bool _isCountdownPaused = false;
    public float TimeRemaining => _timeRemaining;

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
        CurrentGameState = GameState.Countdown;
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
            Debug.Log("Time Remaining: " + _timeRemaining);
            yield return new WaitForSeconds(1.0f);
            _timeRemaining--;
        }
        if (_timeRemaining <= 0)
        {
            CurrentGameState = GameState.Boss;
        }
    }
        void Update()
    {
        // Si se presiona la tecla ESC, pausar o reanudar el juego
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
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

  

    
}
