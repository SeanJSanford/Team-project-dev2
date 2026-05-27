using UnityEngine;

// Temporary script for testing skill point gain without enemies.
public class SkillPointTester : MonoBehaviour
{
    private PlayerSkillPoints playerSkillPoints;

    private void Start()
    {
        if (gamemanager.instance == null)
        {
            Debug.LogError("No gamemanager instance found.");
            return;
        }

        playerSkillPoints = gamemanager.instance.GetComponent<PlayerSkillPoints>();

        if (playerSkillPoints == null)
        {
            Debug.LogError("PlayerSkillPoints was not found on the GameManager.");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (playerSkillPoints != null)
            {
                playerSkillPoints.AddEnemyKill();
            }
        }
    }
}