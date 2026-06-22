using UnityEngine;
using UnityEngine.UI;

public class Volume : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;

    void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat("Volume", 1f);

        volumeSlider.value = savedVolume;
        AudioListener.volume = savedVolume;

        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;

        PlayerPrefs.SetFloat("Volume", volume);
        PlayerPrefs.Save();
    }
}