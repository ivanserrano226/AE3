using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Método para iniciar el juego
    public void PlayGame()
    {
        SceneManager.LoadScene("Game"); // Cargar la escena "Game"
    }

    // Método para salir del juego
    public void QuitGame()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit(); 
    }
}
