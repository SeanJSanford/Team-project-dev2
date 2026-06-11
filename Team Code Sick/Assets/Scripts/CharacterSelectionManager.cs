using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectionManager : MonoBehaviour
{
    [SerializeField] GameObject[] characters;
    [SerializeField] string gameSceneName = "MainLevel";

    int selectedCharacter;

    void Start()
    {
        selectedCharacter = PlayerPrefs.GetInt("SelectedCharacter", 0);
        UpdateCharacterDisplay();
    }

    public void NextCharacter()
    {
        selectedCharacter++;

        if (selectedCharacter >= characters.Length)
        {
            selectedCharacter = 0;
        }

        UpdateCharacterDisplay();
    }

    public void PreviousCharacter()
    {
        selectedCharacter--;

        if (selectedCharacter < 0)
        {
            selectedCharacter = characters.Length - 1;
        }

        UpdateCharacterDisplay();
    }

    void UpdateCharacterDisplay()
    {
        for (int i = 0; i < characters.Length; i++)
        {
            characters[i].SetActive(i == selectedCharacter);
        }
    }

    public void SelectCharacter()
    {
        PlayerPrefs.SetInt("SelectedCharacter", selectedCharacter);
        PlayerPrefs.Save();

        SceneManager.LoadScene(gameSceneName);
    }
}