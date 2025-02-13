using System;
using System.Collections;
using UnityEngine;

public enum GameState { Game, Victory, Defeat }
public enum GameOverStatus {Victory, Defeat}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private PlayerController _player;
    [SerializeField] private AudioClip _shootSound;
    [SerializeField] private AudioClip _loseSound;
    [SerializeField] private AudioClip _winSound;
    private float _timeRemaining = 120.0f;
    private bool _isCountdownPaused = false;
    public float TimeRemaining => _timeRemaining;
    public AudioClip ShootSound => _shootSound;
    public GameState CurrentGameState { get; private set; }
    public int EnemyCount { get; private set; }
    public PlayerController Player => _player;
    public event Action<bool> OnGamePausedEvent;
    public event Action OnPlayerSpawnedEvent;
    public event Action OnEnemyKilledEvent;
    public event Action<GameOverStatus> OnGameOverEvent;

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

    IEnumerator Countdown()
    {
        while (_timeRemaining > 0 && !_isCountdownPaused)
        {
            yield return new WaitForSeconds(1.0f);
            _timeRemaining--;
        }
        if (_timeRemaining <= 0)
        {
            OnGameOver(GameOverStatus.Victory);
        }
    }

    public void PlaySound(AudioClip clip)
    {
        GetComponent<AudioSource>().PlayOneShot(clip, 0.4f);
    }
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        OnGamePausedEvent?.Invoke(false);
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        OnGamePausedEvent?.Invoke(true);
    }

    public void TogglePause()
    {
        if (Time.timeScale == 0f)
        {
             ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void AddTime(float time)
    {
        _timeRemaining += time;
    }

    public void OnPlayerSpawned(PlayerController player)
    {
        _player = player;
        OnPlayerSpawnedEvent?.Invoke();
    }
    public void OnEnemyKilled()
    {
        EnemyCount++;
        OnEnemyKilledEvent?.Invoke();
    }

    public void OnGameOver(GameOverStatus gameOverStatus)
    {
        GetComponent<AudioSource>().Stop();
        if (gameOverStatus == GameOverStatus.Victory)
        {
            PlaySound(_winSound);
            CurrentGameState = GameState.Victory;
        }
        else
        {
            PlaySound(_loseSound);
            CurrentGameState = GameState.Defeat;
        }
        OnGameOverEvent?.Invoke(gameOverStatus);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
    }
}
