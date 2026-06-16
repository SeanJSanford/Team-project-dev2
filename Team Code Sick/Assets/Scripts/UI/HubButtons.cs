using UnityEngine;
using UnityEngine.SceneManagement;

public class HubButtons : MonoBehaviour
{
    [SerializeField] private Inventory playerInventory;
    [SerializeField] private Inventory hubInventory;

    public void LoadGame()
    {
        playerInventory.SaveInventory();
        hubInventory.SaveInventory();

        SceneManager.LoadScene("Character Selection");
    }

    public void LoadMainMenu()
    {
        playerInventory.SaveInventory();
        hubInventory.SaveInventory();

        SceneManager.LoadScene("MainMenu");
    }
}