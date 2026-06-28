using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CharacterSelectionManager : MonoBehaviour
{
    [Header("Skill UI")]
    [SerializeField] TMP_Text skillNameText;
    [SerializeField] TMP_Text skillDescriptionText;

    [SerializeField]
    string[] skillNames =
    {
        "Overclock",
        "Scatter Shot",
        "Dash Rush",
        "Invulnerable"
    };

    [SerializeField]
    string[] skillDescriptions =
    {
        "Doubles your shoot rate for 3 seconds.",
        "Your next 5 shots fire shotgun-style scatter bullets.",
        "Removes dash cooldown for 2 seconds.",
        "Become invulnerable for 3 seconds."
    };

    [Header("Character Selection")]
    [SerializeField] GameObject[] characters;
    [SerializeField] string gameSceneName = "MainLevel";

    [Header("UI To Hide On Select")]
    [SerializeField] GameObject[] objectsToHideOnSelect;

    [Header("Animators")]
    [SerializeField] Animator gateAnimator;

    [Header("Gate Audio")]
    [SerializeField] AudioSource gateAudio;
    [SerializeField] AudioClip gateOpenClip;

    [Header("Cutscene Timing")]
    [SerializeField] float waitBeforeTurn = 0.3f;
    [SerializeField] float turnAnimationTime = 1.5f;
    [SerializeField] float waitBeforeRun = 0.3f;
    [SerializeField] float runAnimationTime = 1.5f;
    [SerializeField] float waitBeforeSceneLoad = 0.5f;

    int selectedCharacter;
    bool isSelecting;

    void Start()
    {
        selectedCharacter = 0;
        UpdateCharacterDisplay();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SelectCharacter();
        }
    }

    public void NextCharacter()
    {
        if (isSelecting)
            return;

        selectedCharacter++;

        if (selectedCharacter >= characters.Length)
            selectedCharacter = 0;

        UpdateCharacterDisplay();
    }

    public void PreviousCharacter()
    {
        if (isSelecting)
            return;

        selectedCharacter--;

        if (selectedCharacter < 0)
            selectedCharacter = characters.Length - 1;

        UpdateCharacterDisplay();
    }
    
    void UpdateCharacterDisplay()
    {
        for (int i = 0; i < characters.Length; i++)
        {
            characters[i].SetActive(i == selectedCharacter);
        }

        GameObject activeCharacter = characters[selectedCharacter];
        Animator anim = activeCharacter.GetComponentInChildren<Animator>();

        if (anim != null && anim.isActiveAndEnabled && anim.runtimeAnimatorController != null)
        {
            anim.SetBool("isRunning", false);
            anim.ResetTrigger("Turn180");
        }

        UpdateSkillDisplay();
    }

    void UpdateSkillDisplay()
    {
        if (skillNameText != null && selectedCharacter < skillNames.Length)
        {
            skillNameText.text = "Skill: " + skillNames[selectedCharacter];
        }

        if (skillDescriptionText != null && selectedCharacter < skillDescriptions.Length)
        {
            skillDescriptionText.text = skillDescriptions[selectedCharacter];
        }
    }
    public void Back()
    {
        if (isSelecting)
            return;

        StartCoroutine(BackRoutine());
    }

    IEnumerator BackRoutine()
    {
        isSelecting = true;

        // Clears selected UI button in the game EventSystem
        if (UnityEngine.EventSystems.EventSystem.current != null)
        {
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
        }

#if UNITY_EDITOR
    // Clears selected object in Unity Inspector
    UnityEditor.Selection.activeObject = null;
#endif

        yield return null;

        SceneManager.LoadScene("MainMenu");
    }

    public void SelectCharacter()
    {
        if (isSelecting)
            return;

        StartCoroutine(SelectCharacterRoutine());
    }

    IEnumerator SelectCharacterRoutine()
    {
        isSelecting = true;

        HideSelectionUI();

        PlayerPrefs.SetInt("SelectedCharacter", selectedCharacter);

        GameObject activeCharacter = characters[selectedCharacter];

        CharacterCosmeticSet cosmeticSet = GetActiveCosmeticSet();

        if (cosmeticSet != null)
        {
            cosmeticSet.SaveAllCosmetics();
        }

        PlayerPrefs.Save();

        Animator characterAnimator = activeCharacter.GetComponentInChildren<Animator>();

        if (characterAnimator == null || characterAnimator.runtimeAnimatorController == null)
        {
            isSelecting = false;
            yield break;
        }

        if (gateAnimator != null)
        {
            gateAnimator.SetTrigger("OpenGate");
        }

        if (gateAudio != null && gateOpenClip != null)
        {
            gateAudio.PlayOneShot(gateOpenClip);
        }

        yield return new WaitForSeconds(waitBeforeTurn);

        characterAnimator.SetBool("isRunning", false);
        characterAnimator.ResetTrigger("Turn180");
        characterAnimator.SetTrigger("Turn180");

        yield return new WaitForSeconds(turnAnimationTime);

        characterAnimator.SetBool("isRunning", true);

        yield return new WaitForSeconds(waitBeforeRun);

        yield return new WaitForSeconds(runAnimationTime);

        yield return new WaitForSeconds(waitBeforeSceneLoad);

        SceneManager.LoadScene(gameSceneName);
    }
   
    void HideSelectionUI()
    {
        for (int i = 0; i < objectsToHideOnSelect.Length; i++)
        {
            if (objectsToHideOnSelect[i] != null)
            {
                objectsToHideOnSelect[i].SetActive(false);
            }
        }
    }

    CharacterCosmeticSet GetActiveCosmeticSet()
    {
        if (characters == null || characters.Length == 0)
            return null;

        if (selectedCharacter < 0 || selectedCharacter >= characters.Length)
            return null;

        return characters[selectedCharacter].GetComponentInChildren<CharacterCosmeticSet>(true);
    }

    public void NextCosmetic(int cosmeticIndex)
    {
        CharacterCosmeticSet cosmeticSet = GetActiveCosmeticSet();

        if (cosmeticSet != null)
        {
            cosmeticSet.NextCosmetic(cosmeticIndex);
        }
    }

    public void PreviousCosmetic(int cosmeticIndex)
    {
        CharacterCosmeticSet cosmeticSet = GetActiveCosmeticSet();

        if (cosmeticSet != null)
        {
            cosmeticSet.PreviousCosmetic(cosmeticIndex);
        }
    }
}