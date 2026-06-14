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

        SceneManager.LoadScene("Ryan (Heather)Dev");
    }

    public void LoadMainMenu()
    {
        playerInventory.SaveInventory();
        hubInventory.SaveInventory();

        SceneManager.LoadScene("MainMenu");
    }
}