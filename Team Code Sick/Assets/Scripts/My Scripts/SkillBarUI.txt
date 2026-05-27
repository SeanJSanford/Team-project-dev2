using UnityEngine;
using UnityEngine.UI;

// Controls one segmented skill bar.
public class SkillBarUI : MonoBehaviour
{
    private Image[] barSegments;

    private void Awake()
    {
        barSegments = new Image[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            barSegments[i] = transform.GetChild(i).GetComponent<Image>();
        }
    }

    public void UpdateBar(int currentLevel)
    {
        for (int i = 0; i < barSegments.Length; i++)
        {
            if (barSegments[i] != null)
            {
                barSegments[i].enabled = i < currentLevel;
            }
        }
    }
}