using UnityEngine;

public class LoadCharacter : MonoBehaviour
{
    [SerializeField] GameObject[] characterPrefabs;
    [SerializeField] Transform visualHolder;
    [SerializeField] playerMovement playerScript;
    [SerializeField] RuntimeAnimatorController playerAnimatorController;

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

        if (playerScript == null)
        {
            playerScript = GetComponentInParent<playerMovement>();
        }

        currentCharacter = Instantiate(characterPrefabs[selectedCharacter], visualHolder);

        currentCharacter.transform.localPosition = Vector3.zero;
        currentCharacter.transform.localRotation = Quaternion.identity;
        currentCharacter.transform.localScale = Vector3.one;

        currentCharacter.SetActive(true);

        // Find the Animator on the loaded character model.
        Animator modelAnimator = currentCharacter.GetComponentInChildren<Animator>();

        if (modelAnimator != null)
        {
            if (modelAnimator.runtimeAnimatorController != null)
            {
                Debug.Log("Before switch: " + modelAnimator.runtimeAnimatorController.name);
            }
            else
            {
                Debug.Log("Before switch: No Animator Controller assigned.");
            }

            if (playerAnimatorController != null)
            {
                modelAnimator.runtimeAnimatorController = playerAnimatorController;
                Debug.Log("After switch: " + modelAnimator.runtimeAnimatorController.name);
            }
            else
            {
                Debug.LogWarning("Player Animator Controller is not assigned in LoadCharacter.");
            }

            modelAnimator.applyRootMotion = false;

            if (playerScript != null)
            {
                playerScript.SetAnimator(modelAnimator);
            }
            else
            {
                Debug.LogWarning("playerMovement script was not found.");
            }
        }
        else
        {
            Debug.LogWarning("No Animator found on loaded character.");
        }

        // Find ShootPos on the loaded character model.
        Transform modelShootPos = FindDeepChild(currentCharacter.transform, "ShootPos");

        if (modelShootPos != null && playerScript != null)
        {
            playerScript.SetShootPos(modelShootPos);
        }
        else
        {
            Debug.LogWarning("Could not assign ShootPos. Make sure the character prefab has a child named ShootPos.");
        }

        Debug.Log("Loaded character index: " + selectedCharacter + " prefab: " + characterPrefabs[selectedCharacter].name);
    }

    Transform FindDeepChild(Transform parent, string childName)
    {
        foreach (Transform child in parent)
        {
            if (child.name == childName)
            {
                return child;
            }

            Transform result = FindDeepChild(child, childName);

            if (result != null)
            {
                return result;
            }
        }

        return null;
    }
}