using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControlsMenu : MonoBehaviour
{

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); //cargo la escena "MainMenu"
    }
    public void Audio(){
        SceneManager.LoadScene("Audio");// cargas la escena "Audio"

    }
    public void WhoWeAre(){
        SceneManager.LoadScene("WhoWeAre");// cargas la escena "WhoWeAre"
    }
 
}