using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class HUDController : MonoBehaviour
{
    [SerializeField] private UIDocument _uiDocument;
    private PlayerController _playerController;
    private Label _countdownLabel;
    private VisualElement _gameOver;
    private Label _winLabel;
    private Label _loseLabel;
    private Button _quitButton;

    private void Awake()
    {
        _countdownLabel = _uiDocument.rootVisualElement.Q<Label>("Contador");
        _gameOver = _uiDocument.rootVisualElement.Q<VisualElement>("GameOver");
        _winLabel = _uiDocument.rootVisualElement.Q<Label>("win-text");
        _loseLabel = _uiDocument.rootVisualElement.Q<Label>("lose-text");
        _quitButton = _uiDocument.rootVisualElement.Q<Button>("quit-button");
        _quitButton.clicked += OnQuitButton;
        _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        _playerController.OnHealthChanged += HandleHealthChanged;
        GameManager.Instance.OnEnemyKilledEvent += HandleEnemyKilled;
        GameManager.Instance.OnGameOverEvent += OnGameOver;
        HandleHealthChanged(_playerController.Health);
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
        if (gameOverStatus == GameOverStatus.Victory)
        {
            _winLabel.style.display = DisplayStyle.Flex;
        }
        else
        {
            _loseLabel.style.display = DisplayStyle.Flex;
        }
    }

    private void OnQuitButton()
    {
        SceneManager.LoadScene(1);
        GameManager.Instance.ResumeGame();
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
