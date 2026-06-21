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

    public void SelectCharacter()
    {
        if (isSelecting)
            return;

        StartCoroutine(SelectCharacterRoutine());
    }

    IEnumerator SelectCharacterRoutine()
    {
        isSelecting = true;

        PlayerPrefs.SetInt("SelectedCharacter", selectedCharacter);
        PlayerPrefs.Save();

        GameObject activeCharacter = characters[selectedCharacter];
        Animator characterAnimator = activeCharacter.GetComponentInChildren<Animator>();

        if (characterAnimator == null || characterAnimator.runtimeAnimatorController == null)
        {
            Debug.LogWarning("Selected character is missing Animator or Animator Controller.");
            isSelecting = false;
            yield break;
        }

        // Open gate and play gate sound.
        if (gateAnimator != null)
        {
            gateAnimator.SetTrigger("OpenGate");
        }

        if (gateAudio != null && gateOpenClip != null)
        {
            gateAudio.PlayOneShot(gateOpenClip);
        }

        yield return new WaitForSeconds(waitBeforeTurn);

        // Play 180 turn.
        characterAnimator.SetBool("isRunning", false);
        characterAnimator.ResetTrigger("Turn180");
        characterAnimator.SetTrigger("Turn180");

        yield return new WaitForSeconds(turnAnimationTime);

        // Play running animation, but DO NOT move the character yet.
        characterAnimator.SetBool("isRunning", true);

        yield return new WaitForSeconds(waitBeforeRun);

        // Let the running animation play in place for a bit.
        yield return new WaitForSeconds(runAnimationTime);

        yield return new WaitForSeconds(waitBeforeSceneLoad);

        SceneManager.LoadScene(gameSceneName);
    }
}