using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    public void LoadHub()
    {
        SceneManager.LoadScene("Hub");
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("RyanHeatherDev");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }
}