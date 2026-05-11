using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealthUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private Image healthFillImage;

    private void Update()
    {
        if (playerStats == null)
            return;

        float currentHealth = playerStats.CurrentHealth;
        float maxHealth = playerStats.MaxHealth;

        if (healthText != null)
        {
            healthText.text =
                "HP: " +
                Mathf.RoundToInt(currentHealth) +
                " / " +
                Mathf.RoundToInt(maxHealth);
        }

        if (healthFillImage != null)
        {
            healthFillImage.fillAmount = currentHealth / maxHealth;
        }
    }
}

/*
========================================================
Project: Team Code Sick
Script: PlayerHealthUI.cs

Primary Developer:
- Avery Wilson

System Category:
- UI System
- Health Display
- Runtime Stat Visualization

Purpose:
- Displays the player's current runtime health values
  on the gameplay UI.
- Reads health data directly from PlayerStats
  and updates the health display in real time.

Core Responsibilities:
- Read CurrentHealth from PlayerStats
- Read MaxHealth from PlayerStats
- Update TextMeshPro health display
- Reflect runtime stat changes visually

Connected Team Systems:
- Avery: PlayerStats runtime architecture
- Dai: Player gameplay integration
- Sean: Combat feedback and gameplay readability support
- Nilo: Gameplay readability oversight
- Future UI systems

How It Works:
- Retrieves runtime health values from PlayerStats
- Converts values into readable UI text
- Displays:
    HP: Current / Maximum

Example:
HP: 75 / 100

Design Philosophy:
This script intentionally handles ONLY UI display logic.

Responsibilities intentionally excluded:
- Damage calculations
- Healing logic
- Stat modification
- Combat handling
- Gameplay state management

Those systems remain centralized inside PlayerStats.

Why This Exists:
Separating gameplay logic from UI logic:
- Improves modularity
- Simplifies debugging
- Prevents duplicated gameplay calculations
- Makes future UI upgrades easier

Development Notes:
- Built as an early prototype UI system.
- Uses TextMeshPro for clean scalable text rendering.
- Reads values directly from the runtime stat framework.
- Designed to support future UI expansion.
- Intended to integrate cleanly with:
    - PlayerStats
    - Future HUD systems
    - Combat feedback systems

Optimization Notes:
- Currently updates every frame for simplicity.
- Future versions should transition to:
    - Event-driven updates
    - UI callbacks
    - Health change notifications
    - Reduced string rebuilding

Future Expansion Ideas:
- Animated health bars
- Damage flash effects
- Armor/shield display
- Critical HP warnings
- Regeneration visuals
- Multiplayer player frames
- Status effect icons
- UI transitions/animations
========================================================
*/