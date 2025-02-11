using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.XR;

public class HUDController : MonoBehaviour
{
    [SerializeField] private UIDocument _uiDocument;
    private Label _countdownLabel;
    private VisualElement _gameOver;
    private VisualElement _pause;
    private Label _winLabel;
    private Label _loseLabel;
    private Label _enemyCountLabel;
    private Button _quitButton;

    private void Start()
    {
        _countdownLabel = _uiDocument.rootVisualElement.Q<Label>("Contador");
        _gameOver = _uiDocument.rootVisualElement.Q<VisualElement>("GameOver");
        _pause = _uiDocument.rootVisualElement.Q<VisualElement>("Pause");
        _winLabel = _uiDocument.rootVisualElement.Q<Label>("win-text");
        _loseLabel = _uiDocument.rootVisualElement.Q<Label>("lose-text");
        _quitButton = _uiDocument.rootVisualElement.Q<Button>("quit-button");
        _enemyCountLabel = _uiDocument.rootVisualElement.Q<Label>("enemies-killed-text");
        _quitButton.clicked += OnQuitButton;
    }

    void OnEnable()
    {
        GameManager.Instance.OnEnemyKilledEvent += HandleEnemyKilled;
        GameManager.Instance.OnGameOverEvent += OnGameOver;
        GameManager.Instance.OnGamePausedEvent += OnGamePaused;
        GameManager.Instance.OnPlayerSpawnedEvent += OnPlayerSpawned;
    }

    void OnDisable()
    {
        GameManager.Instance.Player.OnHealthChanged -= HandleHealthChanged;
        GameManager.Instance.OnEnemyKilledEvent -= HandleEnemyKilled;
        GameManager.Instance.OnGameOverEvent -= OnGameOver;
        GameManager.Instance.OnGamePausedEvent -= OnGamePaused;
        GameManager.Instance.OnPlayerSpawnedEvent -= OnPlayerSpawned;
        _quitButton.clicked -= OnQuitButton;
    }

    void Update()
    {
        DisplayCountdown();
    }

    void DisplayCountdown()
    {
        //show countdown timer
        _countdownLabel.text = String.Format("{0}:{1:00}", (int)GameManager.Instance.TimeRemaining / 60, (int)GameManager.Instance.TimeRemaining % 60);
    }

    void OnGameOver(GameOverStatus gameOverStatus)
    {
        _gameOver.style.display = DisplayStyle.Flex;
        _enemyCountLabel.text = GameManager.Instance.EnemyCount.ToString();
        if (gameOverStatus == GameOverStatus.Victory)
        {
            _winLabel.style.display = DisplayStyle.Flex;
        }
        else
        {
            _loseLabel.style.display = DisplayStyle.Flex;
        }
    }

    private void OnGamePaused(bool isPaused)
    {
        if (isPaused)
        {
            _pause.style.display = DisplayStyle.Flex;
        }
        else
        {
            _pause.style.display = DisplayStyle.None;
        }
    }

    private void OnQuitButton()
    {
        SceneManager.LoadScene(1);
        GameManager.Instance.ResumeGame();
    }

    private void OnPlayerSpawned()
    {
        GameManager.Instance.Player.OnHealthChanged += HandleHealthChanged;
        HandleHealthChanged(GameManager.Instance.Player.Health);
    }

    void HandleHealthChanged(float health)
    {
        //show player health
        _uiDocument.rootVisualElement.Q<Label>("HP").text = health.ToString();
    }

    void HandleEnemyKilled()
    {
        //show enemy count
        _uiDocument.rootVisualElement.Q<Label>("Enemigos").text = GameManager.Instance.EnemyCount.ToString();
    }
}
