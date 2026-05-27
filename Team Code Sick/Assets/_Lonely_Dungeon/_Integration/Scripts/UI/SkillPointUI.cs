using UnityEngine;
using TMPro;

// Updates the skill UI and connects button presses to PlayerSkillPoints.
public class SkillPointUI : MonoBehaviour
{
    private PlayerSkillPoints playerSkillPoints;

    public TMP_Text availablePointsText;

    public SkillBarUI healthBar;
    public SkillBarUI speedBar;
    public SkillBarUI damageBar;
    public SkillBarUI defenseBar;

    private void Start()
    {
        GetSkillPointsFromGameManager();
        UpdateSkillUI();
    }

    private void Update()
    {
        UpdateSkillUI();
    }

    private bool GetSkillPointsFromGameManager()
    {
        if (playerSkillPoints != null)
            return true;

        if (gamemanager.instance == null)
        {
            Debug.LogError("No gamemanager instance found.");
            return false;
        }

        playerSkillPoints = gamemanager.instance.GetComponent<PlayerSkillPoints>();

        if (playerSkillPoints == null)
        {
            Debug.LogError("PlayerSkillPoints was not found on the GameManager.");
            return false;
        }

        return true;
    }

    public void IncreaseHealth()
    {
        if (!GetSkillPointsFromGameManager())
            return;

        playerSkillPoints.IncreaseHealth();
        UpdateSkillUI();
    }

    public void DecreaseHealth()
    {
        if (!GetSkillPointsFromGameManager())
            return;

        playerSkillPoints.DecreaseHealth();
        UpdateSkillUI();
    }

    public void IncreaseSpeed()
    {
        if (!GetSkillPointsFromGameManager())
            return;

        playerSkillPoints.IncreaseSpeed();
        UpdateSkillUI();
    }

    public void DecreaseSpeed()
    {
        if (!GetSkillPointsFromGameManager())
            return;

        playerSkillPoints.DecreaseSpeed();
        UpdateSkillUI();
    }

    public void IncreaseDamage()
    {
        if (!GetSkillPointsFromGameManager())
            return;

        playerSkillPoints.IncreaseDamage();
        UpdateSkillUI();
    }

    public void DecreaseDamage()
    {
        if (!GetSkillPointsFromGameManager())
            return;

        playerSkillPoints.DecreaseDamage();
        UpdateSkillUI();
    }

    public void IncreaseDefense()
    {
        if (!GetSkillPointsFromGameManager())
            return;

        playerSkillPoints.IncreaseDefense();
        UpdateSkillUI();
    }

    public void DecreaseDefense()
    {
        if (!GetSkillPointsFromGameManager())
            return;

        playerSkillPoints.DecreaseDefense();
        UpdateSkillUI();
    }

    public void UpdateSkillUI()
    {
        if (!GetSkillPointsFromGameManager())
            return;

        availablePointsText.text =
            "Skill Points: " + playerSkillPoints.availableSkillPoints;

        healthBar.UpdateBar(playerSkillPoints.healthLevel);
        speedBar.UpdateBar(playerSkillPoints.speedLevel);
        damageBar.UpdateBar(playerSkillPoints.damageLevel);
        defenseBar.UpdateBar(playerSkillPoints.defenseLevel);
    }
}