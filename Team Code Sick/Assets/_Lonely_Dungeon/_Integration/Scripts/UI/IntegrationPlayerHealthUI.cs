using UnityEngine;
using TMPro;

/*
 * IntegrationPlayerHealthUI
 * 
 * Purpose:
 * Displays the player's current health values on the UI.
 * 
 * How It Works:
 * - Reads health values from IntegrationPlayerStats
 * - Updates a TextMeshPro text element every frame
 * - Displays current HP and maximum HP
 * 
 * Example:
 * HP: 75 / 100
 * 
 * Connected Systems:
 * - IntegrationPlayerStats
 * - UI Canvas
 * - Future damage/healing systems
 * 
 * Design Note:
 * This script should ONLY handle UI display logic.
 * 
 * It should NOT:
 * - Modify player stats
 * - Handle combat
 * - Handle healing/damage calculations
 * 
 * Those systems belong in IntegrationPlayerStats.
 * 
 * Future Expansion Ideas:
 * - Animated health bars
 * - Damage flash effects
 * - Shield/armor display
 * - Critical health warnings
 * - Regeneration visuals
 * - Multiplayer player frames
 * 
 * Optimization Note:
 * This currently updates every frame for simplicity.
 * 
 * Later improvements could:
 * - Update only when HP changes
 * - Use UI events/callbacks
 * - Reduce unnecessary string rebuilding
 */

public class IntegrationPlayerHealthUI : MonoBehaviour
{
    [Header("References")]

    // Reference to the player's stat system.
    [SerializeField] private IntegrationPlayerStats playerStats;

    // Text element used to display health values.
    [SerializeField] private TMP_Text healthText;

    private void Update()
    {
        // Prevent null reference errors if references are missing.
        if (playerStats == null || healthText == null)
            return;

        // Update the UI text using the player's current HP values.
        healthText.text =
            "HP: " +
            playerStats.CurrentHP +
            " / " +
            playerStats.MaxHP;
    }
}