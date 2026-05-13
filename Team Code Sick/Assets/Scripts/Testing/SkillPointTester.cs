using UnityEngine;

// Temporary script for testing skill point gain without enemies.
public class SkillPointTester : MonoBehaviour
{
    public PlayerSkillPoints playerSkillPoints;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            playerSkillPoints.AddEnemyKill();
        }
    }
}

// To fix after merge, it should only take something like: 
// playerSkillPoints.AddEnemyKill();