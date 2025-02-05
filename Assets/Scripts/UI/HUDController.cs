using System;
using UnityEngine;
using UnityEngine.UIElements;

public class HUDController : MonoBehaviour
{
    [SerializeField] private UIDocument _uiDocument;
    private Label _countdownLabel;

    private void Awake()
    {
        _countdownLabel = _uiDocument.rootVisualElement.Q<Label>("Contador");
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
}
