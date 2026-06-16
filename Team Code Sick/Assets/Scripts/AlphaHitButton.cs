using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Button))]
public class AlphaHitButton : MonoBehaviour
{
    [Range(0.01f, 1f)]
    [SerializeField] float alphaThreshold = 0.1f;

    void Awake()
    {
        Image buttonImage = GetComponent<Image>();
        Button button = GetComponent<Button>();

        buttonImage.raycastTarget = true;
        buttonImage.alphaHitTestMinimumThreshold = alphaThreshold;

        button.targetGraphic = buttonImage;

        // Turn off raycast on children like Text/TMP
        Graphic[] childGraphics = GetComponentsInChildren<Graphic>(true);

        foreach (Graphic graphic in childGraphics)
        {
            if (graphic.gameObject != gameObject)
            {
                graphic.raycastTarget = false;
            }
        }
    }
}