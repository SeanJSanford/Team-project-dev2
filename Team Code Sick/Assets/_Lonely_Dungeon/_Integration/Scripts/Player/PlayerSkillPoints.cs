using UnityEngine;

// Handles skill points and applies upgrades through PlayerStats modifiers.
public class PlayerSkillPoints : MonoBehaviour
{
    public int availableSkillPoints = 0;
    public int enemiesKilled = 0;

    public int healthLevel = 0;
    public int speedLevel = 0;
    public int damageLevel = 0;
    public int defenseLevel = 0;

    private PlayerStats playerStats;

    private StatModifier healthModifier;
    private StatModifier speedModifier;
    private StatModifier damageModifier;
    private StatModifier defenseModifier;

    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>();
    }

    public void AddEnemyKill()
    {
        enemiesKilled++;

        int pointsEarned = Mathf.RoundToInt(Mathf.Pow(1.25f, enemiesKilled));

        availableSkillPoints += pointsEarned;

        Debug.Log("Enemy killed. Earned " + pointsEarned + " skill point(s).");
    }

    public void IncreaseHealth()
    {
        if (!TrySpendSkillPoint())
            return;

        healthLevel++;
        ReplaceModifier(ref healthModifier, StatType.MaxHealth, healthLevel * 10f);
    }

    public void DecreaseHealth()
    {
        if (healthLevel <= 0)
            return;

        healthLevel--;
        availableSkillPoints++;
        ReplaceModifier(ref healthModifier, StatType.MaxHealth, healthLevel * 10f);
    }

    public void IncreaseSpeed()
    {
        if (!TrySpendSkillPoint())
            return;

        speedLevel++;
        ReplaceModifier(ref speedModifier, StatType.MoveSpeed, speedLevel * 0.5f);
    }

    public void DecreaseSpeed()
    {
        if (speedLevel <= 0)
            return;

        speedLevel--;
        availableSkillPoints++;
        ReplaceModifier(ref speedModifier, StatType.MoveSpeed, speedLevel * 0.5f);
    }

    public void IncreaseDamage()
    {
        if (!TrySpendSkillPoint())
            return;

        damageLevel++;
        ReplaceModifier(ref damageModifier, StatType.Damage, damageLevel * 2f);
    }

    public void DecreaseDamage()
    {
        if (damageLevel <= 0)
            return;

        damageLevel--;
        availableSkillPoints++;
        ReplaceModifier(ref damageModifier, StatType.Damage, damageLevel * 2f);
    }

    public void IncreaseDefense()
    {
        if (!TrySpendSkillPoint())
            return;

        defenseLevel++;
        ReplaceModifier(ref defenseModifier, StatType.Armor, defenseLevel * 1f);
    }

    public void DecreaseDefense()
    {
        if (defenseLevel <= 0)
            return;

        defenseLevel--;
        availableSkillPoints++;
        ReplaceModifier(ref defenseModifier, StatType.Armor, defenseLevel * 1f);
    }

    private bool TrySpendSkillPoint()
    {
        if (availableSkillPoints <= 0)
        {
            Debug.Log("No skill points available.");
            return false;
        }

        availableSkillPoints--;
        return true;
    }

    private void ReplaceModifier(ref StatModifier currentModifier, StatType statType, float value)
    {
        if (playerStats == null)
            return;

        if (currentModifier != null)
            playerStats.RemoveModifier(currentModifier);

        if (value <= 0f)
        {
            currentModifier = null;
            return;
        }

        currentModifier = new StatModifier(
            statType,
            StatModifierType.Flat,
            value
        );

        playerStats.AddModifier(currentModifier);
    }
}