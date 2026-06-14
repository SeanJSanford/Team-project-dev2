using UnityEngine;

// Handles skill points and applies upgrades through the PlayerStats on the current player.
public class PlayerSkillPoints : MonoBehaviour
{
    public int availableSkillPoints = 0;
    public int enemiesKilled = 0;

    public int healthLevel;
    public int speedLevel;
    public int damageLevel;
    public int defenseLevel;

    private PlayerStats playerStats;

    private StatModifier healthModifier;
    private StatModifier speedModifier;
    private StatModifier damageModifier;
    private StatModifier defenseModifier;

    private void Start()
    {
        FindPlayerStats();
    }

    private bool FindPlayerStats()
    {
        if (playerStats != null)
            return true;

        if (gamemanager.instance == null)
        {
            Debug.LogError("No gamemanager instance found.");
            return false;
        }

        if (gamemanager.instance.player == null)
        {
            Debug.LogError("No player found on gamemanager.");
            return false;
        }

        playerStats = gamemanager.instance.player.GetComponent<PlayerStats>();

        if (playerStats == null)
        {
            Debug.LogError("PlayerStats was not found on the player.");
            return false;
        }

        return true;
    }
    bool updateStats = true;

    private void Awake() => playerStats = GetComponent<PlayerStats>();

    private void Update()
    {
        if(updateStats)
        {
            gamemanager.instance.playerScript.updatePlayerUI();
            gamemanager.instance.playerScript.speed = gamemanager.instance.playerScript.OriginalSpeed + gamemanager.instance.playerScript.OriginalSpeed * (speedLevel * .1f);
            gamemanager.instance.playerScript.sprintMod = gamemanager.instance.playerScript.OriginalSprintMod + gamemanager.instance.playerScript.OriginalSprintMod * (speedLevel * .1f);
            updateStats = false;
        }
    }

    public void AddEnemyKill()
    {
        enemiesKilled++;

        int pointsEarned = Mathf.RoundToInt(Mathf.Pow(1.25f, enemiesKilled));

        availableSkillPoints += 1;

        Debug.Log("Enemy killed. Earned " + pointsEarned + " skill point(s).");
    }

    public void IncreaseHealth()
    {
        if (!TrySpendSkillPoint())
            return;
        gamemanager.instance.playerScript.HP += 1;
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
        updateStats = true;
        return true;
    }

    private void ReplaceModifier(ref StatModifier currentModifier, StatType statType, float value)
    {
        if (!FindPlayerStats())
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
            StatModifierType.PercentAdd,
            value
        );

        playerStats.AddModifier(currentModifier);
    }
}