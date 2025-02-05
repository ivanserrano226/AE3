using System.Collections;
using UnityEngine;

public enum GameState { Ready, Countdown, Boss, Victory, Defeat }
public class GameManager : MonoBehaviour
{
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
}
