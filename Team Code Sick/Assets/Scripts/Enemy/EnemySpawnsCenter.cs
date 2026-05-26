using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawnsCenter : MonoBehaviour
{
    [SerializeField] List<GameObject> allPosibleEnemies;

    [SerializeField] List<int> enemyWeights;
    [SerializeField] int maxWave;
    [SerializeField] int baseEnemyWeight;
    [SerializeField] int baseMaxAmountOfEnemies;

    public static EnemySpawnsCenter instance;

    int currentWave = 0;
    
    int roomDifficulty = 1;

    int roomSize = 25;
    List<List<int>> grid = new List<List<int>>();

    bool roomStarted = false;
    bool waveStarted = false;
    bool lastWave = false;

    List<GameObject> currentEnemies = new List<GameObject>();
    
    void Start()
    {
        instance = this;

        for (int y = 0; y < roomSize; y++)
        {
            List<int> row = new List<int>();
            for (int x = 0; x < roomSize; x++)
            {
                row.Add(0);
            }
            grid.Add(row);
        }
    }

    void Update()
    {
        

        StartWave();

    }

    void ResetWave()
    {
        if (currentEnemies.Count <= 0)
        {
            waveStarted = false;
        }
    }
    void ResetRoom()
    {
        if (lastWave && currentEnemies.Count <= 0)
        {
            roomStarted = false;
            waveStarted = false;
            lastWave = false;
            gamemanager.instance.roomCleared = true;
            for (int y = 0; y < roomSize; y++)
            {
                for (int x = 0; x < roomSize; x++)
                {
                    grid[y][x] = 0;
                }
            }

        }
    }

    void NextWave()
    {
        if (roomStarted && !waveStarted && !lastWave)
        {
            currentWave++;
            if (currentWave == maxWave) lastWave = true;

            waveStarted = true;

            int totalEnemyWeight = baseEnemyWeight + 5 * roomDifficulty * (currentWave * currentWave);
            int totalEnemyCount = baseMaxAmountOfEnemies + roomDifficulty * (currentWave * currentWave);

            int currentWeight = 0;

            (int x, int y) originalCenter = LevelCreation.instance.allCenters[gamemanager.instance.currentRoom];

            int offsetX = (gamemanager.instance.unitSize * (int)(LevelCreation.instance.FightRoomSize.x / 2));
            int offsetY = (gamemanager.instance.unitSize * (int)(LevelCreation.instance.FightRoomSize.y / 2));

            (int x, int y) roomWorldPosition = (originalCenter.x * gamemanager.instance.unitSize - offsetX,
                                                originalCenter.y * gamemanager.instance.unitSize - offsetY);

            while (currentWeight < totalEnemyWeight && currentEnemies.Count < totalEnemyCount)
            {
                // Spawning the right amount of enemies

                for (int currentSpawnedEnemy = 0; currentSpawnedEnemy < totalEnemyCount; currentSpawnedEnemy++)
                {

                    // Getting a Random Enemy

                    int randomEnemyChoice = Random.Range(0, enemyWeights.Sum());
                    int randomEnemyIndex = 0;
                    int weightSum = 0;
                    for (int chanceIndex = 0; chanceIndex < enemyWeights.Count; chanceIndex++)
                    {
                        weightSum += enemyWeights[chanceIndex];
                        if (weightSum > randomEnemyChoice)
                        {
                            randomEnemyIndex = chanceIndex;
                            break;
                        }
                    }

                    currentWeight += enemyWeights[randomEnemyIndex];

                    // Choosing where to spawn the enemy

                    while (true)
                    {
                        int x = Random.Range(0, roomSize);
                        int y = Random.Range(0, roomSize);

                        if (grid[y][x] != 1)
                        {
                            grid[y][x] = 1;
                            int roomUnitSizeX = (gamemanager.instance.unitSize * LevelCreation.instance.FightRoomSize.x / roomSize);
                            int roomUnitSizeY = (gamemanager.instance.unitSize * LevelCreation.instance.FightRoomSize.y / roomSize);
                            currentEnemies.Add(Instantiate(allPosibleEnemies[randomEnemyIndex], new Vector3(roomWorldPosition.x + (roomUnitSizeX * x), 1, roomWorldPosition.y + (roomUnitSizeY * y)), Quaternion.identity));
                            break;
                        }    
                    }
                }
            }
        }
        ResetRoom();
        ResetWave();
    }

    void StartWave()
    {
        if (!roomStarted && gamemanager.instance.currentRoom > -1) // The second check is if we are in a room but we havent done it yet
        {
            roomStarted = true;
        }
        NextWave();
    }

    public void RemoveEnemy(GameObject enemy)
    {
        currentEnemies.Remove(enemy);
    }
}
