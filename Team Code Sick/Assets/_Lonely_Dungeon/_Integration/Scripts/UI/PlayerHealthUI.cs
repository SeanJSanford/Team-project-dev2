using UnityEngine;
using TMPro;

/*
 * PlayerHealthUI
 * 
 * Purpose:
 * Displays the player's current health values on the UI.
 * 
 * How It Works:
 * - Reads health values from PlayerStats
 * - Updates a TextMeshPro text element every frame
 * - Displays current HP and maximum HP
 * 
 * Example:
 * HP: 75 / 100
 * 
 * Connected Systems:
 * - PlayerStats
 * - UI Canvas
 * - Future damage/healing systems
 * - Future shield/armor systems
 * 
 * Design Notes:
 * This script ONLY handles UI display logic.
 * 
 * It should NOT:
 * - Modify player stats
 * - Handle combat calculations
 * - Handle healing/damage logic
 * - Manage gameplay systems
 * 
 * Those systems belong in PlayerStats.
 * 
 * Current Stat System:
 * PlayerStats now uses a modifier-based stat architecture.
 * 
 * This UI reads:
 * - CurrentHealth
 * - MaxHealth
 * 
 * directly from the runtime stat system.
 * 
 * Future Expansion Ideas:
 * - Animated health bars
 * - Damage flash effects
 * - Shield/armor display
 * - Critical health warnings
 * - Regeneration visuals
 * - Multiplayer player frames
 * - Status effect indicators
 * 
 * Optimization Note:
 * This currently updates every frame for simplicity.
 * 
 * Later improvements could:
 * - Update only when HP changes
 * - Use events/callbacks
 * - Reduce unnecessary string rebuilding
 */

public class PlayerHealthUI : MonoBehaviour
{
    [Header("References")]

    // Reference to the player's runtime stat system.
    [SerializeField] private PlayerStats playerStats;

    // TextMeshPro element used to display health values.
    [SerializeField] private TMP_Text healthText;

    private void Update()
    {
        // Prevent null reference errors if references are missing.
        if (playerStats == null || healthText == null)
            return;

        // Update the UI using the player's current runtime health values.
        healthText.text =
            "HP: " +
            Mathf.RoundToInt(playerStats.CurrentHealth) +
            " / " +
            Mathf.RoundToInt(playerStats.MaxHealth);
    }
}