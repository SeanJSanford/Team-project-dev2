using UnityEngine;
using System.Collections;

public class _EnemyAI : MonoBehaviour, AW_IDamage
{
    [Header("Enemy Health")]
    [SerializeField] private int maxHP = 40;
    [SerializeField] private int currentHP;

    [Header("Visual Feedback")]
    [SerializeField] private Renderer rend;
    [SerializeField] private Transform healthBarFill;

    private Color originalColor;
    private Vector3 originalHealthBarScale;

    private void Start()
    {
        currentHP = maxHP;

        if (rend == null)
            rend = GetComponentInChildren<Renderer>();

        originalColor = rend.material.color;

        if (healthBarFill != null)
            originalHealthBarScale = healthBarFill.localScale;
    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        UpdateHealthBar();

        Debug.Log($"{gameObject.name} took {amount} damage. HP: {currentHP}/{maxHP}");

        if (currentHP <= 0)
        {
            GetComponent<AW_LootDrop>()?.DropLoot();
            Destroy(gameObject);
        }
        else
        {
            StartCoroutine(FlashRed());
        }
    }

    private void UpdateHealthBar()
    {
        if (healthBarFill == null)
            return;

        float healthPercent = (float)currentHP / maxHP;

        healthBarFill.localScale = new Vector3(
            originalHealthBarScale.x * healthPercent,
            originalHealthBarScale.y,
            originalHealthBarScale.z
        );
    }

    private IEnumerator FlashRed()
    {
        rend.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        rend.material.color = originalColor;
    }
}

/*
========================================================
Project: Team Code Sick
Script: _EnemyAI.cs

Primary Developer:
- Avery Wilson

System Category:
- Early Enemy Prototype
- Combat Testing Framework
- Stat Validation Utility

Original Framework Source:
- Modified from Full Sail lecture enemy prototype code

Purpose:
- Early enemy testing script used to validate:
    - Damage handling
    - Player stat scaling
    - Health systems
    - Loot drops
    - Hit feedback systems

Why This Exists:
Before the finalized enemy architecture was completed,
this script provided a lightweight enemy target for
testing player combat systems and stat modifiers.

This allowed rapid iteration on:
- Damage values
- Health balancing
- Loot spawning
- Combat feedback
- UI health bar scaling

Connected Team Systems:
- Avery: PlayerStats / StatModifier systems
- Heather: LootDrop integration
- Sean: Combat systems / enemy combat interactions
- Dai: Movement/combat gameplay testing
- Nilo: Gameplay integration oversight

Development Notes:
- Uses older project-specific AW_IDamage interface.
- Intended to be migrated to the newer IDamageable system.
- Served as a temporary bridge between lecture prototype
  combat and the team's modular combat architecture.
- Built primarily for isolated systems testing
  before modular enemy components were separated.

Current Status:
- Legacy/testing script
- Replaced by newer enemy implementations
- Still useful for isolated combat/stat testing
- Maintained for prototype/debug validation purposes

Future Refactor Goals:
- Replace AW_IDamage with IDamageable
- Separate health into EnemyHealth component
- Separate visuals into EnemyFeedback component
- Integrate with finalized combat systems
- Connect into future enemy state machine architecture
========================================================
*/