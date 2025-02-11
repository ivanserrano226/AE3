using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AudioSettingsMenu : MonoBehaviour
{
    [SerializeField] private AudioListener _audioListener;
    public Button audioOnButton;
    public Button audioOffButton;
    
    private void Start()
    {
        // Cargar estado del audio
        bool isAudioOn = PlayerPrefs.GetInt("AudioEnabled", 1) == 1;
        ApplyAudioSettings(isAudioOn);

        // Agregar listeners a los botones
        audioOnButton.onClick.AddListener(() => SetAudio(true));
        audioOffButton.onClick.AddListener(() => SetAudio(false));
    }

    public void SetAudio(bool isOn)
    {
        ApplyAudioSettings(isOn);
        PlayerPrefs.SetInt("AudioEnabled", isOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void ApplyAudioSettings(bool isOn)
    {
        AudioListener.volume = isOn ? 1f : 0f;
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // carga la escena "MainMenu"
}}
