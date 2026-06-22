using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject settingsPanel;

    public void LoadHub()
    {
        SceneManager.LoadScene("Hub");
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("Character Selection");
    }

    public void OpenSettings()
    {
        mainPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        mainPanel.SetActive(true);
    }

    public void LoadCredits()
    {
        SceneManager.LoadScene("Credits");
    }



    public void QuitGame()
    {
        if (Application.platform != RuntimePlatform.WebGLPlayer)
        {
            Debug.Log("Quitting game...");
            Application.Quit();
        }
    }
}