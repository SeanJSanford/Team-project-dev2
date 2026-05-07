using UnityEngine;
using TMPro;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private TMP_Text healthText;

    private void Update()
    {
        if (playerStats == null || healthText == null)
            return;

        healthText.text = "HP: " + playerStats.CurrentHP + " / " + playerStats.MaxHP;
    }
}