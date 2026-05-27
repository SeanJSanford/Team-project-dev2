using UnityEngine;
using TMPro;

// Updates the skill UI and connects button presses to PlayerSkillPoints.
public class SkillPointUI : MonoBehaviour
{
    public PlayerSkillPoints playerSkillPoints;

    public TMP_Text availablePointsText;

    public SkillBarUI healthBar;
    public SkillBarUI speedBar;
    public SkillBarUI damageBar;
    public SkillBarUI defenseBar;

    private void Update()
    {
        UpdateSkillUI();
    }

    public void IncreaseHealth()
    {
        playerSkillPoints.IncreaseHealth();
        UpdateSkillUI();
    }

    public void DecreaseHealth()
    {
        playerSkillPoints.DecreaseHealth();
        UpdateSkillUI();
    }

    public void IncreaseSpeed()
    {
        playerSkillPoints.IncreaseSpeed();
        UpdateSkillUI();
    }

    public void DecreaseSpeed()
    {
        playerSkillPoints.DecreaseSpeed();
        UpdateSkillUI();
    }

    public void IncreaseDamage()
    {
        playerSkillPoints.IncreaseDamage();
        UpdateSkillUI();
    }

    public void DecreaseDamage()
    {
        playerSkillPoints.DecreaseDamage();
        UpdateSkillUI();
    }

    public void IncreaseDefense()
    {
        playerSkillPoints.IncreaseDefense();
        UpdateSkillUI();
    }

    public void DecreaseDefense()
    {
        playerSkillPoints.DecreaseDefense();
        UpdateSkillUI();
    }

    public void UpdateSkillUI()
    {
        availablePointsText.text =
            "Skill Points: " + playerSkillPoints.availableSkillPoints;

        healthBar.UpdateBar(playerSkillPoints.healthLevel);
        speedBar.UpdateBar(playerSkillPoints.speedLevel);
        damageBar.UpdateBar(playerSkillPoints.damageLevel);
        defenseBar.UpdateBar(playerSkillPoints.defenseLevel);
    }
}