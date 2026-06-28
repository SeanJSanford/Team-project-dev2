using TMPro;
using UnityEngine;
using System.Collections;

public class PickupNotification : MonoBehaviour
{
    public static PickupNotification Instance;

    [SerializeField] private TMP_Text pickupText;

    private Coroutine currentRoutine;

    private void Awake()
    {
        Instance = this;
        pickupText.gameObject.SetActive(false);
    }

    public void Show(string text)
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(ShowRoutine(text));
    }

    IEnumerator ShowRoutine(string text)
    {
        pickupText.text = text;
        pickupText.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);

        pickupText.gameObject.SetActive(false);
    }
}