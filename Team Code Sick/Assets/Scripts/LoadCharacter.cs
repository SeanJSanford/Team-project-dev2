using UnityEngine;

public class LoadCharacter : MonoBehaviour
{
    [SerializeField] GameObject[] characterPrefabs;
    [SerializeField] Transform visualHolder;
    [SerializeField] Renderer capsuleRenderer;

    GameObject currentCharacter;

    void Start()
    {
        int selectedCharacter = PlayerPrefs.GetInt("SelectedCharacter", 0);

        if (characterPrefabs == null || characterPrefabs.Length == 0)
        {
            Debug.LogWarning("No character prefabs assigned.");
            return;
        }

        if (selectedCharacter < 0 || selectedCharacter >= characterPrefabs.Length)
        {
            selectedCharacter = 0;
        }

        if (characterPrefabs[selectedCharacter] == null)
        {
            Debug.LogWarning("Selected character prefab is missing.");
            return;
        }

        currentCharacter = Instantiate(characterPrefabs[selectedCharacter], visualHolder);

        currentCharacter.transform.localPosition = Vector3.zero;
        currentCharacter.transform.localRotation = Quaternion.identity;
        currentCharacter.transform.localScale = Vector3.one;

        currentCharacter.SetActive(true);

        if (capsuleRenderer != null)
        {
            capsuleRenderer.enabled = false;
        }

        Debug.Log("Loaded character index: " + selectedCharacter + " prefab: " + characterPrefabs[selectedCharacter].name);
    }
}