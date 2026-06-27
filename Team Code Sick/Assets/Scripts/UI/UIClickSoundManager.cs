using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIClickSoundManager : MonoBehaviour
{
    public static UIClickSoundManager instance;

    [Header("Click Sound")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip clickClip;
    [Range(0f, 1f)][SerializeField] float clickVolume = 0.5f;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (audioSource != null)
            audioSource.ignoreListenerPause = true;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        RegisterAllButtons();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RegisterAllButtons();
    }

    void RegisterAllButtons()
    {
        Button[] buttons = FindObjectsOfType<Button>(true);

        foreach (Button button in buttons)
        {
            button.onClick.RemoveListener(PlayClickSound);
            button.onClick.AddListener(PlayClickSound);
        }
    }

    public void PlayClickSound()
    {
        if (audioSource != null && clickClip != null)
        {
            audioSource.PlayOneShot(clickClip, clickVolume);
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}