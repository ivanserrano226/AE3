using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    
    // Método para iniciar el juego
    public void PlayGame()
    {
        SceneManager.LoadScene("Game"); // Cargar la escena "Game"
    }
public void Options(){
    SceneManager.LoadScene("Options");// cargas la escena "Options"
}
    // Método para salir del juego
    public void QuitGame()
    {
        Debug.Log("Saliendo del juego...");//solo para mostrar un mensaje en la consola
        Application.Quit(); //solo funciona en la build final
    }
}
