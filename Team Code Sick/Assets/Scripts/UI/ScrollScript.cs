using UnityEngine;

public class ScrollScript : MonoBehaviour
{
    [SerializeField] float scrollSpeed = 50f;

    RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        rectTransform.anchoredPosition +=
            Vector2.up * scrollSpeed * Time.deltaTime;
    }
}