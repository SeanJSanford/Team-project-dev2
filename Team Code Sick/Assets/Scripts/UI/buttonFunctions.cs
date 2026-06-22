using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{
    public void resume()
    {
        gamemanager.instance.stateUnpause();
    }

    public void restart()
    {
        Time.timeScale = gamemanager.instance.timeScaleOrig;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void quit()
    {
        if (Application.platform != RuntimePlatform.WebGLPlayer)
        {
            UnityEditor.EditorApplication.isPlaying = false;
            Application.Quit();
        }
    }
}
