using UnityEngine;
public class SettingsInGame : MonoBehaviour
{
    [SerializeField] private GameObject settingsPanel;

    public void OpenInventory()
    {
        settingsPanel.SetActive(true);
    }

    public void CloseInventory()
    {
        settingsPanel.SetActive(false);
    }
}