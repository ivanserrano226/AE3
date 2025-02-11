using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Importar TextMeshPro

public class WhoWeAreMenu : MonoBehaviour
{
   

    private void Start()
    {
      
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // carga la escena "MainMenu"
    }
}
