using System;
using UnityEngine;
using UnityEngine.UIElements;

public class HUDController : MonoBehaviour
{
    [SerializeField] private UIDocument _uiDocument;
    private PlayerController _playerController;
    private Label _countdownLabel;

    private void Awake()
    {
        _countdownLabel = _uiDocument.rootVisualElement.Q<Label>("Contador");
        _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        _playerController.OnHealthChanged += HandleHealthChanged;
        GameManager.Instance.OnEnemyKilledEvent += HandleEnemyKilled;
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
