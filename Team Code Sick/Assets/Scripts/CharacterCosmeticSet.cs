using UnityEngine;

public class CharacterCosmeticSet : MonoBehaviour
{
    [Header("Cosmetic Cyclers")]
    [SerializeField] CosmeticCycler[] cyclers;

    public void NextCosmetic(int index)
    {
        if (index < 0 || index >= cyclers.Length)
        {
            Debug.LogWarning("Cosmetic index is out of range: " + index);
            return;
        }

        if (cyclers[index] != null)
        {
            cyclers[index].NextOption();
        }
    }

    public void PreviousCosmetic(int index)
    {
        if (index < 0 || index >= cyclers.Length)
        {
            Debug.LogWarning("Cosmetic index is out of range: " + index);
            return;
        }

        if (cyclers[index] != null)
        {
            cyclers[index].PreviousOption();
        }
    }

    public void SaveAllCosmetics()
    {
        for (int i = 0; i < cyclers.Length; i++)
        {
            if (cyclers[i] != null)
            {
                cyclers[i].SaveOption();
            }
        }

        PlayerPrefs.Save();
    }

    public void LoadAllCosmetics()
    {
        for (int i = 0; i < cyclers.Length; i++)
        {
            if (cyclers[i] != null)
            {
                cyclers[i].LoadOption();
            }
        }
    }
}