using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;


public class EnemySpawnsCenter : MonoBehaviour
{

    [SerializeField] GameObject melee;
    [SerializeField] GameObject range;
    [SerializeField] GameObject scatter;
    [SerializeField] GameObject split;

    void Update()
    { 
        if(gamemanager.instance.currentRoom > -1 && !gamemanager.instance.roomStarted && gamemanager.instance.enemyInRoom == 0)
        {
            gamemanager.instance.roomStarted = true;
            gamemanager.instance.waveCleared = true;
        }
        if (gamemanager.instance.roomStarted && gamemanager.instance.waveCleared && gamemanager.instance.currentWave <= gamemanager.instance.waves)
        {
            gamemanager.instance.waveCleared = false;
            List<GameObject> allEnemies = new List<GameObject> { melee, range, scatter, split };
            List<int> enemiesWeight = new List<int> { 10, 5, 1 };
            int sum = 0;
            int randomEnemy = 0;
            for (int cycle = 0; cycle < gamemanager.instance.startingAmountOfEnemies * gamemanager.instance.currentWave; cycle++)
            {
                int randomEnemyChance = UnityEngine.Random.Range(0, enemiesWeight.Sum());
                sum = 0;
                for (int chanceIndex = 0; chanceIndex < enemiesWeight.Count; chanceIndex++)
                {
                    sum += enemiesWeight[chanceIndex];
                    if (sum < randomEnemyChance)
                    {
                        randomEnemy = chanceIndex;
                        break;
                    }
                }
                Instantiate(allEnemies[randomEnemy], new Vector3(gamemanager.instance.unitSize * LevelCreation.instance.allCenters[gamemanager.instance.currentRoom].x, 1, gamemanager.instance.unitSize * LevelCreation.instance.allCenters[gamemanager.instance.currentRoom].y), Quaternion.identity);
            }
        }
        if (gamemanager.instance.enemyInRoom == 0)
        {
            gamemanager.instance.waveCleared = true;
        }
    }
}
