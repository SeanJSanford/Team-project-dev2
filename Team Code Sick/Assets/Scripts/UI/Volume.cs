using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Volume : MonoBehaviour
{
    [Header("Mixer")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("Sliders")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider uiSlider;

    private const string MasterParam = "MasterVolume";
    private const string MusicParam = "MusicVolume";
    private const string SFXParam = "SFXVolume";
    private const string UIParam = "UIVolume";

    private void Start()
    {
        float savedMaster = PlayerPrefs.GetFloat(MasterParam, 1f);
        float savedMusic = PlayerPrefs.GetFloat(MusicParam, 1f);
        float savedSFX = PlayerPrefs.GetFloat(SFXParam, 1f);
        float savedUI = PlayerPrefs.GetFloat(UIParam, 1f);

        masterSlider.value = savedMaster;
        musicSlider.value = savedMusic;
        sfxSlider.value = savedSFX;
        uiSlider.value = savedUI;

        SetMasterVolume(savedMaster);
        SetMusicVolume(savedMusic);
        SetSFXVolume(savedSFX);
        SetUIVolume(savedUI);

        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        uiSlider.onValueChanged.AddListener(SetUIVolume);
    }

    public void SetMasterVolume(float volume)
    {
        SetMixerVolume(MasterParam, volume);
    }

    public void SetMusicVolume(float volume)
    {
        SetMixerVolume(MusicParam, volume);
    }

    public void SetSFXVolume(float volume)
    {
        SetMixerVolume(SFXParam, volume);
    }

    public void SetUIVolume(float volume)
    {
        SetMixerVolume(UIParam, volume);
    }

    private void SetMixerVolume(string parameterName, float volume)
    {
        float mixerVolume;

        if (volume <= 0.0001f)
        {
            mixerVolume = -80f;
        }
        else
        {
            mixerVolume = Mathf.Log10(volume) * 20f;
        }

        audioMixer.SetFloat(parameterName, mixerVolume);

        PlayerPrefs.SetFloat(parameterName, volume);
        PlayerPrefs.Save();
    }
}