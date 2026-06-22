using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyOptions : MonoBehaviour
{
    public static DifficultyOptions instance;

    enum SliderOptions
    {
        StartDifficulty,
        DifficultyRampUp,
        AmountOfRooms,
        BossFrequency,
        ExtractionAmount,
        MAX,
        Luck // Not being Used yet
    }

    List<NxStatType> targetedStats = new List<NxStatType> { NxStatType.FireRate, NxStatType.Damage, NxStatType.Resistance, NxStatType.HP, NxStatType.Speed };
    List<float> weightsLow = new List<float> { 0.02f, .1f, .01f, 1f, .8f };
    List<float> weightsHigh = new List<float> { 0.05f, .25f, .03f, 3f, 1.2f };

    public List<Slider> optionsSliders = new List<Slider>();
    public List<TMP_Text> optionsCurrent = new List<TMP_Text>();
    public List<TMP_Text> optionsHighest = new List<TMP_Text>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SetStartingValues();
        }
        else
        {
            instance.TurnOn();
            Destroy(gameObject);
            SetStartingValues();
        }
    }
    private void Start()
    {
        foreach (Slider slider in optionsSliders)
        {
            slider.onValueChanged.AddListener(UpdateCurrent);
        }
    }
    public void LoadSettings(gamemanager gameManager)
    {
        gameManager.startDifficulty = (int)optionsSliders[(int)SliderOptions.StartDifficulty].value;
        gameManager.difficultyRampUp = (int)optionsSliders[(int)SliderOptions.DifficultyRampUp].value;
        gameManager.amountOfRooms = (int)optionsSliders[(int)SliderOptions.AmountOfRooms].value;
        gameManager.bossFrequncy = (int)optionsSliders[(int)SliderOptions.BossFrequency].value;
        gameManager.extractionAmount = (int)optionsSliders[(int)SliderOptions.ExtractionAmount].value;
        //gameManager.luckAmount = (int)optionsSliders[(int)SliderOptions.Luck].value;
    }

    public void TurnOff()
    {
        gameObject.SetActive(false);
    }
    public void TurnOn()
    {
        gameObject.SetActive(true);
    }
    void UpdateCurrent(float _)
    {
        for (int i = 0; i < (int)SliderOptions.MAX; i++)
        {
            optionsCurrent[i].text = optionsSliders[i].value.ToString("0");
        }
    }
    void SetStartingValues()
    {
        for (int i = 0; i < (int)SliderOptions.MAX; i++)
        {
            optionsHighest[i].text = optionsSliders[i].maxValue.ToString("0");
            optionsCurrent[i].text = optionsSliders[i].value.ToString("0");
        }
    }

    public void CalculateBoost(bool start, ICharacter statsToEdit)
    {
        List<float> boosts = new List<float>();

        if (start)
        {
            for (int statOption = 0; statOption < (int)SliderOptions.MAX; statOption++)
            {
                boosts.Add(0);
                for (int i = 0; i < (int)optionsSliders[statOption].value; i++)
                {
                    boosts[statOption] += Random.Range(weightsLow[statOption], weightsHigh[statOption]);
                }
            }
        }
        else
        {
            boosts = new List<float> { Random.Range(weightsLow[0], weightsHigh[0]), 0, 0, 0, 0 };
        }

        for (int stat = 0; stat < boosts.Count; stat++) 
        { 
            statsToEdit.ModifyStat(targetedStats[stat], boosts[stat]);
        }
    }
}

