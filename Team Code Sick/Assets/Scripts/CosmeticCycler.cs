using UnityEngine;

public class CosmeticCycler : MonoBehaviour
{
    [Header("Cosmetic Objects")]
    [SerializeField] GameObject[] options;

    [Header("Save Settings")]
    [SerializeField] string saveKey = "cosmetic_head_armor";
    [SerializeField] bool loadSavedOnStart = true;
    [SerializeField] bool saveWhenChanged = true;

    int currentIndex;

    void Start()
    {
        if (options.Length == 0)
        {
            Debug.LogWarning(gameObject.name + " has no cosmetic options assigned.");
            return;
        }

        if (loadSavedOnStart)
        {
            currentIndex = PlayerPrefs.GetInt(saveKey, 0);
        }
        else
        {
            currentIndex = 0;
        }

        currentIndex = Mathf.Clamp(currentIndex, 0, options.Length - 1);

        ApplyOption();
    }

    public void NextOption()
    {
        if (options.Length == 0)
            return;

        currentIndex++;

        if (currentIndex >= options.Length)
            currentIndex = 0;

        ApplyOption();

        if (saveWhenChanged)
            SaveOption();
    }

    public void PreviousOption()
    {
        if (options.Length == 0)
            return;

        currentIndex--;

        if (currentIndex < 0)
            currentIndex = options.Length - 1;

        ApplyOption();

        if (saveWhenChanged)
            SaveOption();
    }

    public void SaveOption()
    {
        PlayerPrefs.SetInt(saveKey, currentIndex);
        PlayerPrefs.Save();

        Debug.Log("Saved " + saveKey + ": " + currentIndex);
    }

    public void LoadOption()
    {
        if (options.Length == 0)
            return;

        currentIndex = PlayerPrefs.GetInt(saveKey, 0);
        currentIndex = Mathf.Clamp(currentIndex, 0, options.Length - 1);

        ApplyOption();

        Debug.Log("Loaded " + saveKey + ": " + currentIndex);
    }

    void ApplyOption()
    {
        for (int i = 0; i < options.Length; i++)
        {
            if (options[i] != null)
            {
                options[i].SetActive(i == currentIndex);
            }
        }
    }
}